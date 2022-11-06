using Protocols.Common;
using System.Numerics;

namespace Protocols.Programs.DiffieHellman.Steps
{
    internal class GenerateOpenKeyAlice : AbstractProtocolStep<OpenKeyAlice>
    {
        public override string FileName => "ОткрытыйКлючАлисы.txt";

        public override string Description => $"Вычисляет {nameof(OpenKeyAlice.OpenA)} = {nameof(CommonParameters.G)} ^ {nameof(CloseKeyAlice.CloseA)} mod {nameof(CommonParameters.P)}. Отправляет полученное значение Бобу";
    }

    internal class OpenKeyAlice
    {
        public BigInteger OpenA { get; set; }
    }
}
