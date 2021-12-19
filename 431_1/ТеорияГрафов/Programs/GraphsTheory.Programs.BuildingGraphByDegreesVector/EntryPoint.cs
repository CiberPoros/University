using System;
using System.Collections.Generic;
using System.Linq;
using GraphsTheory.Common;

namespace GraphsTheory.Programs.BuildingGraphByDegreesVector
{
    class EntryPoint
    {
        static void Main(string[] args)
        {
            for (; ; )
            {
                var vector = ReadDegreesVector();
                var graph = BuildGraphByDegreesVector(vector);

                var graphIO = new GraphIOConsole();
                graphIO.WriteGraph(graph); 
            }
        }

        private static List<List<int>> BuildGraphByDegreesVector(List<int> vector)
        {
            var graph = new List<List<int>>();
            for (int i = 0; i < vector.Count; i++)
            {
                graph.Add(new List<int>());
            }

            for (int fromVertex = 0; fromVertex < vector.Count; fromVertex++)
            {
                for (int toVertex = fromVertex + 1; vector[fromVertex] > 0; toVertex++)
                {
                    if (vector[toVertex] <= 0)
                    {
                        continue; // already builded vertex
                    }

                    graph[fromVertex].Add(toVertex);
                    graph[toVertex].Add(fromVertex);
                    vector[fromVertex]--;
                    vector[toVertex]--;
                }
            }

            return graph;
        }

        private static List<int> ReadDegreesVector()
        {
            Console.WriteLine("Введите вектор смежности неориентированного графа, используя ' ' или ',' в качестве разделителя:");
            Console.WriteLine();

            for (; ; )
            {
                var input = Console.ReadLine().Split(' ', ',', StringSplitOptions.RemoveEmptyEntries);

                if (input.Any(x => !int.TryParse(x, out var parsed) || parsed < 0 || parsed >= input.Length))
                {
                    Console.WriteLine("Ожидались числа больше 0 и меньше n, где n - количество вершин графа. Повторите попытку...");
                    Console.WriteLine();
                    continue;
                }

                var result = input.Select(x => int.Parse(x)).ToList();

                if (!result.SequenceEqual(result.OrderByDescending(x => x)))
                {
                    Console.WriteLine("Степени вершин графа должны идти в порядке невозрастания. Повторите попытку...");
                    Console.WriteLine();
                    continue;
                }

                if (result.Sum() % 2 != 0)
                {
                    Console.WriteLine("Сумма степеней вершин должна быть четной. Повторите попытку...");
                    Console.WriteLine();
                    continue;
                }

                return result;
            }
        }
    }
}
