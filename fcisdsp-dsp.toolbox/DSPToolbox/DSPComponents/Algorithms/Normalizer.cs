//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using DSPAlgorithms.DataStructures;

//namespace DSPAlgorithms.Algorithms
//{
//    public class Normalizer : Algorithm
//    {
//        public Signal InputSignal { get; set; }
//        public float InputMinRange { get; set; }
//        public float InputMaxRange { get; set; }
//        public Signal OutputNormalizedSignal { get; set; }

//        public override void Run()
//        {
//            throw new NotImplementedException();
//        }
//    }
//}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class Normalizer : Algorithm
    {
        public Signal InputSignal { get; set; }
        public float InputMinRange { get; set; }
        public float InputMaxRange { get; set; }
        public Signal OutputNormalizedSignal { get; set; }

        public override void Run()
        {
            List<float> outp = new List<float>();
            for (int i = 0; i < InputSignal.Samples.Count; i++)
            {
                float r = InputSignal.Samples.Max();
                float j = InputSignal.Samples.Min();
                float k = (InputMaxRange - InputMinRange) * ((InputSignal.Samples[i] - j)) / (r - j) + InputMinRange;
                outp.Add(k);
            }
            OutputNormalizedSignal = new Signal(outp, false);
        
        }
    }
}