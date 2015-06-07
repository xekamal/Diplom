using System;
using Simulator.Neuro.Domain;
using Simulator.Map;

namespace Simulator.Neuro.Infrastructure
{
    public class CrossroadControllerStaticTimer : ICrossroadController
    {
        private ICrossroad _crossroad;
        private int _stepNumber;
        private int _nofStepsBeforeSwitch;

        public CrossroadControllerStaticTimer(ICrossroad crossroad)
        {
            _crossroad = crossroad;
            _stepNumber = 0;
            _nofStepsBeforeSwitch = 3;

            InitCrossroad();
        }

        public CrossroadControllerStaticTimer(ICrossroad crossroad, int nofStepsBeforeSwitch)
        {
            _crossroad = crossroad;
            _stepNumber = 0;
            _nofStepsBeforeSwitch = nofStepsBeforeSwitch;

            InitCrossroad();
        }

        public void Step()
        {
            _stepNumber++;

            Switch4StatesOneByOne();
        }

        public void Reinforce(double value)
        {
        }

        public void Reinforce(double v0, double v1, double v2, double v3, double v4, double v5, double v6, double v7, double v8, double v9, double v10, double v11)
        {
        }

        public void Reinforce(double[] values)
        {
        }

        private void InitCrossroad()
        {
            _crossroad.DownToLeftTrafficLight.SetTrafficLightState(TrafficLightState.Green);
            _crossroad.RightToLeftTrafficLight.SetTrafficLightState(TrafficLightState.Green);
        }

        private void Switch4StatesOneByOne()
        {
            if (_stepNumber % _nofStepsBeforeSwitch == 0)
            {
                if (_crossroad.DownToLeftTrafficLight.State == TrafficLightState.Green)
                {
                    _crossroad.DownToLeftTrafficLight.SetTrafficLightState(TrafficLightState.Red);
                    _crossroad.RightToLeftTrafficLight.SetTrafficLightState(TrafficLightState.Red);
                    _crossroad.RightToDownTrafficLight.SetTrafficLightState(TrafficLightState.Green);
                    _crossroad.UpToDownTrafficLight.SetTrafficLightState(TrafficLightState.Green);
                }
                else if (_crossroad.RightToDownTrafficLight.State == TrafficLightState.Green)
                {
                    _crossroad.RightToDownTrafficLight.SetTrafficLightState(TrafficLightState.Red);
                    _crossroad.UpToDownTrafficLight.SetTrafficLightState(TrafficLightState.Red);
                    _crossroad.LeftToRightTrafficLight.SetTrafficLightState(TrafficLightState.Green);
                    _crossroad.UpToRightTrafficLight.SetTrafficLightState(TrafficLightState.Green);
                }
                else if (_crossroad.LeftToRightTrafficLight.State == TrafficLightState.Green)
                {
                    _crossroad.LeftToRightTrafficLight.SetTrafficLightState(TrafficLightState.Red);
                    _crossroad.UpToRightTrafficLight.SetTrafficLightState(TrafficLightState.Red);
                    _crossroad.LeftToUpTrafficLight.SetTrafficLightState(TrafficLightState.Green);
                    _crossroad.DownToUpTrafficLight.SetTrafficLightState(TrafficLightState.Green);
                }
                else if (_crossroad.LeftToUpTrafficLight.State == TrafficLightState.Green)
                {
                    _crossroad.LeftToUpTrafficLight.SetTrafficLightState(TrafficLightState.Red);
                    _crossroad.DownToUpTrafficLight.SetTrafficLightState(TrafficLightState.Red);
                    _crossroad.DownToLeftTrafficLight.SetTrafficLightState(TrafficLightState.Green);
                    _crossroad.RightToLeftTrafficLight.SetTrafficLightState(TrafficLightState.Green);
                }
            }
        }
    }
}
