using System;
using System.Collections.Generic;
using System.Linq;

namespace Generators.Programs.Mapper.Distributions
{
    internal abstract class AbstractMapper
    {
        public static AbstractMapper Create(string destr)
        {
            return destr switch
            {
                "st" => new ToStandartUniformMapper(),
                "tr" => new ToTriangularMapper(),
                "ex" => new ToCommonExponentMapper(),
                "nr" => new ToNormalMapper(),
                "gm" => new ToGammaMapper(),
                "ln" => new ToLogNormalMapper(),
                "ls" => new ToLogisticMapper(),
                "bi" => new ToBinomMapper(),
                _ => throw new ArgumentOutOfRangeException(nameof(destr), destr, null),
            };
        }

        public IEnumerable<double> Map(IEnumerable<double> source, double? p1, double? p2)
        {
            var max = source.Max();

            return MapInternal(source.Select(x => x / max), p1 ?? GenerateP1(), p2 ?? GenerateP2());
        }

        protected abstract IEnumerable<double> MapInternal(IEnumerable<double> source, double firstarameter, double secondarameter);

        protected abstract double GenerateP1();

        protected abstract double GenerateP2();
    }
}
