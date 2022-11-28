using System;
using System.Threading.Tasks;

namespace Test
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Test started work!");
            for (int i = 0; ; i++)
            {
                Console.WriteLine($"Work iteration {i}");
                Task.Delay(TimeSpan.FromSeconds(1)).GetAwaiter().GetResult();
            }
        }
    }
}
