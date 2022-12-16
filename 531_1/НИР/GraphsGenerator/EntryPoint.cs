using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
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

                    var result = generator.Generate(o.VertexCount);

                    var totalRes = new List<(string graph, string vectorDegree, int chromaticNumber)>();
                    var degreeVectorCalculator = new DegreeVectorCalculator();
                    var chromaticNumberCalculator = new ChromaticNumberCalculator();

                    foreach (var g6 in result)
                    {
                        var graph = Graph.G6ToGraph(g6);
                        var vectorFormated = string.Join("", degreeVectorCalculator.GetDegreeVector(graph));
                        var chromaricNumber = chromaticNumberCalculator.GetChromaticNumber(graph);
                        totalRes.Add((g6, vectorFormated, chromaricNumber));
                    }

                    var grouped = totalRes.GroupBy(x => x.vectorDegree);

                    foreach (var group in grouped)
                    {
                        Console.WriteLine($"vector: {group.Key}");

                        foreach (var val in group)
                        {
                            Console.WriteLine($"    graph: {val.graph}; chromatic number: {val.chromaticNumber}");
                        }

                        Console.WriteLine();
                    }
                    
                    //if (o.WriteGraphsToFile)
                    //{
                    //    File.WriteAllLines(o.FileName, result);
                    //    Console.WriteLine($"Graphs was written to \"{o.FileName}\" file.");
                    //    return;
                    //}

                    
                    //Console.WriteLine("Graphs in g6 format: ");
                    //foreach (var g6 in result)
                    //{
                    //    var graph = Graph.G6ToGraph(g6);
                    //    var vector = degreeVectorCalculator.GetDegreeVector(graph);
                    //    var chromaricNumber = chromaticNumberCalculator.GetChromaticNumber(graph);
                    //    Console.WriteLine($"Graph: {g6}");
                    //    Console.WriteLine($"Vector: {string.Join(", ", vector)}");
                    //    Console.WriteLine($"Chromatic number: {chromaricNumber}");
                    //};

                    Console.WriteLine($"Total graphs count: {result.Count()}.");
                    Console.WriteLine($"Total calculating time in seconds: {(DateTime.Now - startTime).TotalSeconds}.");
                });
        }
    }
}
