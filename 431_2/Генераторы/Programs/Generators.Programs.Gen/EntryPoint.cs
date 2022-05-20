using CommandLine;
using Generators.Programs.Gen.Generators;
using System;
using System.IO;

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

                       if (o.OutputFilePath is null)
                       {
                           Console.WriteLine($"Result: {string.Join($"{Environment.NewLine} ", result)}");
                       }
                       else
                       {
                           File.WriteAllText(o.OutputFilePath, string.Join(", ", result));
                       }
                   });
        }
    }
}
