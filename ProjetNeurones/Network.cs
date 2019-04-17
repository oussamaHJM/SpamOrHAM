using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetNeurones
{
    public class Network
    {
        private List<Layer> layers;
        int count = 0;
        private List<(List<List<double>>, double)> histories = new List<(List<List<double>>, double)>();
        private List<int> stck = new List<int>();
        public Network(int input, int hidden, int output)
        {
            layers = new List<Layer>();
            var inputLayer = new Layer();
            for (int i = 0; i < input; i++)
            {
                inputLayer.Neurals.Add(new Neural());
            }

            var hiddenLayer = new Layer();
            for (int i = 0; i < hidden; i++)
            {
                hiddenLayer.Neurals.Add(new Neural());
            }

            var outputLayer = new Layer();
            for (int i = 0; i < output; i++)
            {
                outputLayer.Neurals.Add(new Neural());
            }

            layers.Add(inputLayer);
            layers.Add(hiddenLayer);
            layers.Add(outputLayer);

        }


        public void Intilization()
        {
            foreach (var hidden in layers[1].Neurals)
            {
                hidden.RandomWeights(layers[0].Neurals.Count);
            }


            foreach (var output in layers[2].Neurals)
            {
                output.RandomWeights(layers[1].Neurals.Count);
            }
        }

        public double Forward(List<List<double>> inputs)
        {
            var inputLayer = layers[0];
            for (int i = 0; i < inputLayer.Neurals.Count; i++)
            {
                inputLayer.Neurals[i].ClearInput();
                for (int j = 0; j < inputs[i].Count; j++)
                {
                    inputLayer.Neurals[i].AddOrUpdateInput(j, inputs[i][j]);
                }
            }

            //clear input
            for (int i = 0; i < layers[1].Neurals.Count; i++)
            {
                layers[1].Neurals[i].ClearInput();
            }

            layers[2].Neurals[0].ClearInput();

            for (int i = 1; i < layers.Count; i++)
            {
                int count = 0;
                foreach (var layer in layers[i - 1].GetResult())
                {
                    for (int j = 0; j < layers[i].Neurals.Count; j++)
                    {
                        layers[i].Neurals[j].AddOrUpdateInput(count, layer);
                    }

                    count++;
                }
            }

            return layers[layers.Count - 1].GetResult().First();
        }

        public bool Train(List<List<double>> inputs, double target, ref int epoch)
        {
            var guess = Forward(inputs);
            //Console.WriteLine(guess);
            var error = 0.5 * Math.Pow((target - guess), 2);
            var x = target - guess;
            if (error >= 0.25)
            {
                var output = layers[layers.Count - 1];
                double alpha = 0.5;
                // update output layer
                for (int i = 0; i < output.Neurals.Count; i++)
                {
                    for (int j = 0; j < output.Neurals[i].GetCountInput(); j++)
                    {
                        /*var result = output.Neurals[i].GetResult(false);
                        var sig = Utils.dsigmoid(result);
                        var inp = layers[1].Neurals[i].GetInput(j);
                        var value = output.Neurals[i].GetWeight(j) - ((alpha * (target - guess) * sig * inp));*/
                        var v = output.Neurals[i].GetWeight(j) - (alpha * output.Neurals[i].GetResult(false) * (guess - target));
                        output.Neurals[i].UpdateWeight(j,v);
                    }
                }

                //update hidden layer
                var hidden = layers[1];
                for (int i = 0; i < hidden.Neurals.Count; i++)
                {
                    for (int j = 0; j < hidden.Neurals[i].GetCountInput(); j++)
                    {
                        var inpt = layers[0].Neurals[j].GetInput(0);
                        double sum = 0;

                        foreach (var er in layers[2].Neurals[0].GetWeights())
                        {
                            sum += (er * x);
                        }
                        /*   var errorHiddenF = Utils.dsigmoid(layers[2].Neurals[0].GetResult(false)) * sum;
                           var result = hidden.Neurals[i].GetResult(false);
                           var errorHidden = result * errorHiddenF;
                           var value = hidden.Neurals[i].GetWeight(j) - (layers[0].Neurals[j].GetInput(0) * errorHidden * alpha * inpt);*/
                        var v = hidden.Neurals[i].GetWeight(j) - (alpha * inpt * output.Neurals[0].GetWeight(j) * Utils.dsigmoid(layers[2].Neurals[0].GetResult(false)) * ((guess - target)));
                        hidden.Neurals[i].UpdateWeight(j,v);
                    }

                }
            }
            else
            {
                histories.Add((inputs, target));
            }


            double errorGlob = 0;
            foreach (var history in histories)
            {
                var hError = Forward(history.Item1);
                errorGlob += 0.5 * Math.Pow((history.Item2 - hError), 2);
            }
            errorGlob += error;
            if (errorGlob <= 0.25 || count > 1000)
            {
                epoch++;
                stck.Add(epoch);
                count = 0;
                return false;
            }
            else
            {
                count++;
                histories.Clear();
                epoch = stck.Count == 0 ? 0 : stck.Last();
                return true;

            }
        }
    }
}
