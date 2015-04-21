using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using Modeller.CustomControls;
using Simulator.Engine.Domain;
using Simulator.Engine.Infrastructure;
using Simulator.Map;
using Simulator.Map.Infrastructure;
using Simulator.Traffic.Domain;
using Simulator.Traffic.Infrastructure;
using Simulator.Utils.Infrastructure;
using Crossroad = Modeller.CustomControls.Crossroad;
using Road = Modeller.CustomControls.Road;
using Turn = Modeller.CustomControls.Turn;

namespace Modeller.WindowsApplication
{
    public partial class ModellerWindow : Form
    {
        private readonly Pen[] _availableRoadLineColors =
        {
            Pens.Red, Pens.Green, Pens.Blue, Pens.Purple, Pens.Orange, Pens.Aqua,
            Pens.Coral, Pens.DarkMagenta, Pens.Fuchsia, Pens.Navy, Pens.Chartreuse
        };

        private readonly IMap _map;
        private readonly Dictionary<ITrafficFlow, RoadLine> _trafficFlowToRoadLineDictionary;
        private readonly ITrafficManager _trafficManager;
        private CurrentMapElementType _currentMapElementType;
        private ITrafficFlow _currentTrafficFlow;
        private int _customControlSize = 50;
        private bool _isTrafficFlowConfigure;
        private ISimulatorEngine _simulatorEngine;
        private int _workingFieldNofColumns = 10;
        private int _workingFieldNofRows = 10;

        public ModellerWindow()
        {
            InitializeComponent();

            _currentMapElementType = CurrentMapElementType.None;
            _map = new Map(_workingFieldNofRows, _workingFieldNofColumns);
            _isTrafficFlowConfigure = false;

            _trafficFlowToRoadLineDictionary = new Dictionary<ITrafficFlow, RoadLine>();
            _trafficManager = new TrafficManager(_map);
        }

        private void Modeller_Load(object sender, EventArgs e)
        {
            DrawWorkingGrid();
        }

        private void DrawWorkingGrid()
        {
            var bitmap = new Bitmap(_workField.Width, _workField.Height);
            using (Graphics graphics = Graphics.FromImage(bitmap))
            {
                var pen = new Pen(Color.Black) {Width = 1, DashStyle = DashStyle.Dash};
                for (int i = 1; i < _workingFieldNofRows; i++)
                {
                    graphics.DrawLine(pen, 0, i*50 + i - 1, _workField.Width, i*50 + i - 1);
                }

                for (int i = 1; i < _workingFieldNofColumns; i++)
                {
                    graphics.DrawLine(pen, i*50 + i - 1, 0, i*50 + i - 1, _workField.Height);
                }
            }

            _workField.BackgroundImage = bitmap;
        }

        private ILocation CoordinateToLocation(int x, int y)
        {
            int row = y/_customControlSize;
            int column = x/_customControlSize;
            return new Location(row, column);
        }

        private Point LocationToPoint(ILocation location)
        {
            return new Point(location.Column*(_customControlSize + 1), location.Row*(_customControlSize + 1));
        }

        private Control LocationToControl(ILocation location)
        {
            foreach (Control control in _workField.Controls)
            {
                Point locationLeftTopPoint = LocationToPoint(location);
                var locationRightBottomPoint = new Point(locationLeftTopPoint.X + _customControlSize,
                    locationLeftTopPoint.Y + _customControlSize);
                var controlCenter = new Point(control.Location.X + control.Width/2,
                    control.Location.Y + control.Height/2);
                if (locationLeftTopPoint.X < controlCenter.X && controlCenter.X < locationRightBottomPoint.X &&
                    locationLeftTopPoint.Y < controlCenter.Y && controlCenter.Y < locationRightBottomPoint.Y)
                {
                    return control;
                }
            }

            return null;
        }

        private Pen GetNewPen()
        {
            foreach (Pen availableRoadLineColor in _availableRoadLineColors)
            {
                var roadLine = new RoadLine {Color = availableRoadLineColor};
                if (!_trafficFlowToRoadLineDictionary.ContainsValue(roadLine))
                {
                    return availableRoadLineColor;
                }
            }


            var random = new Random();
            return new Pen(Color.FromArgb(random.Next(255), random.Next(255), random.Next(255)));
        }

        private void _workField_MouseClick(object sender, MouseEventArgs e)
        {
            if (_currentMapElementType == CurrentMapElementType.None)
            {
                return;
            }

            int row = e.Y/50;
            int column = e.X/50;

            switch (_currentMapElementType)
            {
                case CurrentMapElementType.Crossroad:
                    AddCrossroad(row, column);
                    break;

                case CurrentMapElementType.VerticalRoad:
                    AddVerticalRoad(row, column);
                    break;

                case CurrentMapElementType.HorizontalRoad:
                    AddHorizontalRoad(row, column);
                    break;

                case CurrentMapElementType.LeftToUpTurn:
                    AddLeftToUpTurn(row, column);
                    break;

                case CurrentMapElementType.UpToRightTurn:
                    AddUpToRightTurn(row, column);
                    break;

                case CurrentMapElementType.RightToDownTurn:
                    AddRightToDownTurn(row, column);
                    break;

                case CurrentMapElementType.DownToLeftTurn:
                    AddDownToLeftTurn(row, column);
                    break;

                case CurrentMapElementType.None:
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void AddDownToLeftTurn(int row, int column)
        {
            var turn = new Turn
            {
                Type = TurnType.DownToLeft,
                Size = new Size(50, 50),
                Location = new Point(column*50 + column, row*50 + row)
            };
            _workField.Controls.Add(turn);

            _map.AddElement(row, column, new Simulator.Map.Infrastructure.Turn());
            if (row + 1 < _workingFieldNofRows)
            {
                _map.SetConnected(row + 1, column, row, column);
            }

            if (column - 1 >= 0)
            {
                _map.SetConnected(row, column - 1, row, column);
            }
        }

        private void AddRightToDownTurn(int row, int column)
        {
            var turn = new Turn
            {
                Type = TurnType.RightToDown,
                Size = new Size(50, 50),
                Location = new Point(column*50 + column, row*50 + row)
            };
            _workField.Controls.Add(turn);

            _map.AddElement(row, column, new Simulator.Map.Infrastructure.Turn());
            if (column + 1 < _workingFieldNofColumns)
            {
                _map.SetConnected(row, column + 1, row, column);
            }

            if (row + 1 < _workingFieldNofRows)
            {
                _map.SetConnected(row + 1, column, row, column);
            }
        }

        private void AddUpToRightTurn(int row, int column)
        {
            var turn = new Turn
            {
                Type = TurnType.UpToRight,
                Size = new Size(50, 50),
                Location = new Point(column*50 + column, row*50 + row)
            };
            _workField.Controls.Add(turn);

            _map.AddElement(row, column, new Simulator.Map.Infrastructure.Turn());
            if (row - 1 >= 0)
            {
                _map.SetConnected(row - 1, column, row, column);
            }

            if (column + 1 < _workingFieldNofColumns)
            {
                _map.SetConnected(row, column + 1, row, column);
            }
        }

        private void AddLeftToUpTurn(int row, int column)
        {
            var turn = new Turn
            {
                Type = TurnType.LeftToUp,
                Size = new Size(50, 50),
                Location = new Point(column*50 + column, row*50 + row)
            };
            _workField.Controls.Add(turn);

            _map.AddElement(row, column, new Simulator.Map.Infrastructure.Turn());
            if (column - 1 >= 0)
            {
                _map.SetConnected(row, column - 1, row, column);
            }

            if (row - 1 >= 0)
            {
                _map.SetConnected(row - 1, column, row, column);
            }
        }

        private void AddHorizontalRoad(int row, int column)
        {
            var road = new Road
            {
                Type = RoadType.Horizontal,
                Size = new Size(50, 50),
                Location = new Point(column*50 + column, row*50 + row)
            };
            road.PinClick += RoadOnPinClick;
            _workField.Controls.Add(road);

            _map.AddElement(row, column, new Simulator.Map.Infrastructure.Road());
            if (column - 1 >= 0)
            {
                _map.SetConnected(row, column - 1, row, column);
            }

            if (column + 1 < _workingFieldNofColumns)
            {
                _map.SetConnected(row, column + 1, row, column);
            }
        }

        private void AddVerticalRoad(int row, int column)
        {
            var road = new Road
            {
                Type = RoadType.Vertical,
                Size = new Size(50, 50),
                Location = new Point(column*50 + column, row*50 + row)
            };
            road.PinClick += RoadOnPinClick;
            _workField.Controls.Add(road);

            _map.AddElement(row, column, new Simulator.Map.Infrastructure.Road());
            if (row - 1 >= 0)
            {
                _map.SetConnected(row - 1, column, row, column);
            }

            if (row + 1 < _workingFieldNofRows)
            {
                _map.SetConnected(row + 1, column, row, column);
            }
        }

        private void AddCrossroad(int row, int column)
        {
            var crossroad = new Crossroad
            {
                Size = new Size(50, 50),
                Location = new Point(column*50 + column, row*50 + row)
            };
            _workField.Controls.Add(crossroad);

            _map.AddElement(row, column, new Simulator.Map.Infrastructure.Crossroad(row, column));
            if (column - 1 >= 0)
            {
                _map.SetConnected(row, column - 1, row, column);
            }

            if (row - 1 >= 0)
            {
                _map.SetConnected(row - 1, column, row, column);
            }

            if (column + 1 < _workingFieldNofColumns)
            {
                _map.SetConnected(row, column + 1, row, column);
            }

            if (row + 1 < _workingFieldNofRows)
            {
                _map.SetConnected(row + 1, column, row, column);
            }
        }

        private void _currentMapElementCrossroad_MouseClick(object sender, MouseEventArgs e)
        {
            _currentMapElementType = CurrentMapElementType.Crossroad;
        }

        private void _currentMapElementVerticalRoad_MouseClick(object sender, MouseEventArgs e)
        {
            _currentMapElementType = CurrentMapElementType.VerticalRoad;
        }

        private void _currentMapElementHorizontalRoad_MouseClick(object sender, MouseEventArgs e)
        {
            _currentMapElementType = CurrentMapElementType.HorizontalRoad;
        }

        private void _currentMapElementLeftToUpTurn_MouseClick(object sender, MouseEventArgs e)
        {
            _currentMapElementType = CurrentMapElementType.LeftToUpTurn;
        }

        private void _currentMapElementUpToRightTurn_MouseClick(object sender, MouseEventArgs e)
        {
            _currentMapElementType = CurrentMapElementType.UpToRightTurn;
        }

        private void _currentMapElementRightToDownTurn_MouseClick(object sender, MouseEventArgs e)
        {
            _currentMapElementType = CurrentMapElementType.RightToDownTurn;
        }

        private void _currentMapElementDownToLeftTurn_MouseClick(object sender, MouseEventArgs e)
        {
            _currentMapElementType = CurrentMapElementType.DownToLeftTurn;
        }

        private void _startTrafficFlow_Click(object sender, EventArgs e)
        {
            _isTrafficFlowConfigure = true;
            _currentTrafficFlow = new TrafficFlow();
            var roadLine = new RoadLine {Color = GetNewPen()};

            _trafficFlowToRoadLineDictionary.Add(_currentTrafficFlow, roadLine);
        }

        private void RoadOnPinClick(object sender, EventArgs eventArgs)
        {
            if (_isTrafficFlowConfigure)
            {
                var road = sender as Road;
                Point controlPosition = road.Location;
                int row = (controlPosition.Y + 1)/50;
                int column = (controlPosition.X + 1)/50;

                if (_currentTrafficFlow.Path.Count == 0)
                {
                    _currentTrafficFlow.Path.Add(new Location(row, column));
                }
                else
                {
                    ILocation from = _currentTrafficFlow.Path[_currentTrafficFlow.Path.Count - 1];
                    var to = new Location(row, column);

                    /* Check whether 'to' location already exists in traffic flow
                     * to avoid loop configuration. */
                    for (int i = 0; i < _currentTrafficFlow.Path.Count; i++)
                    {
                        if (to.Row == _currentTrafficFlow.Path[i].Row && to.Column == _currentTrafficFlow.Path[i].Column)
                        {
                            // TODO: Message box with error
                            return;
                        }
                    }

                    ILocation[] path = _map.FindShortestPath(from, to);
                    for (int i = 1; i < path.Length; i++)
                    {
                        _currentTrafficFlow.Path.Add(path[i]);
                    }
                }

                foreach (ILocation location in _currentTrafficFlow.Path)
                {
                    var control = LocationToControl(location) as ACrossroadControl;
                    if (control == null)
                    {
                        throw new Exception("Something went wrong...");
                    }

                    RoadLine roadLine = _trafficFlowToRoadLineDictionary[_currentTrafficFlow];
                    control.DeleteLine(roadLine);
                    control.AddLine(roadLine);

                    control.Invalidate();
                }
            }
        }


        private void _endTrafficFlow_Click(object sender, EventArgs e)
        {
            double speed = 0.0;
            double density = 0.0;

            try
            {
                speed = double.Parse(_trafficFlowSpeed.Text, CultureInfo.InvariantCulture);
                density = double.Parse(_trafficFlowDensity.Text, CultureInfo.InvariantCulture);
            }
            catch (Exception)
            {
                MessageBox.Show(@"Invalid traffic flow speed or density", @"Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }

            _currentTrafficFlow.TrafficSpeed = speed;
            _currentTrafficFlow.TrafficDensity = density;
            _trafficManager.AddTrafficFlow(_currentTrafficFlow);
        }

        private void _btnStep_Click(object sender, EventArgs e)
        {
            if (_simulatorEngine == null)
            {
                _simulatorEngine = new SimulatorEngine(_trafficManager);
            }

            _simulatorEngine.Step(60);

            WriteLog();
            UpdateLog();
        }

        private void UpdateLog()
        {
            Logger logger = Logger.Instance;
            _tbxLog.Text = "";
            foreach (string message in logger.Messages)
            {
                _tbxLog.AppendText(message + "\r\n");
            }
        }

        public void WriteLog()
        {
            Logger logger = Logger.Instance;
            logger.WriteMessage("===========================================");
            logger.WriteMessage("===========================================");
            logger.WriteMessage("");

            for (int i = 0; i < _workingFieldNofRows; i++)
            {
                for (int j = 0; j < _workingFieldNofColumns; j++)
                {
                    var element = _map.GetElement(i, j) as ICrossroad;
                    if (element == null)
                    {
                        continue;
                    }

                    logger.WriteMessage(string.Format("MATRIX FOR CROSSROAD [{0},{1}]:", i, j));

                    double[,] m0 = element.CrossroadController.W0;
                    for (int k = 0; k < 12; k++)
                    {
                        string str =
                            string.Format(
                                "{0,7:0.0000}  {1,7:0.0000}  {2,7:0.0000}  {3,7:0.0000}  {4,7:0.0000}  {5,7:0.0000}  {6,7:0.0000}  {7,7:0.0000}  {8,7:0.0000}  {9,7:0.0000}  {10,7:0.0000}  {11,7:0.0000}",
                                m0[k, 0], m0[k, 1], m0[k, 2], m0[k, 3], m0[k, 4], m0[k, 5], m0[k, 6], m0[k, 7], m0[k, 8],
                                m0[k, 9], m0[k, 10], m0[k, 11]);
                        logger.WriteMessage(str);
                    }
                }
            }

            logger.WriteMessage("");
        }

        private void _tsmiSaveConfig_Click(object sender, EventArgs e)
        {
            var xmlStream = new MemoryStream();
            var xmlWriter = new XmlTextWriter(xmlStream, Encoding.ASCII);

            xmlWriter.WriteStartDocument();
            xmlWriter.WriteStartElement("Config");
            xmlWriter.WriteStartElement("Map");
            for (int row = 0; row < _workingFieldNofRows; row++)
            {
                for (int column = 0; column < _workingFieldNofColumns; column++)
                {
                    IMapElement mapElement = _map.GetElement(row, column);
                    if (mapElement != null)
                    {
                        xmlWriter.WriteStartElement("MapElement");
                        xmlWriter.WriteAttributeString("Row", row.ToString(CultureInfo.InvariantCulture));
                        xmlWriter.WriteAttributeString("Column", column.ToString(CultureInfo.InvariantCulture));
                        if (mapElement is ICrossroad)
                        {
                            xmlWriter.WriteAttributeString("Type", "Crossroad");
                        }
                        else if (mapElement is IRoad)
                        {
                            xmlWriter.WriteAttributeString("Type", "Road");
                            var roadControl = (Road) LocationToControl(new Location(row, column));
                            if (roadControl.Type == RoadType.Horizontal)
                            {
                                xmlWriter.WriteAttributeString("Orientation", "Horizontal");
                            }
                            else if (roadControl.Type == RoadType.Vertical)
                            {
                                xmlWriter.WriteAttributeString("Orientation", "Vertical");
                            }
                            else
                            {
                                throw new ArgumentException("Invalid road orientation");
                            }
                        }
                        else if (mapElement is ITurn)
                        {
                            xmlWriter.WriteAttributeString("Type", "Turn");
                            var turnControl = (Turn) LocationToControl(new Location(row, column));
                            if (turnControl.Type == TurnType.DownToLeft)
                            {
                                xmlWriter.WriteAttributeString("Orientation", "DownToLeft");
                            }
                            else if (turnControl.Type == TurnType.LeftToUp)
                            {
                                xmlWriter.WriteAttributeString("Orientation", "LeftToUp");
                            }
                            else if (turnControl.Type == TurnType.RightToDown)
                            {
                                xmlWriter.WriteAttributeString("Orientation", "RightToDown");
                            }
                            else if (turnControl.Type == TurnType.UpToRight)
                            {
                                xmlWriter.WriteAttributeString("Orientation", "UpToRight");
                            }
                            else
                            {
                                throw new ArgumentException("Invalid turn orientation");
                            }
                        }

                        xmlWriter.WriteEndElement();
                    }
                }
            }

            xmlWriter.WriteEndElement();
            xmlWriter.WriteStartElement("TrafficFlows");

            foreach (ITrafficFlow trafficFlow in _trafficManager.TrafficFlows)
            {
                xmlWriter.WriteStartElement("TrafficFlow");
                xmlWriter.WriteAttributeString("Density",
                    trafficFlow.TrafficDensity.ToString(CultureInfo.InvariantCulture));
                xmlWriter.WriteAttributeString("Speed", trafficFlow.TrafficSpeed.ToString(CultureInfo.InvariantCulture));
                xmlWriter.WriteStartElement("Path");
                foreach (ILocation location in trafficFlow.Path)
                {
                    xmlWriter.WriteStartElement("Location");
                    xmlWriter.WriteAttributeString("Row", location.Row.ToString(CultureInfo.InvariantCulture));
                    xmlWriter.WriteAttributeString("Column", location.Column.ToString(CultureInfo.InvariantCulture));
                    xmlWriter.WriteEndElement();
                }

                xmlWriter.WriteEndElement();
                xmlWriter.WriteEndElement();
            }

            xmlWriter.WriteEndElement();
            xmlWriter.WriteEndElement();
            xmlWriter.WriteEndDocument();

            xmlWriter.Flush();
            xmlStream.Position = 0;
            var xmlDocument = new XmlDocument();
            xmlDocument.Load(xmlStream);
            xmlDocument.Save("d:/xx.xml");
        }
    }
}