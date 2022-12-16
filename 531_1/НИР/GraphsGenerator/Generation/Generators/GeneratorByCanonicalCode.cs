﻿using System.Collections.Generic;

namespace GraphsGenerator
{
    public class GeneratorByCanonicalCode : IGenerator
    {
        public IEnumerable<string> Generate(int vertexCount)
        {
            var bitsCount = (int)(((vertexCount + .0) / 2) * (vertexCount - 1));
            long limit = 1L << (bitsCount);
            for (var i = 0L; i < limit; i++)
            {
                var vector = GenerationUtils.ToAjentityVector(i, vertexCount);
                var graph = new Graph(vector);

                if (!ConnectivityChecker.IsConnected(graph))
                {
                    continue;
                }

                var canonicalCode = IsomorphismChecker.GetCon(graph);
                var simpleCode = GenerationUtils.GetBigIntegerSimpleCode(graph);

                if (canonicalCode == simpleCode)
                {
                    yield return graph.ToG6();
                }
            }
        }
    }
}
