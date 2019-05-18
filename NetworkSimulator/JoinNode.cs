using System;
using System.Collections.Generic;
using RandomVariables;

namespace NetworkSimulator
{
    public class JoinNode : Node
    {

        //Буффер для хранения фрагментов в дивайдере
        protected List<Fragment> InBuffet { get; set; }

        public int JoinDegree { get; set; }

        public int LeaveKind { get; set; }

        public Dictionary<long, int> count;

        //Прием фрагмента в интегратор
        public override void Receive(Fragment fragment)
        {
            Console.WriteLine("Id fragment -- {0}. " +
                "Reception of a fragment in the integrator", 
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

        //Отправляет фрагмент по сети
        public override void Route(Fragment fragment)
        {
            Console.WriteLine("Id fragment -- {0}. " +
                "Sends a fragment over the network from the integrator",
                fragment.ID);
            fragment.Kind = LeaveKind;
            Send(fragment, Nodes[0]);
        }

        //Отправка фрагмента в заданный узел сети
        public override void Send(Fragment fragment, Node node)
        {
            Console.WriteLine("Id fragment -- {0}. " +
                "Sends a fragment to the specified node from the integrator", 
                fragment.ID);
            node.Receive(fragment);
        }

        //Выполняемое действие дивайдера
        public override void Activate()
        {
            Console.WriteLine("Integrator activation");
            //Следующий момент активации
            NextEventTime = double.PositiveInfinity;
        }

        //Создание и инициализация интегратора
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
            InBuffet = new List<Fragment>();
            count = new Dictionary<long, int>();
        }
    }
}
