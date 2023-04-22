using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Turing
{
    class Addition : IOperation
    {
        public decimal CalculateResultOfOperation(List<decimal> arguments)
        {
            Console.WriteLine($"Evaluando: {arguments[0]} + {arguments[1]} = {arguments[0] + arguments[1]}");
            return arguments[0] + arguments[1];
        }
    }

    class Division : IOperation
    {
        public decimal CalculateResultOfOperation(List<decimal> arguments)
        {
            Console.WriteLine($"Evaluando: {arguments[0]} / {arguments[1]} = {arguments[0] / arguments[1]}");
            return arguments[0] / arguments[1];
        }
    }

    class Exponent : IOperation
    {
        public decimal CalculateResultOfOperation(List<decimal> arguments)
        {
            Console.WriteLine($"Evaluando: {arguments[0]} ^ {arguments[1]} = {Convert.ToDecimal(Math.Pow(Convert.ToDouble(arguments[0]), Convert.ToDouble(arguments[1])))}");
            return Convert.ToDecimal(Math.Pow(Convert.ToDouble(arguments[0]), Convert.ToDouble(arguments[1])));
        }
    }

    class Multiplication : IOperation
    {
        public decimal CalculateResultOfOperation(List<decimal> arguments)
        {
            Console.WriteLine($"Evaluando: {arguments[0]} * {arguments[1]} = {arguments[0] * arguments[1]}");
            return arguments[0] * arguments[1];
        }
    }

    class Subtraction : IOperation
    {
        public decimal CalculateResultOfOperation(List<decimal> arguments)
        {
            Console.WriteLine($"Evaluando: {arguments[0]} - {arguments[1]} = {arguments[0] - arguments[1]}");
            return arguments[0] - arguments[1];
        }
    }
}
