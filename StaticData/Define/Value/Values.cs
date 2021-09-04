namespace StaticData.Define
{

    public enum EValueType
    {
        Group,
        SubGroup,
        Array,
        String,
        Float,
        Integer,
        Double,

        Reference,
    }

    public class Integer : IValue
    {
        public string Name
        {
            get; set;
        }
        public EValueType Type { get; set; } = EValueType.Integer;

        public Integer()
        {
            Name = "NULL";
        }

        public Integer(string Name)
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

    public class Float : IValue
    {
        public string Name
        {
            get; set;
        }
        public EValueType Type { get; set; } = EValueType.Float;

        public Float()
        {
            Name = "NULL";
        }

        public Float(string Name)
        {
            this.Name = Name;
        }
    }

    public class Double : IValue
    {
        public string Name
        {
            get; set;
        }
        public EValueType Type { get; set; } = EValueType.Double;

        public Double()
        {
            Name = "NULL";
        }

        public Double(string Name)
        {
            this.Name = Name;
        }
    }
}
