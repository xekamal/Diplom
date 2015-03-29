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
        private const string fileWtek = "d:\\Kate\\Wtek.txt";
        private const string fileState = "d:\\Kate\\states.txt";
        private const int nOftarfficLigths = 12;
        private const int nOfStates = 4;
        private readonly HNeuron[] HNeurons;
        private readonly RNeuron[] RNeurons;
        private readonly SNeuron[] SNeurons;
        private readonly double[,] W1;
        private readonly ICrossroad _crossroad;

        public CrossroadControllerReinforcement(ICrossroad crossroad)
        {
            _crossroad = crossroad;
            SNeurons = new SNeuron[nOftarfficLigths];
            for (int i = 0; i < SNeurons.Length; i++)
            {
                SNeurons[i] = new SNeuron();
            }
            setTrafficData();
            HNeurons = new HNeuron[nOftarfficLigths];
            for (int i = 0; i < HNeurons.Length; i++)
            {
                HNeurons[i] = new HNeuron(nOftarfficLigths);
            }
            RNeurons = new RNeuron[nOfStates];
            for (int i = 0; i < RNeurons.Length; i++)
            {
                RNeurons[i] = new RNeuron(nOftarfficLigths);
            }

            W0 = new double[nOftarfficLigths, nOftarfficLigths];
            W1 = new double[nOftarfficLigths, nOfStates];
            W_reader(fileWtek);

//            EducationWithTeacher(educationFileName);
        }

        public double[,] W0 { get; set; }

        public void Step()
        {
            for (int i = 0; i < nOftarfficLigths; i++)
            {
                SNeurons[i].Activation();
            }
            for (int i = 0; i < nOftarfficLigths; i++)
            {
                for (int j = 0; j < nOftarfficLigths; j++)
                {
                    HNeurons[i].dendrits[j] = W0[i, j]*SNeurons[j].axon;
                }

                HNeurons[i].Activation();
            }
            for (int i = 0; i < nOfStates; i++)
            {
                for (int j = 0; j < nOftarfficLigths; j++)
                {
                    RNeurons[i].dendrits[j] = W1[j, i]*HNeurons[j].axon;
                }
                RNeurons[i].Activation();
            }
            SetTrafficLights();
        }

        public void Reinforce(double marck)
        {
            if (_crossroad.LeftToUpTrafficLight.State == TrafficLightState.Green)
            {
                for (int i = 0; i < nOftarfficLigths; i++)
                {
                    W0[i, 0] -= marck;
                }
            }
            if (_crossroad.LeftToRightTrafficLight.State == TrafficLightState.Green)
            {
                for (int i = 0; i < nOftarfficLigths; i++)
                {
                    W0[i, 1] -= marck;
                }
            }
            if (_crossroad.LeftToDownTrafficLight.State == TrafficLightState.Green)
            {
                for (int i = 0; i < nOftarfficLigths; i++)
                {
                    W0[i, 2] -= marck;
                }
            }
            if (_crossroad.DownToLeftTrafficLight.State == TrafficLightState.Green)
            {
                for (int i = 0; i < nOftarfficLigths; i++)
                {
                    W0[i, 3] -= marck;
                }
            }
            if (_crossroad.DownToUpTrafficLight.State == TrafficLightState.Green)
            {
                for (int i = 0; i < nOftarfficLigths; i++)
                {
                    W0[i, 4] -= marck;
                }
            }
            if (_crossroad.DownToRightTrafficLight.State == TrafficLightState.Green)
            {
                for (int i = 0; i < nOftarfficLigths; i++)
                {
                    W0[i, 5] -= marck;
                }
            }
            if (_crossroad.RightToDownTrafficLight.State == TrafficLightState.Green)
            {
                for (int i = 0; i < nOftarfficLigths; i++)
                {
                    W0[i, 6] -= marck;
                }
            }
            if (_crossroad.RightToLeftTrafficLight.State == TrafficLightState.Green)
            {
                for (int i = 0; i < nOftarfficLigths; i++)
                {
                    W0[i, 7] -= marck;
                }
            }
            if (_crossroad.RightToUpTrafficLight.State == TrafficLightState.Green)
            {
                for (int i = 0; i < nOftarfficLigths; i++)
                {
                    W0[i, 8] -= marck;
                }
            }
            if (_crossroad.UpToRightTrafficLight.State == TrafficLightState.Green)
            {
                for (int i = 0; i < nOftarfficLigths; i++)
                {
                    W0[i, 9] -= marck;
                }
            }
            if (_crossroad.UpToDownTrafficLight.State == TrafficLightState.Green)
            {
                for (int i = 0; i < nOftarfficLigths; i++)
                {
                    W0[i, 10] -= marck;
                }
            }
            if (_crossroad.UpToLeftTrafficLight.State == TrafficLightState.Green)
            {
                for (int i = 0; i < nOftarfficLigths; i++)
                {
                    W0[i, 11] -= marck;
                }
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
        }

        public void W_reader(string f_name)
        {
            double l = 0;
            if (File.Exists(f_name))
            {
                //Read states
                StreamReader sr = File.OpenText(f_name);
                for (int j = 0; j < nOftarfficLigths; j++)
                {
                    string s = "";
                    s = sr.ReadLine();
                    string[] v = s.Split(' ');
                    for (int i = 0; i < nOftarfficLigths; i++)
                    {
                        l = double.Parse(v[i], CultureInfo.InvariantCulture);
                        W0[i, j] = l;
                    }
                }
                for (int i = 0; i < nOftarfficLigths; i++)
                {
                    string s = "";
                    s = sr.ReadLine();
                    string[] v = s.Split(' ');
                    for (int j = 0; j < nOfStates; j++)
                    {
                        l = double.Parse(v[j], CultureInfo.InvariantCulture);
                        W1[i, j] = l;
                    }
                }
                sr.Close();
            }
        }

        private void EducationWithTeacher(string educationFile)
        {
            //	float T; // порог функции
            //	W_reader(file_W0);
            var input = new double[nOftarfficLigths];
            var output = new double[nOfStates];
            int i = 0;
            const double a = 0.01;
            var sr = new StreamReader(educationFile);

            double l = 0;
            string s = "";
            while ((s = sr.ReadLine()) != null)
            {
                i = 0;
                string[] v = s.Split(' ');
                ///////Считываем из файла строку с обучающим примером////////////////
                while (i < nOftarfficLigths + nOfStates)
                {
                    if (i < nOftarfficLigths)
                    {
                        l = double.Parse(v[i], CultureInfo.InvariantCulture);
                        input[i] = l;
                        i++;
                    }
                    if (i >= nOftarfficLigths)
                    {
                        l = double.Parse(v[i + 4], CultureInfo.InvariantCulture);
                        output[i - nOftarfficLigths] = l;
                        i++;
                    }
                }

                ///////подаем на вход данные обучающего примера		
                for (int k = 0; k < nOfStates; k++)
                {
                    for (int j = 0; j < nOftarfficLigths; j++)
                    {
                        RNeurons[k].dendrits[j] = input[j]*W1[j, k];
                    }
                    RNeurons[k].Activation();
                    /////////// Проверяем правильность полученных выходов			
                    if ((RNeurons[k].axon - output[k]) == -1) /////если полученные результаты не верны, регулируем веса
                    {
                        for (int j = 0; j < nOftarfficLigths; j++)
                        {
                            W1[j, k] += a*(input[j] - W1[j, k]);
                        }
                        k = k - 1; /////снова подаем данные этого же обучающего примера 
                        W_writer(fileWtek);
                    }
                    else if ((RNeurons[k].axon - output[k]) == 1)
                        /////если полученные результаты не верны, регулируем веса
                    {
                        for (int j = 0; j < nOftarfficLigths; j++)
                        {
                            W1[j, k] -= a*(input[j] - W1[j, k]);
                        }
                        k = k - 1; /////снова подаем данные этого же обучающего примера 
                    }
                }
            }
            sr.Close();
            W_writer(fileWtek);
        }

        private void W_writer(string fileWtek)
        {
            if (File.Exists(fileWtek))
            {
                var sw = new StreamWriter(fileWtek);
                for (int i = 0; i < nOftarfficLigths; i++)
                {
                    for (int j = 0; j < nOftarfficLigths; j++)
                    {
                        sw.Write("{0} ", W0[i, j].ToString(CultureInfo.InvariantCulture));
                    }
                    sw.WriteLine();
                }
                for (int i = 0; i < nOftarfficLigths; i++)
                {
                    for (int j = 0; j < nOfStates; j++)
                    {
                        sw.Write("{0} ", W1[i, j].ToString(CultureInfo.InvariantCulture));
                    }
                    sw.WriteLine();
                }
                sw.Close();
            }
        }

        private void SetTrafficLights()
        {
            double x = 0;
            for (int i = 0; i < nOfStates; i++)
            {
                x += RNeurons[i].axon;
            }
            double T = x/nOfStates;
            for (int i = 0; i < nOfStates; i++)
            {
                if (x < T)
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
            for (int i = 0; i < nOfStates; i++)
            {
                string s = "";
                s = sr.ReadLine();
                string[] v = s.Split(' ');
                var mas = new TrafficLightState[nOftarfficLigths];
                if (RNeurons[i].axonState == TrafficLightState.Green &&
                    (double.Parse(v[i], CultureInfo.InvariantCulture) - 1) < 0.0001)
                {
                    for (int j = 0; j < nOftarfficLigths; j++)
                    {
                        if (int.Parse(v[j + nOfStates - 1]) != 0)
                        {
                            mas[j] = TrafficLightState.Green;
                        }
                        else
                        {
                            mas[j] = TrafficLightState.Red;
                        }
                    }

                    _crossroad.LeftToUpTrafficLight.State = mas[0];
                    _crossroad.LeftToRightTrafficLight.State = mas[1];
                    _crossroad.LeftToDownTrafficLight.State = mas[2];
                    _crossroad.DownToLeftTrafficLight.State = mas[3];
                    _crossroad.DownToUpTrafficLight.State = mas[4];
                    _crossroad.DownToRightTrafficLight.State = mas[5];
                    _crossroad.RightToDownTrafficLight.State = mas[6];
                    _crossroad.RightToLeftTrafficLight.State = mas[7];
                    _crossroad.RightToUpTrafficLight.State = mas[8];
                    _crossroad.UpToRightTrafficLight.State = mas[9];
                    _crossroad.UpToDownTrafficLight.State = mas[10];
                    _crossroad.UpToLeftTrafficLight.State = mas[11];
                }
            }
            sr.Close();
        }
    }
}