using System.Collections.Generic;
using System.Linq;

namespace GraphsGenerator.ChromaticNumber
{
    internal class ChromaticNumberCalculator
    {
        public int GetChromaticNumber(Graph graph)
        {
            for (var color = 2; ; color++)
            {
                if (CanColorize(graph, color))
                {
                    return color;
                }
            }
        }

        private bool CanColorize(Graph graph, int colorsCount)
        {
            var vector = graph.AdjacencyVector;

            var colors = new int[vector.Count];

            var notColored = new LinkedList<int>();
            for (int i = 0; i < vector.Count; i++)
            {
                notColored.AddLast(i);
            }

            return TryColorizeNotColoredVertexes(vector, colors, colorsCount, notColored);
        }

        private (int maxDegreeVertex, int maxDegree) GetMaxDegreeVertex(List<int> adjacencyVector)
        {
            var maxDegreeVertex = -1;
            var maxDegree = -1;

            for (var from = 0; from < adjacencyVector.Count; from++)
            {
                var currentDegree = 0;
                for (int to = 0, toMask = 1; to < adjacencyVector.Count; to++, toMask <<= 1)
                {
                    if ((adjacencyVector[from] & toMask) > 0)
                    {
                        currentDegree++;
                    }
                }

                if (currentDegree > maxDegree)
                {
                    maxDegree = currentDegree;
                    maxDegreeVertex = from;
                }
            }

            return (maxDegreeVertex, maxDegree);
        }

        private void ColorizeMaxVertex(List<int> adjacencyVector, int maxDegreeVertex, int[] colors)
        {
            colors[maxDegreeVertex] = 1;

            int color = 2;
            for (int to = 0, toMask = 1; to < adjacencyVector.Count; to++, toMask <<= 1)
            {
                if ((adjacencyVector[maxDegreeVertex] & toMask) > 0)
                {
                    colors[to] = color;
                    color++;
                }
            }
        }

        private bool TryColorizeNotColoredVertexes(List<int> adjacencyVector, int[] colors, int colorsCount, LinkedList<int> notColored)
        {
            if (!notColored.Any())
            {
                return true;
            }

            var from = notColored.Last.Value;
            var freeColorsMask = (1 << (colorsCount)) - 1;

            for (int to = 0, toMask = 1; to < adjacencyVector.Count; to++, toMask <<= 1)
            {
                if ((adjacencyVector[from] & toMask) > 0)
                {
                    var color = colors[to];

                    if (color == 0)
                    {
                        continue;
                    }

                    if ((freeColorsMask & (1 << (color - 1))) > 0)
                    {
                        freeColorsMask ^= 1 << (color - 1);
                    }     
                }
            }

            if (freeColorsMask == 0)
                return false;

            for (int color = 1, colorMask = 1; color <= colorsCount; color++, colorMask <<= 1)
            {
                if ((freeColorsMask & colorMask) > 0)
                {
                    colors[from] = color;
                    notColored.RemoveLast();

                    var colorizeResult = TryColorizeNotColoredVertexes(adjacencyVector, colors, colorsCount, notColored);

                    notColored.AddLast(from);
                    colors[from] = 0;

                    if (colorizeResult)
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
