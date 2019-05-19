using System;

namespace RandomVariables
{
    // Генерирует значение случайной величины
    // сэкспоненциальным распределением
    public class ExponentialVariable : RandomVariable
    {
        // Параметр для экспоненциального 
        // распределения случайной велечины
        public double Rate { get; set; }
        // Следующее значение случайной величины
        public override double NextValue()
        {
            return -1.0 / Rate * Math.Log(Random.NextDouble());
        }
        // Конструктор
        public ExponentialVariable(Random random, double Rate)
        {
            Random = random;
            this.Rate = Rate;
        }
    }
}
