using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2
{
    class Program
    {
        static void Main(string[] args)
        {

            //0   0  | 0
            //0   1  | 0
            //1   0  | 0
            //1   1  | 1

            var inputs = new double[,]
            {
               {0,0 },
               {0,1 },
               {1,0 },
               {1,1 }
            };

            var targets = new double[]
            {
                0,
                0,
                0,
                1
            };

            var perceptron = new SingleLayerPerceptron(inputs, targets);
            var iterations = 100;
            perceptron.Train(iterations, 0.25);
            perceptron.ConfusionMatrix(inputs, targets, iterations);
        }
    }
}
