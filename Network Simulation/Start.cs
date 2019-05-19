using System;
using NetworkDescriptions;
using NetworkSimulator;
using RandomVariables;
using NLog;

namespace Network_Simulation
{
    public class Start
    {
        private static readonly Logger logger =
            LogManager.GetCurrentClassLogger();

        public static void StartModel()
        {
            // Сеть из курсовой
            double lambda = 3;
            double mu = 5;
            // Информационный узел
            InfoNode Info = new InfoNode();
            Random random = new Random();
            // Источник требований
            Node N0 = new SourceNode(0, random,
                new ExponentialVariable(random, lambda), null, Info);
            // Базовые системы
            // S1 - S3 принадлежат балансировщику B1
            Node S1 = new ServiceNode(1, random,
                new ExponentialVariable(random, mu),
                new QueueBuffer(), 1, null, Info);
            Node S2 = new ServiceNode(2, random,
                new ExponentialVariable(random, mu),
                new QueueBuffer(), 1, null, Info);
            Node S3 = new ServiceNode(3, random,
                new ExponentialVariable(random, mu),
                new QueueBuffer(), 1, null, Info);
            // S4 - S6 принадлежат балансировщику B2
            Node S4 = new ServiceNode(4, random,
                new ExponentialVariable(random, mu),
                new QueueBuffer(), 1, null, Info);
            Node S5 = new ServiceNode(5, random,
                new ExponentialVariable(random, mu),
                new QueueBuffer(), 1, null, Info);
            Node S6 = new ServiceNode(6, random,
                new ExponentialVariable(random, mu),
                new QueueBuffer(), 1, null, Info);
            // S7 - S9 принадлежат балансировщику B3
            Node S7 = new ServiceNode(7, random,
                new ExponentialVariable(random, mu),
                new QueueBuffer(), 1, null, Info);
            Node S8 = new ServiceNode(8, random,
                new ExponentialVariable(random, mu),
                new QueueBuffer(), 1, null, Info);
            Node S9 = new ServiceNode(9, random,
                new ExponentialVariable(random, mu),
                new QueueBuffer(), 1, null, Info);
            // S10 - S12 принадлежат балансировщику B4
            Node S10 = new ServiceNode(10, random,
                new ExponentialVariable(random, mu),
                new QueueBuffer(), 1, null, Info);
            Node S11 = new ServiceNode(11, random,
                new ExponentialVariable(random, mu),
                new QueueBuffer(), 1, null, Info);
            Node S12 = new ServiceNode(12, random,
                new ExponentialVariable(random, mu),
                new QueueBuffer(), 1, null, Info);

            // Балансировщики
            Node B1 = new BalancerNode(6);
            Node B2 = new BalancerNode(7);
            Node B3 = new BalancerNode(8);
            Node B4 = new BalancerNode(9);
            // Правила маршрутизации фрагментов
            // для базовых систем
            (S1 as ServiceNode).AddRule(1, 1, 5);
            (S2 as ServiceNode).AddRule(1, 1, 5);
            (S3 as ServiceNode).AddRule(1, 1, 5);
            (S1 as ServiceNode).AddRule(2, 2, 2);
            (S2 as ServiceNode).AddRule(2, 2, 2);
            (S3 as ServiceNode).AddRule(2, 2, 2);

            (S4 as ServiceNode).AddRule(3, 3, 4);
            (S5 as ServiceNode).AddRule(3, 3, 4);
            (S6 as ServiceNode).AddRule(3, 3, 4);

            (S7 as ServiceNode).AddRule(4, 4, 3);
            (S8 as ServiceNode).AddRule(4, 4, 3);
            (S9 as ServiceNode).AddRule(4, 4, 3);
            (S7 as ServiceNode).AddRule(5, 6, 9);
            (S8 as ServiceNode).AddRule(5, 6, 9);
            (S9 as ServiceNode).AddRule(5, 6, 9);

            (S10 as ServiceNode).AddRule(6, 6, 9);
            (S11 as ServiceNode).AddRule(6, 6, 9);
            (S12 as ServiceNode).AddRule(6, 6, 9);
            // Добавляем балансировщикам базовые системы
            (B1 as BalancerNode).AddServiceNode(S1 as ServiceNode);
            (B1 as BalancerNode).AddServiceNode(S2 as ServiceNode);
            (B1 as BalancerNode).AddServiceNode(S3 as ServiceNode);
            (B2 as BalancerNode).AddServiceNode(S4 as ServiceNode);
            (B2 as BalancerNode).AddServiceNode(S5 as ServiceNode);
            (B2 as BalancerNode).AddServiceNode(S6 as ServiceNode);
            (B3 as BalancerNode).AddServiceNode(S7 as ServiceNode);
            (B3 as BalancerNode).AddServiceNode(S8 as ServiceNode);
            (B3 as BalancerNode).AddServiceNode(S9 as ServiceNode);
            (B4 as BalancerNode).AddServiceNode(S10 as ServiceNode);
            (B4 as BalancerNode).AddServiceNode(S11 as ServiceNode);
            (B4 as BalancerNode).AddServiceNode(S12 as ServiceNode);
            // Массивы типов фрагментов для их смены
            int[] KindsF0 = new int[3];
            KindsF0[0] = 1;
            KindsF0[1] = 2;
            KindsF0[2] = 3;
            int[] KindsF1 = new int[2];
            KindsF1[0] = 2;
            KindsF1[1] = 2;

            // Дивайдеры
            Node F0 = new ForkNode(1, random, null, Info, KindsF0);
            Node F1 = new ForkNode(2, random, null, Info, KindsF1);

            // Интеграторы
            Node J0 = new JoinNode(3, random, null, Info, 0, 2);
            Node J1 = new JoinNode(4, random, null, Info, 5, 2);
            Node J2 = new JoinNode(5, random, null, Info, 4, 2);

            // Массывы смежности для каждого узла в сети
            Node[] nodesN0 = new Node[1];
            Node[] nodesF0 = new Node[2];
            Node[] nodesB1 = new Node[2];
            Node[] nodesB2 = new Node[1];
            Node[] nodesF1 = new Node[2];
            Node[] nodesJ1 = new Node[1];
            Node[] nodesJ2 = new Node[1];
            Node[] nodesB3 = new Node[2];
            Node[] nodesB4 = new Node[1];
            Node[] nodesJ0 = new Node[1];

            nodesN0[0] = F0;

            nodesF0[0] = B1;
            nodesF0[1] = B2;

            nodesB1[0] = F1;
            nodesB1[1] = J2;

            nodesB2[0] = J1;

            nodesF1[0] = J1;
            nodesF1[1] = J2;

            nodesJ1[0] = B3;

            nodesJ2[0] = B3;

            nodesB3[0] = B4;
            nodesB3[1] = J0;

            nodesB4[0] = J0;

            nodesJ0[0] = N0;

            // Добавляем каждому узлу смежные с ним узлы
            N0.Nodes = nodesN0;
            F0.Nodes = nodesF0;
            B1.Nodes = nodesB1;
            B2.Nodes = nodesB2;
            B3.Nodes = nodesB3;
            B4.Nodes = nodesB4;
            S1.Nodes = nodesB1;
            S2.Nodes = nodesB1;
            S3.Nodes = nodesB1;
            S4.Nodes = nodesB2;
            S5.Nodes = nodesB2;
            S6.Nodes = nodesB2;
            S7.Nodes = nodesB3;
            S8.Nodes = nodesB3;
            S9.Nodes = nodesB3;
            S10.Nodes = nodesB4;
            S11.Nodes = nodesB4;
            S12.Nodes = nodesB4;
            F1.Nodes = nodesF1;
            J1.Nodes = nodesJ1;
            J2.Nodes = nodesJ2;
            J0.Nodes = nodesJ0;

            // Массив для всех узлов в Семо
            Node[] nodes = new Node[10];
            nodes[0] = N0;
            nodes[1] = F0;
            nodes[2] = B1;
            nodes[3] = B2;
            nodes[4] = B3;
            nodes[5] = B4;
            nodes[6] = F1;
            nodes[7] = J0;
            nodes[8] = J1;
            nodes[9] = J2;
            //nodes[10] = S1;
            //nodes[11] = S2;
            //nodes[12] = S3;
            //nodes[13] = S4;
            //nodes[14] = S5;
            //nodes[15] = S6;
            //nodes[16] = S7;
            //nodes[17] = S8;
            //nodes[18] = S9;
            //nodes[19] = S10;
            //nodes[20] = S11;
            //nodes[21] = S12;


            // Создаем модель СеМО
            NetworkModel model = new NetworkModel(nodes, Info, random);

            // Запускаем моделирование
            model.Run(1000);
            // Анализ сети
            double tau = 0;
            model.Analysis(out tau);

            double rho = lambda / mu;
            //Мат. ожидание
            Console.WriteLine("EXACT RESULT E[tau] = {0:f4}",
                (12 - rho) / (8 * mu * (1 - rho)));
        }

        public static void StartModel2()
        {
            // Информационный узел
            InfoNode Info = new InfoNode();
            // Обслуживающие системы (базовые)
            Node S1 = new ServiceNode(1, new Random(), 
                new ExponentialVariable(new Random(), 3),
                new QueueBuffer(), 1, null, Info);
            Node S2 = new ServiceNode(2, new Random(), 
                new ExponentialVariable(new Random(), 3),
                new QueueBuffer(), 1, null, Info);
            Node S3 = new ServiceNode(1, new Random(), 
                new ExponentialVariable(new Random(), 3),
               new QueueBuffer(), 1, null, Info);
            Node S4 = new ServiceNode(2, new Random(), 
                new ExponentialVariable(new Random(), 3),
                new QueueBuffer(), 1, null, Info);
            // Источник
            Node N0 = new SourceNode(0, new Random(), 
                new ExponentialVariable(new Random(), 1), 
                null, Info);
            // Балансировщики
            Node B1 = new BalancerNode(1);
            Node B2 = new BalancerNode(2);
            // Массивы смежности для узлов
            Node[] nodesN0 = new Node[1];
            Node[] nodesS1 = new Node[1];
            Node[] nodesS2 = new Node[1];
            Node[] nodesS3 = new Node[1];
            Node[] nodesS4 = new Node[1];
            Node[] nodesF0 = new Node[2];
            Node[] nodesJ0 = new Node[1];
            // Добавляем правила маршрутизации
            (S1 as ServiceNode).AddRule(0, 0, 0);
            (S2 as ServiceNode).AddRule(0, 0, 0);
            (S3 as ServiceNode).AddRule(0, 0, 0);
            (S4 as ServiceNode).AddRule(0, 0, 0);
            // Добавляем балансировщикам базовые системы
            (B1 as BalancerNode).AddServiceNode(S1 as ServiceNode);
            (B1 as BalancerNode).AddServiceNode(S2 as ServiceNode);
            (B2 as BalancerNode).AddServiceNode(S3 as ServiceNode);
            (B2 as BalancerNode).AddServiceNode(S4 as ServiceNode);

            // Добавляем дивайдеру связи
            nodesF0[0] = B1;
            nodesF0[1] = B2;
            // Связь интегратора с источником
            nodesJ0[0] = N0;
            // Массив для типов фрагментов для дивайдера
            int[] KindsF = new int[2];
            KindsF[0] = 0;
            KindsF[1] = 0;
            // Дивайдер
            Node F = new ForkNode(3, new Random(), nodesF0, 
                Info, KindsF);
            // Интегратор
            Node J = new JoinNode(4, new Random(), nodesJ0, 
                Info, 0, 2);
            // Связь источника с дивайдером
            nodesN0[0] = F;
            N0.Nodes = nodesN0;
            // Связь базовых систем с интегратором
            nodesS1[0] = J;
            nodesS2[0] = J;
            S1.Nodes = nodesS1;
            S2.Nodes = nodesS2;
            // Связь базовых систем с интегратором
            nodesS3[0] = J;
            nodesS4[0] = J;
            S3.Nodes = nodesS3;
            S4.Nodes = nodesS4;
            // Массив всех узлов в СеМО
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
            // Создание модели СеМО
            NetworkModel net = new NetworkModel(nodes, Info, new Random());
            // Запуск моделирования
            net.Run(1000);
            // Анализ сети
            double tau = 0;
            net.Analysis(out tau);
        }
        
        public static void StartModel3()
        {
            // Классическая конфигурация 
            // с двумя ветками без балансировщика
            double lambda = 3;
            double mu = 5;
            // Информационный узел
            InfoNode Info = new InfoNode();
            Random r = new Random();
            // Базовые системы
            Node S1 = new ServiceNode(2, r,
                new ExponentialVariable(r, mu),
                new QueueBuffer(), 1, null, Info);
            Node S2 = new ServiceNode(3, r,
                new ExponentialVariable(r, mu),
                new QueueBuffer(), 1, null, Info);
            // Источник требований
            Node N0 = new SourceNode(0, r, 
                new ExponentialVariable(r, lambda), null, Info);

            // Массив смежных узлов для узлов N0, S1, S2 
            Node[] nodesN0 = new Node[1];
            Node[] nodesS1 = new Node[1];
            Node[] nodesS2 = new Node[1];

            // Правила маршрутизации
            (S1 as ServiceNode).AddRule(0, 0, 0);
            (S2 as ServiceNode).AddRule(0, 0, 0);
            // Смежные узлы для дивайдера
            Node[] nodesF0 = new Node[2];
            nodesF0[0] = S1;
            nodesF0[1] = S2;
            // Смежные узля для интегратора
            // Требование возвращается в истояник
            Node[] nodesJ0 = new Node[1];
            nodesJ0[0] = N0;

            // Массив для типов фрагментов для дивайдера
            int[] KindsF = new int[2];
            KindsF[0] = 0;
            KindsF[1] = 0;
            // Дивайдер
            Node F = new ForkNode(1, r, nodesF0, Info, KindsF);
            // Интегратор
            Node J = new JoinNode(4, r, nodesJ0, Info, 0, 2);
            // Связь источника с дивайдером
            nodesN0[0] = F;
            N0.Nodes = nodesN0;
            // Связи базовых систем с интегратором
            nodesS1[0] = J;
            nodesS2[0] = J;
            S1.Nodes = nodesS1;
            S2.Nodes = nodesS2;

            // Массив всех узлов в СеМО
            Node[] nodes = new Node[5];
            nodes[0] = N0;
            nodes[1] = F;
            nodes[2] = S1;
            nodes[3] = S2;
            nodes[4] = J;

            // Создание модели СеМО
            NetworkModel net = new NetworkModel(nodes, Info, r);
            // Запуск моделирования
            net.Run(1000);
            // Анализ сети
            double tau = 0;
            net.Analysis(out tau);
            
            double rho = lambda / mu;
            //Мат. ожидание
            Console.WriteLine("EXACT RESULT E[tau] = {0:f4}", 
                (12 - rho) / (8 * mu * (1 - rho)));
        }

        static void Main()
        { 
            logger.Info("Start simulation");
            logger.Trace("Start simulation");
            logger.Debug("Start simulation");
            logger.Error("Start simulation");
            logger.Warn("Start simulation");


            Console.WriteLine("\nPress any key to start: ");
            Console.ReadKey();

            Console.WriteLine("Start simulation");
            StartModel();
        }
    }
}
