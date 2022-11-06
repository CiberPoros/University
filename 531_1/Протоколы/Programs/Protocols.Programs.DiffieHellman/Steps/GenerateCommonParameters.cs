using Protocols.Common;
using System.Numerics;

namespace Protocols.Programs.DiffieHellman.Steps
{
    internal class GenerateCommonParameters : AbstractProtocolStep<CommonParameters>
    {
        public override string FileName => "ОбщиеПараметры.txt";

        public override string Description => $"Генерация общих параметров. {nameof(CommonParameters.P)} - большое простое число, {nameof(CommonParameters.G)} - первообразный корень по модулю P";
    }

    public class CommonParameters
    {
        public BigInteger P { get; set; }
        public BigInteger G { get; set; }
    }
}
