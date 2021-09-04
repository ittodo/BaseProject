namespace StaticData.Define
{

    public class Array : IArray
    {
        public string Name { get; set; }
        public EValueType Type { get; private set; } = EValueType.Array;

        public IValue Value { get; } // Group 를 제외한 모든 타입이 올수 있다.

        public IValue[] Childs { get; private set; }

        public Array()
        {
            Name = "NULL";
        }

        public Array(string Name)
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
