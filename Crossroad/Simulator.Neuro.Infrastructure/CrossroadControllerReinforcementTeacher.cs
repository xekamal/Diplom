using System;
using System.Globalization;
using System.IO;

namespace Simulator.Neuro.Infrastructure
{
    public class CrossroadControllerReinforcementTeacher
    {
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

        public static double ComputePassRate(string[] lines, CrossroadControllerReinforcement crossroadController)
        {
            var passRate = 0.0;

            foreach (var line in lines)
            {
                var input = new double[crossroadController.NofTrafficLights];
                var output = new double[crossroadController.NofStates];
                var values = line.Split(' ');

                for (var i = 0; i < crossroadController.NofTrafficLights; i++)
                {
                    input[i] = double.Parse(values[i], CultureInfo.InvariantCulture);
                }

                for (var i = 0; i < crossroadController.NofStates; i++)
                {
                    output[i] = double.Parse(values[crossroadController.NofTrafficLights + i]);
                }

                var isPassed = true;
                for (var i = 0; i < crossroadController.NofStates; i++)
                {
                    for (var j = 0; j < crossroadController.NofTrafficLights; j++)
                    {
                        crossroadController.RNeurons[i].dendrits[j] = input[j]*crossroadController.W1[j, i];
                    }

                    crossroadController.RNeurons[i].Activation();

                    if (Math.Abs(crossroadController.RNeurons[i].axon - output[i]) > 0.1)
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

        public static void TeachW1(CrossroadControllerReinforcement crossroadController, string pathToEducationExample)
        {
            var lines = File.ReadAllLines(pathToEducationExample);

            double bestPassRate = ComputePassRate(lines, crossroadController);
            var bestMatrix = new double[crossroadController.NofTrafficLights, crossroadController.NofStates];
            CopyMatrix(crossroadController.W1, bestMatrix, crossroadController.NofTrafficLights, crossroadController.NofStates);
            
            foreach (var line in lines)
            {
                var input = new double[crossroadController.NofTrafficLights];
                var output = new double[crossroadController.NofStates];
                var values = line.Split(' ');

                for (var i = 0; i < crossroadController.NofTrafficLights; i++)
                {
                    input[i] = double.Parse(values[i], CultureInfo.InvariantCulture);
                }

                for (var i = 0; i < crossroadController.NofStates; i++)
                {
                    output[i] = double.Parse(values[crossroadController.NofTrafficLights + i]);
                }

                var nofRepeats = 0;
                for (var i = 0; i < crossroadController.NofStates; i++)
                {
                    if (nofRepeats >= 100)
                    {
                        nofRepeats = 0;
                        continue;
                    }

                    for (var j = 0; j < crossroadController.NofTrafficLights; j++)
                    {
                        crossroadController.RNeurons[i].dendrits[j] = input[j]*crossroadController.W1[j, i];
                    }

                    crossroadController.RNeurons[i].Activation();

                    if (Math.Abs(crossroadController.RNeurons[i].axon - output[i]) > 0.1)
                    {
                       // double e = (output[i] - crossroadController.RNeurons[i].axon) *
                       //           crossroadController.RNeurons[i].axon * (1 - crossroadController.RNeurons[i].axon);
                        for (var j = 0; j < crossroadController.NofTrafficLights; j++)
                        {

                            //crossroadController.W1[j, i] += 0.01 * (input[j] - crossroadController.W1[j, i]);
                            crossroadController.W1[j, i] = crossroadController.W1[j, i] +
                                                           (output[i] - crossroadController.RNeurons[i].axon)*input[j];
                            //    crossroadController.W1[j, i] = crossroadController.W1[j, i] + 0.025*e*input[j];
                        }

                        double newPassRate = ComputePassRate(lines, crossroadController);
                        if (newPassRate > bestPassRate)
                        {
                            bestPassRate = newPassRate;
                            CopyMatrix(crossroadController.W1, bestMatrix, crossroadController.NofTrafficLights, crossroadController.NofStates);
                        }

                        i--;
                        nofRepeats++;
                    }
                    else
                    {
                        nofRepeats = 0;
                    }

                    /* if (Math.Abs(crossroadController.RNeurons[i].axon - output[i] - (-1)) < double.Epsilon)
                    {
                        double e = (output[i] - crossroadController.RNeurons[i].axon)*
                                   crossroadController.RNeurons[i].axon*(1 - crossroadController.RNeurons[i].axon);

                        for (var j = 0; j < crossroadController.NofTrafficLights; j++)
                        {
                            //crossroadController.W1[j, i] += 0.01*(input[j] - crossroadController.W1[j, i]);
                            crossroadController.W1[j, i] = crossroadController.W1[j, i] + 0.025*e*input[j];
                        }

                        double newPassRate = ComputePassRate(lines, crossroadController);
                        if (newPassRate > bestPassRate)
                        {
                            bestPassRate = newPassRate;
                            CopyMatrix(crossroadController.W1, bestMatrix, crossroadController.NofTrafficLights, crossroadController.NofStates);
                        }

                        i--;
                        nofRepeats++;
                    }
                    else if (Math.Abs(crossroadController.RNeurons[i].axon - output[i] - 1) < double.Epsilon)
                    {
                        double e = (output[i] - crossroadController.RNeurons[i].axon) *
                                   crossroadController.RNeurons[i].axon * (1 - crossroadController.RNeurons[i].axon);

                        for (var j = 0; j < crossroadController.NofTrafficLights; j++)
                        {
                            //crossroadController.W1[j, i] -= 0.01*(input[j] - crossroadController.W1[j, i]);
                            crossroadController.W1[j, i] = crossroadController.W1[j, i] + 0.025 * e * input[j];
                        }

                        double newPassRate = ComputePassRate(lines, crossroadController);
                        if (newPassRate > bestPassRate)
                        {
                            bestPassRate = newPassRate;
                            CopyMatrix(crossroadController.W1, bestMatrix, crossroadController.NofTrafficLights, crossroadController.NofStates);
                        }

                        i--;
                        nofRepeats++;
                    }
                    else
                    {
                        nofRepeats = 0;
                    }*/
                }
            }
        }

    }
}