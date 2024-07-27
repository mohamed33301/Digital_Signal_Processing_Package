using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using DSPAlgorithms.DataStructures;
namespace DSPAlgorithms.Algorithms
{
    public class FastConvolution : Algorithm
    {
        public Signal InputSignal1 { get; set; }
        public Signal InputSignal2 { get; set; }
        public Signal OutputConvolvedSignal { get; set; }

        /// <summary>
        /// Convolved InputSignal1 (considered as X) with InputSignal2 (considered as H)
        /// </summary>
        public override void Run()
        {

            List<float> s1_Phase_shift = new List<float>();
            List<float> s1_Amp = new List<float>();
            List<float> s2_Phase_shift = new List<float>();
            List<float> s2_Amp = new List<float>();
            float s1_Real, s2_Real, s1_Imaginary, s2_Imaginary;
            List<float> multiOp = new List<float>();
            List<float> multiOp2 = new List<float>();
            Complex complex1, complex2;
            List<Complex> ans = new List<Complex>();
            Signal output;

            int count = InputSignal1.Samples.Count + InputSignal2.Samples.Count - 1;


            for (int i = InputSignal1.Samples.Count; i < count; i++)
            {
                InputSignal1.Samples.Add(0);
            }
            for (int i = InputSignal2.Samples.Count; i < count; i++)
            {
                InputSignal2.Samples.Add(0);
            }
            // DFT
            DiscreteFourierTransform dft = new DiscreteFourierTransform();
            dft.InputTimeDomainSignal = InputSignal1;
            dft.Run();
            s1_Amp = dft.OutputFreqDomainSignal.FrequenciesAmplitudes;
            s1_Phase_shift = dft.OutputFreqDomainSignal.FrequenciesPhaseShifts;

            dft.InputTimeDomainSignal = InputSignal2;
            dft.Run();
            s2_Amp = dft.OutputFreqDomainSignal.FrequenciesAmplitudes;
            s2_Phase_shift = dft.OutputFreqDomainSignal.FrequenciesPhaseShifts;

            //getting real and imaginary num
            for (int i = 0; i < count; i++)
            {
                s1_Real = s1_Amp[i] * (float)Math.Cos(s1_Phase_shift[i]);
                s2_Real = s2_Amp[i] * (float)Math.Cos(s2_Phase_shift[i]);
                s1_Imaginary = s1_Amp[i] * (float)Math.Sin(s1_Phase_shift[i]);
                s2_Imaginary = s2_Amp[i] * (float)Math.Sin(s2_Phase_shift[i]);
                complex1 = new Complex(s1_Real, s1_Imaginary);
                complex2 = new Complex(s2_Real, s2_Imaginary);
                ans.Add(Complex.Multiply(complex1, complex2));
            }
            for (int i = 0; i < count; i++)
            {
                float realsq = (float)Math.Pow(ans[i].Real, 2);
                float imgsq = (float)Math.Pow(ans[i].Imaginary, 2);
                multiOp.Add((float)(Math.Sqrt(realsq + imgsq)));
                multiOp2.Add((float)Math.Atan2(ans[i].Imaginary, ans[i].Real));
            }

            //IDFT
            InverseDiscreteFourierTransform idft = new InverseDiscreteFourierTransform();
            output = new Signal(false, multiOp, multiOp, multiOp2);
            idft.InputFreqDomainSignal = output;
            idft.Run();
            OutputConvolvedSignal = new Signal(idft.OutputTimeDomainSignal.Samples, false);


        }
    }
}
