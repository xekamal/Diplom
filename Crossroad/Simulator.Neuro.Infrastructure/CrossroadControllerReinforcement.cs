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
        private const string fileWtek = "d:\\Kate\\Wtek.txt";
        const int nOftarfficLigths = 12;
        const int nOfStates = 4;
        private readonly ICrossroad _crossroad;
        private SNeuron[] SNeurons;
        private HNeuron[] HNeurons;
        private RNeuron[] RNeurons;
        private double[,] W0;
        private double[,] W1;

        private void setTrafficData()
        {
            SNeurons[0].TrafficData = _crossroad.UpToLeftTrafficData;
            SNeurons[1].TrafficData = _crossroad.RightToLeftTrafficData;
            SNeurons[2].TrafficData = _crossroad.DownToLeftTrafficData;
            SNeurons[3].TrafficData = _crossroad.LeftToDownTrafficData;
            SNeurons[4].TrafficData = _crossroad.UpToDownTrafficData;
            SNeurons[5].TrafficData = _crossroad.RightToDownTrafficData;
            SNeurons[6].TrafficData = _crossroad.DownToRightTrafficData;
            SNeurons[7].TrafficData = _crossroad.LeftToRightTrafficData;
            SNeurons[8].TrafficData = _crossroad.UpToRightTrafficData;
            SNeurons[9].TrafficData = _crossroad.RightToUpTrafficData;
            SNeurons[10].TrafficData = _crossroad.DownToUpTrafficData;
            SNeurons[11].TrafficData = _crossroad.LeftToUpTrafficData;
        }

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
            W0= new double[nOftarfficLigths,nOftarfficLigths];
            W1 = new double[nOftarfficLigths, nOfStates];            
            W_reader(fileW0);
//            EducationWithTeacher(educationFileName);
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
                    var v = s.Split(' ');
                    for (int i = 0; i < nOftarfficLigths; i++)
                    {
                        l = double.Parse(v[i], CultureInfo.InvariantCulture);
                        W0[i,j] = l;
                    }
                }
                for (int i = 0; i < nOftarfficLigths; i++)
                {
                    string s = "";
                    s = sr.ReadLine();
                    var v = s.Split(' ');
                    for (int j = 0; j < nOfStates; j++)
                    {
                        l = double.Parse(v[j], CultureInfo.InvariantCulture);
                        W1[i,j] = l;
                    }
                }
                sr.Close();
            }
            else
            {
                //File doesn't exist
            }
        }
        private void EducationWithTeacher(string educationFile)
        {
            //	float T; // порог функции
            //	W_reader(file_W0);
            double[] input = new double[nOftarfficLigths];
            double[] output = new double[nOfStates];
            int i = 0;
            const double a = 0.01;
            StreamReader sr = new StreamReader(educationFile);

            double l = 0;
            string s = "";
            while ((s = sr.ReadLine()) != null)
            {
                i = 0;
                var v = s.Split(' ');
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
                        RNeurons[k].dendrits[j] = input[j] * W1[j,k];
                    }
                    RNeurons[k].Activation();
                    /////////// Проверяем правильность полученных выходов			
                    if ((RNeurons[k].axon - output[k]) == -1) /////если полученные результаты не верны, регулируем веса
                    {
                        for (int j = 0; j < nOftarfficLigths; j++)
                        {
                            W1[j,k] += a * (input[j] - W1[j,k]);
                        }
                        k = k - 1;						/////снова подаем данные этого же обучающего примера 
                        W_writer(fileWtek);
                    }
                    else
                        if ((RNeurons[k].axon - output[k]) == 1) /////если полученные результаты не верны, регулируем веса
                        {
                            for (int j = 0; j < nOftarfficLigths; j++)
                            {
                                W1[j,k] -= a * (input[j] - W1[j,k]);
                            }
                            k = k - 1;						/////снова подаем данные этого же обучающего примера 
                        }
                }
            }
            sr.Close();
            W_writer(fileW0);
        }

        private void W_writer(string fileWtek)
        {
            if (File.Exists(fileWtek))
            {
                StreamWriter sw = new StreamWriter(fileWtek);
                for (int i = 0; i < nOftarfficLigths; i++)
                {
                    for (int j = 0; j < nOftarfficLigths; j++)
                    {
                        sw.Write(string.Format("{0} ", W0[i,j]));
                    }
                    sw.WriteLine();
                }
                for (int i = 0; i < nOftarfficLigths; i++)
                {
                    for (int j = 0; j < nOfStates; j++)
                    {
                        sw.Write(string.Format("{0} ", W1[i,j]));
                    }
                    sw.WriteLine();
                }
                sw.Close();
            }

        }
        public void Step()
        {
            throw new System.NotImplementedException();
        }

        public void Reinforce(double value)
        {
            throw new System.NotImplementedException();
        }
    }
}