using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class Folder : Algorithm
    {
        public Signal InputSignal { get; set; }
        public Signal OutputFoldedSignal { get; set; }

        public override void Run()
        {

            List<int> indices = new List<int>();
            List<float> val = new List<float>();

            int[] ind = new int[InputSignal.Samples.Count];
            float[] vals = new float[InputSignal.Samples.Count];
            for (int i = 0; i < InputSignal.Samples.Count; i++)
            {
                int sam_ind = -(InputSignal.SamplesIndices[i]);
                ind[i] = sam_ind;
                vals[i] = InputSignal.Samples[i];
            }
            for (int i = InputSignal.Samples.Count - 1; i >= 0; i--)
            {
                indices.Add(ind[i]);
                val.Add(vals[i]);
            }
            OutputFoldedSignal = new Signal(val, indices, false);

            if (InputSignal.Periodic == false)
            {
                OutputFoldedSignal.Periodic = true;
            }
            else
            {
                OutputFoldedSignal.Periodic = false;
            }
        }
    }
}