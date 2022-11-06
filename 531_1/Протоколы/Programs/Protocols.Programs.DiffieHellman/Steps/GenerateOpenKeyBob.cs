using Protocols.Common;
using System.Numerics;

namespace Protocols.Programs.DiffieHellman.Steps
{
    internal class GenerateOpenKeyBob : AbstractProtocolStep<OpenKeyBob>
    {
        public override string FileName => "ОткрытыйКлючБоба.txt";

        public override string Description => $"Вычисляет {nameof(OpenKeyBob.OpenB)} = {nameof(CommonParameters.G)} ^ {nameof(CloseKeyBob.CloseB)} mod {nameof(CommonParameters.P)}. Отправляет полученное значение Алисе";
    }

    internal class OpenKeyBob
    {
        public BigInteger OpenB { get; set; }
    }
}
