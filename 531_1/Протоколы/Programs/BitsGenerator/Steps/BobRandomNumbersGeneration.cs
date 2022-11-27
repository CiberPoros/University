using Protocols.Common;
using System.Numerics;

namespace BitsGenerator.Steps
{
    internal class BobRandomNumbersGeneration : AbstractProtocolStep<BobRandomNumbers>
    {
        public override string FileName => "СлучайныеЧислаБоба.txt";

        public override string Description => $"Боб генерирует случайные числа. {nameof(BobRandomNumbers.H)} - случайный примитивный элемент в GF(P), {nameof(BobRandomNumbers.T)} - случайный примитивный элемент в GF(P). Отправляет числа Алисе";
    }

    public class BobRandomNumbers
    {
        public BigInteger H { get; set; }
        public BigInteger T { get; set; }
    }
}
