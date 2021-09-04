namespace StaticData.Define
{
    public interface IGroup : IValue
    {
        IValue[] Childs { get; }

        void AddChild(IValue child);

        void RemoveChild(IValue child);
    }

    public interface IArray : IGroup
    {
        IValue Value { get; }
    }

    public interface IReference : IValue
    {
        IGroup Group { get; }
    }

    public interface IValue
    {
        EValueType Type { get; }

        string Name { get; set; }
    }


}
