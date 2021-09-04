namespace StaticData.Define
{
    public class Reference : IReference
    {
        public EValueType Type { get; } = EValueType.Reference;

        public string Name { get; set; }

        public IGroup Group { get; private set; }

        public Reference()
        {
            Name = "NULL";
        }

        public Reference(string Name)
        {
            this.Name = Name;
        }
    }
}
