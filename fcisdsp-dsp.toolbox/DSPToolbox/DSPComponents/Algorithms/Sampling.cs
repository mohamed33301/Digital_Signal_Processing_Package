using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DSPAlgorithms.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amazon.CloudFormation.Model;

namespace DSPAlgorithms.Algorithms
{
    public class Sampling : Algorithm
    {
        public int L { get; set; } //upsampling factor
        public int M { get; set; } //downsampling factor
        public Signal InputSignal { get; set; }
        public Signal OutputSignal { get; set; }

        public override void Run()
        {

            List<float> output = new List<float>();
            List<float> output1 = new List<float>();
            FIR f = new FIR();

            f.InputFilterType = FILTER_TYPES.LOW;
            f.InputFS = 8000;
            f.InputStopBandAttenuation = 50;
            f.InputTransitionBand = 500;
            f.InputCutOffFrequency = 1500;

            if (M == 0 && L != 0)
            {
                L = L - 1;

                for (int i = 0; i < InputSignal.Samples.Count; i++)
                {
                    output.Add(InputSignal.Samples[i]);
                    if (i == InputSignal.Samples.Count - 1)
                    { break; }
                    for (int j = 0; j < L; j++)
                    {
                        output.Add(0);
                    }
                }

                f.InputTimeDomainSignal = new Signal(output, false);

                f.Run();
                OutputSignal = f.OutputYn;

            }


            else if (L == 0 && M != 0)
            {
                M = M - 1;



                f.InputTimeDomainSignal = InputSignal;

                f.Run();
                OutputSignal = f.OutputYn;

                int check = M;


                for (int i = 0; i < OutputSignal.Samples.Count; i++)
                {
                    if (check == M)
                    {
                        output.Add(OutputSignal.Samples[i]);
                        check = 0;
                    }
                    else
                    {
                        check++;
                    }


                }

                OutputSignal = new Signal(output, false);

            }

            else if (L != 0 && M != 0)
            {
                L = (L - 1);
                M = (M - 1);


                for (int i = 0; i < InputSignal.Samples.Count; i++)
                {
                    if (InputSignal.Samples[i] != 0)
                    {
                        output.Add(InputSignal.Samples[i]);
                    }

                    for (int j = 0; j < L; j++)
                    {
                        output.Add(0);
                    }
                }

                f.InputTimeDomainSignal = new Signal(output, false);

                f.Run();
                OutputSignal = f.OutputYn;

                int check = M;

                output = new List<float>();

                for (int i = 0; i < OutputSignal.Samples.Count; i++)
                {
                    if (check == M)
                    {
                        if (OutputSignal.Samples[i] != 0)
                        {
                            output.Add(OutputSignal.Samples[i]);
                        }
                        check = 0;
                    }
                    else
                    {
                        check++;
                    }
                }
                OutputSignal = new Signal(output, false);

            }

            else
            {
                Console.WriteLine("error");
            }

            //throw new NotImplementedException();
        }
    }
}