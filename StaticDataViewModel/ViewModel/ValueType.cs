using StaticData.Define;
using System.Linq;
using System.Collections.Generic;
using System.Collections;

namespace StaticDataViewModel.ViewModel
{

    public class ValueType : Notify.NotifyBase
    {
        StaticData.Define.IGroup _group;
        StaticData.Define.IValue _value;
        int _index;

        // . 나자신임 .
        Group subGroup = null;
        Group parent = null;

        public string Name
        {
            get
            {
                return _value.Name;
            }
            set
            {
                this._ChangeName(value);
            }
        }

        public string HierarchyName
        {
            get
            {
                string name = _value.Name;
                var p = parent;
                while (p.Parent != null)
                {
                    name = p.Name + "." + name;
                    p = p.Parent;
                } 
                return name;
            }
        }

        public IEnumerable<EValueType> ValueTypes = System.Enum.GetValues(typeof(EValueType)).Cast<EValueType>();

        public EValueType Type
        {
            get
            {
                return _value.Type;
            }
            set
            {
                if(_value.Type == value)
                {
                    return;
                }
                _ChangeType(value.ToString());
            }
        }

        // subGroup를 지원 하기 위함
        public ValueType[] Childs
        {
            get
            {
                if (_value.Type == EValueType.SubGroup)
                {
                    if (subGroup != null)
                        return subGroup.Childs;

                    var group = _value as IGroup;
                    subGroup = new Group(this.parent, group);
                    return subGroup.Childs;
                }
                if (_value.Type == EValueType.Array)
                {
                    if (subGroup != null)
                        return subGroup.Childs;

                    var group = _value as IGroup;
                    subGroup = new Group(this.parent, group);
                    return subGroup.Childs;
                }
                return null;
            }
        }

        public Group SubGroup
        {
            get
            {
                if (_value.Type == EValueType.SubGroup)
                {
                    if (subGroup != null)
                        return subGroup;

                    var group = _value as IGroup;
                    subGroup = new Group(this.parent, group);

                    return subGroup;
                }
                if (_value.Type == EValueType.Array)
                {
                    if (subGroup != null)
                        return subGroup;

                    var group = _value as IGroup;
                    subGroup = new Group(this.parent, group);

                    return subGroup;
                }
                return subGroup;
            }
        }


        public Command.RelayCommand<string> ChangeType { get; private set; }

        public Command.RelayCommand<string> ChangeName { get; private set; }

        public Command.RelayCommand Destory { get; private set; }

        public ValueType(Group parent, IGroup group, IValue value , int index)
        {
            this.parent = parent;
            _group = group;
            _value = value;
            _index = index;
            _group.AddChild(value);
            ChangeType = new Command.RelayCommand<string>(_ChangeType);
            ChangeType.IsEnabled = true;

            ChangeName = new Command.RelayCommand<string>(_ChangeName);
            ChangeName.IsEnabled = true;

            Destory = new Command.RelayCommand(_Destory);
            Destory.IsEnabled = true;
        }

        public ValueType(Group parent , IGroup group , int type , string name , int index)
        {
            this.parent = parent;
            _group = group;

            EValueType evalue = (EValueType)type;
            //System.Enum.TryParse(type, out evalue);
            IValue instance = evalue.Create(name);
            _value = instance;
            _index = index;
            _group.AddChild(_value);
            ChangeType = new Command.RelayCommand<string>(_ChangeType);
            ChangeType.IsEnabled = true;

            ChangeName = new Command.RelayCommand<string>(_ChangeName);
            ChangeName.IsEnabled = true;

            Destory = new Command.RelayCommand(_Destory);
            Destory.IsEnabled = true;
        }


        public void _ChangeType(string type)
        {
            EValueType evalue = EValueType.Group;
            System.Enum.TryParse(type, out evalue);

            IValue instance = evalue.Create(Name);

            _group.Childs[_index] = instance;
            _value = instance;
            OnPropertyChanged("Type");
            parent.CheckCommand();
        }

        public void _ChangeName(string name)
        {
            if (parent != null)
            {
                if (parent.IsExist(name) == true)
                {
                    return;
                }
            }
            _value.Name = name;
            OnPropertyChanged("Childs");
            OnPropertyChanged("Name");
            OnPropertyChanged("HierarchyName");
            if(SubGroup != null)
            {
                this.SubGroup.Update();
            }
        }

        public void OnPropertyChangedChild()
        {
            OnPropertyChanged("Childs");
            OnPropertyChanged("Name");
            OnPropertyChanged("HierarchyName");
            if(Childs != null)
            {
                foreach (var child in this.Childs)
                {
                    child.OnPropertyChangedChild();
                }
            }
            
        }

        public void _Destory()
        {
            this._group.RemoveChild(_value);
            parent.Remove(this);
        }
    }
}
