using Protocols.Common;
using System.Numerics;

namespace BitsGenerator.Steps
{
    internal class GenerateCommonParameters : AbstractProtocolStep<CommonParameters>
    {
        public override string FileName => "ОбщиеПараметры.txt";

        public override string Description => $"Генерация общих параметров. {nameof(CommonParameters.P)} - большое простое число такое, что {nameof(CommonParameters.P)} - 1 имеет имеет большой простой делитель. {nameof(CommonParameters.UniqueDividers)} - список уникальных делителей {nameof(CommonParameters.P)} - 1";
    }

    public class CommonParameters
    {
        public BigInteger P { get; set; }
        public IList<BigInteger> UniqueDividers { get; set; }
    }
}
