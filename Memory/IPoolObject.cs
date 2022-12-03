namespace Memory
{
    public interface IPoolObject
    {

        bool IsUsed { get; set; }

        void InitInstance();

        void Use();
        void Clear();
    }
}
