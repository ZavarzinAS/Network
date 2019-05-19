using System;
using System.Collections.Generic;
using RandomVariables;
using NLog;

namespace NetworkSimulator
{
    // Узел - Интегратор
    public class JoinNode : Node
    {
        private static readonly Logger logger = 
            LogManager.GetCurrentClassLogger();

        // Буффер для хранения фрагментов в дивайдере
        protected List<Fragment> InBuffer { get; set; }
        // Число фрагментов для соединения в один фрагмент
        public int JoinDegree { get; set; }
        // Тип, с которым фрагмент покинет инрегратор
        public int LeaveKind { get; set; }
        // Словарь для хранения. Ключ - ID фрагмента,
        // значение - количество фрагментов, 
        // которые произошли от фрагмента с ID равным ключю 
        public Dictionary<long, int> count;
        // Прием фрагмента в интегратор
        public override void Receive(Fragment fragment)
        {
            logger.Info("Id fragment -- {0}. " +
                "Reception of a fragment in the join-node", 
                fragment.ID);

            if (!count.ContainsKey(fragment.ID))
            {
                count.Add(fragment.ID, 1);
            }
            else
            {
                count[fragment.ID]++;
                if (count[fragment.ID] == JoinDegree)
                {
                    count.Remove(fragment.ID);
                    fragment.Kind = LeaveKind;
                    Route(fragment);   
                }
            }
        } 
        // Отправляет фрагмент по сети
        public override void Route(Fragment fragment)
        {
            logger.Info("Id fragment -- {0}. " +
                "Sends a fragment over the network from " +
                "the join-node",
                fragment.ID);
            fragment.Kind = LeaveKind;
            Send(fragment, Nodes[0]);
        }
        // Отправка фрагмента в заданный узел сети
        public override void Send(Fragment fragment, Node node)
        {
            logger.Info("Id fragment -- {0}. " +
                "Sends a fragment to the specified " +
                "node from the join-node", 
                fragment.ID);
            node.Receive(fragment);
        }
        // Выполняемое действие дивайдера
        public override void Activate()
        {
            logger.Info("Join-node activation");
            // Следующий момент активации
            NextEventTime = double.PositiveInfinity;
        }
        // Конструктор
        public JoinNode(int ID, Random random, Node[] nodes, 
            InfoNode info, int LeaveKind, int JoinDegree)
        {
            this.ID = ID;
            this.random = random;
            Nodes = nodes;
            Info = info;
            this.LeaveKind = LeaveKind;
            this.JoinDegree = JoinDegree;

            NextEventTime = double.PositiveInfinity;
            InBuffer = new List<Fragment>();
            count = new Dictionary<long, int>();
        }
    }
}
