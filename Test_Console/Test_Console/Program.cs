using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test_Console
{
    class Program
    {
        static void Main(string[] args)
        {



            List<int> Numbers = new List<int> { 1, 4, 5, 6, 7, 8, 6, 4, 354, 422, 3, 42, 4, 55, 2, 5, 535, 43, 524, 54, 5, 43, 45, 34, 2, 4, 324, 23, 4, 324, 3, 423, 4, 324, 32, 4, 324, };
            var matching = Numbers.Any(s => s.Equals(5));
            var numbers = Numbers.FirstOrDefault(s => s.Equals(1000));
            var distinct = Numbers.Distinct().ToList();

            Console.ReadLine();
        }
    }
}
