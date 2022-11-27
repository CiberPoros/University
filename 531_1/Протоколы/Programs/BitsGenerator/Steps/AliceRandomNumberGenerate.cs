using Protocols.Common;
using System.Numerics;

namespace BitsGenerator.Steps
{
    internal class AliceRandomNumberGenerate : AbstractProtocolStep<AliceRandomNumber>
    {
        public override string FileName => "СлучайноеЧислоАлисы.txt";

        public override string Description => $"Алиса проверяет, что {nameof(BobRandomNumbers.H)} и {nameof(BobRandomNumbers.T)} - примитивные элементы в GF({nameof(CommonParameters.P)}), затем генерирует случайное число {nameof(AliceRandomNumber.X)}, взаимнопростое с {nameof(CommonParameters.P)} - 1";
    }

    public class AliceRandomNumber
    {
        public BigInteger X { get; set; }
    }
}
