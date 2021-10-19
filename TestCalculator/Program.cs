using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using ConsoleCalculator;

namespace TestCalculator
{
    class Program
    {
        static void Main(string[] args)
        {
            //while (true)
            //{
            //    try
            //    {
            //        Console.Write("Введите выражение: ");
            //        Console.WriteLine(ExpressionEngine.Calculate(Console.ReadLine()));
            //    }
            //    catch (Exception e)
            //    {
            //        Console.WriteLine(e);
            //    }
            //}

            PerformanceTest();
        }

        private static void PerformanceTest()
        {
            var sw = new Stopwatch();
            sw.Start();
            
            var expr = "10*4+(2+8)+10*4+(2+8)"; // 21 symbol  
            var sb = new StringBuilder(expr);
            
            // 21 * 48 000 = 1 008 000 symbols
            for (var i = 0; i < 48000; i++)
            {
                sb.AppendJoin('+', expr);
            }
            
            using (var steamWriter = new StreamWriter(@"test.txt"))
            {
                steamWriter.Write(sb.ToString());
            }
            
            var input = "";
            var path = Path.Combine(Directory.GetCurrentDirectory(), "test.txt");
            using (var sr = new StreamReader(path))
            {
                input = sr.ReadToEnd();
            }

            var actual = ExpressionEngine.Calculate(input);
            sw.Stop();

            Console.WriteLine();
            Console.WriteLine($"Result: {actual}" + Environment.NewLine +
                              $"Execution time: {sw.ElapsedMilliseconds} ms");
        }
    }
}