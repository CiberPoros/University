using NeuralNets.Common;
using System.Text;

namespace NeuralNets.Programs.FunctionCalculator
{
    internal class Program
    {
        private string? _input1 = null;
        private string? _input2 = null;
        private string? _output1 = null;
        private string? _output2 = null;

        static async Task Main(string[] args)
        {
            var program = new Program();
            program.ParseParams(args);

            if (program._input1 is null)
            {
                Console.WriteLine("Обязательный параметр input1 не задан!");
                return;
            }

            if (program._input2 is null)
            {
                Console.WriteLine("Обязательный параметр input2 не задан!");
                return;
            }

            if (program._output1 is null)
            {
                Console.WriteLine("Обязательный параметр output1 не задан!");
                return;
            }

            var description = File.ReadAllText(program._input1);
            var (success, error) = Graph.TryCreateByStringDescription(description, out var graph);
            if (!success)
            {
                Console.WriteLine($"Ошибка в формате файла {program._input1}. Подробности: {error}");
                return;
            }

            var operationDescriptions = (await File.ReadAllLinesAsync(program._input2)).Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();
            (success, error) = graph.AddOperations(operationDescriptions);
            if (!success)
            {
                Console.WriteLine($"Ошибка в формате файла {program._input2}. Подробности: {error}");
                return;
            }

            var bracketResult = graph.ToBracketSequence();
            graph.CalculateValues(bracketResult.Select(x => x.stock));

            var sb = new StringBuilder();
            foreach (var (stock, result) in bracketResult)
            {
                sb.Append(result);
                sb.Append('=');
                sb.Append(stock.Value!.Value.ToString("0.0000"));
                sb.Append(' ');
            }

            await File.WriteAllTextAsync(program._output1, sb.ToString());
            Console.WriteLine($"Вычисления произведены, результат записан в файл {program._output1}");
        }

        private void ParseParams(string[] args)
        {
            foreach (var arg in args)
            {
                var splited = arg.Split('=').Where(x => !string.IsNullOrEmpty(x)).Select(x => x.Trim().ToLower()).ToArray();

                if (splited.Length != 2)
                {
                    Console.WriteLine($"Неизвестный параметр {arg} был пропущен.");
                    continue;
                }

                switch (splited[0])
                {
                    case "input1":
                        _input1 = splited[1];
                        break;
                    case "input2":
                        _input2 = splited[1];
                        break;
                    case "output1":
                        _output1 = splited[1];
                        break;
                    case "output2":
                        _output2 = splited[1];
                        break;
                    default:
                        Console.WriteLine($"Неизвестный параметр {arg} был пропущен.");
                        continue;
                }
            }
        }
    }
}
