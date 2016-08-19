using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulator
{
    class Controller
    {
        public double CalculateControlVariable(double time, double targetVelocity, double currentVelocity)
        {
            double algo = 0;
            if (currentVelocity > targetVelocity)
                return currentVelocity / (time * time);
            if (targetVelocity > currentVelocity)
                return targetVelocity / (time / 2);
            //else
            //    return time + (targetVelocity*currentVelocity);
            return algo;
        }
    }
}
