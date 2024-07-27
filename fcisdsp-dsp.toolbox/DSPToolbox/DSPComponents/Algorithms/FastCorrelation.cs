using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class FastCorrelation : Algorithm
    {
        public Signal InputSignal1 { get; set; }
        public Signal InputSignal2 { get; set; }
        public List<float> OutputNonNormalizedCorrelation { get; set; }
        public List<float> OutputNormalizedCorrelation { get; set; }

        public override void Run()
        {
            int coun = InputSignal1.Samples.Count;
            int ib = 0;
            if (InputSignal2 == null)
            {
                ib = 1;

                InputSignal2 = new Signal(new List<float>(), InputSignal1.Periodic);
                for (int i = 0; i < coun; i++)
                {
                    InputSignal2.Samples.Add(InputSignal1.Samples[i]);

                }
            }

            List<Complex> X11 = new List<Complex>();
            DiscreteFourierTransform DFT = new DiscreteFourierTransform();
            DFT.InputTimeDomainSignal = new DSPAlgorithms.DataStructures.Signal(InputSignal1.Samples, InputSignal1.Periodic);
            DFT.Run();


            for (int i = 0; i < DFT.OutputFreqDomainSignal.FrequenciesAmplitudes.Count; i++) 
            {

                Complex s = new Complex(DFT.OutputFreqDomainSignal.FrequenciesAmplitudes[i] * (float)Math.Cos(DFT.OutputFreqDomainSignal.FrequenciesPhaseShifts[i]),
                    -1 * DFT.OutputFreqDomainSignal.FrequenciesAmplitudes[i] * (float)Math.Sin(DFT.OutputFreqDomainSignal.FrequenciesPhaseShifts[i]));


                X11.Add(s);

            }

            //List<Complex> X12 = new List<Complex>();
            DiscreteFourierTransform DFfT = new DiscreteFourierTransform();

            List<float> Amp = new List<float>();
            List<float> Phase = new List<float>();

            DFfT.InputTimeDomainSignal = new DSPAlgorithms.DataStructures.Signal(InputSignal2.Samples, InputSignal1.Periodic);

            DFfT.Run();
            for (int i = 0; i < DFfT.OutputFreqDomainSignal.FrequenciesAmplitudes.Count; i++)
            {
                Complex s = new Complex(DFfT.OutputFreqDomainSignal.FrequenciesAmplitudes[i] * (float)Math.Cos(DFfT.OutputFreqDomainSignal.FrequenciesPhaseShifts[i]),
                DFfT.OutputFreqDomainSignal.FrequenciesAmplitudes[i] * (float)Math.Sin(DFfT.OutputFreqDomainSignal.FrequenciesPhaseShifts[i]));


                s = s * X11[i];
                Amp.Add((float)s.Magnitude);
                Phase.Add((float)(Math.Atan2(s.Imaginary, s.Real)));

            }

            InverseDiscreteFourierTransform IDFT = new InverseDiscreteFourierTransform();
            // test case 2

            var Frequencies = new List<float> { 0, 1, 2, 3, 4, 5, 6, 7 };
            OutputNonNormalizedCorrelation = new List<float>();
            OutputNormalizedCorrelation = new List<float>();

            IDFT.InputFreqDomainSignal = new DSPAlgorithms.DataStructures.Signal(true, Frequencies, Amp, Phase);
            IDFT.Run();

            for (int i = 0; i < IDFT.OutputTimeDomainSignal.Samples.Count(); i++)
            {
                OutputNonNormalizedCorrelation.Add(IDFT.OutputTimeDomainSignal.Samples[i] / IDFT.OutputTimeDomainSignal.Samples.Count());
            }

            if (ib == 1)
            {

                float sum1 = 0;
                float sum2 = 0;
                for (int ic = 0; ic < coun; ic++)
                {
                    sum1 += (float)Math.Pow(InputSignal1.Samples[ic], 2);
                    sum2 += (float)Math.Pow(InputSignal2.Samples[ic], 2);

                }

                sum1 *= sum2;

                sum1 = (float)Math.Sqrt(sum1);
                sum1 /= InputSignal1.Samples.Count;



                if (InputSignal1.Periodic == false)
                {


                    float nor = 0;
                    for (int l = 0; l < InputSignal1.Samples.Count; l++)
                    {
                        nor += (float)Math.Pow(InputSignal1.Samples[l], 2);

                    }
                    nor /= InputSignal1.Samples.Count;


                    for (int i = 0; i < IDFT.OutputTimeDomainSignal.Samples.Count(); i++)
                    {
                        OutputNormalizedCorrelation.Add((float)((IDFT.OutputTimeDomainSignal.Samples[i] / IDFT.OutputTimeDomainSignal.Samples.Count()) / nor));

                    }

                    InputSignal2.Samples.RemoveAt(0);
                    InputSignal2.Samples.Add(0);

                }
                else
                {
                    for (int i = 0; i < IDFT.OutputTimeDomainSignal.Samples.Count(); i++)
                    {

                        OutputNormalizedCorrelation.Add((float)((IDFT.OutputTimeDomainSignal.Samples[i] / IDFT.OutputTimeDomainSignal.Samples.Count()) / sum1));

                    }

                    InputSignal2.Samples.Add(InputSignal2.Samples[0]);
                    InputSignal2.Samples.RemoveAt(0);

                }
            }
            else
            {

                float sum1 = 0;
                float sum2 = 0;
                for (int ic = 0; ic < coun; ic++)
                {
                    sum1 += (float)Math.Pow(InputSignal1.Samples[ic], 2);
                    sum2 += (float)Math.Pow(InputSignal2.Samples[ic], 2);

                }

                sum1 *= sum2;

                sum1 = (float)Math.Sqrt(sum1);
                sum1 /= InputSignal1.Samples.Count;


                for (int i = 0; i < IDFT.OutputTimeDomainSignal.Samples.Count(); i++)
                {

                    OutputNormalizedCorrelation.Add((float)((IDFT.OutputTimeDomainSignal.Samples[i] / IDFT.OutputTimeDomainSignal.Samples.Count()) / sum1));

                }

            }

        }
    }
}