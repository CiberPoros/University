using Protocols.Common;
using System.Numerics;

namespace BitsGenerator.Steps
{
    internal class AliceShareResult : AbstractProtocolStep<ResultBit>
    {
        public override string FileName => "СлучайныйБит.txt";

        public override string Description => $"Алиса объявляет результат {nameof(ResultBit.Bit)}. Если догадка Боба верна, то {nameof(ResultBit.Bit)} = 1 (орел), иначе {nameof(ResultBit.Bit)} = 0 (решка)";
    }

    public class ResultBit
    {
        public BigInteger Bit { get; set; }
    }
}
