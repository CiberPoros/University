using System;
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
                    foreach (var graph in result)
                    {
                        var model = new GraphWithVectorModel
                        {
                            G6 = graph.ToG6(),
                            VertexCount = (short)o.VertexCount,
                            DegreeVectorValue = long.Parse(string.Join("", degreeVectorCalculator.GetDegreeVector(graph))),
                            ChromaticNumber = chromaticNumberCalculator.GetChromaticNumber(graph)
                        };

                        graphsPartition.Add(model);

                        if (currentNumber > 0 && currentNumber % 1000 == 0)
                        {
                            await dataAccessService.UpsertGraphWithVector(graphsPartition);
                            graphsPartition.Clear();
                        }
                    }

                    await dataAccessService.UpsertGraphWithVector(graphsPartition);

                    // Console.WriteLine($"Total graphs count: {result.Count()}.");
                    Console.WriteLine($"Total calculating time in seconds: {(DateTime.Now - startTime).TotalSeconds}.");
                });
        }
    }
}
