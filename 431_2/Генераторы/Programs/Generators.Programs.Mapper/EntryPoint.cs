using CommandLine;
using Generators.Programs.Mapper.Distributions;
using System;
using System.IO;
using System.Linq;
using System.Text;

namespace Generators.Programs.Mapper
{
    internal class EntryPoint
    {
        static void Main(string[] args)
        {
            Parser.Default.ParseArguments<CommandLineOptions>(args)
                   .WithParsed(o =>
                   {
                       var numbers = File.ReadAllText(o.FileName)
                            .Split(' ', ',')
                            .Where(x => !string.IsNullOrEmpty(x))
                            .Select(x => double.Parse(x))
                            .ToList();

                       var distrType = o.Distribution;

                       var mapper = AbstractMapper.Create(distrType);

                       var result = mapper.Map(numbers, o.P1, o.P2);

                       var stringBuilder = new StringBuilder();
                       Console.WriteLine("Результат: ");
                       foreach (var currentRes in result)
                       {
                           Console.WriteLine($"{currentRes:0.00} ");
                           stringBuilder.Append($"{currentRes:0.00}{Environment.NewLine}");
                       }

                       Console.WriteLine();

                       File.WriteAllText($"MappingResult_{distrType}.txt", stringBuilder.ToString());
                   });
        }
    }
}
