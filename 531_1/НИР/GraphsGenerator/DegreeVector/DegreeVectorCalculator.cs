using System.Collections.Generic;
using System.Linq;

namespace GraphsGenerator.DegreeVector
{
    internal class DegreeVectorCalculator
    {
        public List<int> GetDegreeVector(Graph graph)
        {
            var vector = graph.AdjacencyVector;
            var result = new int[vector.Count];

            for (int i = 0; i < vector.Count; i++)
            {
                for (int j = 0, jMask = 1; j < vector.Count; j++, jMask <<= 1)
                {
                    if ((vector[i] & jMask) > 0)
                    {
                        result[i]++;
                    }
                }
            }

            return result.OrderByDescending(x => x).ToList();
        }
    }
}
