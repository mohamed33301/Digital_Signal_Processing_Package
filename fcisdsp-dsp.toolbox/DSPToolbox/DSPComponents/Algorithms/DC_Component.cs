using DSPAlgorithms.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSPAlgorithms.Algorithms
{
    public class DC_Component: Algorithm
    {
        public Signal InputSignal { get; set; }
        public Signal OutputSignal { get; set; }

        public override void Run()
        {
            List<float> output_signal = new List<float>();
            float sum = 0;
            float mean = 0;
            for(int i = 0; i < InputSignal.Samples.Count(); i++)
            {
                sum += InputSignal.Samples[i];
            }
            mean = sum / InputSignal.Samples.Count();
            for (int i = 0; i < InputSignal.Samples.Count(); i++)
            {
                output_signal.Add(InputSignal.Samples[i] - mean) ;
            }
            OutputSignal = new Signal(output_signal , false);

        }
    }
}
