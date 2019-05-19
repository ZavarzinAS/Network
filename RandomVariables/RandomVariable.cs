using System;

namespace RandomVariables
{
    // Абстрактный класс для генерации случайных величин
    public abstract class RandomVariable
    {
        // Генератор случайных чисел
        protected Random Random { get; set; }
        // Для генерации
        public abstract double NextValue();
    }
}
