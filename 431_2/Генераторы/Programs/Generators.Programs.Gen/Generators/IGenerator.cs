﻿using System;
using System.Collections.Generic;

namespace Generators.Programs.Gen.Generators
{
    internal interface IGenerator
    {
        public static Random Rnd = new();

        IEnumerable<int> Generate(IEnumerable<int> initialVector, int seed, int numbersCount, int maxValue);

        IEnumerable<int> GetDefaultParameters();

        public static IGenerator Create(string generatorType)
        {
            return generatorType switch
            {
                "lc" => new LinearCongruentGenerator(),
                "add" => new AdditiveGenerator(),
                "5p" => new FiveParamsGenerator(),
                "nfsr" => new NFSRGenerator(),
                "lfsr" => new LFSRGenerator(),
                "mt" => new MersenneVortexGenerator(),
                "rc4" => new RC4Generator(),
                "rsa" => new RSAGenerator(),
                "bbs" => new BbsGenerator(),
                _ => throw new ArgumentOutOfRangeException(nameof(generatorType), "Value must be only lc|add|5p|lfsr|nfsr|mt|rc4|rsa|bbs.")
            };
        }
    }
}
