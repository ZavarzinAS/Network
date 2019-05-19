using System.Collections.Generic;
using System.Linq;

namespace NetworkSimulator
{
    // Класс очереди
    public class QueueBuffer : Buffer
    {
        // Очередь фрагментов
        private Queue<Fragment> fragments;
        // Конструктор
        public QueueBuffer()
        {
            fragments = new Queue<Fragment>();
        }
        // Проверка очереди на пустоту
        public override bool IsEmpty()
        {
            if (fragments.Count() == 0)
            {
                return true;
            }
            return false;
        }
        // Возвращает значение числа фрагментов в очереди
        public override int Count()
        {
            return fragments.Count();
        }
        // Помещает фрагмент в очередь
        public override void Put(Fragment fragment)
        {
            fragments.Enqueue(fragment);
        }
        // Извлекает фрагмент из очереди
        public override Fragment Take()
        {
            return fragments.Dequeue();
        }
    }
}
