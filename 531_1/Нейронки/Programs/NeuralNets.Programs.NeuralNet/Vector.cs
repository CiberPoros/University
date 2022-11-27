namespace NeuralNets.Programs.NeuralNet
{
    internal class Vector
    {
        public static List<List<List<double>>> ReadNetowrk(string input)
        {
            var res = new List<List<List<double>>>();
            var file = new FileStream(input, FileMode.Open);
            var read = new StreamReader(file);

            while (!read.EndOfStream)
            {
                var matrix = new List<List<double>>();

                var line = read.ReadLine()!
                                    .Replace("],", "|")
                                    .Replace("[", "")
                                    .Replace("]", "")
                                    .Split('|');

                foreach (var data in line)
                {
                    var tempData = data.Split(',');
                    matrix.Add(new List<double>());
                    foreach (string doub in tempData)
                    {
                        matrix[^1].Add(Convert.ToDouble(doub.Replace('.', ',')));
                    }
                }

                res.Add(matrix);
            }

            read.Close();
            file.Close();

            return res;
        }

        public static List<double> ReadVector(string input)
        {
            var res = new List<double>();
            var file = new FileStream(input, FileMode.Open);
            var read = new StreamReader(file);

            var temp = read.ReadLine()!.Split(' ');

            read.Close();
            file.Close();

            foreach (string s in temp)
            {
                res.Add(Convert.ToDouble(s.Replace('.', ',')));
            }

            return res;
        }

        public static void Write(string s, string output)
        {
            var file = new FileStream(output, FileMode.Create);
            var write = new StreamWriter(file);

            write.Write(s);
            write.Close();
            file.Close();
        }
    }
}

