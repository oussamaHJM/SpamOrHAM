using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetNeurones
{
    public class Layer
    {
        public List<Neural> Neurals
        {
            get;
            private set;
        }

        public Layer()
        {
            Neurals = new List<Neural>();
        }


        public IEnumerable<double> GetResult(bool state = true)
        {
            foreach (var neural in Neurals)
            {
                yield return neural.GetResult(state);
            }
        }

    }
}
