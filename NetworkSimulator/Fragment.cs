namespace NetworkSimulator
{
    //Фрагмент в сети с делением и слиянием требований
    public class Fragment : Demand
    {
        //Сигнатура фрагмента
        public Fragment ParentFragment { get; set; }

        public int Kind { get; set; }



        public Fragment(double TimeGeneration, long ID, Fragment ParentFragment, int Kind)
        {
            this.TimeGeneration = TimeGeneration;
            this.ID = ID;
            this.Kind = Kind;
            this.ParentFragment = ParentFragment;
            NumberOfParts = 1;
        }

        //Число частей на которое был поделен фрагмент
        public int NumberOfParts { get; set; }
        //Время поступления
        public double TimeArrivale { get; set; }
        //Время начала обслуживания
        public double TimeStartService { get; set; }
        //Время завершения обслуживания
        public double TimeLeave { get; set; }
        //Общее время пребывания в сети
        public double TotalTime { get; set; }

        public override string ToString()
        {
            string s = string.Format("ID = {0}, TimeGen = {1:f4}, Kind = {2}", ID, TimeGeneration, Kind);
            return s;
        }
    }
}
