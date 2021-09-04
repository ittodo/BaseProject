
namespace StaticData.Define
{
    public class Group : IGroup
    {
        public string Name { get; set; }
        public EValueType Type { get; set; } = EValueType.Group;
        public IValue[] Childs { get; set; }

        public Group()
        {
            Name = "Null";
        }

        public Group(string Name)
        {
            this.Name = Name;
            this.Childs = new IValue[0];
        }

        public void AddChild(IValue child)
        {
            var _array = Childs;
            System.Array.Resize(ref _array, Childs.Length + 1);
            _array[Childs.Length] = child;
            Childs = _array;
        }
        public void RemoveChild(IValue child)
        {

            var _array = Childs;
            for (int i = 0; i < _array.Length; i++)
            {
                if (_array[i] == child)
                {

                    System.Array.Copy(_array, i, _array, i + 1, _array.Length - (i + 1));
                }
            }

            System.Array.Resize(ref _array, Childs.Length - 1);
            Childs = _array;
        }
    }

    public class SubGroup : IGroup, IValue
    {
        public string Name { get; set; }
        public EValueType Type { get; private set; } = EValueType.SubGroup;
        public IValue[] Childs { get; private set; }

        public SubGroup()
        {
            Name = "Null";
        }
        public SubGroup(string Name)
        {
            this.Name = Name;
            this.Childs = new IValue[0];
        }

        public void AddChild(IValue child)
        {
            var _array = Childs;
            System.Array.Resize(ref _array, Childs.Length + 1);
            _array[Childs.Length] = child;
            Childs = _array;
        }

        public void RemoveChild(IValue child)
        {

            var _array = Childs;
            for (int i = 0; i < _array.Length; i++)
            {
                if (_array[i] == child)
                {

                    System.Array.Copy(_array, i, _array, i + 1, _array.Length - (i + 1));
                }
            }

            System.Array.Resize(ref _array, Childs.Length - 1);
            Childs = _array;
        }
    }

}
