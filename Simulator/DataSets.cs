using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Simulator
{
    static class DataSets
    {
        public static Dictionary<string, DataSection[]> CreateTargetVelocities()
        {
            var random = new Random();
            int velocity = random.Next(1, 100);
            decimal maxVelocity = (decimal) velocity / 10;

            decimal startTime = random.Next(1, 10);
            decimal duration = random.Next(1, 10);

            return new Dictionary<string, DataSection[]>
            {
                {
                    "Flat", new[]
                    {
                        new DataSection {duration = 40, offset = 10, gradient = 0 }
                    }
                },
                {
                    "One change", new[]
                    {
                        new DataSection {duration = 5, offset = 0, gradient = 0},
                        new DataSection {duration = 35, offset = 15, gradient = 0}
                    }
                },
                {
                    "Two changes", new[]
                    {
                        new DataSection {duration = startTime, offset = 0, gradient = 0},
                        new DataSection {duration = duration, offset = maxVelocity, gradient = 0},
                        new DataSection {duration = 40 - (startTime + duration), offset = -(maxVelocity * 0.7m), gradient = 0}
                    }
                },
                {
                    "Sloped", new[]
                    {
                        new DataSection {duration = 1, offset = 0, gradient = 0},
                        new DataSection {duration = 19, offset = 0, gradient = 1},
                        new DataSection {duration = 20, offset = 0, gradient = -0.5m}
                    }
                },
                {
                    "Sine wave", new[]
                    {
                        new DataSection {duration = 1, offset = 5, gradient = 0.9m},
                        new DataSection {duration = 1, offset = 0, gradient = 0.8m},
                        new DataSection {duration = 1, offset = 0, gradient = 0.7m},
                        new DataSection {duration = 1, offset = 0, gradient = 0.6m},
                        new DataSection {duration = 1, offset = 0, gradient = 0.5m},
                        new DataSection {duration = 1, offset = 0, gradient = 0.4m},
                        new DataSection {duration = 1, offset = 0, gradient = 0.3m},
                        new DataSection {duration = 1, offset = 0, gradient = 0.2m},
                        new DataSection {duration = 1, offset = 0, gradient = 0.1m},
                        new DataSection {duration = 1, offset = 0, gradient = 0.0m},
                        new DataSection {duration = 1, offset = 0, gradient = -0.1m},
                        new DataSection {duration = 1, offset = 0, gradient = -0.2m},
                        new DataSection {duration = 1, offset = 0, gradient = -0.3m},
                        new DataSection {duration = 1, offset = 0, gradient = -0.4m},
                        new DataSection {duration = 1, offset = 0, gradient = -0.5m},
                        new DataSection {duration = 1, offset = 0, gradient = -0.6m},
                        new DataSection {duration = 1, offset = 0, gradient = -0.7m},
                        new DataSection {duration = 1, offset = 0, gradient = -0.8m},
                        new DataSection {duration = 1, offset = 0, gradient = -0.9m},
                        new DataSection {duration = 1, offset = 0, gradient = -0.8m},
                        new DataSection {duration = 1, offset = 0, gradient = -0.7m},
                        new DataSection {duration = 1, offset = 0, gradient = -0.6m},
                        new DataSection {duration = 1, offset = 0, gradient = -0.5m},
                        new DataSection {duration = 1, offset = 0, gradient = -0.4m},
                        new DataSection {duration = 1, offset = 0, gradient = -0.3m},
                        new DataSection {duration = 1, offset = 0, gradient = -0.2m},
                        new DataSection {duration = 1, offset = 0, gradient = -0.1m},
                        new DataSection {duration = 1, offset = 0, gradient = 0.0m},
                        new DataSection {duration = 1, offset = 0, gradient = 0.1m},
                        new DataSection {duration = 1, offset = 0, gradient = 0.2m},
                        new DataSection {duration = 1, offset = 0, gradient = 0.3m},
                        new DataSection {duration = 1, offset = 0, gradient = 0.4m},
                        new DataSection {duration = 1, offset = 0, gradient = 0.5m},
                        new DataSection {duration = 1, offset = 0, gradient = 0.6m},
                        new DataSection {duration = 1, offset = 0, gradient = 0.7m},
                        new DataSection {duration = 1, offset = 0, gradient = 0.8m},
                        new DataSection {duration = 1, offset = 0, gradient = 0.9m},
                        new DataSection {duration = 1, offset = 0, gradient = 0.8m},
                        new DataSection {duration = 1, offset = 0, gradient = 0.7m},
                        new DataSection {duration = 1, offset = 0, gradient = 0.6m}
                    }
                }
            };
        }
    }
}
