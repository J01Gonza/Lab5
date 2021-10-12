using System;
using Parte1;

namespace Parte2
{
    class Program
    {
        static void Main(string[] args)
        {
            int[] P10 = { 3, 7, 5, 1, 2, 9, 10, 4, 8, 6 };
            int[] P8 = { 10, 4, 2, 8, 3, 9, 1, 6 };
            string mainKey = Convert.ToString(364, 2).PadLeft(10, '0');
            SDES sdes = new ImplementationClass();
            var keys = sdes.generateKey(mainKey, P10, P8);
            int[] P4 = { 3, 1, 4, 2 };
            int[] EP = { 2, 1, 3, 4, 3, 2, 4, 1 };
            int[] IP = { 4, 1, 7, 2, 8, 3, 5, 6 };
            int[] IP1 = { 2, 4, 6, 1, 7, 8, 3, 5 };
            mainKey = Convert.ToString(211, 2).PadLeft(8, '0');
            byte x = sdes.Enconde(mainKey, keys.key1, keys.key2, P4, EP, IP, IP1);
            mainKey = Convert.ToString(x, 2).PadLeft(8, '0');
            byte y = sdes.Enconde(mainKey, keys.key2, keys.key1, P4, EP, IP, IP1);
            Console.WriteLine("Hello World!");
        }
    }
}
