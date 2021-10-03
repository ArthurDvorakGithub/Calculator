using System;
using System.Collections.Generic;

namespace ConsoleCalculator
{
    internal class Tokenizer
    {
        internal static List<string> Tokenize(string input)
        {
            var result = new List<string>(); 
            var operationStack = new Stack<char>();
            char prevSymbol = default; 
            var isNegative = false; 
            
            for (var i = 0; i < input.Length; i++) 
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
                        currentNumber = $"-{currentNumber}"; 
                        isNegative = false;
                    }
                    
                    result.Add(currentNumber);
                    i--;
                    prevSymbol = input[i];
                }

                if (!input[i].IsOperator()) continue;

                if (!char.IsDigit(prevSymbol) && !(input[i] == '(' || input[i] == ')'))
                {
                    if ((prevSymbol == '*' || prevSymbol == '/') && input[i] == '-')
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
                        if (prevSymbol == '(') 
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
                            if (input[i].GetOperationPriority() <= operationStack.Peek().GetOperationPriority())
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