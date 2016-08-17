using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using OxyPlot;

namespace Simulator
{
    class Simulation : INotifyPropertyChanged
    {

        private readonly bool calcualteBlocked;

        private double[] targetVelocityForDataPoint;

        private readonly Dictionary<string, DataSection[]> TargetVelocities;

        public Simulation()
        {
            calcualteBlocked = true;

            TargetVelocities = DataSets.CreateTargetVelocities();
            SelectedVelocityTarget = "Two changes (random)";

            ControllerTimeInterval = 0.1m;
            TimeMinimum = 0;
            TimeMaximum = 40;

            VelocityDisplayMinimum = 0;
            VelocityDisplayMaximum = 20;

            Mass = 0.1m;
            KDrag = 0.003m;

            InitialVelocity = 0;

            OutputLag = 5;

            calcualteBlocked = false;
            Calculate();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertChanged([CallerMemberName] string name = "")
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler.Invoke(this, new PropertyChangedEventArgs(name));
            }
        }

        #region Creating data sets

        private void CreateTargetVelocityPoints()
        {
            var velocitySections = TargetVelocities[SelectedVelocityTarget];
            TargetVelocityPoints = DataSetConverter.ConvertSectionsToGraphData(velocitySections);
            targetVelocityForDataPoint = DataSetConverter.ConvertSectionsToTimeDataSet(
                VelocityDataPoints, DataPointTickInterval,
                velocitySections, DataSetConverter.DataPointForTimeFromSections);
        }

        #endregion Creating data sets

        #region Selected Velocity Target

        public IEnumerable<string> PossibleVelocityTargets
        {
            get { return TargetVelocities.Keys; }
        }

        private string _selectedVelocityTarget;
        public string SelectedVelocityTarget
        {
            get
            {
                return _selectedVelocityTarget;

            }
            set
            {
                _selectedVelocityTarget = value;
                CreateTargetVelocityPoints();
                Calculate();
            }
        }

        #endregion Selected Velocity Target

        #region Controller Settings

        private decimal _controllerTimeInterval;
        public decimal ControllerTimeInterval
        {
            get { return _controllerTimeInterval; }
            set
            {
                _controllerTimeInterval = value;
                OnPropertChanged();
                CreateTargetVelocityPoints();
                Calculate();
            }
        }

        #endregion Controller Settings

        #region System Settings

        private decimal _mass;
        public decimal Mass
        {
            get { return _mass; }
            set
            {
                _mass = value;
                OnPropertChanged();
                Calculate();
            }
        }

        private decimal _initialVelocity;
        public decimal InitialVelocity
        {
            get { return _initialVelocity; }
            set
            {
                _initialVelocity = value;
                OnPropertChanged();
                Calculate();
            }
        }

        private decimal _kDrag;
        public decimal KDrag
        {
            get { return _kDrag; }
            set
            {
                _kDrag = value;
                OnPropertChanged();
                Calculate();
            }
        }

        private uint _outputLag;
        public uint OutputLag
        {
            get { return _outputLag; }
            set
            {
                _outputLag = value;
                OnPropertChanged();
                Calculate();
            }
        }

        #endregion System Settings

        #region Axis limits

        private double _timeMinimum;
        public double TimeMinimum
        {
            get { return _timeMinimum; }
            set
            {
                _timeMinimum = value;
                OnPropertChanged();
            }
        }

        private double _timeMaximum;
        public double TimeMaximum
        {
            get { return _timeMaximum; }
            set
            {
                _timeMaximum = value;
                OnPropertChanged();
            }
        }

        public double TimeMaximumMaximum
        {
            get { return 40.0; }
        }

        private double _velocityDisplayMinimum;
        public double VelocityDisplayMinimum
        {
            get { return _velocityDisplayMinimum; }
            set
            {
                _velocityDisplayMinimum = value;
                OnPropertChanged();
            }
        }

        private double _velocityDisplayMaximum;
        public double VelocityDisplayMaximum
        {
            get { return _velocityDisplayMaximum; }
            set
            {
                _velocityDisplayMaximum = value;
                OnPropertChanged();
            }
        }
        #endregion Axis limits

        #region Graph Data

        private IList<DataPoint> _targetVelocityPoints;
        public IList<DataPoint> TargetVelocityPoints
        {
            get { return _targetVelocityPoints; }
            private set
            {
                _targetVelocityPoints = value;
                OnPropertChanged();
            }
        }

        private IList<DataPoint> _actualVelocityPoints;
        public IList<DataPoint> ActualVelocityPoints
        {
            get { return _actualVelocityPoints; }
            private set
            {
                _actualVelocityPoints = value;
                OnPropertChanged();
            }
        }

        private IList<DataPoint> _controlVariablePoints;
        public IList<DataPoint> ControlVariablePoints
        {
            get { return _controlVariablePoints; }
            private set
            {
                _controlVariablePoints = value;
                OnPropertChanged();
            }
        }

        #endregion Graph Data

        private double Drag(int timeIndex, double velocity)
        {
            return (double)KDrag * velocity * velocity;
        }

        private double VelocityChangeForForce(double force, double interval)
        {
            return (force / (double)Mass) * interval;
        }

        private int VelocityDataPoints
        {
            get { return (int) (TimeMaximumMaximum / (double) ControllerTimeInterval) * 10; }
        }

        private double DataPointTickInterval
        {
            get { return (double) ControllerTimeInterval / 10; }
        }

        private void Calculate()
        {
            if (calcualteBlocked)
                return;

            var controller = new Controller();

            var actualVelocityData = new List<DataPoint>();
            var controlVariableData = new List<DataPoint>();

            double currentVelocity = (double)InitialVelocity;
            double[] outputForce = new double[OutputLag + 1];
            uint currentOutputIndex = 0;

            for (int t = 0; t < VelocityDataPoints; t++)
            {
                double time = t * DataPointTickInterval;
                double drag = Drag(t, currentVelocity);
                currentVelocity += VelocityChangeForForce(
                    outputForce[currentOutputIndex] - drag, DataPointTickInterval);

                if (t % 10 == 0)
                {
                    outputForce[currentOutputIndex] = controller.CalculateControlVariable(
                        DataPointTickInterval * t,
                        targetVelocityForDataPoint[t],
                        currentVelocity);

                    controlVariableData.Add(new DataPoint(time, outputForce[currentOutputIndex]));

                    currentOutputIndex++;
                    currentOutputIndex %= (OutputLag + 1);
                }

                actualVelocityData.Add(new DataPoint(time, currentVelocity));
            }

            ActualVelocityPoints = actualVelocityData;
            ControlVariablePoints = controlVariableData;
        }

    }
}
