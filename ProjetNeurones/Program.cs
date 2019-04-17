using System.Collections.Generic;
using System.IO;
using console;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetNeurones
{

    class Program
    {
        static void Main(string[] args)
        {
            var network = new Network(5, 5, 1);
            network.Intilization();


            List<(string, float)> datas = new List<(string, float)>();
            List<(List<List<double>>, float)> results = new List<(List<List<double>>, float)>();
            for (int j = 1; j <= 5; j++)
            {
                using (StreamReader sr = new StreamReader($"{j}.csv"))
                {
                    string currentLine;
                    // currentLine will be null when the StreamReader reaches the end of file
                    int i = 0;
                    while ((currentLine = sr.ReadLine()) != null)
                    {
                        if (i > 0)
                        {
                            var split = currentLine.Split(',');
                            if (split.Length == 5)
                                datas.Add((split[3].Replace("\"", ""), float.Parse(split[4].Replace("\"", ""))));
                            else if (split.Length == 6)
                                datas.Add((split[4].Replace("\"", ""), float.Parse(split[5].Replace("\"", ""))));

                        }
                        i++;
                    }
                }
            }
            //  datas[0] = (datas[0].Item1, 0);
            for (int j = 0; j < datas.Count; j++)
            {
                var splitData = new List<List<double>>();
                for (int i = 0; i < 5; i++)
                {
                    double sum = 0;
                    for (int z = 0; z < 1; z++)
                    {
                        if (datas[(i + 1) * z].Item1.Count() < i + 1)
                        {
                            sum += (double)System.Convert.ToChar(" ") / 300;

                        }
                        else
                        {
                            sum += (double)System.Convert.ToChar(datas[j].Item1[(i + 1) * z]) / 300;
                        }
                    }
                    splitData.Add(new List<double>() { sum });


                }
                results.Add((splitData, datas[j].Item2));
            }
            for (int j = 0; j < results.Count;)
            {

                network.Train(results[j].Item1, datas[j].Item2, ref j);
            }

            for (int i = 0;i< results.Count; i++)
             {
                 var v = results[i].Item1;
                 Console.WriteLine($"result  :#1  target : #2",new List<double> { network.Forward(v), { results[i].Item2 } });
             }

            Console.ReadLine();

        }
    }
}
