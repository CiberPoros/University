namespace NeuralNets.Programs.NeuralNet
{
    internal class Matrix
    {
        public List<double> State { get; set; }
        public Network NeuralNetwork { get; set; }
        public ActionFunction ActivationFunction { get; set; }
        public int Constant { get; set; }
        public List<double> Ss { get; set; }
        public List<List<double>> LayerOutputs { get; set; }

        public Matrix(Network neuralNetwork, int constant, List<double> inputVector)
        {
            NeuralNetwork = neuralNetwork;
            State = inputVector;
            Constant = constant;
            Ss = new List<double>();
            LayerOutputs = new List<List<double>>
            {
                inputVector
            };
        }


        public List<double> Evaluate()
        {
            int curStateLength;
            foreach (var layer in NeuralNetwork.Neurons)
            {
                curStateLength = State.Count;
                var nextState = new List<double>();

                for (int i = 0; i < layer.Count; i++)
                {
                    if (layer[i].Count != curStateLength)
                    {
                        Console.WriteLine("Длина входа не совпадает с длиной строки матрицы");
                    }

                    double sum = 0;
                    for (int j = 0; j < layer[i].Count; j++)
                    {
                        sum += layer[i][j] * State[j];
                    }

                    Ss.Add(sum);
                    nextState.Add(ActionFunction.HyperbolicTanFunction(sum, Constant));
                }

                LayerOutputs.Add(nextState);
                State = nextState;
            }

            return State;
        }
    }
}
