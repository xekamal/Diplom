using System;
using NUnit.Framework;
using Simulator.Map;
using Simulator.Map.Infrastructure;

namespace Simulator.Neuro.Infrastructure.Tests
{
    [TestFixture]
    public class CrossroadControllerReinforcementTeacherTests
    {
        private const string PathToEducationExample = "d:\\Kate\\Education_example.txt";

        [Test]
        public void TeachW1Test()
        {
            var random = new Random();
            ICrossroad crossroad = new Crossroad();
            var crossroadController = (CrossroadControllerReinforcement) crossroad.CrossroadController;

            for (var i = 0; i < crossroadController.NofTrafficLights; i++)
            {
                for (var j = 0; j < crossroadController.NofStates; j++)
                {
                    crossroadController.W1[i, j] = random.NextDouble()/50 -0.01 ;
                }
            }

            CrossroadControllerReinforcementTeacher.TeachW1(crossroadController, PathToEducationExample);

            var example1 = new double[] {0.06, 0.04, 0.95, 0.64, 0.18, 0.45, 0.67, 0.13, 0.86, 0.05, 0.53, 0.07};
            for (int i = 0; i < crossroadController.NofStates; i++)
            {
                for (int j = 0; j < crossroadController.NofTrafficLights; j++)
                {
                    crossroadController.RNeurons[i].dendrits[j] = crossroadController.W1[j, i]*example1[j];
                }

                crossroadController.RNeurons[i].Activation();
            }

            Assert.AreEqual(0, crossroadController.RNeurons[0].axon, Double.Epsilon);
            Assert.AreEqual(1, crossroadController.RNeurons[1].axon, Double.Epsilon);
            Assert.AreEqual(0, crossroadController.RNeurons[2].axon, Double.Epsilon);
            Assert.AreEqual(0, crossroadController.RNeurons[3].axon, Double.Epsilon);

            /*var example2 = new double[] { 0.06, 0.04, 0.99, 0.64, 0.18, 0.45, 0.67, 0.13, 0.86, 0.05, 0.53, 0.07 };
            for (int i = 0; i < crossroadController.NofStates; i++)
            {
                for (int j = 0; j < crossroadController.NofTrafficLights; j++)
                {
                    crossroadController.RNeurons[i].dendrits[j] = crossroadController.W1[j, i] * example2[j];
                }

                crossroadController.RNeurons[i].Activation();
            }

            Assert.AreEqual(0, crossroadController.RNeurons[0].axon, Double.Epsilon);
            Assert.AreEqual(1, crossroadController.RNeurons[1].axon, Double.Epsilon);
            Assert.AreEqual(0, crossroadController.RNeurons[2].axon, Double.Epsilon);
            Assert.AreEqual(0, crossroadController.RNeurons[3].axon, Double.Epsilon);*/
        }

        [Test]
        public void W1Manual()
        {
            ICrossroad crossroad = new Crossroad();
            var crossroadController = (CrossroadControllerReinforcement)crossroad.CrossroadController;
            crossroadController.W1[0, 0] = 0.00482;
            crossroadController.W1[0, 1] = 0.00165;
            crossroadController.W1[0, 2] = 0.00048;
            crossroadController.W1[0, 3] = 0.94023;
            crossroadController.W1[1, 0] = 0.00049;
            crossroadController.W1[1, 1] = 0.00838;
            crossroadController.W1[1, 2] = 0.96567;
            crossroadController.W1[1, 3] = 0.00239;
            crossroadController.W1[2, 0] = 0.00117;
            crossroadController.W1[2, 1] = 0.00380;
            crossroadController.W1[2, 2] = 0.00830;
            crossroadController.W1[2, 3] = 0.00028;
            crossroadController.W1[3, 0] = 0.92079;
            crossroadController.W1[3, 1] = 0.00716;
            crossroadController.W1[3, 2] = 0.00044;
            crossroadController.W1[3, 3] = 0.00692;
            crossroadController.W1[4, 0] = 0.00631;
            crossroadController.W1[4, 1] = 0.00867;
            crossroadController.W1[4, 2] = 0.00245;
            crossroadController.W1[4, 3] = 0.94431;
            crossroadController.W1[5, 0] = 0.00042;
            crossroadController.W1[5, 1] = 0.00810;
            crossroadController.W1[5, 2] = 0.00993;
            crossroadController.W1[5, 3] = 0.00703;
            crossroadController.W1[6, 0] = 0.00850;
            crossroadController.W1[6, 1] = 0.95461;
            crossroadController.W1[6, 2] = 0.00176;
            crossroadController.W1[6, 3] = 0.00449;
            crossroadController.W1[7, 0] = 0.97469;
            crossroadController.W1[7, 1] = 0.00070;
            crossroadController.W1[7, 2] = 0.00495;
            crossroadController.W1[7, 3] = 0.00060;
            crossroadController.W1[8, 0] = 0.00176;
            crossroadController.W1[8, 1] = 0.00103;
            crossroadController.W1[8, 2] = 0.00291;
            crossroadController.W1[8, 3] = 0.00296;
            crossroadController.W1[9, 0] = 0.00099;
            crossroadController.W1[9, 1] = 0.00015;
            crossroadController.W1[9, 2] = 0.95052;
            crossroadController.W1[9, 3] = 0.00125;
            crossroadController.W1[10, 0] = 0.00068;
            crossroadController.W1[10, 1] = 0.93327;
            crossroadController.W1[10, 2] = 0.00184;
            crossroadController.W1[10, 3] = 0.00045;
            crossroadController.W1[11, 0] = 0.05932;
            crossroadController.W1[11, 1] = 0.03902;
            crossroadController.W1[11, 2] = 0.00409;
            crossroadController.W1[11, 3] = 0.00727;

            CrossroadControllerReinforcementTeacher.TeachW1(crossroadController, PathToEducationExample);



        }
    }
}