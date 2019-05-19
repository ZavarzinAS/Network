using System;
using NLog;

namespace NetworkSimulator
{
    // Узел - Дивайдер
    public class ForkNode : Node
    {
        private static readonly Logger logger = 
            LogManager.GetCurrentClassLogger();

        // Массив типов фрагментов
        public int[] Kinds { get; set; }
        // Коструктор
        public ForkNode(int id, Random random, 
            Node[] nodes, InfoNode info, int[] Kinds)
        {
            ID = id;
            this.random = random;
            Nodes = nodes;
            Info = info;
            this.Kinds = Kinds;

            NumberOfArrivedDemands = 0;

            NextEventTime = double.PositiveInfinity;
        }
        // Отправляет фрагмент в указанному узлу
        public override void Send(Fragment fragment, Node node)
        {
            logger.Info("Id fragment -- {0}. " +
                "Sends a fragment from the fork-node " +
                "to the specified node", 
                fragment.ID);
            node.Receive(fragment);
        }
        // Распределяет фрагмент по узлам
        public override void Route(Fragment fragment)
        {
            logger.Info("Id fragment -- {0}. " +
                "Sends a fragment to the specified node " +
                "from the fork-node",
                fragment.ID);
            for (int j = 0; j < Nodes.Length; j++)
            {
                Fragment part = new Fragment(fragment.TimeGeneration,
                    fragment.ID, fragment, Kinds[j]);
                logger.Info(fragment);
                Send(part, Nodes[j]);
            }
        }
        // Получение фрагмента из некого узла
        public override void Receive(Fragment fragment)
        {
            logger.Info("Id fragment -- {0}. " +
                "Receiving a fragment by the fork-node",
                fragment.ID);
            NumberOfArrivedDemands++;
            Route(fragment);
        }
        // Активация дивайдера
        public override void Activate()
        { }
    }
}
