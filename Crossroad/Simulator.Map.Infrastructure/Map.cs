using System;
using System.Collections.Generic;
using System.Linq;

namespace Simulator.Map.Infrastructure
{
    public class Map : IMap
    {
        private readonly bool[,] _mapConnection;
        private readonly IMapElement[,] _mapElements;
        private readonly int _nofColumns;
        private readonly int _nofRows;

        public Map(int nofRows, int nofColumns)
        {
            _nofRows = nofRows;
            _nofColumns = nofColumns;
            _mapElements = new IMapElement[_nofRows, _nofColumns];
            _mapConnection = new bool[_nofRows*_nofColumns, _nofRows*_nofColumns];
        }

        public void AddElement(int row, int column, IMapElement mapElement)
        {
            _mapElements[row, column] = mapElement;
        }

        public IMapElement GetElement(int row, int column)
        {
            return _mapElements[row, column];
        }

        public bool IsConnected(int e1Row, int e1Column, int e2Row, int e2Column)
        {
            return _mapConnection[e1Row*_nofColumns + e1Column, e2Row*_nofColumns + e2Column];
        }

        public void SetConnected(int e1Row, int e1Column, int e2Row, int e2Column)
        {
            _mapConnection[e1Row*_nofColumns + e1Column, e2Row*_nofColumns + e2Column] = true;
            _mapConnection[e2Row*_nofColumns + e2Column, e1Row*_nofColumns + e1Column] = true;
        }

        public bool IsConnected(ILocation first, ILocation second)
        {
            return IsConnected(first.Row, first.Column, second.Row, second.Column);
        }

        public void SetConnected(ILocation first, ILocation second)
        {
            _mapConnection[first.Row*_nofColumns + first.Column, second.Row*_nofColumns + second.Column] = true;
            _mapConnection[second.Row*_nofColumns + second.Column, first.Row*_nofColumns + first.Column] = true;
        }

        public ILocation[] FindShortestPath(ILocation @from, ILocation to)
        {
            int fromVertex = from.Row*_nofRows + from.Column;
            int toVertex = to.Row*_nofRows + to.Column;
            int nofVertexes = _nofRows*_nofColumns;
            var dist = new double[nofVertexes];
            var paths = new IList<ILocation>[nofVertexes];
            var sptSet = new bool[nofVertexes];

            for (int i = 0; i < nofVertexes; i++)
            {
                dist[i] = double.MaxValue;
                sptSet[i] = false;
            }

            for (int i = 0; i < nofVertexes; i++)
            {
                paths[i] = new List<ILocation>();
            }

            dist[fromVertex] = 0;

            for (int i = 0; i < nofVertexes - 1; i++)
            {
                double currentMinDist = double.MaxValue;
                int currentMinVertex = int.MaxValue;

                for (int j = 0; j < nofVertexes; j++)
                {
                    if (!sptSet[j] && dist[j] <= currentMinDist)
                    {
                        currentMinDist = dist[j];
                        currentMinVertex = j;
                    }
                }

                sptSet[currentMinVertex] = true;
                for (int j = 0; j < nofVertexes; j++)
                {
                    if (!sptSet[j] && IsConnected(NumberToLocation(currentMinVertex), NumberToLocation(j)) &&
                        Math.Abs(dist[currentMinVertex] - double.MaxValue) > 0.00001 &&
                        dist[currentMinVertex] + 100.0 < dist[j]) // TODO: length 100
                    {
                        dist[j] = dist[currentMinVertex] + 100.0; // TODO: length 100
                        paths[j] = new List<ILocation>(paths[currentMinVertex]) {NumberToLocation(j)};
                    }
                }
            }

            paths[LocationToNumber(to)].Insert(0, from);
            return paths[LocationToNumber(to)].ToArray();
        }

        public int NofRows
        {
            get { return _nofRows; }
        }

        public int NofColumns
        {
            get { return _nofColumns; }
        }

        private ILocation NumberToLocation(int number)
        {
            int row = number/_nofColumns;
            int column = number%_nofColumns;
            return new Location(row, column);
        }

        private int LocationToNumber(ILocation location)
        {
            return location.Row*_nofColumns + location.Column;
        }
    }
}