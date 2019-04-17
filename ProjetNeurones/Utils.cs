using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetNeurones
{
    public class Utils
    {
        public static double sigmoid(double x)
        {
            return 1 / (1 + Math.Exp((-8 * x) + 4));
        }

        public static double dsigmoid(double x)
        {
            var a = 8 * Math.Exp((-8 * x) + 4);
            var b = Math.Pow((1 + Math.Exp((-8 * x) + 4)), 2);
            return a / b;

        }
    }
}
