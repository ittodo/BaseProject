using System;
using System.Collections.Generic;
using System.Text;

namespace StaticData.Define
{
    public static class Factory
    {
        public static Define.IValue Create(this Define.EValueType Value , string name)
        {
            Define.IValue value = null;
            switch (Value)
            {
                case Define.EValueType.Array:
                    value = new Define.Array(name);
                    break;
                case Define.EValueType.Group:
                    value = new Define.Group(name);
                    break;
                case Define.EValueType.Integer:
                    value = new Define.Integer(name);
                    break;
                case Define.EValueType.Reference:
                    value = new Define.Reference(name);
                    break;
                case Define.EValueType.Double:
                    value = new Define.Double(name);
                    break;
                case Define.EValueType.Float:
                    value = new Define.Float(name);
                    break;
                case Define.EValueType.String:
                    value = new Define.String(name);
                    break;
                case Define.EValueType.SubGroup:
                    value = new Define.SubGroup(name);
                    break;
                default:
                    System.Diagnostics.Debug.Assert(false, $"Add Define ValueType");
                    break;
                
            }
            return value;
        }
    }
}
