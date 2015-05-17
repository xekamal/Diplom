using System;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
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
        private readonly ICrossroad _crossroad;
        private readonly string fileWtek;
        private readonly SNeuron[] SNeurons;
        public int NofStates = 4;
        public int NofTrafficLights = 12;

        public CrossroadControllerReinforcement(ICrossroad crossroad)
        {
            _crossroad = crossroad;
            SNeurons = new SNeuron[NofTrafficLights];
            for (var i = 0; i < SNeurons.Length; i++)
            {
                SNeurons[i] = new SNeuron();
            }
            setTrafficData();
            HNeurons = new HNeuron[NofTrafficLights];
            for (var i = 0; i < HNeurons.Length; i++)
            {
                HNeurons[i] = new HNeuron(NofTrafficLights);
            }
            RNeurons = new RNeuron[NofStates];
            for (var i = 0; i < RNeurons.Length; i++)
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

        public HNeuron[] HNeurons { get; set; }
        public RNeuron[] RNeurons { get; set; }
        public double[,] W1 { get; set; }
        public double[,] W0 { get; set; }

        public void Step()
        {
            /*int flag = 0;
            for (int i = 0; i < NofTrafficLights; i++)
            {
                for (int j = 0; j < NofTrafficLights; j++)
                {
                    if (W0[i, j] > 1.0) flag++;
                }
            }

            if (flag == 144)
            {
                for (int i = 0; i < NofTrafficLights; i++)
                {
                    for (int j = 0; j < NofTrafficLights; j++)
                    {
                        W0[i, j] -= 1;
                    }
                }
            }*/
            for (var i = 0; i < NofTrafficLights; i++)
            {
                SNeurons[i].Activation();
            }
            for (var i = 0; i < NofTrafficLights; i++)
            {
                for (var j = 0; j < NofTrafficLights; j++)
                {
                    //                   HNeurons[i].dendrits[j] = W0[i, j]*SNeurons[j].axon;
                    HNeurons[i].dendrits[j] = W0[j, i]*SNeurons[j].axon;
                }

                HNeurons[i].Activation();
            }
            for (var i = 0; i < NofStates; i++)
            {
                for (var j = 0; j < NofTrafficLights; j++)
                {
                    RNeurons[i].dendrits[j] = W1[j, i]*HNeurons[j].axon;
                }
                RNeurons[i].Activation();
            }
            SetTrafficLights();
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
            for (var i = 0; i < NofTrafficLights; i++)
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

                /* W0[0, i] += 0.025*( marck - _crossroad.LeftToUpTrafficData.TrafficDensity);
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
               W0[11, i] +=0.025 * (marck - _crossroad.UpToRightTrafficData.TrafficDensity);*/

                W0[0, i] += (Math.Atan(_crossroad.LeftToUpTrafficData.TrafficDensity - marck) + Math.PI/2.0)*0.1;
                W0[1, i] += (Math.Atan(_crossroad.LeftToRightTrafficData.TrafficDensity - marck) + Math.PI/2.0)*0.1;
                W0[2, i] += (Math.Atan(_crossroad.LeftToDownTrafficData.TrafficDensity - marck) + Math.PI/2.0)*0.1;
                W0[3, i] += (Math.Atan(_crossroad.DownToLeftTrafficData.TrafficDensity - marck) + Math.PI/2.0)*0.1;
                W0[4, i] += (Math.Atan(_crossroad.DownToUpTrafficData.TrafficDensity - marck) + Math.PI/2.0)*0.1;
                W0[5, i] += (Math.Atan(_crossroad.DownToRightTrafficData.TrafficDensity - marck) + Math.PI/2.0)*0.1;
                W0[6, i] += (Math.Atan(_crossroad.RightToDownTrafficData.TrafficDensity - marck) + Math.PI/2.0)*0.1;
                W0[7, i] += (Math.Atan(_crossroad.RightToLeftTrafficData.TrafficDensity - marck) + Math.PI/2.0)*0.1;
                W0[8, i] += (Math.Atan(_crossroad.RightToUpTrafficData.TrafficDensity - marck) + Math.PI/2.0)*0.1;
                W0[9, i] += (Math.Atan(_crossroad.UpToLeftTrafficData.TrafficDensity - marck) + Math.PI/2.0)*0.1;
                W0[10, i] += (Math.Atan(_crossroad.UpToDownTrafficData.TrafficDensity - marck) + Math.PI/2.0)*0.1;
                W0[11, i] += (Math.Atan(_crossroad.UpToRightTrafficData.TrafficDensity - marck) + Math.PI/2.0)*0.1;
            }

            W_writer(fileWtek);
        }

        public void Reinforce(double v0, double v1, double v2, double v3, double v4, double v5, double v6, double v7,
            double v8,
            double v9, double v10, double v11)
        {
            /*for (int i = 0; i < NofTrafficLights; i++)
            {
                W0[0, i] += (Math.Atan(_crossroad.LeftToUpTrafficData.TrafficDensity - marck)+Math.PI/2.0)*0.1;
                W0[1, i] += (Math.Atan(_crossroad.LeftToRightTrafficData.TrafficDensity - marck) + Math.PI / 2.0) * 0.1;
                W0[2, i] += (Math.Atan(_crossroad.LeftToDownTrafficData.TrafficDensity - marck) + Math.PI / 2.0) * 0.1;
                W0[3, i] += (Math.Atan(_crossroad.DownToLeftTrafficData.TrafficDensity - marck) + Math.PI / 2.0) * 0.1;
                W0[4, i] += (Math.Atan(_crossroad.DownToUpTrafficData.TrafficDensity - marck) + Math.PI / 2.0) * 0.1;
                W0[5, i] += (Math.Atan(_crossroad.DownToRightTrafficData.TrafficDensity - marck) + Math.PI / 2.0) * 0.1;
                W0[6, i] += (Math.Atan(_crossroad.RightToDownTrafficData.TrafficDensity - marck) + Math.PI / 2.0) * 0.1;
                W0[7, i] += (Math.Atan(_crossroad.RightToLeftTrafficData.TrafficDensity - marck) + Math.PI / 2.0) * 0.1;
                W0[8, i] += (Math.Atan(_crossroad.RightToUpTrafficData.TrafficDensity - marck) + Math.PI / 2.0) * 0.1;
                W0[9, i] += (Math.Atan(_crossroad.UpToLeftTrafficData.TrafficDensity - marck) + Math.PI / 2.0) * 0.1;
                W0[10, i] += (Math.Atan(_crossroad.UpToDownTrafficData.TrafficDensity - marck) + Math.PI / 2.0) * 0.1;
                W0[11, i] += (Math.Atan(_crossroad.UpToRightTrafficData.TrafficDensity - marck) + Math.PI / 2.0) * 0.1;

            }
            
            W_writer(fileWtek);*/

            throw new NotImplementedException();
        }

        public void Reinforce(double[] values)
        {
            W0[0, 0] += values[0] + values[0];
            W0[0, 1] += values[0]+ values[1];
            W0[0, 2] += values[0] + values[2];
            W0[0, 3] += values[0] + values[9];
            W0[0, 4] += values[0]+ values[10];
            W0[0, 5] += values[0] + values[11];
            W0[0, 6] += values[0] + values[8];
            W0[0, 7] += values[0] + values[7];
            W0[0, 8] +=values[0]+ values[6];
            W0[0, 9] +=values[0]+ values[3];
            W0[0, 10] +=values[0] + values[4];
            W0[0, 11] +=values[0] + values[5];

            W0[1, 0] += values[1] + values[0];
            W0[1, 1] += values[1]+ values[1];
            W0[1, 2] += values[1] + values[2];
            W0[1, 3] += values[1] + values[9];
            W0[1, 4] +=values[1] + values[10];
            W0[1, 5] +=values[1] + values[11];
            W0[1, 6] +=values[1] + values[8];
            W0[1, 7] += values[1] + values[7];
            W0[1, 8] +=values[1] + values[6];
            W0[1, 9] +=values[1] + values[3];
            W0[1, 10] +=values[1] + values[4];
            W0[1, 11] +=values[1] + values[5];

            W0[2, 0] += values[2] + values[0];
            W0[2, 1] +=values[2] + values[1];
            W0[2, 2] += values[2] + values[2];
            W0[2, 3] +=values[2] + values[9];
            W0[2, 4] +=values[2] + values[10];
            W0[2, 5] +=values[2] + values[11];
            W0[2, 6] +=values[2] + values[8];
            W0[2, 7] +=values[2] + values[7];
            W0[2, 8] +=values[2]+ values[6];
            W0[2, 9] +=values[2] + values[3];
            W0[2, 10] +=values[2] + values[4];
            W0[2, 11] +=values[2]+ values[5];

            W0[3, 0] +=values[9]+ values[0];
            W0[3, 1] +=values[9]+ values[1];
            W0[3, 2] +=values[9] + values[2];
            W0[3, 3] +=values[9] + values[9];
            W0[3, 4] += values[9] + values[10];
            W0[3, 5] +=values[9]+ values[11];
            W0[3, 6] +=values[9] + values[8];
            W0[3, 7] +=values[9] + values[7];
            W0[3, 8] +=values[9] + values[6];
            W0[3, 9] +=values[9] + values[3];
            W0[3, 10] +=values[9] + values[4];
            W0[3, 11] += values[9] + values[5];

            W0[4, 0] += values[10]+ values[0];
            W0[4, 1] +=values[10]+ values[1];
            W0[4, 2] +=values[10]+ values[2];
            W0[4, 3] +=values[10]+ values[9];
            W0[4, 4] +=values[10] + values[10];
            W0[4, 5] +=values[10]+ values[11];
            W0[4, 6] +=values[10] + values[8];
            W0[4, 7] +=values[10] + values[7];
            W0[4, 8] +=values[10]+ values[6];
            W0[4, 9] +=values[10] + values[3];
            W0[4, 10] +=values[10]+ values[4];
            W0[4, 11] +=values[10]+ values[5];

            W0[5, 0] +=values[11] + values[0];
            W0[5, 1] +=values[11] + values[1];
            W0[5, 2] +=values[11] + values[2];
            W0[5, 3] +=values[11] + values[9];
            W0[5, 4] +=values[11] + values[10];
            W0[5, 5] +=values[11] + values[11];
            W0[5, 6] +=values[11] + values[8];
            W0[5, 7] += values[11] + values[7];
            W0[5, 8] += values[11] + values[6];
            W0[5, 9] +=values[11]+ values[3];
            W0[5, 10] +=values[11]+ values[4];
            W0[5, 11] +=values[11] + values[5];

            W0[6, 0] +=values[8] + values[0];
            W0[6, 1] +=values[8] + values[1];
            W0[6, 2] +=values[8] + values[2];
            W0[6, 3] +=values[8]+ values[9];
            W0[6, 4] +=values[8] + values[10];
            W0[6, 5] +=values[8]+ values[11];
            W0[6, 6] += values[8] + values[8];
            W0[6, 7] +=values[8] + values[7];
            W0[6, 8] +=values[8] + values[6];
            W0[6, 9] +=values[8]+ values[3];
            W0[6, 10] +=values[8] + values[4];
            W0[6, 11] += values[8] + values[5];

            W0[7, 0] +=values[7] + values[0];
            W0[7, 1] +=values[7] + values[1];
            W0[7, 2] +=values[7] + values[2];
            W0[7, 3] +=values[7] + values[9];
            W0[7, 4] +=values[7] + values[10];
            W0[7, 5] += values[7] + values[11];
            W0[7, 6] +=values[7] + values[8];
            W0[7, 7] +=values[7]+ values[7];
            W0[7, 8] += values[7]+ values[6];
            W0[7, 9] += values[7]+ values[3];
            W0[7, 10] +=values[7] + values[4];
            W0[7, 11] +=values[7]+ values[5];

            W0[8, 0] +=values[6]+ values[0];
            W0[8, 1] +=values[6] + values[1];
            W0[8, 2] +=values[6] + values[2];
            W0[8, 3] +=values[6] + values[9];
            W0[8, 4] +=values[6] + values[10];
            W0[8, 5] +=values[6]+ values[11];
            W0[8, 6] +=values[6]+ values[8];
            W0[8, 7] +=values[6]+ values[7];
            W0[8, 8] += values[6] + values[6];
            W0[8, 9] +=values[6] + values[3];
            W0[8, 10] +=values[6] + values[4];
            W0[8, 11] +=values[6] + values[5];

            W0[9, 0] +=values[3]+ values[0];
            W0[9, 1] +=values[3] + values[1];
            W0[9, 2] += values[3]+ values[2];
            W0[9, 3] +=values[3]+ values[9];
            W0[9, 4] +=values[3]+ values[10];
            W0[9, 5] +=values[3]+ values[11];
            W0[9, 6] +=values[3] + values[8];
            W0[9, 7] +=values[3]+ values[7];
            W0[9, 8] +=values[3]+ values[6];
            W0[9, 9] +=values[3]+ values[3];
            W0[9, 10] +=values[3]+ values[4];
            W0[9, 11] +=values[3]+ values[5];

            W0[10, 0] += values[4] + values[0];
            W0[10, 1] +=values[4]+ values[1];
            W0[10, 2] +=values[4] + values[2];
            W0[10, 3] +=values[4]+ values[9];
            W0[10, 4] +=values[4]+ values[10];
            W0[10, 5] +=values[4]+ values[11];
            W0[10, 6] +=values[4] + values[8];
            W0[10, 7] +=values[4] + values[7];
            W0[10, 8] +=values[4] + values[6];
            W0[10, 9] +=values[4]+ values[3];
            W0[10, 10] += values[4] + values[4];
            W0[10, 11] +=values[4]+ values[5];

            W0[11, 0] += values[5] + values[0];
            W0[11, 1] += values[5] + values[1];
            W0[11, 2] += values[5] + values[2];
            W0[11, 3] += values[5]+ values[9];
            W0[11, 4] += values[5]+ values[10];
            W0[11, 5] +=values[5] + values[11];
            W0[11, 6] += values[5] + values[8];
            W0[11, 7] +=values[5] + values[7];
            W0[11, 8] +=values[5] + values[6];
            W0[11, 9] += values[5] + values[3];
            W0[11, 10] +=values[5] + values[4];
            W0[11, 11] += values[5] + values[5];

            /*double max = Double.MinValue;
            for (int i = 0; i < NofTrafficLights; i++)
            {
                for (int j = 0; j < NofTrafficLights; j++)
                {
                    if (W0[i, j] > max) max = W0[i, j];
                }
            }

            for (int i = 0; i < NofTrafficLights; i++)
            {
                for (int j = 0; j < NofTrafficLights; j++)
                {
                    W0[i, j] /= max;
                }
            }*/

            /*W0[0, 0] += (Math.Atan(values[0]) + Math.PI / 2.0) * 0.1;
            W0[0, 1] += (Math.Atan(values[1]) + Math.PI / 2.0) * 0.1;
            W0[0, 2] += (Math.Atan(values[2]) + Math.PI / 2.0) * 0.1;
            W0[0, 3] += (Math.Atan(values[9]) + Math.PI / 2.0) * 0.1;
            W0[0, 4] += (Math.Atan(values[10]) + Math.PI / 2.0) * 0.1;
            W0[0, 5] += (Math.Atan(values[11]) + Math.PI / 2.0) * 0.1;
            W0[0, 6] += (Math.Atan(values[8]) + Math.PI / 2.0) * 0.1;
            W0[0, 7] += (Math.Atan(values[7]) + Math.PI / 2.0) * 0.1;
            W0[0, 8] += (Math.Atan(values[6]) + Math.PI / 2.0) * 0.1;
            W0[0, 9] += (Math.Atan(values[3]) + Math.PI / 2.0) * 0.1;
            W0[0, 10] += (Math.Atan(values[4]) + Math.PI / 2.0) * 0.1;
            W0[0, 11] += (Math.Atan(values[5]) + Math.PI / 2.0) * 0.1;

            W0[1, 0] += (Math.Atan(values[0]) + Math.PI / 2.0) * 0.1;
            W0[1, 1] += (Math.Atan(values[1]) + Math.PI / 2.0) * 0.1;
            W0[1, 2] += (Math.Atan(values[2]) + Math.PI / 2.0) * 0.1;
            W0[1, 3] += (Math.Atan(values[9]) + Math.PI / 2.0) * 0.1;
            W0[1, 4] += (Math.Atan(values[10]) + Math.PI / 2.0) * 0.1;
            W0[1, 5] += (Math.Atan(values[11]) + Math.PI / 2.0) * 0.1;
            W0[1, 6] += (Math.Atan(values[8]) + Math.PI / 2.0) * 0.1;
            W0[1, 7] += (Math.Atan(values[7]) + Math.PI / 2.0) * 0.1;
            W0[1, 8] += (Math.Atan(values[6]) + Math.PI / 2.0) * 0.1;
            W0[1, 9] += (Math.Atan(values[3]) + Math.PI / 2.0) * 0.1;
            W0[1, 10] += (Math.Atan(values[4]) + Math.PI / 2.0) * 0.1;
            W0[1, 11] += (Math.Atan(values[5]) + Math.PI / 2.0) * 0.1;

            W0[2, 0] += (Math.Atan(values[0]) + Math.PI / 2.0) * 0.1;
            W0[2, 1] += (Math.Atan(values[1]) + Math.PI / 2.0) * 0.1;
            W0[2, 2] += (Math.Atan(values[2]) + Math.PI / 2.0) * 0.1;
            W0[2, 3] += (Math.Atan(values[9]) + Math.PI / 2.0) * 0.1;
            W0[2, 4] += (Math.Atan(values[10]) + Math.PI / 2.0) * 0.1;
            W0[2, 5] += (Math.Atan(values[11]) + Math.PI / 2.0) * 0.1;
            W0[2, 6] += (Math.Atan(values[8]) + Math.PI / 2.0) * 0.1;
            W0[2, 7] += (Math.Atan(values[7]) + Math.PI / 2.0) * 0.1;
            W0[2, 8] += (Math.Atan(values[6]) + Math.PI / 2.0) * 0.1;
            W0[2, 9] += (Math.Atan(values[3]) + Math.PI / 2.0) * 0.1;
            W0[2, 10] += (Math.Atan(values[4]) + Math.PI / 2.0) * 0.1;
            W0[2, 11] += (Math.Atan(values[5]) + Math.PI / 2.0) * 0.1;

            W0[3, 0] += (Math.Atan(values[0]) + Math.PI / 2.0) * 0.1;
            W0[3, 1] += (Math.Atan(values[1]) + Math.PI / 2.0) * 0.1;
            W0[3, 2] += (Math.Atan(values[2]) + Math.PI / 2.0) * 0.1;
            W0[3, 3] += (Math.Atan(values[9]) + Math.PI / 2.0) * 0.1;
            W0[3, 4] += (Math.Atan(values[10]) + Math.PI / 2.0) * 0.1;
            W0[3, 5] += (Math.Atan(values[11]) + Math.PI / 2.0) * 0.1;
            W0[3, 6] += (Math.Atan(values[8]) + Math.PI / 2.0) * 0.1;
            W0[3, 7] += (Math.Atan(values[7]) + Math.PI / 2.0) * 0.1;
            W0[3, 8] += (Math.Atan(values[6]) + Math.PI / 2.0) * 0.1;
            W0[3, 9] += (Math.Atan(values[3]) + Math.PI / 2.0) * 0.1;
            W0[3, 10] += (Math.Atan(values[4]) + Math.PI / 2.0) * 0.1;
            W0[3, 11] += (Math.Atan(values[5]) + Math.PI / 2.0) * 0.1;

            W0[4, 0] += (Math.Atan(values[0]) + Math.PI / 2.0) * 0.1;
            W0[4, 1] += (Math.Atan(values[1]) + Math.PI / 2.0) * 0.1;
            W0[4, 2] += (Math.Atan(values[2]) + Math.PI / 2.0) * 0.1;
            W0[4, 3] += (Math.Atan(values[9]) + Math.PI / 2.0) * 0.1;
            W0[4, 4] += (Math.Atan(values[10]) + Math.PI / 2.0) * 0.1;
            W0[4, 5] += (Math.Atan(values[11]) + Math.PI / 2.0) * 0.1;
            W0[4, 6] += (Math.Atan(values[8]) + Math.PI / 2.0) * 0.1;
            W0[4, 7] += (Math.Atan(values[7]) + Math.PI / 2.0) * 0.1;
            W0[4, 8] += (Math.Atan(values[6]) + Math.PI / 2.0) * 0.1;
            W0[4, 9] += (Math.Atan(values[3]) + Math.PI / 2.0) * 0.1;
            W0[4, 10] += (Math.Atan(values[4]) + Math.PI / 2.0) * 0.1;
            W0[4, 11] += (Math.Atan(values[5]) + Math.PI / 2.0) * 0.1;

            W0[5, 0] += (Math.Atan(values[0]) + Math.PI / 2.0) * 0.1;
            W0[5, 1] += (Math.Atan(values[1]) + Math.PI / 2.0) * 0.1;
            W0[5, 2] += (Math.Atan(values[2]) + Math.PI / 2.0) * 0.1;
            W0[5, 3] += (Math.Atan(values[9]) + Math.PI / 2.0) * 0.1;
            W0[5, 4] += (Math.Atan(values[10]) + Math.PI / 2.0) * 0.1;
            W0[5, 5] += (Math.Atan(values[11]) + Math.PI / 2.0) * 0.1;
            W0[5, 6] += (Math.Atan(values[8]) + Math.PI / 2.0) * 0.1;
            W0[5, 7] += (Math.Atan(values[7]) + Math.PI / 2.0) * 0.1;
            W0[5, 8] += (Math.Atan(values[6]) + Math.PI / 2.0) * 0.1;
            W0[5, 9] += (Math.Atan(values[3]) + Math.PI / 2.0) * 0.1;
            W0[5, 10] += (Math.Atan(values[4]) + Math.PI / 2.0) * 0.1;
            W0[5, 11] += (Math.Atan(values[5]) + Math.PI / 2.0) * 0.1;

            W0[6, 0] += (Math.Atan(values[0]) + Math.PI / 2.0) * 0.1;
            W0[6, 1] += (Math.Atan(values[1]) + Math.PI / 2.0) * 0.1;
            W0[6, 2] += (Math.Atan(values[2]) + Math.PI / 2.0) * 0.1;
            W0[6, 3] += (Math.Atan(values[9]) + Math.PI / 2.0) * 0.1;
            W0[6, 4] += (Math.Atan(values[10]) + Math.PI / 2.0) * 0.1;
            W0[6, 5] += (Math.Atan(values[11]) + Math.PI / 2.0) * 0.1;
            W0[6, 6] += (Math.Atan(values[8]) + Math.PI / 2.0) * 0.1;
            W0[6, 7] += (Math.Atan(values[7]) + Math.PI / 2.0) * 0.1;
            W0[6, 8] += (Math.Atan(values[6]) + Math.PI / 2.0) * 0.1;
            W0[6, 9] += (Math.Atan(values[3]) + Math.PI / 2.0) * 0.1;
            W0[6, 10] += (Math.Atan(values[4]) + Math.PI / 2.0) * 0.1;
            W0[6, 11] += (Math.Atan(values[5]) + Math.PI / 2.0) * 0.1;

            W0[7, 0] += (Math.Atan(values[0]) + Math.PI / 2.0) * 0.1;
            W0[7, 1] += (Math.Atan(values[1]) + Math.PI / 2.0) * 0.1;
            W0[7, 2] += (Math.Atan(values[2]) + Math.PI / 2.0) * 0.1;
            W0[7, 3] += (Math.Atan(values[9]) + Math.PI / 2.0) * 0.1;
            W0[7, 4] += (Math.Atan(values[10]) + Math.PI / 2.0) * 0.1;
            W0[7, 5] += (Math.Atan(values[11]) + Math.PI / 2.0) * 0.1;
            W0[7, 6] += (Math.Atan(values[8]) + Math.PI / 2.0) * 0.1;
            W0[7, 7] += (Math.Atan(values[7]) + Math.PI / 2.0) * 0.1;
            W0[7, 8] += (Math.Atan(values[6]) + Math.PI / 2.0) * 0.1;
            W0[7, 9] += (Math.Atan(values[3]) + Math.PI / 2.0) * 0.1;
            W0[7, 10] += (Math.Atan(values[4]) + Math.PI / 2.0) * 0.1;
            W0[7, 11] += (Math.Atan(values[5]) + Math.PI / 2.0) * 0.1;

            W0[8, 0] += (Math.Atan(values[0]) + Math.PI / 2.0) * 0.1;
            W0[8, 1] += (Math.Atan(values[1]) + Math.PI / 2.0) * 0.1;
            W0[8, 2] += (Math.Atan(values[2]) + Math.PI / 2.0) * 0.1;
            W0[8, 3] += (Math.Atan(values[9]) + Math.PI / 2.0) * 0.1;
            W0[8, 4] += (Math.Atan(values[10]) + Math.PI / 2.0) * 0.1;
            W0[8, 5] += (Math.Atan(values[11]) + Math.PI / 2.0) * 0.1;
            W0[8, 6] += (Math.Atan(values[8]) + Math.PI / 2.0) * 0.1;
            W0[8, 7] += (Math.Atan(values[7]) + Math.PI / 2.0) * 0.1;
            W0[8, 8] += (Math.Atan(values[6]) + Math.PI / 2.0) * 0.1;
            W0[8, 9] += (Math.Atan(values[3]) + Math.PI / 2.0) * 0.1;
            W0[8, 10] += (Math.Atan(values[4]) + Math.PI / 2.0) * 0.1;
            W0[8, 11] += (Math.Atan(values[5]) + Math.PI / 2.0) * 0.1;

            W0[9, 0] += (Math.Atan(values[0]) + Math.PI / 2.0) * 0.1;
            W0[9, 1] += (Math.Atan(values[1]) + Math.PI / 2.0) * 0.1;
            W0[9, 2] += (Math.Atan(values[2]) + Math.PI / 2.0) * 0.1;
            W0[9, 3] += (Math.Atan(values[9]) + Math.PI / 2.0) * 0.1;
            W0[9, 4] += (Math.Atan(values[10]) + Math.PI / 2.0) * 0.1;
            W0[9, 5] += (Math.Atan(values[11]) + Math.PI / 2.0) * 0.1;
            W0[9, 6] += (Math.Atan(values[8]) + Math.PI / 2.0) * 0.1;
            W0[9, 7] += (Math.Atan(values[7]) + Math.PI / 2.0) * 0.1;
            W0[9, 8] += (Math.Atan(values[6]) + Math.PI / 2.0) * 0.1;
            W0[9, 9] += (Math.Atan(values[3]) + Math.PI / 2.0) * 0.1;
            W0[9, 10] += (Math.Atan(values[4]) + Math.PI / 2.0) * 0.1;
            W0[9, 11] += (Math.Atan(values[5]) + Math.PI / 2.0) * 0.1;

            W0[10, 0] += (Math.Atan(values[0]) + Math.PI / 2.0) * 0.1;
            W0[10, 1] += (Math.Atan(values[1]) + Math.PI / 2.0) * 0.1;
            W0[10, 2] += (Math.Atan(values[2]) + Math.PI / 2.0) * 0.1;
            W0[10, 3] += (Math.Atan(values[9]) + Math.PI / 2.0) * 0.1;
            W0[10, 4] += (Math.Atan(values[10]) + Math.PI / 2.0) * 0.1;
            W0[10, 5] += (Math.Atan(values[11]) + Math.PI / 2.0) * 0.1;
            W0[10, 6] += (Math.Atan(values[8]) + Math.PI / 2.0) * 0.1;
            W0[10, 7] += (Math.Atan(values[7]) + Math.PI / 2.0) * 0.1;
            W0[10, 8] += (Math.Atan(values[6]) + Math.PI / 2.0) * 0.1;
            W0[10, 9] += (Math.Atan(values[3]) + Math.PI / 2.0) * 0.1;
            W0[10, 10] += (Math.Atan(values[4]) + Math.PI / 2.0) * 0.1;
            W0[10, 11] += (Math.Atan(values[5]) + Math.PI / 2.0) * 0.1;

            W0[11, 0] += (Math.Atan(values[0]) + Math.PI / 2.0) * 0.1;
            W0[11, 1] += (Math.Atan(values[1]) + Math.PI / 2.0) * 0.1;
            W0[11, 2] += (Math.Atan(values[2]) + Math.PI / 2.0) * 0.1;
            W0[11, 3] += (Math.Atan(values[9]) + Math.PI / 2.0) * 0.1;
            W0[11, 4] += (Math.Atan(values[10]) + Math.PI / 2.0) * 0.1;
            W0[11, 5] += (Math.Atan(values[11]) + Math.PI / 2.0) * 0.1;
            W0[11, 6] += (Math.Atan(values[8]) + Math.PI / 2.0) * 0.1;
            W0[11, 7] += (Math.Atan(values[7]) + Math.PI / 2.0) * 0.1;
            W0[11, 8] += (Math.Atan(values[6]) + Math.PI / 2.0) * 0.1;
            W0[11, 9] += (Math.Atan(values[3]) + Math.PI / 2.0) * 0.1;
            W0[11, 10] += (Math.Atan(values[4]) + Math.PI / 2.0) * 0.1;
            W0[11, 11] += (Math.Atan(values[5]) + Math.PI / 2.0) * 0.1;*/

            W_writer(fileWtek);
        }

        private void W_writer(string fileWtek)
        {
            var sw = new StreamWriter(fileWtek);
            for (var i = 0; i < NofTrafficLights; i++)
            {
                for (var j = 0; j < NofTrafficLights; j++)
                {
                    sw.Write("{0} ", W0[i, j].ToString(CultureInfo.InvariantCulture));
                }
                sw.WriteLine();
            }
            for (var i = 0; i < NofTrafficLights; i++)
            {
                for (var j = 0; j < NofStates; j++)
                {
                    sw.Write("{0} ", W1[i, j].ToString(CultureInfo.InvariantCulture));
                }
                sw.WriteLine();
            }
            sw.Close();
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
                var sr = File.OpenText(f_name);
                for (var j = 0; j < NofTrafficLights; j++)
                {
                    var s = "";
                    s = sr.ReadLine();
                    var v = s.Split(' ');
                    for (var i = 0; i < NofTrafficLights; i++)
                    {
                        l = double.Parse(v[i], CultureInfo.InvariantCulture);
                        W0[j, i] = l;
                    }
                }
                for (var i = 0; i < NofTrafficLights; i++)
                {
                    var s = "";
                    s = sr.ReadLine();
                    var v = s.Split(' ');
                    for (var j = 0; j < NofStates; j++)
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
                        RNeurons[i].dendrits[j] = input[j]*W1[j, i];
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

            return passRate/lines.Length;
        }

        private void EducationWithTeacher(string pathToEducationExample)
        {
            var lines = File.ReadAllLines(pathToEducationExample);

            var bestPassRate = ComputePassRate(lines);
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
                            W1[j, i] = W1[j, i] + (output[i] - RNeurons[i].axon)*input[j];
                            //    crossroadController.W1[j, i] = crossroadController.W1[j, i] + 0.025*e*input[j];
                        }

                        var newPassRate = ComputePassRate(lines);
                        if (newPassRate > bestPassRate)
                        {
                            bestPassRate = newPassRate;
                            CopyMatrix(W1, bestMatrix, NofTrafficLights, NofStates);
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
            for (var i = 0; i < NofStates; i++)
            {
                x += RNeurons[i].atan;
            }
            var T = x/NofStates;
            for (var i = 0; i < NofStates; i++)
            {
                if (RNeurons[i].atan < T)
                {
                    RNeurons[i].axonState = TrafficLightState.Red;
                }
                else
                {
                    RNeurons[i].axonState = TrafficLightState.Green;
                    break;
                }
            }

            var sr = File.OpenText(fileState);
            for (var i = 0; i < NofStates; i++)
            {
                var s = "";
                s = sr.ReadLine();
                var v = s.Split(' ');
                var mas = new TrafficLightState[NofTrafficLights];
                if (RNeurons[i].axonState == TrafficLightState.Green &&
                    (double.Parse(v[i], CultureInfo.InvariantCulture) - 1) < 0.0001)
                {
                    for (var j = 0; j < NofTrafficLights; j++)
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