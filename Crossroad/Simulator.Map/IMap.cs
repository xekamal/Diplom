namespace Simulator.Map
{
    public interface IMap
    {
        void AddElement(int row, int column, IMapElement mapElement);
        IMapElement GetElement(int row, int column);
        bool IsConnected(int e1Row, int e1Column, int e2Row, int e2Column);
        void SetConnected(int e1Row, int e1Column, int e2Row, int e2Column);
        bool IsConnected(ILocation first, ILocation second);
        void SetConnected(ILocation first, ILocation second);
        ILocation[] FindShortestPath(ILocation from, ILocation to);
        int NofRows { get; }
        int NofColumns { get; }
    }
}