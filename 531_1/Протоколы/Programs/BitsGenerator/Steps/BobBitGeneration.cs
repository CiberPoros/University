using Protocols.Common;
using System.Numerics;

namespace BitsGenerator.Steps
{
    internal class BobBitGeneration : AbstractProtocolStep<BitCalculatedByBob>
    {
        public override string FileName => "ПредположениеБоба.txt";

        public override string Description => $"Боб пытается угадать, какое число Алиса возводила в степень и делает предположение, вычисляя {nameof(BitCalculatedByBob.BobsGuess)}: 0 - Алиса использовала {nameof(BobRandomNumbers.H)}, 1 - Алиса использовала {nameof(BobRandomNumbers.T)}. Боб отправляет предположение Алисе";
    }

    public class BitCalculatedByBob
    {
        public BigInteger BobsGuess { get; set; }
    }
}
