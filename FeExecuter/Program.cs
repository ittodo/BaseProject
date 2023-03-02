using System.Reflection.Metadata;
using System.Security.Cryptography.X509Certificates;
using YamlDotNet;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace FeExecuter
{
    internal class Program
    {
        public class Config
        {
            public bool SplitFile { get; set; }
        }



        static void Main(string[] args)
        {
            Fe.Static.Open("Data/Data.yaml");

            var input = new StringReader(Document);

            string Space;

            var deserializer = new DeserializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .Build();
            var items = deserializer.Deserialize(input) as List<object>;

            foreach(Dictionary<object,object> dict in items)
            {
                if(dict.ContainsKey("Space") == true)
                {
                    int i = 0;
                    i++;
                }

                if (dict.ContainsKey("Enum") == true)
                {
                    int i = 0;
                    i++;
                }
            }

            

            return;

        }

        private const string Document = @"

# 스키마 작성 
# Space : NameSpace
# Enum 타입
# Primetive Integer , Float , Double , FixFloat2 , FixFloat3 , Vector2 , Vector3
# 복합 타입 Table , Sub
# Space 파일 단위

- Space : Character
- Enum : EJob
  Value : 
    - Jack
    - Pepe

- Enum : EJob
  Value : 
    - Jack
    - Pepe

- Space : Character
- Sub : Stat
  Value :
    - Integer :
      Name : Speed
      Min : -100
      Max : 100
    - Integer :
      Name : Attack
    - Integer :
      Name : Magic
    - Integer :
      Name : Defence
    - Integer :
      Name : MDefence
    - Vector2 :
      Min : 0,0
      Max : 200,200
    
- Space : Character
- Table :
    Name : Player
    Value :
      - Integer:
        Name : User
        Min : -100
        Max : 100
      - Enum :
        Name : Job
        Type : EJob
      - Link :
        Name : Level
- Table :
    Name : Level
    Value :
      - Integer :
        Name : Index
      - Integer :
        Name : Exp
      - Sub : Stat
        Name : Stat
        Type : Character.Stat
      - Enum : Job
        Type : Character.EJob

 
";
    }
}