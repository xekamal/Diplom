using Simulator.Neuro.Domain;
using Simulator.Neuro.Infrastructure;

namespace Simulator.Map.Infrastructure
{
    public class Crossroad : ICrossroad
    {
        public Crossroad()
        {
            LeftToUpTrafficLight = new TrafficLight(TrafficLightState.Red);
            LeftToRightTrafficLight = new TrafficLight(TrafficLightState.Red);
            LeftToDownTrafficLight = new TrafficLight(TrafficLightState.Red);
            UpToLeftTrafficLight = new TrafficLight(TrafficLightState.Red);
            UpToDownTrafficLight = new TrafficLight(TrafficLightState.Red);
            UpToRightTrafficLight = new TrafficLight(TrafficLightState.Red);
            RightToUpTrafficLight = new TrafficLight(TrafficLightState.Red);
            RightToLeftTrafficLight = new TrafficLight(TrafficLightState.Red);
            RightToDownTrafficLight = new TrafficLight(TrafficLightState.Red);
            DownToLeftTrafficLight = new TrafficLight(TrafficLightState.Red);
            DownToUpTrafficLight = new TrafficLight(TrafficLightState.Red);
            DownToRightTrafficLight = new TrafficLight(TrafficLightState.Red);

            LeftToUpTrafficData = new TrafficData();
            LeftToRightTrafficData = new TrafficData();
            LeftToDownTrafficData = new TrafficData();
            UpToLeftTrafficData = new TrafficData();
            UpToDownTrafficData = new TrafficData();
            UpToRightTrafficData = new TrafficData();
            RightToUpTrafficData = new TrafficData();
            RightToLeftTrafficData = new TrafficData();
            RightToDownTrafficData = new TrafficData();
            DownToLeftTrafficData = new TrafficData();
            DownToUpTrafficData = new TrafficData();
            DownToRightTrafficData = new TrafficData();

            //CrossroadController = new CrossroadControllerReinforcement(this);
            CrossroadController = new CrossroadControllerStaticTimer(this);
        }
        public Crossroad(int row, int column)
        {
            Row = row;
            Column = column;

            LeftToUpTrafficLight = new TrafficLight(TrafficLightState.Red);
            LeftToRightTrafficLight = new TrafficLight(TrafficLightState.Red);
            LeftToDownTrafficLight = new TrafficLight(TrafficLightState.Red);
            UpToLeftTrafficLight = new TrafficLight(TrafficLightState.Red);
            UpToDownTrafficLight = new TrafficLight(TrafficLightState.Red);
            UpToRightTrafficLight = new TrafficLight(TrafficLightState.Red);
            RightToUpTrafficLight = new TrafficLight(TrafficLightState.Red);
            RightToLeftTrafficLight = new TrafficLight(TrafficLightState.Red);
            RightToDownTrafficLight = new TrafficLight(TrafficLightState.Red);
            DownToLeftTrafficLight = new TrafficLight(TrafficLightState.Red);
            DownToUpTrafficLight = new TrafficLight(TrafficLightState.Red);
            DownToRightTrafficLight = new TrafficLight(TrafficLightState.Red);

            LeftToUpTrafficData = new TrafficData();
            LeftToRightTrafficData = new TrafficData();
            LeftToDownTrafficData = new TrafficData();
            UpToLeftTrafficData = new TrafficData();
            UpToDownTrafficData = new TrafficData();
            UpToRightTrafficData = new TrafficData();
            RightToUpTrafficData = new TrafficData();
            RightToLeftTrafficData = new TrafficData();
            RightToDownTrafficData = new TrafficData();
            DownToLeftTrafficData = new TrafficData();
            DownToUpTrafficData = new TrafficData();
            DownToRightTrafficData = new TrafficData();

            //rossroadController = new CrossroadControllerReinforcement(this);
            CrossroadController = new CrossroadControllerStaticTimer(this);
        }

        public double Length { get; set; }
        public ITrafficLight LeftToUpTrafficLight { get; set; }
        public ITrafficLight LeftToRightTrafficLight { get; set; }
        public ITrafficLight LeftToDownTrafficLight { get; set; }
        public ITrafficLight UpToLeftTrafficLight { get; set; }
        public ITrafficLight UpToDownTrafficLight { get; set; }
        public ITrafficLight UpToRightTrafficLight { get; set; }
        public ITrafficLight RightToUpTrafficLight { get; set; }
        public ITrafficLight RightToLeftTrafficLight { get; set; }
        public ITrafficLight RightToDownTrafficLight { get; set; }
        public ITrafficLight DownToLeftTrafficLight { get; set; }
        public ITrafficLight DownToUpTrafficLight { get; set; }
        public ITrafficLight DownToRightTrafficLight { get; set; }
        public ITrafficData LeftToUpTrafficData { get; set; }
        public ITrafficData LeftToRightTrafficData { get; set; }
        public ITrafficData LeftToDownTrafficData { get; set; }
        public ITrafficData UpToLeftTrafficData { get; set; }
        public ITrafficData UpToDownTrafficData { get; set; }
        public ITrafficData UpToRightTrafficData { get; set; }
        public ITrafficData RightToUpTrafficData { get; set; }
        public ITrafficData RightToLeftTrafficData { get; set; }
        public ITrafficData RightToDownTrafficData { get; set; }
        public ITrafficData DownToLeftTrafficData { get; set; }
        public ITrafficData DownToUpTrafficData { get; set; }
        public ITrafficData DownToRightTrafficData { get; set; }
        public double LeftToUpNofPassedCars { get; set; }
        public double LeftToRightNofPassedCars { get; set; }
        public double LeftToDownNofPassedCars { get; set; }
        public double UpToLeftNofPassedCars { get; set; }
        public double UpToDownNofPassedCars { get; set; }
        public double UpToRightNofPassedCars { get; set; }
        public double RightToUpNofPassedCars { get; set; }
        public double RightToLeftNofPassedCars { get; set; }
        public double RightToDownNofPassedCars { get; set; }
        public double DownToLeftNofPassedCars { get; set; }
        public double DownToUpNofPassedCars { get; set; }
        public double DownToRightNofPassedCars { get; set; }
        public int Row { get; set; }
        public int Column { get; set; }
        public ICrossroadController CrossroadController { get; set; }

        private double GetTrafficDataMark(ITrafficLight trafficLight, ITrafficData trafficData)
        {
            if (trafficLight.State == TrafficLightState.Green)
            {
                return trafficData.TrafficDensity + 1 / trafficData.TrafficSpeed;
            }

            return 0.0;
        }

        public double GetMark()
        {
            double mark = 0.0;

            mark += GetTrafficDataMark(LeftToUpTrafficLight, LeftToUpTrafficData);
            mark += GetTrafficDataMark(LeftToRightTrafficLight, LeftToRightTrafficData);
            mark += GetTrafficDataMark(LeftToDownTrafficLight, LeftToDownTrafficData);
            mark += GetTrafficDataMark(UpToLeftTrafficLight, UpToLeftTrafficData);
            mark += GetTrafficDataMark(UpToDownTrafficLight, UpToDownTrafficData);
            mark += GetTrafficDataMark(UpToRightTrafficLight, UpToRightTrafficData);
            mark += GetTrafficDataMark(RightToUpTrafficLight, RightToUpTrafficData);
            mark += GetTrafficDataMark(RightToLeftTrafficLight, RightToLeftTrafficData);
            mark += GetTrafficDataMark(RightToDownTrafficLight, RightToDownTrafficData);
            mark += GetTrafficDataMark(DownToLeftTrafficLight, DownToLeftTrafficData);
            mark += GetTrafficDataMark(DownToUpTrafficLight, DownToUpTrafficData);
            mark += GetTrafficDataMark(DownToRightTrafficLight, DownToRightTrafficData);

            return mark;
        }

        public object Clone()
        {
            ICrossroad copy = new Crossroad();

            copy.Length = Length;
            copy.LeftToUpTrafficLight = LeftToUpTrafficLight.Clone() as ITrafficLight;
            copy.LeftToRightTrafficLight = LeftToRightTrafficLight.Clone() as ITrafficLight;
            copy.LeftToDownTrafficLight = LeftToDownTrafficLight.Clone() as ITrafficLight;
            copy.UpToLeftTrafficLight = UpToLeftTrafficLight.Clone() as ITrafficLight;
            copy.UpToDownTrafficLight = UpToDownTrafficLight.Clone() as ITrafficLight;
            copy.UpToRightTrafficLight = UpToRightTrafficLight.Clone() as ITrafficLight;
            copy.RightToUpTrafficLight = RightToUpTrafficLight.Clone() as ITrafficLight;
            copy.RightToLeftTrafficLight = RightToLeftTrafficLight.Clone() as ITrafficLight;
            copy.RightToDownTrafficLight = RightToDownTrafficLight.Clone() as ITrafficLight;
            copy.DownToLeftTrafficLight = DownToLeftTrafficLight.Clone() as ITrafficLight;
            copy.DownToUpTrafficLight = DownToUpTrafficLight.Clone() as ITrafficLight;
            copy.DownToRightTrafficLight = DownToRightTrafficLight.Clone() as ITrafficLight;
            copy.LeftToUpTrafficData = LeftToUpTrafficData.Clone() as ITrafficData;
            copy.LeftToRightTrafficData = LeftToRightTrafficData.Clone() as ITrafficData;
            copy.LeftToDownTrafficData = LeftToDownTrafficData.Clone() as ITrafficData;
            copy.UpToLeftTrafficData = UpToLeftTrafficData.Clone() as ITrafficData;
            copy.UpToDownTrafficData = UpToDownTrafficData.Clone() as ITrafficData;
            copy.UpToRightTrafficData = UpToRightTrafficData.Clone() as ITrafficData;
            copy.RightToUpTrafficData = RightToUpTrafficData.Clone() as ITrafficData;
            copy.RightToLeftTrafficData = RightToLeftTrafficData.Clone() as ITrafficData;
            copy.RightToDownTrafficData = RightToDownTrafficData.Clone() as ITrafficData;
            copy.DownToLeftTrafficData = DownToLeftTrafficData.Clone() as ITrafficData;
            copy.DownToUpTrafficData = DownToUpTrafficData.Clone() as ITrafficData;
            copy.DownToRightTrafficData = DownToRightTrafficData.Clone() as ITrafficData;
            copy.LeftToUpNofPassedCars = LeftToUpNofPassedCars;
            copy.LeftToRightNofPassedCars = LeftToRightNofPassedCars;
            copy.LeftToDownNofPassedCars = LeftToDownNofPassedCars;
            copy.UpToLeftNofPassedCars = UpToLeftNofPassedCars;
            copy.UpToDownNofPassedCars = UpToDownNofPassedCars;
            copy.UpToRightNofPassedCars = UpToRightNofPassedCars;
            copy.RightToUpNofPassedCars = RightToUpNofPassedCars;
            copy.RightToLeftNofPassedCars = RightToLeftNofPassedCars;
            copy.RightToDownNofPassedCars = RightToDownNofPassedCars;
            copy.DownToLeftNofPassedCars = DownToLeftNofPassedCars;
            copy.DownToUpNofPassedCars = DownToUpNofPassedCars;
            copy.DownToRightNofPassedCars = DownToRightNofPassedCars;
            copy.CrossroadController = CrossroadController;

            return copy;
        }
    }
}