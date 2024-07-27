using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;
using System.Numerics;

using System.ComponentModel;

namespace DSPAlgorithms.Algorithms
{
    public class InverseDiscreteFourierTransform : Algorithm
    {


        public Signal InputFreqDomainSignal { get; set; }
        public Signal OutputTimeDomainSignal { get; set; }
        double Epsilon = .000001d;
        public override void Run()
        {

            OutputTimeDomainSignal = new Signal(new List<float>(), false, new List<float>(), new List<float>(), new List<float>());
            int N = InputFreqDomainSignal.FrequenciesAmplitudes.Count;
            List<Complex> harmonics = new List<Complex>();

            for (int i = 0; i < N; i++)
                harmonics.Add(Complex.FromPolarCoordinates(InputFreqDomainSignal.FrequenciesAmplitudes[i], InputFreqDomainSignal.FrequenciesPhaseShifts[i]));

            for (int i = 0; i < N; i++)
            {
                Complex ith_harmonic = new Complex();
                for (int j = 0; j < N; j++)
                {
                    ith_harmonic += new Complex(Math.Cos(i * 2 * Math.PI * j / N), Math.Sin(i * 2 * Math.PI * j / N)) * harmonics[j];
                }
                ith_harmonic /= N;

                OutputTimeDomainSignal.Samples.Add((float)ith_harmonic.Real);
            }


        }
        //throw new NotImplementedException();

    }
}//=========================================================================================================

//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Numerics;
//using System.Text;
//using System.Threading.Tasks;
//using DSPAlgorithms.DataStructures;
//using System.IO;

//namespace DSPAlgorithms.Algorithms
//{
//    public class DiscreteFourierTransform : Algorithm
//    {
//        public Signal InputTimeDomainSignal { get; set; }
//        public float InputSamplingFrequency { get; set; }
//        public Signal OutputFreqDomainSignal { get; set; }
//        public List<Complex> z { get; set; }

//        public override void Run()
//        {
//            z = new List<Complex>();
//            Complex[] j = new Complex[InputTimeDomainSignal.Samples.Count];
//            float T = 1 / InputSamplingFrequency;
//            List<float> amplitude = new List<float>();
//            List<float> phase = new List<float>();
//            List<float> xaxis = new List<float>();



//            for (int k = 0; k < InputTimeDomainSignal.Samples.Count(); k++)
//            {
//                float re = 0;
//                float img1 = 0;

//                for (int n = 0; n < InputTimeDomainSignal.Samples.Count(); n++)
//                {
//                    //x(k)= 2*pie*k*n/N

//                    float p = (float)(2 * Math.PI * k * n) / InputTimeDomainSignal.Samples.Count();

//                    re += (float)(InputTimeDomainSignal.Samples[n] * Math.Cos(p));
//                    img1 -= (float)(InputTimeDomainSignal.Samples[n] * Math.Sin(p));

//                }




//                j[k] = new Complex(re, img1);
//                z.Add(j[k]);

//                amplitude.Add((float)j[k].Magnitude);
//                phase.Add((float)j[k].Phase);


//                xaxis.Add((float)((k + 1) * ((2 * Math.PI) / (InputTimeDomainSignal.Samples.Count() * T))));
//            }
//            OutputFreqDomainSignal = new Signal(false, xaxis, amplitude, phase);






//        }
//    }
//}
