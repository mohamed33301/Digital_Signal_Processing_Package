using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class TimeDelay : Algorithm
    {
        public Signal InputSignal1 { get; set; }
        public Signal InputSignal2 { get; set; }
        public float InputSamplingPeriod { get; set; }
        public float OutputTimeDelay { get; set; }

        public override void Run()
        {
            DirectCorrelation dc = new DirectCorrelation();

            dc.InputSignal1 = InputSignal1;
            dc.InputSignal2 = InputSignal2;

            dc.Run();
            float maxi = 0;
            int conu = 0;
            for (int i = 0; i < dc.OutputNonNormalizedCorrelation.Count; i++)
            {
                if (dc.OutputNonNormalizedCorrelation[i] > maxi)
                {
                    maxi = dc.OutputNonNormalizedCorrelation[i];
                    conu = i;
                }
            }
            OutputTimeDelay = conu * InputSamplingPeriod;

        }
    }
}
