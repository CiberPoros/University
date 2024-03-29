﻿using System;
using System.Collections.Generic;
using System.IO;
using CommandLine;
using GraphsGenerator.ChromaticNumber;
using GraphsGenerator.DegreeVector;
using Microsoft.Extensions.Configuration;
using GraphsGenerator.DataAccess;
using System.Threading.Tasks;

namespace GraphsGenerator
{
    class EntryPoint
    {
        private const int _partitionsSize = 100;

        static async Task Main(string[] args)
        {
            await Task.CompletedTask;
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");
            IConfiguration config = builder.Build();

            var dataAccessService = new SqlServerDALService(config);

            var consoleArguments = new Options();
            Parser.Default.ParseArguments<Options>(args)
                .WithParsed(async o => 
                {
                    IGenerator generator = IGenerator.CreateNew(o.GeneratorType);
                    var startTime = DateTime.Now;

                    var result = generator.GenerateGraphFormat(o.VertexCount);

                    var degreeVectorCalculator = new DegreeVectorCalculator();
                    var chromaticNumberCalculator = new ChromaticNumberCalculator();

                    var currentNumber = 0;
                    var graphsPartition = new List<GraphWithVectorModel>();
                    var skipPartition = false;
                    foreach (var graph in result)
                    {
                        Console.WriteLine($"Graph number: {currentNumber}; Time: {DateTime.Now:HH:mm:ss}");
                        if (currentNumber % _partitionsSize == 0)
                        {
                            var (success, _) = await dataAccessService.TryGetGraphWithVectorModelByG6(graph.ToG6());
                            skipPartition = success; // если success, то следующие 100 графов уже записаны в базу ранее, можно пропустить
                        }

                        if (skipPartition)
                        {
                            currentNumber++;
                            continue;
                        }

                        var model = new GraphWithVectorModel
                        {
                            G6 = graph.ToG6(),
                            VertexCount = (short)o.VertexCount,
                            DegreeVectorValue = long.Parse(string.Join("", degreeVectorCalculator.GetDegreeVector(graph))),
                            ChromaticNumber = chromaticNumberCalculator.GetChromaticNumber(graph)
                        };

                        graphsPartition.Add(model);

                        if (currentNumber > 0 && currentNumber % _partitionsSize == 0)
                        {
                            await dataAccessService.UpsertGraphWithVector(graphsPartition);
                            graphsPartition.Clear();
                        }

                        currentNumber++;

                        Console.WriteLine($"Current graph number: {currentNumber}");
                    }

                    await dataAccessService.UpsertGraphWithVector(graphsPartition);

                    // Console.WriteLine($"Total graphs count: {result.Count()}.");
                    Console.WriteLine($"Total calculating time in seconds: {(DateTime.Now - startTime).TotalSeconds}.");
                });
        }
    }
}
