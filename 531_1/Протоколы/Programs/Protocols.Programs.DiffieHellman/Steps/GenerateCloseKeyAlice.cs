using Protocols.Common;
using System.Numerics;

namespace Protocols.Programs.DiffieHellman.Steps
{
    internal class GenerateCloseKeyAlice : AbstractProtocolStep<CloseKeyAlice>
    {
        public override string FileName => "СекретныйКлючАлисы.txt";

        public override string Description => $"Алиса генерирует секретный ключ - случайное натуральное число {nameof(CloseKeyAlice.CloseA)}";
    }

    internal class CloseKeyAlice
    {
        public BigInteger CloseA { get; set; }
    }
}
