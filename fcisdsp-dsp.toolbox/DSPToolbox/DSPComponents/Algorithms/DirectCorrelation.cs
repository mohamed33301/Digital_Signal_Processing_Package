using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class DirectCorrelation : Algorithm
    {
        public Signal InputSignal1 { get; set; }
        public Signal InputSignal2 { get; set; }
        public List<float> OutputNonNormalizedCorrelation { get; set; }
        public List<float> OutputNormalizedCorrelation { get; set; }

        public override void Run()
        {
            OutputNonNormalizedCorrelation = new List<float>();
            OutputNormalizedCorrelation = new List<float>();
            List<float> signal2 = new List<float> { };
            int size = InputSignal1.Samples.Count;
            if (InputSignal2 == null)
            {
                for (int i = 0; i < size; i++)
                {
                    signal2.Add(InputSignal1.Samples[i]);

                }
                float sqrsum1 = 0;
                for (int i = 0; i < size; i++)
                {
                    sqrsum1 += InputSignal1.Samples[i] * signal2[i];

                }
                double P = sqrsum1 / size;
                for (int i = 0; i < size; i++)
                {
                    float temp = 0;
                    for (int j = 0; j < size; j++)
                    {
                        if (InputSignal1.Periodic)
                        {
                            temp += (InputSignal1.Samples[j] * signal2[(i + j) % size]);

                        }
                        else
                        {
                            if(i + j >= size)
                            {
                                temp += 0;
                            }
                            else
                            {
                                temp += (InputSignal1.Samples[j] * signal2[i + j]);
                            }

                        }
                    }

                    OutputNonNormalizedCorrelation.Add(temp / size);
                    OutputNormalizedCorrelation.Add((float)((temp / size) / P));

                }
            }
            else
            {
                for (int i = 0; i < size; i++)
                {
                    signal2.Add(InputSignal2.Samples[i]);

                }
                float sqrsum1 = 0;
                float sqrsum2 = 0;
                for (int i = 0; i < size; i++)
                {
                    sqrsum1 += InputSignal1.Samples[i] * InputSignal1.Samples[i];
                    sqrsum2 += signal2[i] * signal2[i];

                }
                double P = (Math.Sqrt(sqrsum1 * sqrsum2)) / size;
                for (int i = 0; i < size; i++)
                {
                    float temp = 0;
                    for (int j = 0; j < size; j++)
                    {
                        if (InputSignal2.Periodic)
                        {
                            temp += (InputSignal1.Samples[j] * signal2[(i + j) % size]);
                        }
                        else
                        {
                            if (i + j >= size)
                            {
                                temp += 0;
                            }
                            else
                            {
                                temp += (InputSignal1.Samples[j] * signal2[i + j]);
                            }

                        }
                    }
                    OutputNonNormalizedCorrelation.Add(temp / size);
                    OutputNormalizedCorrelation.Add((float)((temp / size) / P));

                }
            }
        }
    }
}