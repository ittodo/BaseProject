using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
using Fe.Data;

namespace Fe
{
    public class FakeParser : IParser
    {
        public Queue<ParsingEvent> _event = new Queue<ParsingEvent>();

        public IParser parentParser { get; set; }

        public ParsingEvent? Current
        {
            get
            {
                if (_event.Count != 0)
                {
                    return _event.Peek();
                }
                return parentParser.Current;
            }
        }

        public bool MoveNext()
        {
            if (_event.Count != 0)
            {
                _event.Dequeue();
                return true;
            }
            return parentParser.MoveNext();
        }
    }

    public class Deserializer : INodeDeserializer, INodeTypeResolver
    {
        List<Fe.Data.IData> BuildDerializer = new List<Fe.Data.IData>();

        public bool Deserialize(IParser parser, Type expectedType, Func<IParser, Type, object?> nestedObjectDeserializer, out object? value)
        {
            bool isParse = ParseGlobal(parser, nestedObjectDeserializer, out value);
            if (isParse == true)
            {
                return true;
            }

            if(ParseType == EParseType.Table)
            {
                bool isTable = ParseTable(parser, nestedObjectDeserializer, out value);
                return isTable;
            }
            return false;
        }

        public enum EParseType { Global, Table };

        private bool ParseGlobal(IParser parser, Func<IParser, Type, object> nestedObjectDeserializer, out object? value)
        {
            if (isParse != false)
            {
                value = null;
                return false;
            }

            int Mapping = 0;
            var current = parser.Current;
            int nesting = 0;
            nesting = nesting + current.NestingIncrease;

            Queue<ParsingEvent> _mappingevent = new Queue<ParsingEvent>();
            while (nesting != 0)
            {
                if (current is MappingStart)
                {
                    Mapping++;
                }
                if (current is MappingEnd)
                {
                    Mapping--;
                }
                if (Mapping == 1)
                {
                    _mappingevent.Enqueue(current);
                }

                if (current is YamlDotNet.Core.Events.DocumentEnd)
                {
                    break;
                }

                Console.WriteLine($"{current.ToString()} {current.NestingIncrease} , {current.NestingIncrease}");

                if (current is YamlDotNet.Core.Events.Scalar scalar)
                {
                    var KeyValue = scalar.Value as string;
                    parser.MoveNext();
                    current = parser.Current;
                    var fp = new FakeParser();
                    fp.parentParser = parser;
                    fp._event = _mappingevent;
                    nested++;
                    IData Value = null;
                    switch (KeyValue)
                    {
                        case SpaceData.KeyType:
                            Value = nestedObjectDeserializer(fp, typeof(Fe.Data.SpaceData)) as Fe.Data.SpaceData;
                            break;

                        case EnumData.KeyType:
                            Value = nestedObjectDeserializer(fp, typeof(Fe.Data.EnumData)) as Fe.Data.EnumData;
                            break;

                        case I32Data.KeyType:
                            Value = nestedObjectDeserializer(fp, typeof(Fe.Data.I32Data)) as Fe.Data.I32Data;
                            break;

                        case SubData.KeyType:
                            tableCount++;
                            Value = nestedObjectDeserializer(fp, typeof(Fe.Data.SubData)) as Fe.Data.SubData;
                            tableCount--;
                            break;
                        case TableData.KeyType:
                            tableCount++;
                            Value = nestedObjectDeserializer(fp, typeof(Fe.Data.TableData)) as Fe.Data.TableData;
                            tableCount--;
                            break;

                        case I64Data.KeyType:

                            break;

                        case F32Data.KeyType:

                            break;

                        case F64Data.KeyType:

                            break;

                        case Vector2Data.KeyType:

                            break;
                        case Vector3Data.KeyType:

                            break;
                        default:
                            break;
                    }

                    nested--;
                    _mappingevent.Enqueue(parser.Current);
                    BuildDerializer.Add(Value);

                }
                parser.MoveNext();
                current = parser.Current;
                nesting = nesting + current.NestingIncrease;
            }
            
            current = parser.Current;
            Console.WriteLine($"{current.ToString()} {current.NestingIncrease} , {current.NestingIncrease}");

            value = BuildDerializer;
            return true;
        }

        private bool ParseTable(IParser parser, Func<IParser, Type, object> nestedObjectDeserializer, out object? Value)
        {
            Value = null;
            var current = parser.Current;
            Console.WriteLine(current.ToString());
            //if (current is Scalar s)
            //{
            //    if (s.Value == "Value")
            //    {
            //        value = new List<IData>();
            //        while (parser.MoveNext())
            //        {
            //            if (parser.Current is SequenceEnd)
            //            {
            //                break;
            //            }
            //        }
            //        return true;
            //    }
            //}


            if (current is SequenceStart)
            {
                var list = new List<IData>();
                Value = list;
                Queue<ParsingEvent> _mappingevent = new Queue<ParsingEvent>();
                
                while (parser.MoveNext())
                {
                    _mappingevent.Enqueue(parser.Current);


                    if (parser.Current is Scalar KeyValue)
                    {
                        parser.MoveNext();
                        var fp = new FakeParser();
                        fp.parentParser = parser;
                        fp._event = _mappingevent;
                        IData iData = null;
                        switch (KeyValue.Value)
                        {
                            case SpaceData.KeyType:
                                iData = nestedObjectDeserializer(fp, typeof(Fe.Data.SpaceData)) as Fe.Data.SpaceData;
                                break;

                            case EnumData.KeyType:
                                iData = nestedObjectDeserializer(fp, typeof(Fe.Data.EnumData)) as Fe.Data.EnumData;
                                break;
                            case SubData.KeyType:
                                tableCount++;
                                iData = nestedObjectDeserializer(fp, typeof(Fe.Data.SubData)) as Fe.Data.SubData;
                                tableCount--;
                                break;
                            case TableData.KeyType:
                                tableCount++;
                                iData = nestedObjectDeserializer(fp, typeof(Fe.Data.TableData)) as Fe.Data.TableData;
                                tableCount--;
                                break;

                            case I32Data.KeyType:
                                iData = nestedObjectDeserializer(fp, typeof(Fe.Data.I32Data)) as Fe.Data.I32Data;
                                break;

                            case I64Data.KeyType:
                                iData = nestedObjectDeserializer(fp, typeof(Fe.Data.I64Data)) as Fe.Data.I64Data;
                                break;

                            case F32Data.KeyType:
                                iData = nestedObjectDeserializer(fp, typeof(Fe.Data.F32Data)) as Fe.Data.F32Data;
                                break;

                            case F64Data.KeyType:
                                iData = nestedObjectDeserializer(fp, typeof(Fe.Data.F64Data)) as Fe.Data.F64Data;
                                break;
                            case Vector2Data.KeyType:
                                iData = nestedObjectDeserializer(fp, typeof(Fe.Data.Vector2Data)) as Fe.Data.Vector2Data;
                                break;
                            case Vector3Data.KeyType:
                                iData = nestedObjectDeserializer(fp, typeof(Fe.Data.Vector3Data)) as Fe.Data.Vector3Data;
                                break;

                            case LinkData.KeyType:
                                iData = nestedObjectDeserializer(fp, typeof(Fe.Data.LinkData)) as Fe.Data.LinkData;
                                break;
                            default:
                                break;
                        }
                        list.Add(iData);
                        _mappingevent.Enqueue(parser.Current);
                    }


                    if (parser.Current is SequenceEnd)
                    {
                        break;
                    }
                }
                parser.MoveNext();
                return true;
            }
            return false;
        }

        public bool Resolve(NodeEvent? nodeEvent, ref Type currentType)
        {
            throw new NotImplementedException();
        }

        public static int nested { get; set; } = 0;

        public static bool isParse
        {
            get
            {
                return nested != 0;
            }
        }

        public static int tableCount { get; set; } = 0;

        public static EParseType ParseType
        {
            get
            {
                if (tableCount != 0)
                {
                    return EParseType.Table;
                }
                return EParseType.Global;
            }
        }

    }
}
