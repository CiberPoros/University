using System.Collections.Generic;
using System.IO;
using GraphsTheory.Common;

namespace GraphsTheory.Programs.BuildingGraphByDegreesVector
{
    class EntryPoint
    {
        static void Main()
        {
            var graphIO = new GraphIOFile() { FileName = PathSettings.Task_12_15_DegreesVector };
            List<int> vector = null;
            try
            {
                vector = graphIO.ReadDegreesVector();
            }
            catch (System.Exception e)
            {
                System.Console.WriteLine($"Ошибка формата входных данный. Подробности: {e.Message}");
                return;
            }

            var graph = BuildGraphByDegreesVector(vector);

            graphIO.FileName = PathSettings.Task_12_15_Graph;
            graphIO.WriteGraph(graph);

            System.Console.WriteLine($"Граф построен и сохранен в файл {Path.GetFileName(PathSettings.Task_12_15_Graph)}");
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
    }
}
