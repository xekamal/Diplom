using System;
using System.Globalization;
using System.IO;
using Simulator.Map;
using Simulator.Neuro.Domain;

namespace Simulator.Neuro.Infrastructure
{
    public class CrossroadControllerReinforcement : ICrossroadController
    {
        private const string educationFileName = "d:\\Kate\\Education_example.txt";
        private const string fileW0 = "d:\\Kate\\W0.txt";
//        private const string fileWtek = "e:\\Kate\\Wtek.txt";
        private const string fileState = "d:\\Kate\\states.txt";
        public int NofTrafficLights = 12;
        public int NofStates = 4;
        public HNeuron[] HNeurons { get; set; }
        public RNeuron[] RNeurons { get; set; }
        private readonly SNeuron[] SNeurons;
        public double[,] W1 { get; set; }
        private readonly ICrossroad _crossroad;
        private string fileWtek;

        public CrossroadControllerReinforcement(ICrossroad crossroad)
        {
            _crossroad = crossroad;
            SNeurons = new SNeuron[NofTrafficLights];
            for (int i = 0; i < SNeurons.Length; i++)
            {
                SNeurons[i] = new SNeuron();
            }
            setTrafficData();
            HNeurons = new HNeuron[NofTrafficLights];
            for (int i = 0; i < HNeurons.Length; i++)
            {
                HNeurons[i] = new HNeuron(NofTrafficLights);
            }
            RNeurons = new RNeuron[NofStates];
            for (int i = 0; i < RNeurons.Length; i++)
            {
                RNeurons[i] = new RNeuron(NofTrafficLights);
            }
            fileWtek = string.Format("d:\\Kate\\Wtek{0}{1}.txt", crossroad.Row, crossroad.Column);
            W0 = new double[NofTrafficLights, NofTrafficLights];
            W1 = new double[NofTrafficLights, NofStates];
            
            if (File.Exists(fileWtek))
            {
                W_reader(fileWtek);
            }
            else
            {
                W_reader(fileW0);
                W_writer(fileWtek);
            //    EducationWithTeacher(educationFileName);
            }

           /* _crossroad = crossroad;
            
            SNeurons = new SNeuron[NofTrafficLights];
            HNeurons = new HNeuron[NofTrafficLights];
            RNeurons = new RNeuron[NofStates];

            for (int i = 0; i < NofTrafficLights; i++)
            {
                SNeurons[i] = new SNeuron();
                HNeurons[i] = new HNeuron(NofTrafficLights);
            }

            for (int i = 0; i < NofStates; i++)
            {
                RNeurons[i] = new RNeuron(NofTrafficLights);
            }

            W0 = new double[NofTrafficLights, NofTrafficLights];
            W1 = new double[NofTrafficLights, NofStates];*/
        }

        public double[,] W0 { get; set; }

        public void Step()
        {
            for (int i = 0; i < NofTrafficLights; i++)
            {
                SNeurons[i].Activation();
            }
            for (int i = 0; i < NofTrafficLights; i++)
            {
                for (int j = 0; j < NofTrafficLights; j++)
                {
 //                   HNeurons[i].dendrits[j] = W0[i, j]*SNeurons[j].axon;
                    HNeurons[i].dendrits[j] = W0[j, i] * SNeurons[j].axon;
                }

                HNeurons[i].Activation();
            }
            for (int i = 0; i < NofStates; i++)
            {
                for (int j = 0; j < NofTrafficLights; j++)
                {
                    RNeurons[i].dendrits[j] = W1[j, i]*HNeurons[j].axon;
                }
                RNeurons[i].Activation();
            }
            SetTrafficLights();
        }

        private void W_writer(string fileWtek)
        {
            var sw = new StreamWriter(fileWtek);
            for (int i = 0; i < NofTrafficLights; i++)
            {
                for (int j = 0; j < NofTrafficLights; j++)
                {
                    sw.Write("{0} ", W0[i, j].ToString(CultureInfo.InvariantCulture));
                }
                sw.WriteLine();
            }
            for (int i = 0; i < NofTrafficLights; i++)
            {
                for (int j = 0; j < NofStates; j++)
                {
                    sw.Write("{0} ", W1[i, j].ToString(CultureInfo.InvariantCulture));
                }
                sw.WriteLine();
            }
            sw.Close();
        }

        public void Reinforce(double marck)
        {
            /*if (Math.Abs(marck) < 0.001)
            {
                marck = 1000.0;
                if (_crossroad.LeftToUpTrafficLight.State == TrafficLightState.Green)
                {
                    for (int i = 0; i < NofTrafficLights; i++)
                    {
                        W0[i, 0] += Math.Abs(1 / marck);
                    }
                }
                if (_crossroad.LeftToRightTrafficLight.State == TrafficLightState.Green)
                {
                    for (int i = 0; i < NofTrafficLights; i++)
                    {
                        W0[i, 1] += Math.Abs(1 / marck);
                    }
                }
                if (_crossroad.LeftToDownTrafficLight.State == TrafficLightState.Green)
                {
                    for (int i = 0; i < NofTrafficLights; i++)
                    {
                        W0[i, 2] += Math.Abs(1 / marck);
                    }
                }
                if (_crossroad.DownToLeftTrafficLight.State == TrafficLightState.Green)
                {
                    for (int i = 0; i < NofTrafficLights; i++)
                    {
                        W0[i, 3] += Math.Abs(1 / marck);
                    }
                }
                if (_crossroad.DownToUpTrafficLight.State == TrafficLightState.Green)
                {
                    for (int i = 0; i < NofTrafficLights; i++)
                    {
                        W0[i, 4] += Math.Abs(1 / marck);
                    }
                }
                if (_crossroad.DownToRightTrafficLight.State == TrafficLightState.Green)
                {
                    for (int i = 0; i < NofTrafficLights; i++)
                    {
                        W0[i, 5] += Math.Abs(1 / marck);
                    }
                }
                if (_crossroad.RightToDownTrafficLight.State == TrafficLightState.Green)
                {
                    for (int i = 0; i < NofTrafficLights; i++)
                    {
                        W0[i, 6] += Math.Abs(1 / marck);
                    }
                }
                if (_crossroad.RightToLeftTrafficLight.State == TrafficLightState.Green)
                {
                    for (int i = 0; i < NofTrafficLights; i++)
                    {
                        W0[i, 7] += Math.Abs(1 / marck);
                    }
                }
                if (_crossroad.RightToUpTrafficLight.State == TrafficLightState.Green)
                {
                    for (int i = 0; i < NofTrafficLights; i++)
                    {
                        W0[i, 8] += Math.Abs(1 / marck);
                    }
                }
                if (_crossroad.UpToRightTrafficLight.State == TrafficLightState.Green)
                {
                    for (int i = 0; i < NofTrafficLights; i++)
                    {
                        W0[i, 9] += Math.Abs(1 / marck);
                    }
                }
                if (_crossroad.UpToDownTrafficLight.State == TrafficLightState.Green)
                {
                    for (int i = 0; i < NofTrafficLights; i++)
                    {
                        W0[i, 10] += Math.Abs(1 / marck);
                    }
                }
                if (_crossroad.UpToLeftTrafficLight.State == TrafficLightState.Green)
                {
                    for (int i = 0; i < NofTrafficLights; i++)
                    {
                        W0[i, 11] += Math.Abs(1 / marck);
                    }
                }  
            }

            if (marck < 0)
            {

                if (_crossroad.LeftToUpTrafficLight.State == TrafficLightState.Green)
                {
                    for (int i = 0; i < NofTrafficLights; i++)
                    {
                        W0[i, 0] += Math.Abs(1 / marck);
                    }
                }
                if (_crossroad.LeftToRightTrafficLight.State == TrafficLightState.Green)
                {
                    for (int i = 0; i < NofTrafficLights; i++)
                    {
                        W0[i, 1] += Math.Abs(1 / marck);
                    }
                }
                if (_crossroad.LeftToDownTrafficLight.State == TrafficLightState.Green)
                {
                    for (int i = 0; i < NofTrafficLights; i++)
                    {
                        W0[i, 2] += Math.Abs(1 / marck);
                    }
                }
                if (_crossroad.DownToLeftTrafficLight.State == TrafficLightState.Green)
                {
                    for (int i = 0; i < NofTrafficLights; i++)
                    {
                        W0[i, 3] += Math.Abs(1 / marck);
                    }
                }
                if (_crossroad.DownToUpTrafficLight.State == TrafficLightState.Green)
                {
                    for (int i = 0; i < NofTrafficLights; i++)
                    {
                        W0[i, 4] += Math.Abs(1 / marck);
                    }
                }
                if (_crossroad.DownToRightTrafficLight.State == TrafficLightState.Green)
                {
                    for (int i = 0; i < NofTrafficLights; i++)
                    {
                        W0[i, 5] += Math.Abs(1 / marck);
                    }
                }
                if (_crossroad.RightToDownTrafficLight.State == TrafficLightState.Green)
                {
                    for (int i = 0; i < NofTrafficLights; i++)
                    {
                        W0[i, 6] += Math.Abs(1 / marck);
                    }
                }
                if (_crossroad.RightToLeftTrafficLight.State == TrafficLightState.Green)
                {
                    for (int i = 0; i < NofTrafficLights; i++)
                    {
                        W0[i, 7] += Math.Abs(1 / marck);
                    }
                }
                if (_crossroad.RightToUpTrafficLight.State == TrafficLightState.Green)
                {
                    for (int i = 0; i < NofTrafficLights; i++)
                    {
                        W0[i, 8] += Math.Abs(1 / marck);
                    }
                }
                if (_crossroad.UpToRightTrafficLight.State == TrafficLightState.Green)
                {
                    for (int i = 0; i < NofTrafficLights; i++)
                    {
                        W0[i, 9] += Math.Abs(1 / marck);
                    }
                }
                if (_crossroad.UpToDownTrafficLight.State == TrafficLightState.Green)
                {
                    for (int i = 0; i < NofTrafficLights; i++)
                    {
                        W0[i, 10] +=Math.Abs(1 / marck);
                    }
                }
                if (_crossroad.UpToLeftTrafficLight.State == TrafficLightState.Green)
                {
                    for (int i = 0; i < NofTrafficLights; i++)
                    {
                        W0[i, 11] += Math.Abs(1 / marck);
                    }
                }  
            }
            if (marck > 0)
            {
                if (_crossroad.LeftToUpTrafficLight.State == TrafficLightState.Green)
                {
                    for (int i = 0; i < NofTrafficLights; i++)
                    {
                        W0[i, 0] -= Math.Abs(1 / marck);
                    }
                }
                if (_crossroad.LeftToRightTrafficLight.State == TrafficLightState.Green)
                {
                    for (int i = 0; i < NofTrafficLights; i++)
                    {
                        W0[i, 1] -= Math.Abs(1 / marck);
                    }
                }
                if (_crossroad.LeftToDownTrafficLight.State == TrafficLightState.Green)
                {
                    for (int i = 0; i < NofTrafficLights; i++)
                    {
                        W0[i, 2] -= Math.Abs(1 / marck);
                    }
                }
                if (_crossroad.DownToLeftTrafficLight.State == TrafficLightState.Green)
                {
                    for (int i = 0; i < NofTrafficLights; i++)
                    {
                        W0[i, 3] -= Math.Abs(1 / marck);
                    }
                }
                if (_crossroad.DownToUpTrafficLight.State == TrafficLightState.Green)
                {
                    for (int i = 0; i < NofTrafficLights; i++)
                    {
                        W0[i, 4] -= Math.Abs(1 / marck);
                    }
                }
                if (_crossroad.DownToRightTrafficLight.State == TrafficLightState.Green)
                {
                    for (int i = 0; i < NofTrafficLights; i++)
                    {
                        W0[i, 5] -= Math.Abs(1 / marck);
                    }
                }
                if (_crossroad.RightToDownTrafficLight.State == TrafficLightState.Green)
                {
                    for (int i = 0; i < NofTrafficLights; i++)
                    {
                        W0[i, 6] -= Math.Abs(1 / marck);
                    }
                }
                if (_crossroad.RightToLeftTrafficLight.State == TrafficLightState.Green)
                {
                    for (int i = 0; i < NofTrafficLights; i++)
                    {
                        W0[i, 7] -= Math.Abs(1 / marck);
                    }
                }
                if (_crossroad.RightToUpTrafficLight.State == TrafficLightState.Green)
                {
                    for (int i = 0; i < NofTrafficLights; i++)
                    {
                        W0[i, 8] -= Math.Abs(1 / marck);
                    }
                }
                if (_crossroad.UpToRightTrafficLight.State == TrafficLightState.Green)
                {
                    for (int i = 0; i < NofTrafficLights; i++)
                    {
                        W0[i, 9] -= Math.Abs(1 / marck);
                    }
                }
                if (_crossroad.UpToDownTrafficLight.State == TrafficLightState.Green)
                {
                    for (int i = 0; i < NofTrafficLights; i++)
                    {
                        W0[i, 10] -= Math.Abs(1 / marck);
                    }
                }
                if (_crossroad.UpToLeftTrafficLight.State == TrafficLightState.Green)
                {
                    for (int i = 0; i < NofTrafficLights; i++)
                    {
                        W0[i, 11] -= Math.Abs(1 / marck);
                    }
                }  
            }*/
            for (int i = 0; i < NofTrafficLights; i++)
            {
                /*W0[i, 0] += -0.025*( marck - _crossroad.LeftToUpTrafficData.TrafficDensity);
                W0[i, 1] += -0.025 * (marck - _crossroad.LeftToRightTrafficData.TrafficDensity);
                W0[i, 2] += -0.025 * (marck - _crossroad.LeftToDownTrafficData.TrafficDensity);
                W0[i, 3] += -0.025 * (marck - _crossroad.DownToLeftTrafficData.TrafficDensity);
                W0[i, 4] += -0.025 * (marck - _crossroad.DownToUpTrafficData.TrafficDensity);
                W0[i, 5] += -0.025 * (marck - _crossroad.DownToRightTrafficData.TrafficDensity);
                W0[i, 6] += -0.025 * (marck - _crossroad.RightToDownTrafficData.TrafficDensity);
                W0[i, 7] += -0.025 * (marck - _crossroad.RightToLeftTrafficData.TrafficDensity);
                W0[i, 8] += -0.025 * (marck - _crossroad.RightToUpTrafficData.TrafficDensity);
                W0[i, 9] += -0.025 * (marck - _crossroad.UpToLeftTrafficData.TrafficDensity);
                W0[i, 10] += -0.025 * (marck - _crossroad.UpToDownTrafficData.TrafficDensity);
                W0[i, 11] += -0.025 * (marck - _crossroad.UpToRightTrafficData.TrafficDensity);*/
                
               W0[0, i] += 0.025*( marck - _crossroad.LeftToUpTrafficData.TrafficDensity);
               W0[1, i] += 0.025 * (marck - _crossroad.LeftToRightTrafficData.TrafficDensity);
               W0[2, i] += 0.025 * (marck - _crossroad.LeftToDownTrafficData.TrafficDensity);
               W0[3, i] += 0.025 * (marck - _crossroad.DownToLeftTrafficData.TrafficDensity);
               W0[4, i] += 0.025 * (marck - _crossroad.DownToUpTrafficData.TrafficDensity);
               W0[5, i] += 0.025 * (marck - _crossroad.DownToRightTrafficData.TrafficDensity);
               W0[6, i] += 0.025 * (marck - _crossroad.RightToDownTrafficData.TrafficDensity);
               W0[7, i] += 0.025 * (marck - _crossroad.RightToLeftTrafficData.TrafficDensity);
               W0[8, i] += 0.025 * (marck - _crossroad.RightToUpTrafficData.TrafficDensity);
               W0[9, i] += 0.025 * (marck - _crossroad.UpToLeftTrafficData.TrafficDensity);
               W0[10, i] +=0.025 * (marck - _crossroad.UpToDownTrafficData.TrafficDensity);
               W0[11, i] +=0.025 * (marck - _crossroad.UpToRightTrafficData.TrafficDensity);

            }
            
            W_writer(fileWtek);
        }

        private void setTrafficData()
        {
            SNeurons[0].TrafficData = _crossroad.LeftToUpTrafficData;
            SNeurons[1].TrafficData = _crossroad.LeftToRightTrafficData;
            SNeurons[2].TrafficData = _crossroad.LeftToDownTrafficData;
            SNeurons[3].TrafficData = _crossroad.DownToLeftTrafficData;
            SNeurons[4].TrafficData = _crossroad.DownToUpTrafficData;
            SNeurons[5].TrafficData = _crossroad.DownToRightTrafficData;
            SNeurons[6].TrafficData = _crossroad.RightToDownTrafficData;
            SNeurons[7].TrafficData = _crossroad.RightToLeftTrafficData;
            SNeurons[8].TrafficData = _crossroad.RightToUpTrafficData;
            SNeurons[9].TrafficData = _crossroad.UpToRightTrafficData;
            SNeurons[10].TrafficData = _crossroad.UpToDownTrafficData;
            SNeurons[11].TrafficData = _crossroad.UpToLeftTrafficData;

            SNeurons[0].TrafficLight = _crossroad.LeftToUpTrafficLight;
            SNeurons[1].TrafficLight = _crossroad.LeftToRightTrafficLight;
            SNeurons[2].TrafficLight = _crossroad.LeftToDownTrafficLight;
            SNeurons[3].TrafficLight = _crossroad.DownToLeftTrafficLight;
            SNeurons[4].TrafficLight = _crossroad.DownToUpTrafficLight;
            SNeurons[5].TrafficLight = _crossroad.DownToRightTrafficLight;
            SNeurons[6].TrafficLight = _crossroad.RightToDownTrafficLight;
            SNeurons[7].TrafficLight = _crossroad.RightToLeftTrafficLight;
            SNeurons[8].TrafficLight = _crossroad.RightToUpTrafficLight;
            SNeurons[9].TrafficLight = _crossroad.UpToRightTrafficLight;
            SNeurons[10].TrafficLight = _crossroad.UpToDownTrafficLight;
            SNeurons[11].TrafficLight = _crossroad.UpToLeftTrafficLight;
        
        }

        public void W_reader(string f_name)
        {
            double l = 0;
            if (File.Exists(f_name))
            {
                //Read states
                StreamReader sr = File.OpenText(f_name);
                for (int j = 0; j < NofTrafficLights; j++)
                {
                    string s = "";
                    s = sr.ReadLine();
                    string[] v = s.Split(' ');
                    for (int i = 0; i < NofTrafficLights; i++)
                    {
                        l = double.Parse(v[i], CultureInfo.InvariantCulture);
                        W0[j, i] = l;
                    }
                }
                for (int i = 0; i < NofTrafficLights; i++)
                {
                    string s = "";
                    s = sr.ReadLine();
                    string[] v = s.Split(' ');
                    for (int j = 0; j < NofStates; j++)
                    {
                        l = double.Parse(v[j], CultureInfo.InvariantCulture);
                        W1[i, j] = l;
                    }
                }
                sr.Close();
            }
        }
        public double ComputePassRate(string[] lines)
        {
            var passRate = 0.0;

            foreach (var line in lines)
            {
                var input = new double[NofTrafficLights];
                var output = new double[NofStates];
                var values = line.Split(' ');

                for (var i = 0; i < NofTrafficLights; i++)
                {
                    input[i] = double.Parse(values[i], CultureInfo.InvariantCulture);
                }

                for (var i = 0; i < NofStates; i++)
                {
                    output[i] = double.Parse(values[NofTrafficLights + i]);
                }

                var isPassed = true;
                for (var i = 0; i < NofStates; i++)
                {
                    for (var j = 0; j < NofTrafficLights; j++)
                    {
                        RNeurons[i].dendrits[j] = input[j] * W1[j, i];
                    }

                    RNeurons[i].Activation();

                    if (Math.Abs(RNeurons[i].axon - output[i]) > 0.1)
                    {
                        isPassed = false;
                        break;
                    }

                }

                if (isPassed)
                {
                    passRate += 1.0;
                }
            }

            return passRate / lines.Length;
        }
        private void EducationWithTeacher(string pathToEducationExample)
        {
           var lines = File.ReadAllLines(pathToEducationExample);

            double bestPassRate = ComputePassRate(lines);
            var bestMatrix = new double[NofTrafficLights, NofStates];
            CopyMatrix(W1, bestMatrix, NofTrafficLights, NofStates);

            foreach (var line in lines)
            {
                var input = new double[NofTrafficLights];
                var output = new double[NofStates];
                var values = line.Split(' ');

                for (var i = 0; i < NofTrafficLights; i++)
                {
                    input[i] = double.Parse(values[i], CultureInfo.InvariantCulture);
                }

                for (var i = 0; i < NofStates; i++)
                {
                    output[i] = double.Parse(values[NofTrafficLights + i]);
                }

                var nofRepeats = 0;
                for (var i = 0; i < NofStates; i++)
                {
                    if (nofRepeats >= 100)
                    {
                        nofRepeats = 0;
                        continue;
                    }

                    for (var j = 0; j < NofTrafficLights; j++)
                    {
                        RNeurons[i].dendrits[j] = input[j]*W1[j, i];
                    }

                    RNeurons[i].Activation();

                    if (Math.Abs(RNeurons[i].axon - output[i]) > 0.1)
                    {
                        // double e = (output[i] - crossroadController.RNeurons[i].axon) *
                        //           crossroadController.RNeurons[i].axon * (1 - crossroadController.RNeurons[i].axon);
                        for (var j = 0; j < NofTrafficLights; j++)
                        {

                            //crossroadController.W1[j, i] += 0.01 * (input[j] - crossroadController.W1[j, i]);
                            W1[j, i] = W1[j, i] +(output[i] - RNeurons[i].axon)*input[j];
                            //    crossroadController.W1[j, i] = crossroadController.W1[j, i] + 0.025*e*input[j];
                        }

                        double newPassRate = ComputePassRate(lines);
                        if (newPassRate > bestPassRate)
                        {
                            bestPassRate = newPassRate;
                            CopyMatrix(W1, bestMatrix, NofTrafficLights,NofStates);
                        }

                        i--;
                        nofRepeats++;
                    }
                    else
                    {
                        nofRepeats = 0;
                    }
                }
            }
            CopyMatrix(bestMatrix, W1, NofTrafficLights, NofStates);
            W_reader(fileWtek);
        }
        public static void CopyMatrix(double[,] from, double[,] to, int nofRows, int nofOfColumns)
        {
            for (var i = 0; i < nofRows; i++)
            {
                for (var j = 0; j < nofOfColumns; j++)
                {
                    to[i, j] = from[i, j];
                }
            }
        }
        private void SetTrafficLights()
        {
            double x = 0;
            for (int i = 0; i < NofStates; i++)
            {
                x += RNeurons[i].atan;
            }
            double T = x/NofStates;
            for (int i = 0; i < NofStates; i++)
            {
                if (RNeurons[i].axon < T)
                {
                    RNeurons[i].axonState = TrafficLightState.Red;
                }
                else
                {
                    RNeurons[i].axonState = TrafficLightState.Green;
                    break;
                }
            }

            StreamReader sr = File.OpenText(fileState);
            for (int i = 0; i < NofStates; i++)
            {
                string s = "";
                s = sr.ReadLine();
                string[] v = s.Split(' ');
                var mas = new TrafficLightState[NofTrafficLights];
                if (RNeurons[i].axonState == TrafficLightState.Green &&
                    (double.Parse(v[i], CultureInfo.InvariantCulture) - 1) < 0.0001)
                {
                    for (int j = 0; j < NofTrafficLights; j++)
                    {
                        if (int.Parse(v[j + NofStates]) != 0)
                        {
                            mas[j] = TrafficLightState.Green;
                        }
                        else
                        {
                            mas[j] = TrafficLightState.Red;
                        }
                    }

                    _crossroad.LeftToUpTrafficLight.SetTrafficLightState(mas[0]);
                    _crossroad.LeftToRightTrafficLight.SetTrafficLightState(mas[1]);
                    _crossroad.LeftToDownTrafficLight.SetTrafficLightState(mas[2]);
                    _crossroad.DownToLeftTrafficLight.SetTrafficLightState(mas[3]);
                    _crossroad.DownToUpTrafficLight.SetTrafficLightState(mas[4]);
                    _crossroad.DownToRightTrafficLight.SetTrafficLightState(mas[5]);
                    _crossroad.RightToDownTrafficLight.SetTrafficLightState(mas[6]);
                    _crossroad.RightToLeftTrafficLight.SetTrafficLightState(mas[7]);
                    _crossroad.RightToUpTrafficLight.SetTrafficLightState(mas[8]);
                    _crossroad.UpToRightTrafficLight.SetTrafficLightState(mas[9]);
                    _crossroad.UpToDownTrafficLight.SetTrafficLightState(mas[10]);
                    _crossroad.UpToLeftTrafficLight.SetTrafficLightState(mas[11]);
                }
            }
            sr.Close();
        }
    }
}