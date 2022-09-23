using NeuralNets.Programs.GraphSerializer;
using System.Text.Json;

internal class Program
{
    private string? _input1 = null;
    private string? _input2 = null;
    private string? _output1 = null;
    private string? _output2 = null;

    static void Main(string[] args)
    {
        var program = new Program();
        program.ParseParams(args);

        if (program._input1 is null)
        {
            Console.WriteLine("Обязательный параметр input1 не задан!");
            return;
        }

        if (program._output1 is null)
        {
            Console.WriteLine("Обязательный параметр output1 не задан!");
            return;
        }

        var description = File.ReadAllText(program._input1);

        if (!Graph.TryCreateByStringDescription(description, out var graph))
        {
            Console.WriteLine($"Ошибка в формате файла {program._input1}");
            return;
        }

        string jsonString = JsonSerializer.Serialize(graph, new JsonSerializerOptions() { WriteIndented = true });

        File.WriteAllText(program._output1, jsonString);
        Console.WriteLine($"Граф сериализован и записан в файл {program._output1}");
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