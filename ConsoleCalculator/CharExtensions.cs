﻿using System.Linq;

namespace ConsoleCalculator
{
    public static class CharExtensions
    {
        public static bool IsOperator(this char symbol) => "+-/*()".IndexOf(symbol) != -1;
        public static bool IsOperator(this string token) => token.First().IsOperator();
        public static bool IsOperatorWithoutBrackets(this char symbol) => "+-/*".IndexOf(symbol) != -1;
        public static bool IsOperatorWithoutBrackets(this string token) => token.First().IsOperatorWithoutBrackets();



        public static byte GetOperationPriority(this char operation)
        {
            return operation switch
            {
                '(' => 0,
                ')' => 1,
                '+' => 2,
                '-' => 3,
                '*' => 4,
                '/' => 4,
                _ => 5
            };
        }

        public static bool IsInvalidSymbol(this char symbol)
        {
            return char.IsLetter(symbol) ||
                   char.IsSymbol(symbol) && !symbol.IsOperator() ||
                   char.IsPunctuation(symbol) && ".,".IndexOf(symbol) == -1 && !symbol.IsOperator();
        }
    }
}