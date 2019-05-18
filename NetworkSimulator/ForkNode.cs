using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RandomVariables;

namespace NetworkSimulator
{
    //Дивайдер
    public class ForkNode : Node
    {
        //Идентификатор дивайдера
        public int ForkNodeID { get; protected set; }

        public int[] Kinds { get; set; }

        public ForkNode(int id, int forkNodeID, Random random, 
            Node[] nodes, InfoNode info, int[] Kinds)
        {
            ID = id;
            ForkNodeID = forkNodeID;
            this.random = random;
            Nodes = nodes;
            Info = info;
            this.Kinds = Kinds;

            NumberOfArrivedDemands = 0;

            NextEventTime = double.PositiveInfinity;
        }

        //Отправляет фрагмент в указанному узлу
        public override void Send(Fragment fragment, Node node)
        {
            Console.WriteLine("Id fragment -- {0}. " +
                "Sends a fragment from the devider to the specified node", 
                fragment.ID);
            node.Receive(fragment);
        }

        //Распределяет фрагмент по узлам
        public override void Route(Fragment fragment)
        {
            Console.WriteLine("Id fragment -- {0}. " +
                "Sends a fragment to the specified node from the devider", 
                fragment.ID);
            //Номер фрагмента начиная с единицы
            int partIndex = 1;
            for (int j = 0; j < Nodes.Length; j++)
            {
                Fragment part = new Fragment(fragment.TimeGeneration, 
                    fragment.ID, fragment, Kinds[j]);
                Console.WriteLine(fragment);
                Send(part, Nodes[j]);
                partIndex++;
            }
        }


        //Получение фрагмента из некого узла
        public override void Receive(Fragment fragment)
        {
            Console.WriteLine("Id fragment -- {0}. " +
                "Receiving a fragment by the diver",
                fragment.ID);
            NumberOfArrivedDemands++;
            Route(fragment);
        }

        //Активация дивайдера
        public override void Activate()
        { } 
        
    }
}
