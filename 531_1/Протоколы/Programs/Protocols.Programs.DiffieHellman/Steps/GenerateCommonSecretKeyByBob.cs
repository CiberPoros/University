using Protocols.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Protocols.Programs.DiffieHellman.Steps
{
    internal class GenerateCommonSecretKeyByBob : AbstractProtocolStep<CommonSecretKeyByBob>
    {
        public override string FileName => "ОбщийСекретныйКлючВычисленныйБобом.txt";

        public override string Description => $"Боб вычисляет общий секретный ключ {nameof(CommonSecretKeyByAlice.K)} = {nameof(OpenKeyAlice.OpenA)} ^ {nameof(CloseKeyBob.CloseB)} mod {nameof(CommonParameters.P)}";
    }

    internal class CommonSecretKeyByBob
    {
        public BigInteger K { get; set; }
    }
}
