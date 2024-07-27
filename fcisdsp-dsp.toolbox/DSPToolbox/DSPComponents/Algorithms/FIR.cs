using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class FIR : Algorithm
    {
        public Signal InputTimeDomainSignal { get; set; }
        public FILTER_TYPES InputFilterType { get; set; }
        public float InputFS { get; set; }
        public float? InputCutOffFrequency { get; set; }
        public float? InputF1 { get; set; }
        public float? InputF2 { get; set; }
        public float InputStopBandAttenuation { get; set; }
        public float InputTransitionBand { get; set; }
        public Signal OutputHn { get; set; }
        public Signal OutputYn { get; set; }

        public override void Run()
        {
            OutputHn = new Signal(new List<float>(), new List<int>(), false);
            OutputYn = new Signal(new List<float>(), new List<int>(), false);

            float numberOfCoefficients = 0;
            String equation = "";

            if (InputStopBandAttenuation <= 21)
            {
                equation = "1000";
                numberOfCoefficients = (float)(0.9 / (InputTransitionBand / InputFS));
                numberOfCoefficients = numOfCoefficients(numberOfCoefficients);
            }
            else if (InputStopBandAttenuation <= 44)
            {
                equation = "0100";
                numberOfCoefficients = (float)(3.1 / (InputTransitionBand / InputFS));
                numberOfCoefficients = numOfCoefficients(numberOfCoefficients);
            }
            else if (InputStopBandAttenuation <= 53)
            {
                equation = "0010";
                numberOfCoefficients = (float)(3.3 / (InputTransitionBand / InputFS));
                numberOfCoefficients = numOfCoefficients(numberOfCoefficients);
            }
            else if (InputStopBandAttenuation <= 74)
            {
                equation = "0001";
                numberOfCoefficients = (float)(5.5 / (InputTransitionBand / InputFS));
                numberOfCoefficients = numOfCoefficients(numberOfCoefficients);
            }

            int lower_boundary = (int)(-Math.Floor((float)(numberOfCoefficients / 2)));
            int upper_boundary = (int)(Math.Floor((float)(numberOfCoefficients / 2)));
            float finalCutOffFrequency, finalCutOffFrequency2;

            // differenet types of filters
            if (InputFilterType == FILTER_TYPES.LOW)
            {
                finalCutOffFrequency = (float)(InputCutOffFrequency + (InputTransitionBand / 2)) / InputFS;
                if (equation == "0001")
                {
                    calculateCoefficients(lower_boundary, upper_boundary, (int)numberOfCoefficients, finalCutOffFrequency, 0, blackman, lowpass);
                }
                else if (equation == "0010")
                {
                    calculateCoefficients(lower_boundary, upper_boundary, (int)numberOfCoefficients, finalCutOffFrequency, 0, hamming, lowpass);
                }
                else if (equation == "0100")
                {
                    calculateCoefficients(lower_boundary, upper_boundary, (int)numberOfCoefficients, finalCutOffFrequency, 0, hanning, lowpass);
                }
                else if (equation == "1000")
                {
                    calculateCoefficients(lower_boundary, upper_boundary, (int)numberOfCoefficients, finalCutOffFrequency, 0, rectangular, lowpass);
                }
            }
            else if (InputFilterType == FILTER_TYPES.HIGH)
            {
                finalCutOffFrequency = (float)((InputCutOffFrequency + (InputTransitionBand / 2)) / InputFS);
                if (equation == "0001")
                {
                    calculateCoefficients(lower_boundary, upper_boundary, (int)numberOfCoefficients, finalCutOffFrequency, 0, blackman, highpass);
                }
                else if (equation == "0010")
                {
                    calculateCoefficients(lower_boundary, upper_boundary, (int)numberOfCoefficients, finalCutOffFrequency, 0, hamming, highpass);
                }
                else if (equation == "0100")
                {
                    calculateCoefficients(lower_boundary, upper_boundary, (int)numberOfCoefficients, finalCutOffFrequency, 0, hanning, highpass);
                }
                else if (equation == "1000")
                {
                    calculateCoefficients(lower_boundary, upper_boundary, (int)numberOfCoefficients, finalCutOffFrequency, 0, rectangular, highpass);
                }
            }
            else if (InputFilterType == FILTER_TYPES.BAND_PASS)
            {
                finalCutOffFrequency = (float)((InputF1 - (InputTransitionBand / 2)) / InputFS);
                finalCutOffFrequency2 = (float)((InputF2 + (InputTransitionBand / 2)) / InputFS);
                if (equation == "0001")
                {
                    calculateCoefficients(lower_boundary, upper_boundary, (int)(numberOfCoefficients), finalCutOffFrequency, finalCutOffFrequency2, blackman, bandpass);
                }
                else if (equation == "0010")
                {
                    calculateCoefficients(lower_boundary, upper_boundary, (int)(numberOfCoefficients), finalCutOffFrequency, finalCutOffFrequency2, hamming, bandpass);
                }
                else if (equation == "0100")
                {
                    calculateCoefficients(lower_boundary, upper_boundary, (int)(numberOfCoefficients), finalCutOffFrequency, finalCutOffFrequency2, hanning, bandpass);
                }
                else if (equation == "1000")
                {
                    calculateCoefficients(lower_boundary, upper_boundary, (int)(numberOfCoefficients), finalCutOffFrequency, finalCutOffFrequency2, rectangular, bandpass);
                }
            }
            else if (InputFilterType == FILTER_TYPES.BAND_STOP)
            {
                finalCutOffFrequency = (float)((InputF1 - (InputTransitionBand / 2)) / InputFS);
                finalCutOffFrequency2 = (float)((InputF2 + (InputTransitionBand / 2)) / InputFS);
                if (equation == "0001")
                {
                    calculateCoefficients(lower_boundary, upper_boundary, (int)(numberOfCoefficients), finalCutOffFrequency, finalCutOffFrequency2, blackman, bandreject);
                }
                else if (equation == "0010")
                {
                    calculateCoefficients(lower_boundary, upper_boundary, (int)(numberOfCoefficients), finalCutOffFrequency, finalCutOffFrequency2, hamming, bandreject);
                }
                else if (equation == "0100")
                {
                    calculateCoefficients(lower_boundary, upper_boundary, (int)(numberOfCoefficients), finalCutOffFrequency, finalCutOffFrequency2, hanning, bandreject);
                }
                else if (equation == "1000")
                {
                    calculateCoefficients(lower_boundary, upper_boundary, (int)(numberOfCoefficients), finalCutOffFrequency, finalCutOffFrequency2, rectangular, bandreject);
                }
            }

            DirectConvolution dc = new DirectConvolution();
            dc.InputSignal1 = InputTimeDomainSignal;
            dc.InputSignal2 = OutputHn;
            dc.Run();
            OutputYn = dc.OutputConvolvedSignal;
        }

        //1000
        private float rectangular(int n, int N)
        {
            return (float)(1);
        }

        //0100
        private float hanning(int n, int N)
        {
            float result = (float)(0.5 + 0.5 * Math.Cos((float)(2 * Math.PI * n) / N));
            return result;
        }

        //0010
        private float hamming(int n, int N)
        {
            float result = (float)(0.54 + 0.46 * Math.Cos((float)(2 * Math.PI * n) / N));
            return result;
        }

        //0001
        private float blackman(int n, int N)
        {
            float result = (float)(0.42 + 0.5 * Math.Cos((float)(2 * Math.PI * n) / (N - 1)) + 0.08 * Math.Cos((float)(4 * Math.PI * n) / (N - 1)));
            return result;
        }
        private float lowpass(int n, float cutoffFrequency, float zero)
        {
            if (n != 0)
            {
                return (float)(2 * cutoffFrequency * ((Math.Sin(n * 2 * Math.PI * cutoffFrequency)) / (n * 2 * Math.PI * cutoffFrequency)));
            }
            else
            {
                return 2 * cutoffFrequency;
            }
        }
        private float highpass(int n, float cutoffFrequency, float zero)
        {
            if (n != 0)
            {
                return (float)(-1 * 2 * cutoffFrequency * ((Math.Sin(n * 2 * Math.PI * cutoffFrequency)) / (n * 2 * Math.PI * cutoffFrequency)));
            }
            else
            {
                return 1 - (2 * cutoffFrequency);
            }
        }
        private float bandpass(int n, float cutoffFrequency1, float cutoffFrequency2)
        {
            if (n != 0)
            {
                float res1 = (float)(2 * cutoffFrequency1 * ((Math.Sin(n * 2 * Math.PI * cutoffFrequency1)) / (n * 2 * Math.PI * cutoffFrequency1)));
                float res2 = (float)(2 * cutoffFrequency2 * ((Math.Sin(n * 2 * Math.PI * cutoffFrequency2)) / (n * 2 * Math.PI * cutoffFrequency2)));
                return res2 - res1;
            }
            else
            {
                return 2 * (cutoffFrequency2 - cutoffFrequency1);
            }
        }
        private float bandreject(int n, float cutoffFrequency1, float cutoffFrequency2)
        {
            if (n != 0)
            {
                float res1 = (float)(2 * cutoffFrequency1 * ((Math.Sin(n * 2 * Math.PI * cutoffFrequency1)) / (n * 2 * Math.PI * cutoffFrequency1)));
                float res2 = (float)(2 * cutoffFrequency2 * ((Math.Sin(n * 2 * Math.PI * cutoffFrequency2)) / (n * 2 * Math.PI * cutoffFrequency2)));
                return res1 - res2;
            }
            else
            {
                return 1 - (2 * (cutoffFrequency2 - cutoffFrequency1));
            }
        }
        private float numOfCoefficients(float N)
        {
            float final_num = (int)Math.Ceiling((float)N);
            if (final_num % 2 == 0)
            {
                final_num += 1;
            }
            return final_num;
        }
        private void calculateCoefficients(int small_index, int large_index, int coefficients, float FC1, float FC2, Func<int, int, float> window, Func<int, float, float, float> filter)
        {
            float h, w, coef;
            for (; small_index <= large_index; small_index++)
            {
                OutputHn.SamplesIndices.Add(small_index);
                w = window(small_index, coefficients);
                h = filter(small_index, FC1, FC2);
                coef = h * w;
                OutputHn.Samples.Add(coef);
            }
        }
    }
}