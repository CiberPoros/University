using Protocols.Common;
using System.Numerics;

namespace Protocols.Programs.DiffieHellman.Steps
{
    internal class GenerateCommonSecretKeyByAlice : AbstractProtocolStep<CommonSecretKeyByAlice>
    {
        public override string FileName => "ОбщийСекретныйКлючВычисленныйАлисой.txt";

        public override string Description => $"Алиса вычисляет общий секретный ключ {nameof(CommonSecretKeyByAlice.K)} = {nameof(OpenKeyBob.OpenB)} ^ {nameof(CloseKeyAlice.CloseA)} mod {nameof(CommonParameters.P)}";
    }

    internal class CommonSecretKeyByAlice
    {
        public BigInteger K { get; set; }
    }
}
