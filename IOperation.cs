using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Turing
{
    interface IOperation
    {
        decimal CalculateResultOfOperation(List<decimal> arguments);
    }
}
