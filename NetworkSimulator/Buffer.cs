namespace NetworkSimulator
{
    public abstract class Buffer
    {
        // Помещаем фрагмент в буфер
        public abstract void Put(Fragment fragment);
        // Берет фрагмент из буфера
        public abstract Fragment Take();
        // Число элементов в буфере
        public abstract int Count();
        // Проверка буффера на пустоту
        public abstract bool IsEmpty();
    }
}
