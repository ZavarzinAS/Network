using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RandomVariables;

namespace NetworkSimulator
{
    // Метка требования, находящегося на обслуживании (на приборе)
    public class Label : IComparable<Label>
    {
        // Время ухода фрагмента
        public double TimeLeave
        {
            get;
            set;
        }
        // Идентификатор метки
        public long ID
        {
            get;
            set;
        }

        public Label(double TimeLeave, long ID)
        {
            this.TimeLeave = TimeLeave;
            this.ID = ID;
        }

        public int CompareTo(Label other)
        {
            if (this.TimeLeave > other.TimeLeave)
            {
                return 1;
            }
            if (this.TimeLeave < other.TimeLeave)
            {
                return -1;
            }
            return 0;
        }
    }

    // Базовая система
    public class ServiceNode : Node
    {
        // Класс переход фрагмента из данного узла
        // в узел RouteNode со смней типа фрагмента с ArrivalKind на тип LeaveKind 
        private class RoutingRule
        {
            // Текущий тип фрагмента
            public int ArrivalKind { get; set; }
            // Тип, которым фрагмент покинет узел
            public int LeaveKind { get; set; }
            // Номер узла, в который перейдёт фрагмент
            public int RouteNode { get; set; }

            public RoutingRule(int ArrivalKind, int LeaveKind, int RouteNode)
            {
                this.ArrivalKind = ArrivalKind;
                this.LeaveKind = LeaveKind;
                this.RouteNode = RouteNode; 
            }
        }

        // Правила маршрутизации
        private List<RoutingRule> RoutingRules { get; set; }

        // Добавление правил
        public void AddRule(int ArrivalKind, int LeaveKind, int RouteNode)
        {
            RoutingRules.Add(new RoutingRule(ArrivalKind, LeaveKind, RouteNode)); 
        }
        // Очередь базовой системы
        protected Buffer InBuffer
        {
            get;
            set;
        }
        // Число одинаковых обслуживающих приборов
        public int Kappa
        {
            get;
            private set;
        }
        // Случайная величина - длительность обслуживания фрагмента на приборе
        protected RandomVariable ServiceTime
        {
            get;
            set;
        }

        // Возвращает число фрагментов в базовой системе (очередь + приборы) 
        public int NumberOfFragments()
        {
            return ServercingFragments.Count + InBuffer.Count();
        }


        //Для статистики
        public const int MaxNumber = 10;
        public double[] StateProbabilities
        {
            get;
            set;
        }
        public double[] ArrivalStateProbabilities
        {
            get;
            set;
        }

        // Время предыдущего события
        private double PredEventTime
        {
            get;
            set;
        }

        // Обновляет время активации узла 
        protected void UpdateActionTime()
        {
            if (ServercingFragments.Count > 0)
            {
                NextEventTime = ServercingFragments.Keys.Min().TimeLeave;
            }
            else
            {
                NextEventTime = double.PositiveInfinity;
            }
        }

        // Упорядоченный список отображающий фрагменты находящиеся на приборах  
        // Нулевой элемент списка - фрагмент который следующим покинет систему, 
        // если в списке нет элементов, 
        // значит нет загруженных приборов - время акивациии неизвестно 
        protected SortedDictionary<Label, Fragment> ServercingFragments
        {
            get;
            set;
        }


        // Конструктор - Базовая система
        public ServiceNode(int ID, Random r, RandomVariable ServiceTime,
            Buffer InBuffer, int kappa, Node[] Nodes, InfoNode Info)
        {
            //Копирование параметров
            this.ID = ID;
            random = r;
            this.ServiceTime = ServiceTime;
            this.Nodes = Nodes;
            this.Info = Info;
            this.InBuffer = InBuffer;
            this.Kappa = kappa;

            //Время активизации узла
            this.NextEventTime = Double.PositiveInfinity;

            //Создаем список фаргментов на приборах
            ServercingFragments = new SortedDictionary<Label, Fragment>();
            //Число поступивших фрагментов
            NumberOfArrivedDemands = 0;

            RoutingRules = new List<RoutingRule>();

            //Для статистики 
            this.StateProbabilities = new double[MaxNumber];
            this.ArrivalStateProbabilities = new double[MaxNumber];
            this.PredEventTime = 0;
        }

        // Проверка существования свобдного прибора
        protected bool ExistFreeServer()
        {
            return (Kappa > ServercingFragments.Count());
        }

        // Берет фрагмент из очереди и начинает его обслуживание
        protected void StartService()
        {
            //Берем фрагмент из очереди 
            var new_f = InBuffer.Take();
            //Направляем этот фрагмент на свободный обслуживающий прибор,
            //определив время обслуживания
            new_f.TimeStartService = Info.GetCurrentTime();
            new_f.TimeLeave = new_f.TimeStartService + ServiceTime.NextValue();
            //Увеличиваем число поступивших на прибор требований
            NumberOfArrivedDemands++;
            //Добавление фрагмента на прибор
            ServercingFragments.Add(new Label(new_f.TimeLeave,
                NumberOfArrivedDemands), new_f);

            UpdateActionTime();
        }

        // Процедура получения фрагмента базовой системой
        // Фрагмент ставится в очередь или сразу начинается его обслуживание 
        // Реализация сегмента поступления фрагмента
        public override void Receive(Fragment fragment)
        {
            Console.WriteLine("Current time = {0:f4}", Info.GetCurrentTime()); 
            Console.WriteLine("Demand with id = {0} was received", fragment.ID);
            //Для статистики - Фрагмент застает систему в некотором состоянии
            if (NumberOfFragments() < MaxNumber)
            {
                ArrivalStateProbabilities[NumberOfFragments()]++;

                //Состояние системы изменилось
                //Система находилась в этом состоянии некоторое время
                double delta = Info.GetCurrentTime() - PredEventTime;
                PredEventTime = Info.GetCurrentTime();
                StateProbabilities[NumberOfFragments()] += delta;

            }

            //Увеличение числа поступивших фрагментов
            NumberOfArrivedDemands++;
            //Устанавливаем для фрагмента текущее время
            fragment.TimeArrivale = Info.GetCurrentTime();
            //Плмещаем фрагмент в очередь
            this.InBuffer.Put(fragment);
            //Если существует свободный сервер то можно начать обслуживание
            if (ExistFreeServer())
            {
                StartService();
            }
        }

        // Направляет фрагмент в какой-либо узел согласно 
        // установленным правилам маршрутизации
        public override void Route(Fragment fragment)
        {
            //Для статистики 
            if (NumberOfFragments() < MaxNumber - 1)
            {
                //Состояние системы изменилось
                //Система находилась в этом состоянии некоторое время
                double delta = Info.GetCurrentTime() - PredEventTime;
                PredEventTime = Info.GetCurrentTime();
                StateProbabilities[NumberOfFragments() + 1] += delta;

            }

            foreach (var rule in RoutingRules)
            {
                if (rule.ArrivalKind == fragment.Kind)
                {
                    fragment.Kind = rule.LeaveKind;
                    Send(fragment, Nodes[rule.RouteNode]);
                }
            }
        }

        // Посылает фрагмент в указанный узел
        public override void Send(Fragment fragment, Node node)
        {
            //Посылаем фрагмент
            node.Receive(fragment);
        }


        // Передача управления базовой системе
        public override void Activate()
        {
            //Единственное действие это окончание обслуживания
            var key = ServercingFragments.Keys.Min();
            var value = ServercingFragments[key];
            //Удаляем из списка обслуживаемых фрамгентов
            ServercingFragments.Remove(key);
            //Прибор свободен, пытаемся взять на обслуживание новый фрагмент 
            if (InBuffer.Count() > 0)
            {
                StartService();
            }
            else
            {
                //Обновляем время активации 
                UpdateActionTime();
            }
            //Отправляем обслуженный фрагмент в другие узлы 
            Route(value);
        }
    }
}
