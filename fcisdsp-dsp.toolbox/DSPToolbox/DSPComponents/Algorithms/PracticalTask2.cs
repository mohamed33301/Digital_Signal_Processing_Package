using DSPAlgorithms.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace DSPAlgorithms.Algorithms
{
    public class PracticalTask2 : Algorithm
    {
        public String SignalPath { get; set; }
        public float Fs { get; set; }
        public float miniF { get; set; }
        public float maxF { get; set; } 
        public float newFs { get; set; }
        public int L { get; set; } //upsampling factor
        public int M { get; set; } //downsampling factor
        public Signal OutputFreqDomainSignal { get; set; }

        public override void Run()
        {
            string file_path = @"D:\Package\fcisdsp-dsp.toolbox\DSPToolbox\TES";
            Signal InputSignal = LoadSignal(SignalPath);

            FIR fir = new FIR();
            fir.InputFilterType = DSPAlgorithms.DataStructures.FILTER_TYPES.BAND_PASS;
            fir.InputFS = Fs;
            fir.InputStopBandAttenuation = 50;
            fir.InputTransitionBand = 500;


            //**
            //Bandpass fillter
            fir.InputF1 = miniF;
            fir.InputF2 = maxF;
            fir.InputTimeDomainSignal = InputSignal;
            fir.Run();
            Signal sigfir = fir.OutputYn;

            //save the signal after apply filter
            string full_path = file_path + "\\FilteredSignal.ds";
            SaveSignal(full_path, fir.OutputYn, false, false);
            //**
            //down sampling 
            List<float> samples = new List<float>();
            Signal sigsamplings = new Signal(samples, false);
            if (newFs >= 2 * maxF)
            {
                Sampling sampling = new Sampling();
                sampling.L = L;
                sampling.M = M;
                sampling.InputSignal = sigfir;
                sampling.Run();
                sigsamplings = sampling.OutputSignal;

                full_path = file_path + "\\sampleSignal.ds";
                SaveSignal(full_path, sigsamplings, false, false);
            }
            else
            {
                sigsamplings = fir.OutputYn;

            }
            //**
            // DC component --> mean of sample --> each sample-mean
            DC_Component dc = new DC_Component();
            dc.InputSignal = sigsamplings;
            dc.Run();
            Signal sigdc = dc.OutputSignal;
            full_path = file_path + "\\DC_COMPSignal.ds";
            SaveSignal(full_path, sigdc, false, false);
            //**
            // normilizer
            Normalizer normalization = new Normalizer();
            normalization.InputMaxRange = 1;
            normalization.InputMinRange = -1;
            normalization.InputSignal = sigdc;
            normalization.Run();
            Signal signorm = normalization.OutputNormalizedSignal;

            full_path = file_path + "\\signorm.ds";
            SaveSignal(full_path, signorm, false, false);

            //*
            // DFT
            DiscreteFourierTransform dft = new DiscreteFourierTransform();
            dft.InputSamplingFrequency = Fs;
            dft.InputTimeDomainSignal = signorm;
            dft.Run();
            OutputFreqDomainSignal = dft.OutputFreqDomainSignal;

            full_path = file_path + "\\DFT_COMPSignal.ds";
            SaveSignal(full_path, OutputFreqDomainSignal, true, false);  // true ( freq domain)

        }
        /// <summary>
        /// /.............................................................................
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public Signal LoadSignal(string filePath)
        {
            Stream stream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            var sr = new StreamReader(stream);

            var sigType = byte.Parse(sr.ReadLine());
            var isPeriodic = byte.Parse(sr.ReadLine());
            long N1 = long.Parse(sr.ReadLine());

            List<float> SigSamples = new List<float>(unchecked((int)N1));
            List<int> SigIndices = new List<int>(unchecked((int)N1));
            List<float> SigFreq = new List<float>(unchecked((int)N1));
            List<float> SigFreqAmp = new List<float>(unchecked((int)N1));
            List<float> SigPhaseShift = new List<float>(unchecked((int)N1));

            if (sigType == 1)
            {
                SigSamples = null;
                SigIndices = null;
            }
            int i = 0;
            while(i < N1)
            {
                if (sigType == 0 || sigType == 2)
                {
                    var timeIndex_SampleAmplitude = sr.ReadLine().Split();
                    SigIndices.Add(int.Parse(timeIndex_SampleAmplitude[0]));
                    SigSamples.Add(float.Parse(timeIndex_SampleAmplitude[1]));
                }
                else
                {
                    var Freq_Amp_PhaseShift = sr.ReadLine().Split();
                    SigFreq.Add(float.Parse(Freq_Amp_PhaseShift[0]));
                    SigFreqAmp.Add(float.Parse(Freq_Amp_PhaseShift[1]));
                    SigPhaseShift.Add(float.Parse(Freq_Amp_PhaseShift[2]));
                }
                i++;
            }

            if (!sr.EndOfStream)
            {
                long N2 = long.Parse(sr.ReadLine());

                int j = 0;
                while(j < N2)
                {
                    var Freq_Amp_PhaseShift = sr.ReadLine().Split();
                    SigFreq.Add(float.Parse(Freq_Amp_PhaseShift[0]));
                    SigFreqAmp.Add(float.Parse(Freq_Amp_PhaseShift[1]));
                    SigPhaseShift.Add(float.Parse(Freq_Amp_PhaseShift[2]));
                    j++;
                }
            }

            stream.Close();
            return new Signal(SigSamples, SigIndices, isPeriodic == 1, SigFreq, SigFreqAmp, SigPhaseShift);
        }
        public void SaveSignal(string filePath, Signal sig, bool flag_freq_or_time, bool periodic_or_not)
        {

            using (StreamWriter writer = new StreamWriter(filePath))
            {
                if (flag_freq_or_time == false) //flag_freq_or_time : if freq print 1 else print 0
                {
                    writer.WriteLine(0);
                }
                else
                {
                    writer.WriteLine(1);
                }
                if (periodic_or_not == false) //periodic_or_not : if preodic print 1 else print 0
                {
                    writer.WriteLine(0);
                }
                else
                {
                    writer.WriteLine(1);
                }



                if (flag_freq_or_time == true) // signal in Freq
                {
                    writer.WriteLine(sig.FrequenciesAmplitudes.Count());
                    int i = 0;

                    while (i < sig.FrequenciesAmplitudes.Count())
                    {
                        writer.Write(sig.Frequencies[i]);
                        writer.Write(" ");
                        writer.Write(sig.FrequenciesAmplitudes[i]);
                        writer.Write(" ");
                        writer.Write(sig.FrequenciesPhaseShifts[i]);
                        writer.WriteLine();
                        i++;
                    }

                }
                else  //signal in time domain // signal in Freq
                {
                    writer.WriteLine(sig.Samples.Count());
                    int i = 0;
                    while (i < sig.Samples.Count())
                    {
                        writer.Write(sig.SamplesIndices[i]);
                        writer.Write(" ");
                        writer.Write(sig.Samples[i]);
                        writer.WriteLine();
                        i++;
                    }
                }

            }
        }
    }
}





//using DSPAlgorithms.DataStructures;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.IO;
//using DocumentFormat.OpenXml.Bibliography;

//namespace DSPAlgorithms.Algorithms
//{
//    public class PracticalTask2 : Algorithm
//    {
//        public String SignalPath { get; set; }
//        public float Fs { get; set; }
//        public float miniF { get; set; }
//        public float maxF { get; set; }
//        public float newFs { get; set; }
//        public int L { get; set; } //upsampling factor
//        public int M { get; set; } //downsampling factor
//        public Signal OutputFreqDomainSignal { get; set; }

//        public override void Run()
//        {

//            Signal InputSignal = LoadSignal(SignalPath);

//            /*InverseDiscreteFourierTransform idft = new InverseDiscreteFourierTransform();
//            idft.InputFreqDomainSignal = InputSignal;
//            idft.Run();


//            Signal t_domain_signal = idft.OutputTimeDomainSignal;

//            */


//            //display 1
//            StreamWriter str = new StreamWriter("C:\\Users\\moham\\OneDrive\\Desktop\\test.txt", false);

//            for (int i = 0; i < InputSignal.Samples.Count; i++)
//            {
//                str.WriteLine("Sample : \n" + InputSignal.Samples[i]);
//            }
//            str.Close();

//            FIR filter = new FIR();
//            filter.InputFilterType = FILTER_TYPES.BAND_PASS;
//            filter.InputF1 = miniF;
//            filter.InputF2 = maxF;
//            filter.InputStopBandAttenuation = 50;
//            filter.InputTransitionBand = 500;
//            filter.InputCutOffFrequency = 1500;
//            filter.InputTimeDomainSignal = InputSignal;
//            filter.Run();
//            InputSignal = filter.OutputYn;

//            Sampling resampling = new Sampling();

//            if (newFs >= 2 * maxF)
//            {

//                resampling.InputSignal = InputSignal;
//                resampling.L = L;
//                resampling.M = M;
//                resampling.Run();
//            }
//            Signal removed_signal = resampling.OutputSignal;

//            Signal DC_removed_comp = new Signal(new List<float>(), false);

//            for (int i = 0; i < removed_signal.Samples.Count; i++)
//                DC_removed_comp.Samples.Add(removed_signal.Samples[i] + removed_signal.Samples.Average());

//            //display from 4
//            for (int i = 0; i < DC_removed_comp.Samples.Count; i++)
//            {
//                str.WriteLine("DC_removed_comp : \n" + DC_removed_comp.Samples[i]);
//            }
//            str.Close();

//            Signal normalized = new Signal(new List<float>(), false);

//            Normalizer normalizer = new Normalizer();
//            normalizer.InputSignal = removed_signal;
//            normalizer.InputMinRange = -1;
//            normalizer.InputMaxRange = 1;
//            normalizer.Run();

//            normalized = normalizer.OutputNormalizedSignal;

//            //display from 6
//            for (int i = 0; i < normalized.Samples.Count; i++)
//            {
//                str.WriteLine("Normalizer : \n" + normalized.Samples[i]);
//            }
//            str.Close();

//            DiscreteFourierTransform dft = new DiscreteFourierTransform();
//            dft.InputTimeDomainSignal = normalized;
//            dft.InputSamplingFrequency = newFs;
//            dft.Run();

//            normalized = dft.OutputFreqDomainSignal;
//            //display from 8

//            //StreamWriter str = new StreamWriter("C:\\Users\\moham\\OneDrive\\Desktop\\test.txt", false);
//            for (int i = 0; i < normalized.Samples.Count; i++)
//            {
//                str.WriteLine("Last output : \n" + normalized.Samples[i]);
//            }
//            str.Close();
//        }


//        public Signal LoadSignal(string filePath)
//        {
//            Stream stream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
//            var sr = new StreamReader(stream);

//            var sigType = byte.Parse(sr.ReadLine());
//            var isPeriodic = byte.Parse(sr.ReadLine());
//            long N1 = long.Parse(sr.ReadLine());

//            List<float> SigSamples = new List<float>(unchecked((int)N1));
//            List<int> SigIndices = new List<int>(unchecked((int)N1));
//            List<float> SigFreq = new List<float>(unchecked((int)N1));
//            List<float> SigFreqAmp = new List<float>(unchecked((int)N1));
//            List<float> SigPhaseShift = new List<float>(unchecked((int)N1));

//            if (sigType == 1)
//            {
//                SigSamples = null;
//                SigIndices = null;
//            }

//            for (int i = 0; i < N1; i++)
//            {
//                if (sigType == 0 || sigType == 2)
//                {
//                    var timeIndex_SampleAmplitude = sr.ReadLine().Split();
//                    SigIndices.Add(int.Parse(timeIndex_SampleAmplitude[0]));
//                    SigSamples.Add(float.Parse(timeIndex_SampleAmplitude[1]));
//                }
//                else
//                {
//                    var Freq_Amp_PhaseShift = sr.ReadLine().Split();
//                    SigFreq.Add(float.Parse(Freq_Amp_PhaseShift[0]));
//                    SigFreqAmp.Add(float.Parse(Freq_Amp_PhaseShift[1]));
//                    SigPhaseShift.Add(float.Parse(Freq_Amp_PhaseShift[2]));
//                }
//            }

//            if (!sr.EndOfStream)
//            {
//                long N2 = long.Parse(sr.ReadLine());

//                for (int i = 0; i < N2; i++)
//                {
//                    var Freq_Amp_PhaseShift = sr.ReadLine().Split();
//                    SigFreq.Add(float.Parse(Freq_Amp_PhaseShift[0]));
//                    SigFreqAmp.Add(float.Parse(Freq_Amp_PhaseShift[1]));
//                    SigPhaseShift.Add(float.Parse(Freq_Amp_PhaseShift[2]));
//                }
//            }

//            stream.Close();
//            return new Signal(SigSamples, SigIndices, isPeriodic == 1, SigFreq, SigFreqAmp, SigPhaseShift);
//        }
//        /// <summary>Plot a list of {TimeMagnitude} points.
//        /// </summary>
//        /// <param name="parameters">Parameters of the signal to be plotted.</param>
//        /// <param name="list">List containing the (time, magnitude) points.</param>
//        /// <param name="continuity">Continuity of the signal.
//        /// </param>
//        /*public void Plot(Signal s)
//        {
//            int n, m;

//            if (s != null && (n = s.Samples.Count) != 0)
//            {
//                int x, deltaX, currY, nextY;

//                // Increasing signal-magnitude values are drawn from the
//                // bottom of the {Bitmap} to its top.
//                sigMax_Y = 0;
//                sigMin_Y = bmp.Height;

//                Draw_X_axis();
//                Draw_Y_axis();

//                drawFormat.FormatFlags = StringFormatFlags.DirectionRightToLeft;

//                DrawParameters(parameters, shape);

//                deltaX = this.Width / n;
//                x = 0;
//                m = n - 2;

//                drawFormat.FormatFlags = StringFormatFlags.DirectionVertical;

//                for (int i = 0; i < n; ++i)
//                {
//                    int iScaledMag = ScaledMagnitude(list[i], dScale);

//                    currY = xAxis_Y - iScaledMag;

//                    if (currY > sigMax_Y)
//                    {
//                        sigMax_Y = currY;
//                    }
//                    if (currY < sigMin_Y)
//                    {
//                        sigMin_Y = currY;
//                    }
//                    if (x >= bmp.Width)
//                    {
//                        break;
//                    }
//                    bmp.SetPixel(x, currY, Color.Black);

//                    if (UtilFn.IsDivisible(list[i].Time, parameters.period))
//                    {
//                        string label = String.Format("_ {0:0.0000}", list[i].Time);
//                        SizeF size = gr.MeasureString(label, drawFont);

//                        gr.DrawString(label, drawFont, Brushes.Red,
//                                       new Point(x, bmp.Height - (int)size.Width),
//                                       drawFormat);
//                    }
//                    if (continuity == SignalContinuity.discontinuous && i <= m)
//                    {
//                        int iP1ScaledMag = ScaledMagnitude(list[i + 1], dScale);

//                        nextY = xAxis_Y - iP1ScaledMag;

//                        if (x > 0 && (shape == SignalShape.square ||
//                                        shape == SignalShape.sawtooth))
//                        {
//                            if (i < m)
//                            {
//                                CheckVerticalDiscontinuity(x, currY, nextY);
//                            }
//                            else // i == m
//                            {
//                                DrawVerticalDiscontinuity(x + deltaX, currY);
//                            }
//                        }
//                    }
//                    x += deltaX;
//                }
//                Draw_Y_axisNotches(parameters);
//                this.ShowDialog();                // Display form in modal mode.
//            }
//        }*/
//    }

//}

/*﻿using DSPAlgorithms.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace DSPAlgorithms.Algorithms
{
    public class PracticalTask2 : Algorithm
    {
        public String SignalPath { get; set; }
        public float Fs { get; set; }
        public float miniF { get; set; }
        public float maxF { get; set; }
        public float newFs { get; set; }
        public int L { get; set; } //upsampling factor
        public int M { get; set; } //downsampling factor
        public Signal OutputFreqDomainSignal { get; set; }

        public override void Run()
        {
            Signal InputSignal = LoadSignal(SignalPath);

            throw new NotImplementedException();
        }

        public Signal LoadSignal(string filePath)
        {
            Stream stream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            var sr = new StreamReader(stream);

            var sigType = byte.Parse(sr.ReadLine());
            var isPeriodic = byte.Parse(sr.ReadLine());
            long N1 = long.Parse(sr.ReadLine());

            List<float> SigSamples = new List<float>(unchecked((int)N1));
            List<int> SigIndices = new List<int>(unchecked((int)N1));
            List<float> SigFreq = new List<float>(unchecked((int)N1));
            List<float> SigFreqAmp = new List<float>(unchecked((int)N1));
            List<float> SigPhaseShift = new List<float>(unchecked((int)N1));

            if (sigType == 1)
            {
                SigSamples = null;
                SigIndices = null;
            }

            for (int i = 0; i < N1; i++)
            {
                if (sigType == 0 || sigType == 2)
                {
                    var timeIndex_SampleAmplitude = sr.ReadLine().Split();
                    SigIndices.Add(int.Parse(timeIndex_SampleAmplitude[0]));
                    SigSamples.Add(float.Parse(timeIndex_SampleAmplitude[1]));
                }
                else
                {
                    var Freq_Amp_PhaseShift = sr.ReadLine().Split();
                    SigFreq.Add(float.Parse(Freq_Amp_PhaseShift[0]));
                    SigFreqAmp.Add(float.Parse(Freq_Amp_PhaseShift[1]));
                    SigPhaseShift.Add(float.Parse(Freq_Amp_PhaseShift[2]));
                }
            }

            if (!sr.EndOfStream)
            {
                long N2 = long.Parse(sr.ReadLine());

                for (int i = 0; i < N2; i++)
                {
                    var Freq_Amp_PhaseShift = sr.ReadLine().Split();
                    SigFreq.Add(float.Parse(Freq_Amp_PhaseShift[0]));
                    SigFreqAmp.Add(float.Parse(Freq_Amp_PhaseShift[1]));
                    SigPhaseShift.Add(float.Parse(Freq_Amp_PhaseShift[2]));
                }
            }

            stream.Close();
            return new Signal(SigSamples, SigIndices, isPeriodic == 1, SigFreq, SigFreqAmp, SigPhaseShift);
        }
    }
}*/
