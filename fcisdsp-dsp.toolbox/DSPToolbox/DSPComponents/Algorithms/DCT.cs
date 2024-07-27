using DSPAlgorithms.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSPAlgorithms.Algorithms
{
    public class DCT : Algorithm
    {
        public Signal InputSignal { get; set; }
        public Signal OutputSignal { get; set; }

        public override void Run()
        {
            double N = InputSignal.Samples.Count;
            List<float> output = new List<float>();
            for (int k = 0; k < N; k++)
            {
                float En = 0;
                for (int n = 0; n < N; n++)
                {
                    En = En + (float)(InputSignal.Samples[n] * Math.Cos((Math.PI / (4 * N)) * ((2 * n) - 1) * ((2 * k) - 1)));
                }
                output.Add((float)Math.Sqrt(2 / N) * En);
            }
            OutputSignal = new Signal(output, false);
        }
    }
}
