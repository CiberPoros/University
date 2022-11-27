﻿using Protocols.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace BitsGenerator.Steps
{
    internal class GenerateCommonParameters : AbstractProtocolStep<CommonParameters>
    {
        public override string FileName => "ОбщиеПараметры.txt";

        public override string Description => $"Генерация общих параметров. {nameof(CommonParameters.P)} - большое простое число такое, что {nameof(CommonParameters.P)} - 1 имеет имеет большой простой делитель. {nameof(CommonParameters.Divider)} - большой простой делитель {nameof(CommonParameters.P)} - 1";
    }

    public class CommonParameters
    {
        public BigInteger P { get; set; }
        public BigInteger Divider { get; set; }
    }
}
