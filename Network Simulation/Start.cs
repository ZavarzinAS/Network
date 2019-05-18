using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NetworkDescriptions;
using NetworkSimulator;

namespace Network_Simulation
{
    public class Start
    {
        public static void StartModel2()
        {

            InfoNode Info = new InfoNode();

            Node S1 = new ServiceNode(1, new Random(), new RandomVariables.ExponentialVariable(new Random(), 3),
                new NetworkSimulator.QueueBuffer(), 1, null, Info);
            Node S2 = new ServiceNode(2, new Random(), new RandomVariables.ExponentialVariable(new Random(), 3),
                new NetworkSimulator.QueueBuffer(), 1, null, Info);
            Node S3 = new ServiceNode(1, new Random(), new RandomVariables.ExponentialVariable(new Random(), 3),
               new NetworkSimulator.QueueBuffer(), 1, null, Info);
            Node S4 = new ServiceNode(2, new Random(), new RandomVariables.ExponentialVariable(new Random(), 3),
                new NetworkSimulator.QueueBuffer(), 1, null, Info);
            Node N0 = new SourceNode(0, new Random(), new RandomVariables.ExponentialVariable(new Random(), 1), null, Info);

            Node B1 = new BalancerNode(1);
            Node B2 = new BalancerNode(2);

            Node[] nodesN0 = new Node[1];
            Node[] nodesS1 = new Node[1];
            Node[] nodesS2 = new Node[1];
            Node[] nodesS3 = new Node[1];
            Node[] nodesS4 = new Node[1];



            (S1 as ServiceNode).AddRule(0, 0, 0);
            (S2 as ServiceNode).AddRule(0, 0, 0);
            (S3 as ServiceNode).AddRule(0, 0, 0);
            (S4 as ServiceNode).AddRule(0, 0, 0);

            (B1 as BalancerNode).AddServiceNode(S1 as ServiceNode);
            (B1 as BalancerNode).AddServiceNode(S2 as ServiceNode);
            (B2 as BalancerNode).AddServiceNode(S3 as ServiceNode);
            (B2 as BalancerNode).AddServiceNode(S4 as ServiceNode);

            Node[] nodesF0 = new Node[2];
            nodesF0[0] = B1;
            nodesF0[1] = B2;

            Node[] nodesJ0 = new Node[1];
            nodesJ0[0] = N0;


            int[] KindsF = new int [2];
            KindsF[0] = 0;
            KindsF[1] = 0;

            Node F = new ForkNode(3, 4, new Random(), nodesF0, Info, KindsF);

            Node J = new JoinNode(4, new Random(), nodesJ0, Info, 0, 2);

            nodesN0[0] = F;
            N0.Nodes = nodesN0;

            nodesS1[0] = J;
            nodesS2[0] = J;
            S1.Nodes = nodesS1;
            S2.Nodes = nodesS2;

            nodesS3[0] = J;
            nodesS4[0] = J;
            S3.Nodes = nodesS3;
            S4.Nodes = nodesS4;

            Node[] nodes = new Node[9];
            nodes[0] = N0;
            nodes[1] = F;
            nodes[2] = S1;
            nodes[3] = S2;
            nodes[4] = J;
            nodes[5] = S3;
            nodes[6] = S4;
            nodes[7] = B1;
            nodes[8] = B2;

            NetworkModel net = new NetworkModel(nodes, Info, new Random());

            net.Run(1000);

            double tau = 0;
            net.Analysis(out tau);
        }
 
        static void Main()
        {
            Console.WriteLine("Start simulation");
            StartModel2();
        }
    }
}
