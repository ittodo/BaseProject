namespace StaticData.Define
{

    public enum EValueType
    {
        Group,
        SubGroup,
        Array,
        String,
        F32,
        I32,
        F64,
        Reference,
    }

    public class I32 : IValue
    {
        public string Name
        {
            get; set;
        }
        public EValueType Type { get; set; } = EValueType.I32;

        public I32()
        {
            Name = "NULL";
        }

        public I32(string Name)
        {
            this.Name = Name;
        }
    }

    public class String : IValue
    {
        public string Name
        {
            get; set;
        }
        public EValueType Type { get; set; } = EValueType.String;

        public String()
        {
            Name = "NULL";
        }

        public String(string Name)
        {
            this.Name = Name;
        }
    }

    public class F32 : IValue
    {
        public string Name
        {
            get; set;
        }
        public EValueType Type { get; set; } = EValueType.F32;

        public F32()
        {
            Name = "NULL";
        }

        public F32(string Name)
        {
            this.Name = Name;
        }
    }

    public class F64 : IValue
    {
        public string Name
        {
            get; set;
        }
        public EValueType Type { get; set; } = EValueType.F64;

        public F64()
        {
            Name = "NULL";
        }

        public F64(string Name)
        {
            this.Name = Name;
        }
    }
}
