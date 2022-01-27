using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace GraphsTheory.Common
{
    public interface IGraphsIO
    {
        List<List<int>> ReadGraph(GraphFormat graphFormat = GraphFormat.MATRIX_ADJACENCY);

        void WriteGraph(List<List<int>> graph, GraphFormat graphFormat = GraphFormat.VECTOR_ADJACENCY);

        List<int> ReadDegreesVector();
    }

    public abstract class GraphIOAbstract : IGraphsIO
    {
        public abstract List<int> ReadDegreesVector();

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

        protected abstract List<List<int>> ReadGraphAsMatrixAdjacency();

        protected abstract List<List<int>> ReadGraphAsVectorAdjacency();

        protected abstract void WriteGraphAsMatrixAdjacency(List<List<int>> graph);

        protected abstract void WriteGraphAsVectorAdjacency(List<List<int>> graph);
    }

    public class GraphIOConsole : GraphIOAbstract
    {
        public override List<int> ReadDegreesVector()
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

        protected override List<List<int>> ReadGraphAsMatrixAdjacency()
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

        protected override List<List<int>> ReadGraphAsVectorAdjacency() => throw new NotImplementedException();

        protected override void WriteGraphAsVectorAdjacency(List<List<int>> graph)
        {
            foreach (var list in graph)
            {
                Console.WriteLine(string.Join(", ", list));
            }
            Console.WriteLine();
        }

        protected override void WriteGraphAsMatrixAdjacency(List<List<int>> graph)
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

    public class GraphIOFile : GraphIOAbstract
    {
        public string FileName { get; set; }

        public override List<int> ReadDegreesVector()
        {
            var input = File.ReadAllText(FileName).Split(' ', ',', StringSplitOptions.RemoveEmptyEntries);

            if (input.Any(x => !int.TryParse(x, out var parsed) || parsed < 0 || parsed >= input.Length))
            {
                throw new FormatException("Ожидались числа больше 0 и меньше n, где n - количество вершин графа.");
            }

            var result = input.Select(x => int.Parse(x)).ToList();

            if (!result.SequenceEqual(result.OrderByDescending(x => x)))
            {
                throw new FormatException("Степени вершин графа должны идти в порядке невозрастания.");
            }

            if (result.Sum() % 2 != 0)
            {
                throw new FormatException("Ожидались числа больше 0 и меньше n, где n - количество вершин графа.");
            }

            return input.Select(x => int.Parse(x)).ToList();
        }

        protected override List<List<int>> ReadGraphAsMatrixAdjacency()
        {
            var input = File.ReadAllLines(FileName).Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();
            var result = new List<List<int>>();

            foreach (var list in input)
            {
                var currentResult = new List<int>();
                var currentLine = list.Split(' ', ',', StringSplitOptions.RemoveEmptyEntries);

                for (int i = 0; i < currentLine.Length; i++)
                {
                    if (currentLine[i] == "1")
                    {
                        currentResult.Add(i);
                    }
                }

                result.Add(currentResult);
            }

            return result;
        }

        protected override List<List<int>> ReadGraphAsVectorAdjacency() => throw new NotImplementedException();

        protected override void WriteGraphAsMatrixAdjacency(List<List<int>> graph)
        {
            var result = new StringBuilder();
            foreach (var list in graph)
            {
                var stringBuilder = new StringBuilder();
                for (int i = 0; i < graph.Count; i++)
                {
                    stringBuilder.Append(list.Contains(i) ? "1 " : "0 ");
                }

                result.Append(stringBuilder.ToString().Trim());
                result.Append(Environment.NewLine);
            }

            File.WriteAllText(FileName, result.ToString());
        }

        protected override void WriteGraphAsVectorAdjacency(List<List<int>> graph) => throw new NotImplementedException();
    }

    public enum GraphFormat
    {
        MATRIX_ADJACENCY = 1,
        VECTOR_ADJACENCY = 2,
    }
}
