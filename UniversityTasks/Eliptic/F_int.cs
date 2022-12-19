using System.Collections.Generic;
using System.Numerics;

namespace EllipsCons
{
    public class F_int
    {
        public BigInteger _val;
        public BigInteger _mod;

        public override string ToString()
        {
            return _val.ToString();
        }

        override public int GetHashCode()
        {
            return _val.GetHashCode();
        }

        override public bool Equals(object val)
        {
            if (val.GetType().ToString() != "F_int")
                return false;

            if ((val as F_int)._val == _val && (val as F_int)._mod == _mod)
                return true;
            else
                return false;
        }

        public F_int(BigInteger val, BigInteger mod)
        {
            _val = val;
            _mod = mod;
        }

        public static F_int operator +(F_int a, F_int b)
        {
            return new F_int((((a._val + b._val) % a._mod) + a._mod) % a._mod, a._mod);
        }

        public static F_int operator -(F_int a, F_int b)
        {
            return new F_int((((a._val - b._val) % a._mod) + a._mod) % a._mod, a._mod);
        }

        public static F_int operator -(F_int a)
        {
            return new F_int((((a._val) % a._mod) + a._mod) % a._mod, a._mod);
        }

        public static F_int operator *(F_int a, F_int b)
        {
            return new F_int((((a._val * b._val) % a._mod) + a._mod) % a._mod, a._mod);
        }

        public static F_int operator *(int a, F_int b)
        {
            return new F_int((((a * b._val) % b._mod) + b._mod) % b._mod, b._mod);
        }

        public static F_int operator *(BigInteger a, F_int b)
        {
            return new F_int((((a * b._val) % b._mod) + b._mod) % b._mod, b._mod);
        }

        public static F_int operator *(F_int a, int b)
        {
            return new F_int((((a._val * b) % a._mod) + a._mod) % a._mod, a._mod);
        }

        public static F_int operator /(F_int a, F_int b)
        {
            F_int b_obr = GetInverseElement(b);
            return new F_int((((a._val * b_obr._val) % a._mod) + a._mod) % a._mod, a._mod);
        }

        public static bool operator <(F_int a, F_int b)
        {
            return a._val < b._val;
        }

        public static bool operator >(F_int a, F_int b)
        {
            return a._val > b._val;
        }

        public static bool operator !=(F_int a, int b)
        {
            return a._val != b;
        }

        public static bool operator ==(F_int a, int b)
        {
            return a._val == b;
        }

        public static bool operator !=(F_int a, F_int b)
        {
            return a._val != b._val;
        }

        public static bool operator ==(F_int a, F_int b)
        {
            return a._val == b._val;
        }

        /// <summary>
        /// Находит обратный элемент в поле, вернет -1, если обратного нет
        /// </summary>      
        /// <returns></returns>
        public static F_int GetInverseElement(F_int val)
        {
            BigInteger x = 0, y = 0;
            GcdAdvanced(val._val, val._mod, ref x, ref y);
            x = ((x % val._mod) + val._mod) % val._mod;
            return new F_int(x, val._mod);
        }
        
        /// <summary>
        /// Вычисляет квадратный корень из числа val по алгоритму Шенкса за O(log^3(a))
        /// </summary>      
        /// <returns></returns>
        public static F_int Sqrt(F_int a)
        {
            F_int x = new F_int(0, a._mod);

            // step 1
            if (a._val % 4 == 3)
            {
                x._val = BigMath.Generator.FastPowOnMod(a._val, (a._mod + 1) / 4, a._mod);
                return x;
            }

            // step 2
            BigInteger s = a._mod - 1;
            BigInteger alpha = 0;

            while (s % 2 == 0)
            {
                alpha++;
                s >>= 1;
            }

            if (alpha == 1)
            {
                F_int answerr = new F_int(BigMath.Generator.FastPowOnMod(a._val, (a._mod + 1) / 4, a._mod), a._mod);
                if (answerr._mod - answerr._val < answerr._val)
                    answerr._val = answerr._mod - answerr._val;
                return answerr;
            }

            // step 3
            BigInteger n = -1;
            for (BigInteger i = 2; i < a._mod; i++)
            { 
                if (!IsQuadraticResidue(new F_int(i, a._mod)))
                {
                    n = i;
                    break;
                }
            }

            // step 4
            F_int b = new F_int(BigMath.Generator.FastPowOnMod(n, s, a._mod), a._mod);

            // step 5
            F_int r = new F_int(BigMath.Generator.FastPowOnMod(a._val, (s + 1) / 2, a._mod), a._mod);

            F_int a_obr = GetInverseElement(a);
            List<F_int> j = new List<F_int>();

            // 2^ (alpha - 2)
            BigInteger buffer = BigInteger.Pow(2, (int)alpha - 2);

            if (BigMath.Generator.FastPowOnMod((r * r * a_obr)._val, buffer, a._mod) == 1)
                j.Add(new F_int(0, a._mod));
            else
                j.Add(new F_int(1, a._mod));

            F_int summ_j = new F_int(j[0]._val, a._mod);

            BigInteger degree_2 = 1;
            for (int k = 1; k < alpha - 1; k++)
            {
                buffer /= 2;

                F_int value = new F_int(BigMath.Generator.FastPowOnMod(b._val, summ_j._val, a._mod), a._mod) * r;
                value = value * value * a_obr;
                value = new F_int(BigMath.Generator.FastPowOnMod(value._val, buffer, a._mod), a._mod);

                if (value._val == 1)
                    j.Add(new F_int(0, a._mod));
                else
                    j.Add(new F_int(1, a._mod));

                degree_2 <<= 1;
                summ_j = summ_j + new F_int(j[k]._val, a._mod) * new F_int(degree_2, a._mod); 
            }

            F_int answer = new F_int(BigMath.Generator.FastPowOnMod(b._val, summ_j._val, a._mod), a._mod) * r;
            if (answer._mod - answer._val < answer._val)
                answer._val = answer._mod - answer._val;
            return answer;
        }

        /// <summary>
        /// Проверяет, является ли число квадратичным вычетом по простому модулю. На основе критерия Эйлера
        /// </summary>      
        /// <returns> True, если число является квадратичным вычетом </returns>
        public static bool IsQuadraticResidue(F_int a)
        {
            BigInteger val = BigMath.Generator.FastPowOnMod(a._val, (a._mod - 1) / 2, a._mod);
            if (val == 1)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Расширенный алгоритм Евклида
        /// </summary>
        /// <returns></returns>
        private static BigInteger GcdAdvanced(BigInteger a, BigInteger b, ref BigInteger x, ref BigInteger y)
        {
            if (a == 0)
            {
                x = 0; y = 1;
                return b;
            }

            BigInteger x1 = 0, y1 = 0;
            BigInteger d = GcdAdvanced(b % a, a, ref x1, ref y1);
            x = y1 - (b / a) * x1;
            y = x1;
            return d;
        }
    }
}
