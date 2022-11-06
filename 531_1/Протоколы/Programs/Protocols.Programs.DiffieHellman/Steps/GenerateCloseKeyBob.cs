using Protocols.Common;
using System.Numerics;

namespace Protocols.Programs.DiffieHellman.Steps
{
    internal class GenerateCloseKeyBob : AbstractProtocolStep<CloseKeyBob>
    {
        public override string FileName => "СекретныйКлючБоба.txt";

        public override string Description => $"Боб генерирует секретный ключ - случайное натуральное число {nameof(CloseKeyBob.CloseB)}";
    }

    internal class CloseKeyBob
    {
        public BigInteger CloseB { get; set; }
    }
}
