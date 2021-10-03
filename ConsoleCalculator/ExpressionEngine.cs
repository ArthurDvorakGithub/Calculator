using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleCalculator
{
    public class ExpressionEngine
    {
        public static double Calculate(string input)
        {
            try
            {
                var tokens = Tokenizer.Tokenize(input).ToArray();//2 3 + 6 *
                double result = 0;
                var temp = new Stack<double>();

                foreach (var t in tokens)
                {
                    if (double.TryParse(t, out var tempNumber))
                    {
                        temp.Push(tempNumber);
                    }
                    else if (t.FirstOrDefault().IsOperator()) // “(2+3)*6”
                    {
                        var a = temp.Pop();//-2   3 из временнного стека  temp , 6
                        var b = temp.Pop();//-567 2

                        result = t.FirstOrDefault() switch
                        {
                            '+' => b + a,
                            '-' => b - a,
                            '*' => b * a,
                            '/' => b / a,
                            _ => result
                        };
                        temp.Push(result);
                    }
                }

                return temp.Peek();
            }
            catch (Exception)
            {
                throw new SystemException();
            }
        }
    }
}