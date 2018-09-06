using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BarnardTechnology;

namespace TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            WebConsole wC = new WebConsole();
            Console.WriteLine("Test console!");
            while(true)
            {
                Console.ReadKey(false);
            }
        }
    }
}
