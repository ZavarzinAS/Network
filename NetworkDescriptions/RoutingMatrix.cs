using System.Text;

namespace NetworkDescriptions
{
    // Маршрутная матрица для сети с делениеи слиянием требований
    public class RoutingMatrix
    {
        // Матрица для маршрутизации
        // первый индекс - узел откуда идет передача фрагмента
        // второй индекс - узел куда идет передача фрагмента
        // третий индекс - тип фрагмента
        // четвертый индекс - новый тип фрагмента
        private int[,,,] Theta;

        // Маршрутная матрица (конструктор) 
        public RoutingMatrix(int ij, int kl)
        {
            Theta = new int[ij, ij, kl, kl];
        }
        // Размерность матрицы (количество узлов в сети)
        public int Dimention1 { get { return Theta.GetLength(0); } }
        public int Dimention2 { get { return Theta.GetLength(2); } }
        
        public int this[int i, int j, int k, int l]
        {
            get { return Theta[i, j, k, l]; }
            set { Theta[i, j, k, l] = value; }
        }

        // Заполение матрицы строкой из файла
        public void FillingTheta(string line)
        {
            string[] data = line.Split(';');
            Theta[int.Parse(data[0]), int.Parse(data[1]), 
                int.Parse(data[2]), int.Parse(data[3])] = 1;
        }

        public override string ToString()
        {
            StringBuilder str = new StringBuilder(string.Empty);
            str.AppendLine("Theta: ");

            for (int i = 0; i < Theta.GetLength(0); i++)
            {
                for (int j = 0; j < Theta.GetLength(0); j++)
                {
                    for (int k = 0; k < Theta.GetLength(2); k++)
                    {
                        for (int l = 0; l < Theta.GetLength(2); l++)
                        {
                            if (Theta[i, j, k, l] == 1)
                            {
                                str.AppendLine($"Theta" +
                                    $"[{i}, {j}, {k}, {l}] = 1");
                            }
                        }
                    }
                }
            }
            str.AppendLine();
            return str.ToString();
        }
    }
}
