using CommandLine;
using System.Collections.Generic;

namespace Generators.Programs.Gen
{
    internal class CommandLineOptions
    {
        [Option('g', "GeneratorType",  Required = true, HelpText = "String with format lc|add|5p|lfsr|nfsr|mt|rc4|rsa|bbs.")]
        public string GeneratorType { get; set; }

        [Option('i', "InitialVector", Required = false, Separator = '|', Default = null, HelpText = "Initial vector (array of integer values separated via | symbol)")]
        public IEnumerable<int> InitialVector { get; set; }

        [Option('n', "NumbersCount", Required = false, Default = 10000, HelpText = "Positive integer value.")]
        public int NumbersCount { get; set; }

        [Option('m', "UpperBound", Required = false, Default = int.MaxValue, HelpText = "Positive integer value.")]
        public int MaxValue { get; set; }

        [Option('f', "OutputFile", Required = false, Default = null, HelpText = "Full file path.")]
        public string OutputFilePath { get; set; }
    }
}
