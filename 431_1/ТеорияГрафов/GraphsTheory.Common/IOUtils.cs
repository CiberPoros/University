using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GraphsTheory.Common
{
    public interface IGraphsIO
    {
        List<List<int>> ReadGraph(GraphFormat graphFormat = GraphFormat.MATRIX_ADJACENCY);

        void WriteGraph(List<List<int>> graph, GraphFormat graphFormat = GraphFormat.VECTOR_ADJACENCY);
    }

    public class GraphIOConsole : IGraphsIO
    {
        public List<List<int>> ReadGraph(GraphFormat graphFormat = GraphFormat.MATRIX_ADJACENCY) =>
            graphFormat switch
            {
                GraphFormat.MATRIX_ADJACENCY => ReadGraphAsMatrixAdjacency(),
                GraphFormat.VECTOR_ADJACENCY => throw new NotImplementedException(),
                _ => throw new ArgumentOutOfRangeException(nameof(graphFormat), graphFormat, "Unknown format.")
            };

        public void WriteGraph(List<List<int>> graph, GraphFormat graphFormat = GraphFormat.MATRIX_ADJACENCY)
        {
            switch (graphFormat)
            {
                case GraphFormat.MATRIX_ADJACENCY:
                    WriteGraphAsMatrixAdjacency(graph);
                    break;
                case GraphFormat.VECTOR_ADJACENCY:
                    WriteGraphAsVectorAdjacency(graph);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(graphFormat), graphFormat, "Unknown format.");
            }
        }

        private List<List<int>> ReadGraphAsMatrixAdjacency()
        {
            Console.WriteLine("Введите n - количество вершин графа:");
            Console.WriteLine();
            int n = 0;
            for (; ; )
            {
                if (!int.TryParse(Console.ReadLine(), out n) || n <= 0)
                {
                    Console.WriteLine("Ожидалось целое положительное число. Повторите попытку...");
                    Console.WriteLine();
                    continue;
                }
                break;
            }

            Console.WriteLine("Введите матрицу смежности, ипользуя ' ' или ',' в качестве разделителя. Графы нумеруются с 0 вершины:");
            Console.WriteLine();

            var result = new List<List<int>>();
            for (int i = 0; i < n; i++)
            {
                for (; ; )
                {
                    var input = Console.ReadLine().Split(' ', ',', StringSplitOptions.RemoveEmptyEntries);

                    if (input.Length != n || input.Any(x => x != "0" && x != "1"))
                    {
                        Console.WriteLine($"Ошибка формата для строки {i}. Повторите ввод строки...");
                        Console.WriteLine();
                        continue;
                    }

                    result.Add(new List<int>());
                    for (int j = 0; j < input.Length; j++)
                    {
                        if (input[j] == "1")
                        {
                            result[i].Add(j);
                        }
                    }
                    break;
                }
            }

            Console.WriteLine();
            return result;
        }


        private void WriteGraphAsVectorAdjacency(List<List<int>> graph)
        {
            foreach (var list in graph)
            {
                Console.WriteLine(string.Join(", ", list));
            }
            Console.WriteLine();
        }

        private void WriteGraphAsMatrixAdjacency(List<List<int>> graph)
        {
            foreach (var list in graph)
            {
                var stringBuilder = new StringBuilder();
                for (int i = 0; i < graph.Count; i++)
                {
                    stringBuilder.Append(list.Contains(i) ? "1 " : "0 ");
                }

                Console.WriteLine(stringBuilder.ToString().Trim());
            }
            Console.WriteLine();
        }
    }

    public enum GraphFormat
    {
        MATRIX_ADJACENCY = 1,
        VECTOR_ADJACENCY = 2,
    }
}
