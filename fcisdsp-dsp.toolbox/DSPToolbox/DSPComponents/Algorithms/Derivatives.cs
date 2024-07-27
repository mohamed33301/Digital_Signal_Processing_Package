using DSPAlgorithms.DataStructures;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace DSPAlgorithms.Algorithms
{
    public class Derivatives : Algorithm
    {
        public Signal InputSignal { get; set; }
        public Signal FirstDerivative { get; set; }
        public Signal SecondDerivative { get; set; }

        public override void Run()
        {



            List<float> OutputSignaL1 = new List<float>();
            List<float> OutputSignaL2 = new List<float>();
            for (int i = 1; i < InputSignal.Samples.Count; i++)
            {

                OutputSignaL1.Add(InputSignal.Samples[i] - InputSignal.Samples[i - 1]);

            }
            for (int j = 1; j < InputSignal.Samples.Count; j++)
            {
                if (j == InputSignal.Samples.Count - 1)
                {
                    OutputSignaL2.Add(0);

                }
                else
                {
                    OutputSignaL2.Add(InputSignal.Samples[j + 1] - (2 * InputSignal.Samples[j]) + InputSignal.Samples[j - 1]);
                }
            }
            FirstDerivative = new Signal(OutputSignaL1, false);
            SecondDerivative = new Signal(OutputSignaL2, false);
        }
    }
}