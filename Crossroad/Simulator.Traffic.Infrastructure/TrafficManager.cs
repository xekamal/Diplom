using System;
using System.Collections.Generic;
using Simulator.Map;
using Simulator.Map.Infrastructure;
using Simulator.Traffic.Domain;

namespace Simulator.Traffic.Infrastructure
{
    public class TrafficManager : ITrafficManager
    {
        private const double Epsilon = 0.000001;
        private readonly IMap _map;
        private readonly IList<ITrafficFlow> _trafficFlows;

        public TrafficManager(IMap map)
        {
            _map = map;
            _trafficFlows = new List<ITrafficFlow>();
        }

        public void AddTrafficFlow(ITrafficFlow trafficFlow)
        {
            _trafficFlows.Add(trafficFlow);
        }

        public void CalculateTrafficData()
        {
            for (int row = 0; row < _map.NofRows; row++)
            {
                for (int column = 0; column < _map.NofColumns; column++)
                {
                    IMapElement roadElement = _map.GetElement(row, column);
                    if (roadElement == null || !(roadElement is ICrossroad))
                    {
                        continue;
                    }

                    var crossroadLocation = new Location(row, column);
                    var crossroad = roadElement as ICrossroad;
                    ZeroCrossroadTrafficData(crossroad);

                    foreach (ITrafficFlow trafficFlow in _trafficFlows)
                    {
                        if (trafficFlow.Path.Contains(crossroadLocation))
                        {
                            for (int pathIndex = 0; pathIndex < trafficFlow.Path.Count; pathIndex++)
                            {
                                ILocation pathElementLocation = trafficFlow.Path[pathIndex];
                                IMapElement pathElement = _map.GetElement(pathElementLocation.Row,
                                    pathElementLocation.Column);

                                if (pathElement is ICrossroad)
                                {
                                    if (crossroad == pathElement)
                                    {
                                        AddCrossroadTrafficData(trafficFlow.Path[pathIndex - 1], pathElementLocation,
                                            trafficFlow.Path[pathIndex + 1], crossroad, trafficFlow);
                                        break;
                                    }

                                    if (!IsPathOpen(trafficFlow.Path[pathIndex - 1], pathElementLocation,
                                        trafficFlow.Path[pathIndex + 1], (ICrossroad) pathElement))
                                    {
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        public void CalculateTrafficData(double seconds)
        {
            for (int row = 0; row < _map.NofRows; row++)
            {
                for (int column = 0; column < _map.NofColumns; column++)
                {
                    IMapElement roadElement = _map.GetElement(row, column);
                    if (roadElement == null || !(roadElement is ICrossroad))
                    {
                        continue;
                    }

                    var crossroadLocation = new Location(row, column);
                    var crossroad = roadElement as ICrossroad;
                    var crossroadCopy = crossroad.Clone() as ICrossroad;
                    ZeroCrossroadTrafficData(crossroad);

                    foreach (ITrafficFlow trafficFlow in _trafficFlows)
                    {
                        if (trafficFlow.Path.Contains(crossroadLocation))
                        {
                            for (int pathIndex = 0; pathIndex < trafficFlow.Path.Count; pathIndex++)
                            {
                                ILocation pathElementLocation = trafficFlow.Path[pathIndex];
                                IMapElement pathElement = _map.GetElement(pathElementLocation.Row,
                                    pathElementLocation.Column);

                                if (pathElement is ICrossroad)
                                {
                                    if (crossroad == pathElement)
                                    {
                                        AddCrossroadTrafficData(trafficFlow.Path[pathIndex - 1], pathElementLocation,
                                            trafficFlow.Path[pathIndex + 1], crossroad, trafficFlow);
                                        break;
                                    }

                                    if (!IsPathOpen(trafficFlow.Path[pathIndex - 1], pathElementLocation,
                                        trafficFlow.Path[pathIndex + 1], (ICrossroad) pathElement))
                                    {
                                        break;
                                    }
                                }
                            }
                        }
                    }

                    UpdateDensityAndSpeed(crossroad.LeftToRightTrafficLight, crossroadCopy.LeftToRightTrafficData,
                        crossroad.LeftToRightTrafficData, seconds);
                }
            }
        }

        public void CalculateNofPassedCars(double seconds)
        {
            for (int row = 0; row < _map.NofRows; row++)
            {
                for (int column = 0; column < _map.NofColumns; column++)
                {
                    IMapElement roadElement = _map.GetElement(row, column);
                    if (roadElement == null || !(roadElement is ICrossroad))
                    {
                        continue;
                    }

                    var crossroad = roadElement as ICrossroad;

                    if (crossroad.LeftToUpTrafficLight.State == TrafficLightState.Green)
                    {
                        crossroad.LeftToUpNofPassedCars += GetNofCars(crossroad.LeftToUpTrafficData, seconds);
                    }
                    if (crossroad.LeftToRightTrafficLight.State == TrafficLightState.Green)
                    {
                        crossroad.LeftToRightNofPassedCars += GetNofCars(crossroad.LeftToRightTrafficData, seconds);
                    }
                    if (crossroad.LeftToDownTrafficLight.State == TrafficLightState.Green)
                    {
                        crossroad.LeftToDownNofPassedCars += GetNofCars(crossroad.LeftToDownTrafficData, seconds);
                    }
                    if (crossroad.RightToLeftTrafficLight.State == TrafficLightState.Green)
                    {
                        crossroad.RightToLeftNofPassedCars += GetNofCars(crossroad.RightToLeftTrafficData, seconds);
                    }
                    if (crossroad.RightToDownTrafficLight.State == TrafficLightState.Green)
                    {
                        crossroad.RightToDownNofPassedCars += GetNofCars(crossroad.RightToDownTrafficData, seconds);
                    }
                    if (crossroad.RightToUpTrafficLight.State == TrafficLightState.Green)
                    {
                        crossroad.RightToUpNofPassedCars += GetNofCars(crossroad.RightToUpTrafficData, seconds);
                    }
                    if (crossroad.UpToLeftTrafficLight.State == TrafficLightState.Green)
                    {
                        crossroad.UpToLeftNofPassedCars += GetNofCars(crossroad.UpToLeftTrafficData, seconds);
                    }
                    if (crossroad.UpToDownTrafficLight.State == TrafficLightState.Green)
                    {
                        crossroad.UpToDownNofPassedCars += GetNofCars(crossroad.UpToDownTrafficData, seconds);
                    }
                    if (crossroad.UpToRightTrafficLight.State == TrafficLightState.Green)
                    {
                        crossroad.UpToRightNofPassedCars += GetNofCars(crossroad.UpToRightTrafficData, seconds);
                    }
                    if (crossroad.DownToLeftTrafficLight.State == TrafficLightState.Green)
                    {
                        crossroad.DownToLeftNofPassedCars += GetNofCars(crossroad.DownToLeftTrafficData, seconds);
                    }
                    if (crossroad.DownToUpTrafficLight.State == TrafficLightState.Green)
                    {
                        crossroad.DownToUpNofPassedCars += GetNofCars(crossroad.DownToUpTrafficData, seconds);
                    }
                    if (crossroad.DownToRightTrafficLight.State == TrafficLightState.Green)
                    {
                        crossroad.DownToRightNofPassedCars += GetNofCars(crossroad.DownToRightTrafficData, seconds);
                    }
                }
            }
        }

        public void SwitchTrafficLights()
        {
            for (int row = 0; row < _map.NofRows; row++)
            {
                for (int column = 0; column < _map.NofColumns; column++)
                {
                    IMapElement roadElement = _map.GetElement(row, column);
                    if (roadElement == null || !(roadElement is ICrossroad))
                    {
                        continue;
                    }

                    var crossroad = roadElement as ICrossroad;

                    crossroad.CrossroadController.Step();
                    // calculate nagradu
                    double nagrada = 0.0;
                    crossroad.CrossroadController.Reinforce(nagrada);
                }
            }
        }

        private void UpdateDensityAndSpeed(ITrafficLight trafficLight, ITrafficData oldTrafficData,
            ITrafficData trafficData, double seconds)
        {
            if (Math.Abs(trafficData.TrafficDensity) < Epsilon)
            {
                return;
            }

            if (trafficLight.State == TrafficLightState.Red)
            {
                double hours = seconds/3600.0;
                double length = trafficData.TrafficSpeed*1000.0*hours;
                double nofCars = length/100.0*25*trafficData.TrafficDensity;
                double newDensity = oldTrafficData.TrafficDensity + nofCars/25.0;
                if (newDensity > 1)
                {
                    newDensity = 1;
                }
                trafficData.TrafficDensity = newDensity;
                trafficData.TrafficSpeed = 0.0;
            }
            else
            {
                double hours = seconds/3600.0;
                double length = trafficData.TrafficSpeed*1000.0*hours;
                double nofCars = length/100.0*25*trafficData.TrafficDensity;
                double newDensity = oldTrafficData.TrafficDensity - nofCars/25.0;
                if (newDensity < trafficData.TrafficDensity)
                {
                    newDensity = trafficData.TrafficDensity;
                }
                trafficData.TrafficDensity = newDensity;
            }
        }

        private double GetNofCars(ITrafficData trafficData, double seconds)
        {
            double hours = seconds/3600.0;
            double length = trafficData.TrafficSpeed*1000.0*hours;
            double nofCars = length/100.0*25*trafficData.TrafficDensity;

            return nofCars;
        }

        private void AddCrossroadTrafficData(ILocation prev, ILocation crossroadLocation, ILocation next,
            ICrossroad crossroad, ITrafficFlow trafficFlow)
        {
            bool fromLeft = false;
            bool fromUp = false;
            bool fromRight = false;
            bool fromDown = false;

            if (prev.Column < crossroadLocation.Column)
            {
                fromLeft = true;
            }
            else if (prev.Row < crossroadLocation.Row)
            {
                fromUp = true;
            }
            else if (prev.Column > crossroadLocation.Column)
            {
                fromRight = true;
            }
            else if (prev.Row > crossroadLocation.Row)
            {
                fromDown = true;
            }

            bool toLeft = false;
            bool toUp = false;
            bool toRight = false;
            bool toDown = false;

            if (next.Column < crossroadLocation.Column)
            {
                toLeft = true;
            }
            else if (next.Row < crossroadLocation.Row)
            {
                toUp = true;
            }
            else if (next.Column > crossroadLocation.Column)
            {
                toRight = true;
            }
            else if (next.Row > crossroadLocation.Row)
            {
                toDown = true;
            }

            ITrafficData trafficData = null;

            if (fromLeft && toUp)
            {
                trafficData = crossroad.LeftToUpTrafficData;
            }
            else if (fromLeft && toRight)
            {
                trafficData = crossroad.LeftToRightTrafficData;
            }
            else if (fromLeft && toDown)
            {
                trafficData = crossroad.LeftToDownTrafficData;
            }
            else if (fromUp && toLeft)
            {
                trafficData = crossroad.UpToLeftTrafficData;
            }
            else if (fromUp && toDown)
            {
                trafficData = crossroad.UpToDownTrafficData;
            }
            else if (fromUp && toRight)
            {
                trafficData = crossroad.UpToRightTrafficData;
            }
            else if (fromRight && toUp)
            {
                trafficData = crossroad.RightToUpTrafficData;
            }
            else if (fromRight && toLeft)
            {
                trafficData = crossroad.RightToLeftTrafficData;
            }
            else if (fromRight && toDown)
            {
                trafficData = crossroad.RightToDownTrafficData;
            }
            else if (fromDown && toLeft)
            {
                trafficData = crossroad.DownToLeftTrafficData;
            }
            else if (fromDown && toUp)
            {
                trafficData = crossroad.DownToUpTrafficData;
            }
            else if (fromDown && toRight)
            {
                trafficData = crossroad.DownToRightTrafficData;
            }

            trafficData.TrafficDensity += trafficFlow.TrafficDensity;
            if (trafficData.TrafficDensity > 1.0)
            {
                trafficData.TrafficDensity = 1.0;
            }
            trafficData.TrafficSpeed *= trafficData.NofPassingTrafficFlows;
            trafficData.TrafficSpeed += trafficFlow.TrafficSpeed;
            trafficData.NofPassingTrafficFlows++;
            trafficData.TrafficSpeed /= trafficData.NofPassingTrafficFlows;
        }

        private void ZeroCrossroadTrafficData(ICrossroad crossroad)
        {
            crossroad.LeftToUpTrafficData.TrafficDensity = 0;
            crossroad.LeftToUpTrafficData.TrafficSpeed = 0;
            crossroad.LeftToUpTrafficData.NofPassingTrafficFlows = 0;
            crossroad.LeftToRightTrafficData.TrafficDensity = 0;
            crossroad.LeftToRightTrafficData.TrafficSpeed = 0;
            crossroad.LeftToRightTrafficData.NofPassingTrafficFlows = 0;
            crossroad.LeftToDownTrafficData.TrafficDensity = 0;
            crossroad.LeftToDownTrafficData.TrafficSpeed = 0;
            crossroad.LeftToDownTrafficData.NofPassingTrafficFlows = 0;
            crossroad.UpToLeftTrafficData.TrafficDensity = 0;
            crossroad.UpToLeftTrafficData.TrafficSpeed = 0;
            crossroad.UpToLeftTrafficData.NofPassingTrafficFlows = 0;
            crossroad.UpToDownTrafficData.TrafficDensity = 0;
            crossroad.UpToDownTrafficData.TrafficSpeed = 0;
            crossroad.UpToDownTrafficData.NofPassingTrafficFlows = 0;
            crossroad.UpToRightTrafficData.TrafficDensity = 0;
            crossroad.UpToRightTrafficData.TrafficSpeed = 0;
            crossroad.UpToRightTrafficData.NofPassingTrafficFlows = 0;
            crossroad.RightToUpTrafficData.TrafficDensity = 0;
            crossroad.RightToUpTrafficData.TrafficSpeed = 0;
            crossroad.RightToUpTrafficData.NofPassingTrafficFlows = 0;
            crossroad.RightToLeftTrafficData.TrafficDensity = 0;
            crossroad.RightToLeftTrafficData.TrafficSpeed = 0;
            crossroad.RightToLeftTrafficData.NofPassingTrafficFlows = 0;
            crossroad.RightToDownTrafficData.TrafficDensity = 0;
            crossroad.RightToDownTrafficData.TrafficSpeed = 0;
            crossroad.RightToDownTrafficData.NofPassingTrafficFlows = 0;
            crossroad.DownToLeftTrafficData.TrafficDensity = 0;
            crossroad.DownToLeftTrafficData.TrafficSpeed = 0;
            crossroad.DownToLeftTrafficData.NofPassingTrafficFlows = 0;
            crossroad.DownToUpTrafficData.TrafficDensity = 0;
            crossroad.DownToUpTrafficData.TrafficSpeed = 0;
            crossroad.DownToUpTrafficData.NofPassingTrafficFlows = 0;
            crossroad.DownToRightTrafficData.TrafficDensity = 0;
            crossroad.DownToRightTrafficData.TrafficSpeed = 0;
            crossroad.DownToRightTrafficData.NofPassingTrafficFlows = 0;
        }

        private bool IsPathOpen(ILocation prev, ILocation crossroadLocation, ILocation next, ICrossroad crossroad)
        {
            bool fromLeft = false;
            bool fromUp = false;
            bool fromRight = false;
            bool fromDown = false;

            if (prev.Column < crossroadLocation.Column)
            {
                fromLeft = true;
            }
            else if (prev.Row < crossroadLocation.Row)
            {
                fromUp = true;
            }
            else if (prev.Column > crossroadLocation.Column)
            {
                fromRight = true;
            }
            else if (prev.Row > crossroadLocation.Row)
            {
                fromDown = true;
            }

            bool toLeft = false;
            bool toUp = false;
            bool toRight = false;
            bool toDown = false;

            if (next.Column < crossroadLocation.Column)
            {
                toLeft = true;
            }
            else if (next.Row < crossroadLocation.Row)
            {
                toUp = true;
            }
            else if (next.Column > crossroadLocation.Column)
            {
                toRight = true;
            }
            else if (next.Row > crossroadLocation.Row)
            {
                toDown = true;
            }

            if (fromLeft && toUp)
            {
                return crossroad.LeftToUpTrafficLight.State == TrafficLightState.Green;
            }
            if (fromLeft && toRight)
            {
                return crossroad.LeftToRightTrafficLight.State == TrafficLightState.Green;
            }
            if (fromLeft && toDown)
            {
                return crossroad.LeftToDownTrafficLight.State == TrafficLightState.Green;
            }
            if (fromUp && toLeft)
            {
                return crossroad.UpToLeftTrafficLight.State == TrafficLightState.Green;
            }
            if (fromUp && toDown)
            {
                return crossroad.UpToDownTrafficLight.State == TrafficLightState.Green;
            }
            if (fromUp && toRight)
            {
                return crossroad.UpToRightTrafficLight.State == TrafficLightState.Green;
            }
            if (fromRight && toUp)
            {
                return crossroad.RightToUpTrafficLight.State == TrafficLightState.Green;
            }
            if (fromRight && toLeft)
            {
                return crossroad.RightToLeftTrafficLight.State == TrafficLightState.Green;
            }
            if (fromRight && toDown)
            {
                return crossroad.RightToDownTrafficLight.State == TrafficLightState.Green;
            }
            if (fromDown && toLeft)
            {
                return crossroad.DownToLeftTrafficLight.State == TrafficLightState.Green;
            }
            if (fromDown && toUp)
            {
                return crossroad.DownToUpTrafficLight.State == TrafficLightState.Green;
            }
            if (fromDown && toRight)
            {
                return crossroad.DownToRightTrafficLight.State == TrafficLightState.Green;
            }

            return false;
        }
    }
}