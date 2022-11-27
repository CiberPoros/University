namespace NeuralNets.Programs.NeuralNet
{
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

            if (program._output2 is null)
            {
                Console.WriteLine("Обязательный параметр output1 не задан!");
                return;
            }

            var n = new Network(program._input1);
            var v = Vector.ReadVector(program._input2);

            n.Serialize(program._output1);

            var worker = new Matrix(n, 1, v);
            var res = worker.Evaluate();
            var sres = string.Empty;

            foreach (double t in res)
            {
                sres += ", " + t.ToString();
            }

            sres = sres[2..];
            Console.WriteLine(sres);
            Vector.Write(sres, program._output2);
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