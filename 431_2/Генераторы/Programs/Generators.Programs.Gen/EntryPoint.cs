using CommandLine;
using Generators.Programs.Gen.Generators;
using System;
using System.IO;
using System.Linq;

[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("Generators.Programs.Analizer")]
namespace Generators.Programs.Gen
{
    internal class EntryPoint
    {
        static void Main(string[] args)
        {
            Parser.Default.ParseArguments<CommandLineOptions>(args)
                   .WithParsed(o =>
                   {
                       var generatorType = o.GeneratorType;

                       // TODO: validate it
                       var generator = IGenerator.Create(generatorType);

                       var result = generator.Generate(o.InitialVector, IGenerator.Rnd.Next(10, 50), o.NumbersCount, o.MaxValue);

                       if (o.Normalize)
                       {
                           var temp = result.ToArray();
                           var doubleRes = temp.Select(x => (x + .0) / (temp.Max() + 1 > 0 ? temp.Max() + 1 : int.MaxValue));

                           if (o.OutputFilePath is null)
                           {
                               Console.WriteLine($"Result: {string.Join($"{Environment.NewLine} ", doubleRes)}");
                           }
                           else
                           {
                               File.WriteAllLines(o.OutputFilePath, doubleRes.Select(x => x.ToString("0.0000")).ToArray());
                           }

                           return;
                       }

                       if (o.OutputFilePath is null)
                       {
                           Console.WriteLine($"Result: {string.Join($"{Environment.NewLine} ", result)}");
                       }
                       else
                       {
                           File.WriteAllLines(o.OutputFilePath, result.Select(x => x.ToString("0.0000")).ToArray());
                       }
                   });
        }
    }
}
