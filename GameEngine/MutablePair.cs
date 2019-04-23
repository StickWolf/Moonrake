namespace GameEngine
{
    public class MutablePair<T1,T2>
    {
        public T1 Key { get; set; }
        public T2 Value { get; set; }
        public MutablePair(T1 key, T2 value)
        {
            Key = key;
            Value = value;
        }
    }
}
