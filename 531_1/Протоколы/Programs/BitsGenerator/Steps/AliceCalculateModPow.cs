using Protocols.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace BitsGenerator.Steps
{
    internal class AliceCalculateModPow : AbstractProtocolStep<AliceRandomModPowNumber>
    {
        public override string FileName => "ВозведенноеВСтепеньАлисойЧисло.txt";

        public override string Description => $"Алиса вычисляет {nameof(AliceRandomModPowNumber.Y)} = {nameof(BobRandomNumbers.H)}^{nameof(AliceRandomNumber.X)} mod {nameof(CommonParameters.P)} или {nameof(AliceRandomModPowNumber.Y)} = {nameof(BobRandomNumbers.T)}^{nameof(AliceRandomNumber.X)} mod {nameof(CommonParameters.P)}. Отправляет число Бобу";
    }

    public class AliceRandomModPowNumber
    {
        public BigInteger Y { get; set; }
        public string UsedValue { get; set; }
    }
}
