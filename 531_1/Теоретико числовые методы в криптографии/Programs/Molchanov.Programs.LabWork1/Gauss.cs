using System.Numerics;
using System.Text;
using Molchanov.Common;

namespace Molchanov.Programs.LabWork1
{
    internal class Gauss
    {
        private readonly BigInteger _mod = -1;
        private readonly int _height = -1;
        private readonly int _width = -1;
        private readonly BigInteger[,] _matrix;

        public Gauss(BigInteger mod, int height, int width, BigInteger[,] matrix)
        {
            _mod = mod;
            _height = height;
            _width = width;
            _matrix = matrix;
        }

        public (BigInteger[,] matrix, List<string> outputInfo) Run()
        {
            var info1 = QR();
            var (_, outputInfo) = Solve();

            var resInfo = new List<string>(info1.Concat(outputInfo));

            return (_matrix, resInfo);
        }

        private List<string> QR()
        {
            var outputInfo = new List<string>();
            for (int i = 0; i < _height; i++)
            {
                if (!MultyvectorOnInverseElement(i, outputInfo))
                    continue;

                for (int j = i + 1; j < _height; j++)
                {
                    SubstractVectorOnVector(j, i, _matrix[j, i]);
                    AddMatrixToOutputInfo(outputInfo);
                }
            }

            for (int i = _height - 1; i >= 0; i--)
            {
                if (_matrix[i, i] == 0)
                    continue;

                for (int j = i - 1; j >= 0; j--)
                {
                    SubstractVectorOnVector(j, i, _matrix[j, i]);
                    AddMatrixToOutputInfo(outputInfo);
                }
            }

            return outputInfo;
        }

        private (BigInteger[,] matrix, List<string> outputInfo) Solve()
        {
            var outputInfo = new List<string>();
            for (int i = 0; i < _height; i++)
            {
                string s = "";

                for (int j = _height; j < _width - 1; j++)
                {
                    _matrix[i, j] = (((-_matrix[i, j]) % _mod) + _mod) % _mod;

                    if (_matrix[i, j] != 0)
                    {
                        if (s.Length > 0)
                            s += " + ";

                        if (_matrix[i, j] != 1)
                            s = s + _matrix[i, j].ToString() + "*";

                        s += $"x[{j + 1}]";
                    }
                }

                if (_matrix[i, _width - 1] != 0)
                {
                    if (s.Length > 0)
                        s += " + ";

                    s += _matrix[i, _width - 1].ToString();
                }

                if (s.Length == 0)
                    s = "0";

                outputInfo.Add($"x[{i + 1}] = {s}");
            }

            outputInfo.Add(Environment.NewLine);
            return (_matrix, outputInfo);
        }

        private void SubstractVectorOnVector(int a, int b, BigInteger val)
        {
            if (val == 0)
                return;

            for (int i = 0; i < _width; i++)
                _matrix[a, i] = (((_matrix[a, i] - ((_matrix[b, i] * val) % _mod)) % _mod) + _mod) % _mod;
        }

        private bool MultyvectorOnInverseElement(int k, List<string> outputInfo)
        {
            if (_matrix[k, k] == 0)
                if (TrySwap(k))
                    AddMatrixToOutputInfo(outputInfo);

            if (_matrix[k, k] == 0)
                return false;

            BigInteger inverse_element = MathOverRing.GetInverse(_matrix[k, k], _mod);

            for (int i = k; i < _width; i++)
                _matrix[k, i] = (((_matrix[k, i] * inverse_element) % _mod) + _mod) % _mod;

            return true;
        }

        private void AddMatrixToOutputInfo(List<string> outputInfo, bool f = true)
        {
            for (int i = 0; i < _height; i++)
            {
                var sb = new StringBuilder();
                for (int j = 0; j < _width; j++)
                    sb.Append(_matrix[i, j] + " ");
                outputInfo.Add(sb.ToString());
            }

            if (f)
                outputInfo.Add(Environment.NewLine);
        }

        private bool TrySwap(int k)
        {
            for (int i = k + 1; i < _height; i++)
            {
                if (_matrix[i, k] != 0)
                {
                    for (int j = 0; j < _width; j++)
                    {
                        (_matrix[i, j], _matrix[k, j]) = (_matrix[k, j], _matrix[i, j]);
                    }

                    return true;
                }
            }

            return false;
        }
    }
}
