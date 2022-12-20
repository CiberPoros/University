using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CommandLine;
using GraphsGenerator.ChromaticNumber;
using GraphsGenerator.DegreeVector;

namespace GraphsGenerator
{
    class EntryPoint
    {
        static void Main(string[] args)
        {
            var consoleArguments = new Options();
            Parser.Default.ParseArguments<Options>(args)
                .WithParsed(o => 
                {
                    IGenerator generator = IGenerator.CreateNew(o.GeneratorType);
                    var startTime = DateTime.Now;

                    var result = generator.GenerateGraphFormat(o.VertexCount);

                    var totalRes = new List<(string graph, string vectorDegree, int chromaticNumber)>();
                    var degreeVectorCalculator = new DegreeVectorCalculator();
                    var chromaticNumberCalculator = new ChromaticNumberCalculator();

                    foreach (var graph in result)
                    {
                        var vectorFormated = string.Join("", degreeVectorCalculator.GetDegreeVector(graph));
                        var chromaricNumber = chromaticNumberCalculator.GetChromaticNumber(graph);
                        totalRes.Add((graph.ToG6(), vectorFormated, chromaricNumber));
                    }

                    var grouped = totalRes.GroupBy(x => x.vectorDegree);

                    var resultStrings = new List<string>();
                    foreach (var group in grouped)
                    {
                        resultStrings.Add($"vector: {group.Key}");

                        foreach (var val in group)
                        {
                            resultStrings.Add($"    graph: {val.graph}; chromatic number: {val.chromaticNumber}");
                        }

                        resultStrings.Add(string.Empty);
                    }

                    if (o.WriteGraphsToFile)
                    {
                        File.WriteAllLines(o.FileName, resultStrings);
                        Console.WriteLine($"Graphs was written to \"{o.FileName}\" file.");
                        return;
                    }
                    else
                    {
                        foreach (var str in resultStrings)
                        {
                            Console.WriteLine(str);
                        }
                    }

                    Console.WriteLine($"Total graphs count: {result.Count()}.");
                    Console.WriteLine($"Total calculating time in seconds: {(DateTime.Now - startTime).TotalSeconds}.");
                });
        }
    }
}
