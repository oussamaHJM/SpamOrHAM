using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetNeurones
{
    public class Neural
    {
        //poid * entrer
        private List<double> inputs;
        private List<double> weights;
        private Random random;

        public Neural()
        {
            this.inputs = new List<double>();
            this.weights = new List<double>();
            this.random = new Random();
        }

        public void RandomWeights(int count)
        {
            for (int i = 0; i < count; i++)
            {
                 weights.Add(random.NextDouble());
            }
        }
        public void AddOrUpdateInput(int i, double value)
        {
            if (inputs.Count >= (i + 1))
            {
                inputs[i] = value;
            }
            else
            {
                inputs.Add(value);
            }
        }

        public int GetCountInput()
        {
            return inputs.Count;
        }

        public void UpdateWeight(int i, double value)
        {
            weights[i] = value;
        }

        public void UpdateWeight()
        {
            weights[random.Next(0, weights.Count)] = 0.1;
        }

        public double GetInput(int i)
        {
            return inputs[i];
        }
        public double GetWeight(int i)
        {
            return weights[i];
        }

        public List<double> GetWeights()
        {
            return weights;
        }

        public void ClearInput()
        {
            this.inputs.Clear();
        }


        public double GetResult(bool state = true)
        {
            if (weights.Count == 0)
            {
                return inputs.Sum();
            }
            else
            {
                double sum = 0;
                for (int i = 0; i < inputs.Count; i++)
                {
                    sum += (inputs[i] * weights[i]);
                }

                if (state)
                    return Utils.sigmoid(sum);
                else
                    return sum;
            }

        }
    }
}
