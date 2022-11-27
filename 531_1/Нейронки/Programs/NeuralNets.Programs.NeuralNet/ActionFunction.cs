namespace NeuralNets.Programs.NeuralNet
{
    class ActionFunction
    {
        public static double SigmoidFunction(double z, double c)
        {
            return 1.0 / (1.0 + Math.Exp(-(c * z)));
        }

        public static double HyperbolicTanFunction(double z, double c)
        {
            return Math.Sin(c * z) / Math.Cos(c * z);
        }

        public static double RationalSigmoidFunction(double z, double c)
        {
            return 1.0 / (c + Math.Abs(z)); ;
        }

        public static double SigmoidFunction2(double z, double c)
        {
            double x = SigmoidFunction(z, c);
            return x * (1 - x);
        }
    }
}

