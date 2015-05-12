using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using Modeller.CustomControls;
using Simulator.Engine.Domain;
using Simulator.Engine.Infrastructure;
using Simulator.Map;
using Simulator.Map.Infrastructure;
using Simulator.Neuro.Domain;
using Simulator.Neuro.Infrastructure;
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

        private readonly int _customControlSize = 50;
        private readonly IMap _map;
        private readonly Dictionary<ITrafficFlow, RoadLine> _trafficFlowToRoadLineDictionary;
        private readonly ITrafficManager _trafficManager;
        private readonly int _workingFieldNofColumns = 10;
        private readonly int _workingFieldNofRows = 10;
        private CurrentMapElementType _currentMapElementType;
        private ITrafficFlow _currentTrafficFlow;
        private bool _isTrafficFlowConfigure;
        private bool _simulationFlag;
        private ISimulatorEngine _simulatorEngine;

        public ModellerWindow()
        {
            InitializeComponent();

            _currentMapElementType = CurrentMapElementType.None;
            _map = new Map(_workingFieldNofRows, _workingFieldNofColumns);
            _isTrafficFlowConfigure = false;

            _trafficFlowToRoadLineDictionary = new Dictionary<ITrafficFlow, RoadLine>();
            _trafficManager = new TrafficManager(_map);
        }

        private void UpdateLogFromSimulationThread()
        {
            UpdateLog();
        }

        private void Modeller_Load(object sender, EventArgs e)
        {
            DrawWorkingGrid();
        }

        private void DrawWorkingGrid()
        {
            var bitmap = new Bitmap(_workField.Width, _workField.Height);
            using (var graphics = Graphics.FromImage(bitmap))
            {
                var pen = new Pen(Color.Black) {Width = 1, DashStyle = DashStyle.Dash};
                for (var i = 1; i < _workingFieldNofRows; i++)
                {
                    graphics.DrawLine(pen, 0, i*50 + i - 1, _workField.Width, i*50 + i - 1);
                }

                for (var i = 1; i < _workingFieldNofColumns; i++)
                {
                    graphics.DrawLine(pen, i*50 + i - 1, 0, i*50 + i - 1, _workField.Height);
                }
            }

            _workField.BackgroundImage = bitmap;
        }

        private ILocation CoordinateToLocation(int x, int y)
        {
            var row = y/_customControlSize;
            var column = x/_customControlSize;
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
                var locationLeftTopPoint = LocationToPoint(location);
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
            foreach (var availableRoadLineColor in _availableRoadLineColors)
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

            var row = e.Y/50;
            var column = e.X/50;

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
                var controlPosition = road.Location;
                var row = (controlPosition.Y + 1)/50;
                var column = (controlPosition.X + 1)/50;

                if (_currentTrafficFlow.Path.Count == 0)
                {
                    _currentTrafficFlow.Path.Add(new Location(row, column));
                }
                else
                {
                    var from = _currentTrafficFlow.Path[_currentTrafficFlow.Path.Count - 1];
                    var to = new Location(row, column);

                    /* Check whether 'to' location already exists in traffic flow
                     * to avoid loop configuration. */
                    for (var i = 0; i < _currentTrafficFlow.Path.Count; i++)
                    {
                        if (to.Row == _currentTrafficFlow.Path[i].Row && to.Column == _currentTrafficFlow.Path[i].Column)
                        {
                            // TODO: Message box with error
                            return;
                        }
                    }

                    var path = _map.FindShortestPath(from, to);
                    for (var i = 1; i < path.Length; i++)
                    {
                        _currentTrafficFlow.Path.Add(path[i]);
                    }
                }

                foreach (var location in _currentTrafficFlow.Path)
                {
                    var control = LocationToControl(location) as ACrossroadControl;
                    if (control == null)
                    {
                        throw new Exception("Something went wrong...");
                    }

                    var roadLine = _trafficFlowToRoadLineDictionary[_currentTrafficFlow];
                    control.DeleteLine(roadLine);
                    control.AddLine(roadLine);

                    control.Invalidate();
                }
            }
        }

        private void _endTrafficFlow_Click(object sender, EventArgs e)
        {
            var speed = 0.0;
            var density = 0.0;

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
            var logger = Logger.Instance;
            _tbxLog.Text = "";
            foreach (var message in logger.Messages)
            {
                _tbxLog.AppendText(message + "\r\n");
            }
        }

        public void WriteLog()
        {
            var logger = Logger.Instance;
            logger.WriteMessage("======================================================================");
            logger.WriteMessage("===============================  STEP  ===============================");
            logger.WriteMessage("======================================================================");

            for (var i = 0; i < _workingFieldNofRows; i++)
            {
                for (var j = 0; j < _workingFieldNofColumns; j++)
                {
                    var element = _map.GetElement(i, j) as ICrossroad;
                    if (element == null)
                    {
                        continue;
                    }

                    logger.WriteMessage("");
                    logger.WriteMessage(string.Format("CROSSROAD [{0},{1}] DATA", i, j));
                    logger.WriteMessage("");

                    logger.WriteMessage("MATRIX W0:");
                    logger.WriteMessage("");

                    var m0 = ((CrossroadControllerReinforcement) element.CrossroadController).W0;
                    var ccr = (CrossroadControllerReinforcement) element.CrossroadController;
                    for (var k = 0; k < 12; k++)
                    {
                        var str =
                            string.Format(
                                "{0,9:0.0000}  {1,9:0.0000}  {2,9:0.0000}  {3,9:0.0000}  {4,9:0.0000}  {5,9:0.0000}  {6,9:0.0000}  {7,9:0.0000}  {8,9:0.0000}  {9,9:0.0000}  {10,9:0.0000}  {11,9:0.0000}",
                                m0[k, 0], m0[k, 1], m0[k, 2], m0[k, 3], m0[k, 4], m0[k, 5], m0[k, 6], m0[k, 7], m0[k, 8],
                                m0[k, 9], m0[k, 10], m0[k, 11]);
                        logger.WriteMessage(str);
                    }

                    logger.WriteMessage("");
                    logger.WriteMessage(string.Format("TRAFFIC LIGHTS STATUSES:{0}", GetCrossroadTrafficLightsStatusesAsString(element)));
                    logger.WriteMessage("");

                    logger.WriteMessage(string.Format("LEFT TO UP    TRAFFIC SPEED:{0,7:0.0000}; TRAFFIC DENSITY:{1,7:0.0000}", element.LeftToUpTrafficData.TrafficSpeed, element.LeftToUpTrafficData.TrafficDensity));
                    logger.WriteMessage(string.Format("LEFT TO RIGHT TRAFFIC SPEED:{0,7:0.0000}; TRAFFIC DENSITY:{1,7:0.0000}", element.LeftToRightTrafficData.TrafficSpeed, element.LeftToRightTrafficData.TrafficDensity));
                    logger.WriteMessage(string.Format("LEFT TO DOWN  TRAFFIC SPEED:{0,7:0.0000}; TRAFFIC DENSITY:{1,7:0.0000}", element.LeftToDownTrafficData.TrafficSpeed, element.LeftToDownTrafficData.TrafficDensity));
                    logger.WriteMessage(string.Format("UP TO LEFT    TRAFFIC SPEED:{0,7:0.0000}; TRAFFIC DENSITY:{1,7:0.0000}", element.UpToLeftTrafficData.TrafficSpeed, element.UpToLeftTrafficData.TrafficDensity));
                    logger.WriteMessage(string.Format("UP TO DOWN    TRAFFIC SPEED:{0,7:0.0000}; TRAFFIC DENSITY:{1,7:0.0000}", element.UpToDownTrafficData.TrafficSpeed, element.UpToDownTrafficData.TrafficDensity));
                    logger.WriteMessage(string.Format("UP TO RIGHT   TRAFFIC SPEED:{0,7:0.0000}; TRAFFIC DENSITY:{1,7:0.0000}", element.UpToRightTrafficData.TrafficSpeed, element.UpToRightTrafficData.TrafficDensity));
                    logger.WriteMessage(string.Format("RIGHT TO UP   TRAFFIC SPEED:{0,7:0.0000}; TRAFFIC DENSITY:{1,7:0.0000}", element.RightToUpTrafficData.TrafficSpeed, element.RightToUpTrafficData.TrafficDensity));
                    logger.WriteMessage(string.Format("RIGHT TO LEFT TRAFFIC SPEED:{0,7:0.0000}; TRAFFIC DENSITY:{1,7:0.0000}", element.RightToLeftTrafficData.TrafficSpeed, element.RightToLeftTrafficData.TrafficDensity));
                    logger.WriteMessage(string.Format("RIGHT TO DOWN TRAFFIC SPEED:{0,7:0.0000}; TRAFFIC DENSITY:{1,7:0.0000}", element.RightToDownTrafficData.TrafficSpeed, element.RightToDownTrafficData.TrafficDensity));
                    logger.WriteMessage(string.Format("DOWN TO LEFT  TRAFFIC SPEED:{0,7:0.0000}; TRAFFIC DENSITY:{1,7:0.0000}", element.DownToLeftTrafficData.TrafficSpeed, element.DownToLeftTrafficData.TrafficDensity));
                    logger.WriteMessage(string.Format("DOWN TO UP    TRAFFIC SPEED:{0,7:0.0000}; TRAFFIC DENSITY:{1,7:0.0000}", element.DownToUpTrafficData.TrafficSpeed, element.DownToUpTrafficData.TrafficDensity));
                    logger.WriteMessage(string.Format("DOWN TO RIGHT TRAFFIC SPEED:{0,7:0.0000}; TRAFFIC DENSITY:{1,7:0.0000}", element.DownToRightTrafficData.TrafficSpeed, element.DownToRightTrafficData.TrafficDensity));
                    logger.WriteMessage("");

                    for (var k = 0; k < ccr.HNeurons.Length; k++)
                    {
                        var hNeuron = ccr.HNeurons[k];
                        logger.WriteMessage(string.Format("H NEURON [{0,2}] DENDRITS:{1}", k, GetNeuronDendritsAsString(hNeuron)));
                        logger.WriteMessage(string.Format("H NEURON [{0,2}] AXON:{1,7:0.0000}", k, hNeuron.axon));
                    }

                    logger.WriteMessage("");

                    for (int k = 0; k < ccr.RNeurons.Length; k++)
                    {
                        RNeuron rNeuron = ccr.RNeurons[k];
                        logger.WriteMessage(string.Format("R NEURON [{0,2}] DENDRITS:{1}", k, GetNeuronDendritsAsString(rNeuron)));
                        logger.WriteMessage(string.Format("R NEURON [{0,2}] AXON:{1,7:0.0000}", k, rNeuron.axon));
                    }
                }
            }

            logger.WriteMessage("");
        }

        private string GetNeuronDendritsAsString(ANeuron hNeuron)
        {
            return
                string.Format(
                    "{0,7:0.0000}  {1,7:0.0000}  {2,7:0.0000}  {3,7:0.0000}  {4,7:0.0000}  {5,7:0.0000}  {6,7:0.0000}  {7,7:0.0000}  {8,7:0.0000}  {9,7:0.0000}  {10,7:0.0000}  {11,7:0.0000}",
                    hNeuron.dendrits[0], hNeuron.dendrits[1], hNeuron.dendrits[2], hNeuron.dendrits[3],
                    hNeuron.dendrits[4], hNeuron.dendrits[5], hNeuron.dendrits[6], hNeuron.dendrits[7],
                    hNeuron.dendrits[8], hNeuron.dendrits[9], hNeuron.dendrits[10], hNeuron.dendrits[11]);
        }

        private string GetCrossroadTrafficLightsStatusesAsString(ICrossroad crossroad)
        {
            var res = string.Empty;
            res += crossroad.LeftToUpTrafficLight.State == TrafficLightState.Green ? "1" : "0";
            res += crossroad.LeftToRightTrafficLight.State == TrafficLightState.Green ? "1" : "0";
            res += crossroad.LeftToDownTrafficLight.State == TrafficLightState.Green ? "1" : "0";
            res += crossroad.DownToLeftTrafficLight.State == TrafficLightState.Green ? "1" : "0";
            res += crossroad.DownToUpTrafficLight.State == TrafficLightState.Green ? "1" : "0";
            res += crossroad.DownToRightTrafficLight.State == TrafficLightState.Green ? "1" : "0";
            res += crossroad.RightToDownTrafficLight.State == TrafficLightState.Green ? "1" : "0";
            res += crossroad.RightToLeftTrafficLight.State == TrafficLightState.Green ? "1" : "0";
            res += crossroad.RightToUpTrafficLight.State == TrafficLightState.Green ? "1" : "0";
            res += crossroad.UpToRightTrafficLight.State == TrafficLightState.Green ? "1" : "0";
            res += crossroad.UpToDownTrafficLight.State == TrafficLightState.Green ? "1" : "0";
            res += crossroad.UpToLeftTrafficLight.State == TrafficLightState.Green ? "1" : "0";
            return res;
        }

        private void _tsmiSaveConfig_Click(object sender, EventArgs e)
        {
            var xmlStream = new MemoryStream();
            var xmlWriter = new XmlTextWriter(xmlStream, Encoding.ASCII);

            xmlWriter.WriteStartDocument();
            xmlWriter.WriteStartElement("Config");
            xmlWriter.WriteStartElement("Map");
            for (var row = 0; row < _workingFieldNofRows; row++)
            {
                for (var column = 0; column < _workingFieldNofColumns; column++)
                {
                    var mapElement = _map.GetElement(row, column);
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

            foreach (var trafficFlow in _trafficManager.TrafficFlows)
            {
                xmlWriter.WriteStartElement("TrafficFlow");
                xmlWriter.WriteAttributeString("Density",
                    trafficFlow.TrafficDensity.ToString(CultureInfo.InvariantCulture));
                xmlWriter.WriteAttributeString("Speed", trafficFlow.TrafficSpeed.ToString(CultureInfo.InvariantCulture));
                xmlWriter.WriteStartElement("Path");
                foreach (var location in trafficFlow.Path)
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

            var dialog = new SaveFileDialog
            {
                Filter = "XML file (*.xml)|*.xml",
                RestoreDirectory = true,
                DereferenceLinks = false,
                AutoUpgradeEnabled = false
            };

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                xmlDocument.Save(dialog.FileName);
            }

            xmlWriter.Close();
            xmlStream.Close();
        }

        private void _tsmiOpenConfig_Click(object sender, EventArgs e)
        {
            var dialog = new OpenFileDialog
            {
                Filter = "XML file (*.xml)|*.xml",
                RestoreDirectory = true,
                DereferenceLinks = false,
                AutoUpgradeEnabled = false
            };

            if (dialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            _map.ClearElements();
            _workField.Controls.Clear();
            _trafficManager.TrafficFlows.Clear();

            var xmlDocument = new XmlDocument();
            xmlDocument.Load(dialog.FileName);

            var configList = xmlDocument.SelectNodes("Config");
            if (configList.Count != 1)
            {
                throw new Exception("Invalid config file structure");
            }

            var mapList = configList[0].SelectNodes("Map");
            if (mapList.Count != 1)
            {
                throw new Exception("Invalid config file structure");
            }

            var elementList = mapList[0].SelectNodes("MapElement");
            for (var elementIter = 0; elementIter < elementList.Count; elementIter++)
            {
                var mapElementAttributes = elementList[elementIter].Attributes;
                var row = int.Parse(mapElementAttributes["Row"].Value);
                var column = int.Parse(mapElementAttributes["Column"].Value);
                var type = mapElementAttributes["Type"].Value;

                if (type == "Crossroad")
                {
                    AddCrossroad(row, column);
                }
                else if (type == "Road")
                {
                    var orientation = mapElementAttributes["Orientation"].Value;
                    if (orientation == "Horizontal")
                    {
                        AddHorizontalRoad(row, column);
                    }
                    else if (orientation == "Vertical")
                    {
                        AddVerticalRoad(row, column);
                    }
                    else
                    {
                        throw new ArgumentException("Invalid road orientation.");
                    }
                }
                else if (type == "Turn")
                {
                    var orientation = mapElementAttributes["Orientation"].Value;
                    if (orientation == "DownToLeft")
                    {
                        AddDownToLeftTurn(row, column);
                    }
                    else if (orientation == "LeftToUp")
                    {
                        AddLeftToUpTurn(row, column);
                    }
                    else if (orientation == "RightToDown")
                    {
                        AddRightToDownTurn(row, column);
                    }
                    else if (orientation == "UpToRight")
                    {
                        AddUpToRightTurn(row, column);
                    }
                    else
                    {
                        throw new ArgumentException("Invalid turn orientation.");
                    }
                }
                else
                {
                    throw new ArgumentException("Unknown element type.");
                }
            }

            var trafficFlowsList = configList[0].SelectNodes("TrafficFlows");
            if (trafficFlowsList.Count != 1)
            {
                throw new Exception("Invalid config file structure");
            }

            var trafficFlowList = trafficFlowsList[0].SelectNodes("TrafficFlow");
            for (var trafficFlowIter = 0; trafficFlowIter < trafficFlowList.Count; trafficFlowIter++)
            {
                _currentTrafficFlow = new TrafficFlow();
                var roadLine = new RoadLine {Color = GetNewPen()};
                _trafficFlowToRoadLineDictionary.Add(_currentTrafficFlow, roadLine);

                var trafficFlowAttributes = trafficFlowList[trafficFlowIter].Attributes;
                var density = double.Parse(trafficFlowAttributes["Density"].Value, CultureInfo.InvariantCulture);
                var speed = double.Parse(trafficFlowAttributes["Speed"].Value, CultureInfo.InvariantCulture);

                var pathList = trafficFlowList[trafficFlowIter].SelectNodes("Path");
                if (pathList.Count != 1)
                {
                    throw new Exception("Invalid config file structure");
                }

                var locationList = pathList[0].SelectNodes("Location");
                for (var locationIter = 0; locationIter < locationList.Count; locationIter++)
                {
                    var locationAttributes = locationList[locationIter].Attributes;
                    var locationRow = int.Parse(locationAttributes["Row"].Value, CultureInfo.InvariantCulture);
                    var locationColumn = int.Parse(locationAttributes["Column"].Value, CultureInfo.InvariantCulture);
                    _currentTrafficFlow.Path.Add(new Location(locationRow, locationColumn));
                }

                foreach (var location in _currentTrafficFlow.Path)
                {
                    var control = LocationToControl(location) as ACrossroadControl;
                    if (control == null)
                    {
                        throw new Exception("Something went wrong...");
                    }

                    control.DeleteLine(roadLine);
                    control.AddLine(roadLine);

                    control.Invalidate();
                }

                _currentTrafficFlow.TrafficSpeed = speed;
                _currentTrafficFlow.TrafficDensity = density;
                _trafficManager.AddTrafficFlow(_currentTrafficFlow);
            }
        }

        private void _btnStartSim_Click(object sender, EventArgs e)
        {
            const double seconds = 60;
            var simulationThread = new Thread(SimulationThreadFunc);
            simulationThread.Start(seconds);
        }

        private void _btnEndSim_Click(object sender, EventArgs e)
        {
            _simulationFlag = false;
        }

        private void SimulationThreadFunc(object seconds)
        {
            if (_simulatorEngine == null)
            {
                _simulatorEngine = new SimulatorEngine(_trafficManager);
            }

            _simulationFlag = true;
            while (_simulationFlag)
            {
                _simulatorEngine.Step((double) seconds);

                WriteLog();
                //UpdateLog();
                _tbxLog.Invoke(new UpdateLogFromSimulationThreadDelegate(UpdateLogFromSimulationThread));

                Thread.Sleep(100);
            }
        }

        private delegate void UpdateLogFromSimulationThreadDelegate();
    }
}