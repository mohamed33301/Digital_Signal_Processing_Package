using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;


namespace DSPAlgorithms.Algorithms
{
    public class AccumulationSum : Algorithm
    {
        public Signal InputSignal { get; set; }
        public Signal OutputSignal { get; set; }

        public override void Run()
        {
            List<float> outputSamples = new List<float>();

            for(int x = 0 ; x < InputSignal.Samples.Count ; x++)
            {
                float sum = 0;
                for (int j = 0;j <= x; j++)
                {
                    sum+= InputSignal.Samples[j];
                }
                outputSamples.Add(sum);
            }
            OutputSignal = new Signal(outputSamples,false);
        }
    }
}
