using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Mime;
using System.Text;
using Xunit;

namespace ConsoleCalculator.Tests
{
    public class CalculatorTests
    {
        [Theory]
        [InlineData("2+3",5)]
        [InlineData("(2+3)*6",30)]
        [InlineData("2+-6", -4)]
        [InlineData("-2", -2)]
        [InlineData("-(33*11)/(-11)", 33)]
        [InlineData("2+3*5", 17)]
        [InlineData("(-(+2-1))", -1)]
        [InlineData("10*4+(2+8)+10*4+(2+8)",100)]
        public void Calculate_GetResult(string input, double expected)
        {
            var actual = ExpressionEngine.Calculate(input);

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("()2+3)*6")]
        [InlineData("-(33*11)/-11")]
        [InlineData("2//2")]
        [InlineData("-/-")]
        public void Calculate_ThrowException(string input)
        {
            Assert.ThrowsAny<Exception>(() => ExpressionEngine.Calculate(input));
        }

        [Theory]
        [InlineData("2/0")]
        [InlineData("1234/0")]
        [InlineData("(2+5)/0")]
        [InlineData("2*2+(5-7)/0")]
        public void Calculate_ThrowDivideByNullException(string input)
        {
            Assert.Throws<DivideByZeroException>(() => ExpressionEngine.Calculate(input));
        }

        [Fact]
        public void Calculate_MillionSymbols()
        {
            // arrange
            var sw = new Stopwatch();
            sw.Start();
            
            var expr = "10*4+(2+8)+10*4+(2+8)"; // 21 symbol  
            var sb = new StringBuilder(expr);
            
            // 21 * 48 000 = 1 008 000 symbols
            for (var i = 0; i < 48000; i++)
            {
                sb.AppendJoin('+', expr);
            }
            
            var path = Path.Combine(Directory.GetCurrentDirectory(), "test.txt");

            using (var steamWriter = new StreamWriter(path))
            {
                steamWriter.Write(sb.ToString());
            }
            
            string input;
            
            using (var sr = new StreamReader(path))
            {
                input = sr.ReadToEnd();
            }
            //act 
            var actual = ExpressionEngine.Calculate(input);
            sw.Stop();

            // assert
            Assert.True(60 > sw.ElapsedMilliseconds / 1000);
        }
    }
}