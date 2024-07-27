using System;
using System.Collections.Generic;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class Adder : Algorithm
    {
        public List<Signal> InputSignals { get; set; }
        public Signal OutputSignal { get; set; } //it's just object from signal class not list

        public override void Run()
        {
            //create list to collect sum of 2 signals in samples s1:{1,2,4,5,8,3} , s2:{2,4,7,6.5} , l:{3,6,11,11.5,8,3}
            List<float> output_signal = new List<float>();
            for (int i = 0; i < InputSignals[1].Samples.Count; i++)
            {
                float var = InputSignals[0].Samples[i] + InputSignals[1].Samples[i];
                output_signal.Add(var);
            }
            OutputSignal = new Signal(output_signal , false);

        }
    }
}