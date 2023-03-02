using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YamlDotNet.Serialization;

namespace Fe.Data
{
    public interface IData
    {
        public string Name { get; }

        public string FullPath { get; set; }

    }

    // 정의 타입
    public class SpaceData : IData
    {
        public const string KeyType = "Space";

        [YamlMember(Alias = "Space", ApplyNamingConventions = false)]
        public string Name { get; set; } = string.Empty;

        public string FullPath { get; set; } = string.Empty;
    }


    public class TableData : IData
    {
        public const string KeyType = "Table";

        [YamlMember(Alias = "Table", ApplyNamingConventions = false)]
        public string Name { get; set; } = string.Empty;

        public string FullPath { get; set; } = string.Empty;

        public List<IData> Value { get; set; }
    }

    public class SubData : IData
    {
        public const string KeyType = "Sub";

        [YamlMember(Alias = "Sub", ApplyNamingConventions = false)]
        public string Name { get; set; } = string.Empty;

        public string FullPath { get; set; } = string.Empty;

        public List<IData> Value { get; set; }
    }

    public class EnumData : IData
    {
        public const string KeyType = "Enum";

        [YamlMember(Alias = "Enum", ApplyNamingConventions = false)]
        public string Name { get; set; } = string.Empty;

        public string FullPath { get; set; } = string.Empty;

        public List<string> Value { get; set; } = new List<string>();
    }

    // Data Types
    public class LinkData : IData
    {
        public const string KeyType = "Link";

        [YamlMember(Alias = "Link", ApplyNamingConventions = false)]
        public string Name { get; set; } = string.Empty;

        public string FullPath { get; set; } = string.Empty;

        public string Type { get; set; } = string.Empty;
    }

    public class I32Data : IData
    {
        public const string KeyType = "I32";

        [YamlMember(Alias = "I32", ApplyNamingConventions = false)]
        public string Name { get; set; } = string.Empty;

        public string FullPath { get; set; } = string.Empty;

        public int Min { get; set; } = int.MinValue;
        public int Max { get; set; } = int.MaxValue;
    }

    public class I64Data : IData
    {
        public const string KeyType = "I64";

        [YamlMember(Alias = "I64", ApplyNamingConventions = false)]
        public string Name { get; set; } = string.Empty;

        public string FullPath { get; set; } = string.Empty;

        public long Min { get; set; } = long.MinValue;
        public long Max { get; set; } = long.MaxValue;
    }

    public class F32Data : IData
    {
        public const string KeyType = "F32";

        [YamlMember(Alias = "F32", ApplyNamingConventions = false)]
        public string Name { get; set; } = string.Empty;

        public string FullPath { get; set; } = string.Empty;

        public float Min { get; set; } = float.MinValue;
        public float Max { get; set; } = float.MaxValue;
    }

    public class F64Data : IData
    {
        public const string KeyType = "F64";

        [YamlMember(Alias = "F64", ApplyNamingConventions = false)]
        public string Name { get; set; } = string.Empty;

        public string FullPath { get; set; } = string.Empty;

        public double Min { get; set; } = double.MinValue;
        public double Max { get; set; } = double.MaxValue;
    }

    public class Vector2Data : IData
    {
        public const string KeyType = "Vector2";

        [YamlMember(Alias = "Vector2", ApplyNamingConventions = false)]
        public string Name { get; set; } = string.Empty;

        public string FullPath { get; set; } = string.Empty;

        [YamlMember(Alias = "Min", ApplyNamingConventions = false)]
        public string _min { 
            
            set
            {
                var r = new System.Text.RegularExpressions.Regex("\\s?(\\d+)?\\s?,\\s?(\\d+)?\\s?");
                var m = r.Matches(value);
                var s1 = m[0].Groups[1].Value;
                var s2 = m[0].Groups[2].Value;

                float x, y = 0;
                float.TryParse(s1, out x);
                float.TryParse(s2, out y);

                Min = new(x,y);

            }
        }

        [YamlMember(Alias = "Max", ApplyNamingConventions = false)]
        public string _max {
            set
            {
                var r = new System.Text.RegularExpressions.Regex("\\s?(\\d+)?\\s?,\\s?(\\d+)?\\s?");
                var m = r.Matches(value);
                var s1 = m[0].Groups[1].Value;
                var s2 = m[0].Groups[2].Value;

                float x, y = 0;
                float.TryParse(s1, out x);
                float.TryParse(s2, out y);

                Max = new(x, y);
            }
        }

        [YamlIgnore]
        public (float x, float y) Min, Max;
    }

    public class Vector3Data : IData
    {
        public const string KeyType = "Vector3";

        [YamlMember(Alias = "Vector3", ApplyNamingConventions = false)]
        public string Name { get; set; } = string.Empty;

        public string FullPath { get; set; } = string.Empty;
    }
}
