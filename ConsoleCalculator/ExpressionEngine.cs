﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace ConsoleCalculator
{
    public class ExpressionEngine
    {
        public static double Calculate(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                throw new NullReferenceException("Input data was null");
            
            try
            {
                var tokens = Tokenizer.Tokenize(input).ToArray();
                double result = 0;
                var temp = new Stack<double>();

                foreach (var t in tokens)
                {
                    if (DoubleExtensions.TryParse(t, out var tempNumber))
                    {
                        temp.Push(tempNumber);
                    }
                    else if (t.IsOperator())
                    {
                        var a = temp.Pop();
                        var b = temp.Pop();

                        result = t.FirstOrDefault() switch
                        {
                            '+' => b + a,
                            '-' => b - a,
                            '*' => b * a,
                            '/' => b / a,
                            _ => result
                        };

                        if (double.IsInfinity(result) || double.IsNaN(result))
                            throw new DivideByZeroException("Divide by zero isn't impossible");
                        
                        temp.Push(result);
                    }
                }

                return temp.Peek();
            }
            catch (InvalidOperationException)
            {
                throw new SystemException("Invalid input data. Retry to enter...");
            }
        }
    }
}