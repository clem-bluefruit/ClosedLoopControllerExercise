using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using OxyPlot;

namespace Simulator
{
    internal struct DataSection
    {
        public decimal duration;
        public decimal offset;
        public decimal gradient;
    }

    static class DataSetConverter
    {
        public static double DataPointForTimeFromSections(DataSection[] dataSections, double t)
        {
            decimal timePoint = (decimal)t;
            decimal target = 0;
            decimal time = 0;
            foreach (var p in dataSections)
            {
                target += p.offset;
                if ((time + p.duration) < timePoint)
                {
                    target += (p.duration * p.gradient);
                    time += p.duration;
                }
                else
                {
                    var s = timePoint - time;
                    target += (s * p.gradient);
                    break;
                }
            }
            return (double)target;
        }

        public static IList<DataPoint> ConvertSectionsToGraphData(DataSection[] dataSections)
        {
            var points = new List<DataPoint>();
            decimal target = 0;
            decimal time = 0;
            foreach (var p in dataSections)
            {
                target += p.offset;
                points.Add(new DataPoint((double)time, (double)target));

                time += p.duration;
                target += (p.gradient * p.duration);
                points.Add(new DataPoint((double)time, (double)target));
            }

            return points;
        }

        public static double[] ConvertSectionsToTimeDataSet(int velocityDataPoints, double dataPointTickInterval,
            DataSection[] dataSections, Func<DataSection[], double, double> valueForTime)
        {
            var data = new double[velocityDataPoints];
            for (int t = 0; t < velocityDataPoints; t++)
                data[t] = valueForTime(dataSections, dataPointTickInterval * t);

            return data;
        }
    }
}
