using RandomVariables;
using System;
using System.Collections.Generic;
using NLog;

namespace NetworkSimulator
{
    // Узел - Источник
    public class SourceNode : Node
    {
        private static readonly Logger logger = 
            LogManager.GetCurrentClassLogger();

        // Среднее время отклика
        public List<double> ResponseTimes
        {
            get;
            private set;
        }

        // Получение требования (возврат требования в исчтоник) 
        public override void Receive(Fragment fragment)
        {
            logger.Info("Request returned to source!");
            ResponseTimes.Add(Info.GetCurrentTime() - 
                fragment.TimeGeneration);
        }

        // Отправяление требования из источника по сети 
        public override void Route(Fragment fragment)
        {
            logger.Info("Current time = {0:f4}", 
                Info.GetCurrentTime());
            logger.Info("Send the demand at node");
            Send(fragment, Nodes[0]);
        }

        // Отправление требования от источника к другому узлу
        public override void Send(Fragment fragment, Node node)
        {
            logger.Info("Sending a request from " +
                "the source to another node");
            node.Receive(fragment);
        }

        // Счетчик всех созданных фрагментов
        private long FragmentCounter
        {
            get;
            set;
        }

        // Передача управления источнику
        public override void Activate()
        {
            logger.Info("Create a new demand");
            // Создаем требование
            Fragment f = new Fragment(Info.GetCurrentTime(),
                FragmentCounter, null, 0);
            FragmentCounter++;

            // Время следующего события
            NextEventTime = Info.GetCurrentTime() + 
                ArrivalInterval.NextValue();
            // Отправка требования по сети обслуживания
            Route(f);
        }

        // Случайная величина 
        // (интервалы времени между поступлением требований) 
        protected RandomVariable ArrivalInterval
        {
            get;
            set;
        }
        // Констуктор
        public SourceNode(int ID, Random r, 
            RandomVariable ArrivalInterval,
            Node[] Nodes, InfoNode Info)
        {
            this.ID = ID;
            this.random = r;
            this.ArrivalInterval = ArrivalInterval;
            this.Nodes = Nodes;
            this.Info = Info;

            // Первое поступление происходит в нулевой момент времени
            this.NextEventTime = 0;
            FragmentCounter = 0;
            // Для сбора статистики 
            ResponseTimes = new List<double>();
        }
    }
}
