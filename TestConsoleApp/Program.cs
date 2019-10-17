using BusinessLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var inCaps = new List<InCap>()
            {
                new InCap(){ ID = "T1", Participation = 1.0M/3 },
                new InCap(){ ID = "T4", Participation = 1.0M/4 }
            };

            var outCaps = CapOperations.NormalDistribution(100, inCaps);

            foreach (var item in outCaps)
                Console.WriteLine($"ID: {item.ID}, Quantity: {item.Quantity}");
            Console.ReadKey(); 
        }
    }
}
