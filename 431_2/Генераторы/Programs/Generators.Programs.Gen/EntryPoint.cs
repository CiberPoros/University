using CommandLine;
using Generators.Programs.Gen.Generators;
using System;
using System.IO;

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

                       var generator = IGenerator.Create(generatorType);

                       var result = generator.Generate(o.InitialVector, IGenerator.Rnd.Next(), o.NumbersCount, o.MaxValue);

                       if (o.OutputFilePath is null)
                       {
                           Console.WriteLine($"Result: {string.Join(", ", result)}");
                       }
                       else
                       {
                           File.WriteAllText(o.OutputFilePath, string.Join(", ", result));
                       }
                   });
        }
    }
}
