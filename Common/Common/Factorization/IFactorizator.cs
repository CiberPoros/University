using System.Collections.Generic;
using System.Numerics;

namespace Common.Factorization
{
    public interface IFactorizator
    {
        IEnumerable<BigInteger> Factorize(BigInteger value);
    }
}
