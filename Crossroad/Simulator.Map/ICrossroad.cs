using Simulator.Neuro.Domain;

namespace Simulator.Map
{
    public interface ICrossroad : IMapElement
    {
        ITrafficLight LeftToUpTrafficLight { get; set; }
        ITrafficLight LeftToRightTrafficLight { get; set; }
        ITrafficLight LeftToDownTrafficLight { get; set; }

        ITrafficLight UpToLeftTrafficLight { get; set; }
        ITrafficLight UpToDownTrafficLight { get; set; }
        ITrafficLight UpToRightTrafficLight { get; set; }

        ITrafficLight RightToUpTrafficLight { get; set; }
        ITrafficLight RightToLeftTrafficLight { get; set; }
        ITrafficLight RightToDownTrafficLight { get; set; }

        ITrafficLight DownToLeftTrafficLight { get; set; }
        ITrafficLight DownToUpTrafficLight { get; set; }
        ITrafficLight DownToRightTrafficLight { get; set; }

        ITrafficData LeftToUpTrafficData { get; set; }
        ITrafficData LeftToRightTrafficData { get; set; }
        ITrafficData LeftToDownTrafficData { get; set; }

        ITrafficData UpToLeftTrafficData { get; set; }
        ITrafficData UpToDownTrafficData { get; set; }
        ITrafficData UpToRightTrafficData { get; set; }

        ITrafficData RightToUpTrafficData { get; set; }
        ITrafficData RightToLeftTrafficData { get; set; }
        ITrafficData RightToDownTrafficData { get; set; }

        ITrafficData DownToLeftTrafficData { get; set; }
        ITrafficData DownToUpTrafficData { get; set; }
        ITrafficData DownToRightTrafficData { get; set; }

        ICrossroadController CrossroadController { get; set; }
    }
}