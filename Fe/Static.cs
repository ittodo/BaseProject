

using System;
using System.Linq;
using System.Text;
using Fe.Data;
using YamlDotNet.Serialization.NamingConventions;
using YamlDotNet.Serialization;
using YamlDotNet.Core;
using YamlDotNet.Helpers;
using YamlDotNet.RepresentationModel;
using YamlDotNet.Serialization.Utilities;
using System.Dynamic;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization.NodeTypeResolvers;
using YamlDotNet.Serialization.TypeResolvers;
using YamlDotNet.Serialization.ObjectFactories;
using System.ComponentModel.DataAnnotations;
using YamlDotNet.Serialization.NodeDeserializers;
using YamlDotNet.Serialization.Schemas;

namespace Fe
{
    public class Static
    {
        static BluePrint BluePrint { get; set; } = new BluePrint();


        public static void Open(string FilePath)
        {
            if (File.Exists(FilePath))
            {
                var data = File.ReadAllText(FilePath);
                var input = File.OpenText(FilePath);
                var deserializer = new DeserializerBuilder()
                    //.WithTagMapping("!Space", typeof(ppp))
                    //.WithNodeTypeResolver(new Deserializer())
                    .WithNodeDeserializer(new Deserializer())
                    .Build();

                var items = deserializer.Deserialize<List<IData>>(input);

                SpaceData space = null;
                foreach ( var item in items )
                {
                    if(item is SpaceData s)
                    {
                        space = s;

                        item.FullPath = s.Name;
                    }
                    else
                    {
                        item.FullPath = $"{space.Name}.{item.Name}";
                        int i = 0;
                        i++;
                    }
                }



                //foreach (Dictionary<string, object> dict in items)
                //{

                //    if (dict.ContainsKey("Space") == true)
                //    {
                //        var name = dict["Space"] as string;
                //        BluePrint.CreateSpace(name);
                //    }

                //    if (dict.ContainsKey("Enum") == true)
                //    {
                //        int i = 0;
                //        i++;
                //    }
                //}


            }
        }
    }
}