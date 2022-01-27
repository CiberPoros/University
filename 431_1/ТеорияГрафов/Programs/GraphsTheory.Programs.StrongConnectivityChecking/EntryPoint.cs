using System;
using System.Collections.Generic;
using System.Linq;
using GraphsTheory.Common;

namespace GraphsTheory.Programs.StrongConnectivityChecking
{
    class EntryPoint
    {
        static void Main()
        {
            var graphIO = new GraphIOFile() { FileName = PathSettings.Task_12_9_Graph };
            var graph = graphIO.ReadGraph();

            var isStrongConnected = IsStrongConnected(graph);

            Console.WriteLine(isStrongConnected
                ? "Граф является сильно связным"
                : "Граф не является сильно связным");
            Console.WriteLine();
        }

        private static bool IsStrongConnected(List<List<int>> graph)
        {
            for (int i = 0; i < graph.Count; i++)
            {
                var used = new bool[graph.Count];
                used[i] = true;
                DFS(graph, used, i);

                if (used.Contains(false))
                {
                    return false;
                }
            }

            return true;
        }

        private static void DFS(List<List<int>> graph, bool[] used, int previousVertex)
        {
            used[previousVertex] = true;
            foreach (var nextVertex in graph[previousVertex])
            {
                if (!used[nextVertex])
                {
                    DFS(graph, used, nextVertex);
                }
            }
        }
    }
}
