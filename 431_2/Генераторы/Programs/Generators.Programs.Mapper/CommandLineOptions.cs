using CommandLine;
using System.Collections.Generic;

namespace Generators.Programs.Mapper
{
    internal class CommandLineOptions
    {
        [Option('d', "Distribution", Required = true, HelpText = "String with format st|tr|ex|nr|gm|ln|ls|bi.")]
        public string Distribution { get; set; }

        [Option('f', "FileName", Required = false, Default = "Random.txt", HelpText = "FileName")]
        public string FileName { get; set; }

        [Option('a', "Parameter1", Required = false, Default = null, HelpText = "First parameter")]
        public double? P1 { get; set; }

        [Option('b', "Parameter2", Required = false, Default = null, HelpText = "First parameter")]
        public double? P2 { get; set; }
    }
}
