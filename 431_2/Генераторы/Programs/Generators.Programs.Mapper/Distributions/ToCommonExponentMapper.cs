﻿using System;
using System.Collections.Generic;

namespace Generators.Programs.Mapper.Distributions
{
    /// <summary>
    /// Общее экспоненциальное распределение
    /// </summary>
    internal class ToCommonExponentMapper : AbstractMapper
    {
        // a
        protected override double GenerateP1()
        {
            return 0d;
        }

        // b
        protected override double GenerateP2()
        {
            return 1d;
        }

        protected override IEnumerable<double> MapInternal(IEnumerable<double> source, double a, double b)
        {
            foreach (var val in source)
            {
                yield return (-b) * Math.Log(val) + a;
            }
        }
    }
}
