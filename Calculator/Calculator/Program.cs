using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator
{
    class Program
    {
        static void Main(string[] args)
        {
            // Command line interface for using Calculator class
            //
            // expects 0 or 1 argument
            //
            // If run without argument, or with more than one, show usage description
            // If run with one argument, use argument as string paramater input to Calculator class Evaluate() function, displaying result or error message.
            
            if (args.Count() == 1)
            {
                bool success = false;
                string message = "unknown";
                int result = Calculator.CalculateNoThrow(args[0], out success, out message);
                if (success)
                {
                    Console.WriteLine(string.Format("Result is {0}", result));
                }
                else
                {
                    Console.WriteLine(string.Format("Failed: Error reported as {0}", message));
                }
            }
            else
            {
                ShowUsage();
            }
        }

        static void ShowUsage()
        {
            Console.WriteLine("Calculator.exe - simple integer expression evaluator command line interface.");
            Console.WriteLine("Usage: Calculator [expression]");
            Console.WriteLine("Result: displays the result of computing the given value for expression");
            Console.WriteLine("expression is a simple arithmetic sequence of numbers separated by operators.");
            Console.WriteLine("Available operators are:");
            Console.WriteLine("\t+ (addition)");
            Console.WriteLine("\t- (subtraction)");
            Console.WriteLine("\t* (multiplication)");
            Console.WriteLine("\t/ (division)");
            Console.WriteLine("\t^ (exponention)");
            Console.WriteLine();
            Console.WriteLine("Order of perecedence is:");
            Console.WriteLine("1) exponention left to right");
            Console.WriteLine("2) multiplication and division left to right");
            Console.WriteLine("3) addition and subreaction left to right");
            Console.WriteLine("Examples:");
            Console.WriteLine("calculator 5+5+32");
            Console.WriteLine("calculator \"3 * 4\"");
        }
    }
}
