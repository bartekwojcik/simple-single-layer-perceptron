using System;
using System.Linq;

namespace ConsoleApp2
{
    public class SingleLayerPerceptron
    {
        private readonly double[] _weights;
        private readonly double[,] _trainingInputs;
        private readonly double[] _trainingTargets;



        public SingleLayerPerceptron(double[,] inputs, double[] trainingTargets)
        {
            this._weights = new double[inputs.GetLength(1) + 1];
            var random = new Random();
            for (int i = 0; i < _weights.Length; i++)
            {
                var rand = (double)random.Next(0, 100);
                rand = rand / 1000.0;
                var randWeight = rand - 0.05;
                var rounded = Math.Round(randWeight, 2);
                _weights[i] = rounded;
            }
            this._trainingTargets = trainingTargets;
            this._trainingInputs = AddBiasInput(inputs);

        }

        public void Train(int iterations, double eta)
        {
            if (this._trainingTargets.Length != _trainingInputs.GetLength(0))
            {
                throw new Exception("meh!");
            }
            for (int i = 0; i < iterations; i++)
            {
                double[] outputs = CalculateOutputs(_trainingInputs);

                if (outputs.Length != _trainingTargets.Length)
                {
                    throw new Exception("somethings wrong!");
                }

                for (int j = 0; j < _trainingInputs.GetLength(0); j++)
                {
                    var currentVector = GetRow(_trainingInputs, j);
                    var target = _trainingTargets[j];
                    var output = outputs[j];
                    for (int k = 0; k < currentVector.Length; k++)
                    {
                        var input = currentVector[k];
                        var weight = _weights[k];
                        var delta = eta * (output - target) * input;
                        _weights[k] = weight - delta;
                    }
                }
            }
        }

        private double[] CalculateOutputs(double[,] inputs)
        {
            var outputs = new double[inputs.GetLength(0)];
            for (int j = 0; j < inputs.GetLength(0); j++)
            {
                var currentVector = GetRow(inputs, j);
                var thisWeightsMultipliedByVectors = new double[inputs.GetLength(1)];
                for (int k = 0; k < currentVector.Length; k++)
                {
                    var input = currentVector[k];
                    var weight = _weights[k];
                    thisWeightsMultipliedByVectors[k] = input * weight;
                }

                var linear = thisWeightsMultipliedByVectors.Sum();
                var discrete = linear > 0 ? 1 : 0;
                outputs[j] = discrete;
            }

            return outputs;
        }

        public void ConfusionMatrix(double[,] inputs, double[] targets, int iterations)
        {
            var falsePositive = 0;
            var truePositives = 0;
            var falseNegatives = 0;
            var trueNegatives = 0;

            var biasedInputs = AddBiasInput(inputs);
            var outputs = CalculateOutputs(biasedInputs);
            Console.WriteLine($"iterations: {iterations}");
            for (var index = 0; index < outputs.Length; index++)
            {
                var output = outputs[index];
                var target = targets[index];
                //false positives
                if (output == 1 && target == 0)
                {
                    falsePositive++;
                }
                if (output == 1 && target == 1)
                {
                    truePositives++;
                }
                if (output == 0 && target == 1)
                {
                    falseNegatives++;
                }
                if (output == 0 && target == 0)
                {
                    trueNegatives++;
                }
                Console.WriteLine($"outpus is: {output} | target is: {target}");
            }
            Console.WriteLine("------------" + Environment.NewLine);
            Console.WriteLine($"Confution matrix: {Environment.NewLine}----------");
            Console.WriteLine($"| {truePositives} | {falsePositive} |");
            Console.WriteLine("----------");
            Console.WriteLine($"| {falseNegatives} | {trueNegatives} |");
            Console.WriteLine("----------" + Environment.NewLine);

            var precistion = truePositives / (truePositives + falsePositive);
            var recall = truePositives / (truePositives + falseNegatives);

            var f1 = 2 * ((precistion * recall) / (precistion + recall));
            Console.WriteLine($"F1 = {f1}");

            Console.Read();
        }

        private static double[,] AddBiasInput(double[,] inputs)
        {
            var localInputs = new double[inputs.GetLength(0), inputs.GetLength(1) + 1];
            for (int i = 0; i < localInputs.GetLength(0); i++)
            {
                for (int j = 0; j < localInputs.GetLength(1); j++)
                {
                    if (j == localInputs.GetLength(1) - 1)
                    {
                        localInputs[i, j] = -1;
                    }
                    else
                    {
                        localInputs[i, j] = inputs[i, j];
                    }
                }
            }

            return localInputs;
        }

        private double[] GetRow(double[,] trainingInputs, int rowIndex)
        {
            var result = new double[trainingInputs.GetLength(1)];
            for (int i = 0; i < trainingInputs.GetLength(1); i++)
            {
                result[i] = trainingInputs[rowIndex, i];
            }
            return result;
        }

        private double[] GetColumn(double[,] array, int columnIndex)
        {
            var result = new double[array.GetLength(0)];
            for (int i = 0; i < array.GetLength(0); i++)
            {
                result[i] = array[i, columnIndex];
            }

            return result;
        }

        public void ConfustionMatrix()
        {

        }
    }
}