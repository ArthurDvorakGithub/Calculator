using System;
using System.Collections.Generic;

namespace ConsoleCalculator
{
    internal class Tokenizer// нам нужен что бы разбить строку на токены-элементы выражения
    {
        internal static List<string> Tokenize(string input)
        {
            var result = new List<string>(); //разделенные готовые токены (оперции, числа) в коллекции
            var operationStack = new Stack<char>();
            char prevSymbol = default; // ппеременная для определения , отрицательное ли след число
            var isNegative = false; // если число отрицательное
            
            for (var i = 0; i < input.Length; i++) //идем по всем символам, что бы спарсить токены
            {
                if (char.IsDigit(input[i]))
                {
                    var currentNumber = "";

                    while (!input[i].IsOperator())
                    {
                        currentNumber += input[i];
                        i++;

                        if (i == input.Length) break;
                    }

                    if (isNegative)
                    {
                        currentNumber = $"-{currentNumber}"; //“(2+3)*6”
                        isNegative = false;
                    }
                    
                    result.Add(currentNumber);
                    i--;
                    prevSymbol = input[i];
                }

                if (!input[i].IsOperator()) continue;

                if (!char.IsDigit(prevSymbol) && !(input[i] == '(' || input[i] == ')'))
                {
                    if ((prevSymbol == '*' || prevSymbol == '/') && input[i] == '-')// нужно для нуля
                    {
                        isNegative = true;
                        continue;
                    }
                    else
                    {
                        switch (input[i])
                        {
                            case '-':
                                isNegative = true;
                                continue;
                            case '+':
                                continue;
                        }
                    }
                }
                
                switch (input[i])
                {
                    case '(':
                        operationStack.Push(input[i]);

                        break;
                    case ')':
                    {
                        if (prevSymbol == '(') // Нужно для обработки пустых скобочек //“(2+3)*6”
                                throw new SystemException();

                        var symbol = operationStack.Pop();
                        
                        while (symbol != '(')
                        {
                            result.Add(symbol.ToString());
                            symbol = operationStack.Pop();
                        }
                        break;
                    }
                    default:
                    {
                        if (operationStack.Count > 0)
                        {
                            if (input[i].GetOperationPriority() <= operationStack.Peek().GetOperationPriority())// забираем текущую операцию
                            {
                                result.Add(operationStack.Pop().ToString());
                            }
                        }
                        
                        operationStack.Push(input[i]);
                        
                        break;
                    }
                }
                
                prevSymbol = input[i];
            }

            while (operationStack.Count > 0)
            {
                result.Add(operationStack.Pop().ToString());   
            }

            return result;
        }
    }
}