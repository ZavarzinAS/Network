using System.Collections.Generic;
using NLog;

namespace NetworkSimulator
{
    // Узел СеМО. Отвечает за баланчировку
    public class BalancerNode : Node
    {
        private static Logger logger = 
            LogManager.GetCurrentClassLogger();

        // Список базовых систем
        public List<ServiceNode> ServiceNodes { get; set; }
        // Конструктор балансировщика
        public BalancerNode(int id)
        {
            ID = id;
            ServiceNodes = new List<ServiceNode>();
            NextEventTime = double.PositiveInfinity;

            logger.Info("Node was created");
        }
        // Добавляет узел (базовую систему) к балансировщику
        public void AddServiceNode(ServiceNode serviceNode)
        {
            ServiceNodes.Add(serviceNode);
        }
        // Возвращает узел с наименьшей числом требований
        private ServiceNode PassNode()
        {          
            int min = int.MaxValue;
            int i_min = 0;

            for (int i = 0; i < ServiceNodes.Count; i++)
            {
                if (min > ServiceNodes[i].NumberOfFragments())
                {
                    min = ServiceNodes[i].NumberOfFragments();
                    i_min = i;
                }
            }
            return ServiceNodes[i_min];
        }
        // Активация узал
        public override void Activate()
        { }
        // Получение фрагмента 
        public override void Receive(Fragment fragment)
        {
            PassNode().Receive(fragment);
        }
        // Отправка фрагмента
        public override void Route(Fragment fragment)
        {
            Node node = PassNode();
            logger.Info("Route fragment {0} " +
                "to node with id = {}", fragment, node.ID);
            node.Route(fragment);
        }
        // Отправка фрагмента в указанный узел
        public override void Send(Fragment fragment, Node node)
        {
            PassNode().Receive(fragment);
        }
    }
}
