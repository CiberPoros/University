using Protocols.Common;

namespace BitsGenerator.Steps
{
    internal class BobCheckHonest : AbstractProtocolStep<BobCheckHonestMock>
    {
        public override string FileName => "None";

        public override string Description => $"Алиса раскрывает Бобу значение {nameof(AliceRandomNumber.X)}. Боб вычисляет H^X mod P и T^X mod P, проверяя, что Алиса играла честно. Боб проверяет, что X и P-1 - взаимнопросты";
    }

    public class BobCheckHonestMock
    {
    }
}
