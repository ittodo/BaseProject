using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace StaticDataViewModel.ViewModel
{
    public class Group : Notify.NotifyBase, IEnumerable<Group>
    {
        private StaticData.Define.IGroup _group;
        private Group _parent;


        private ValueType[] _childs;
        private string _registedChildName;

        public string Name
        {
            get
            {
                return _group.Name;
            }
            set
            {
                if(_parent != null)
                {
                    if(_parent.IsExist(value) == true)
                    {
                        return;
                    }
                }
                else
                {
                    if(Static.Inst.Collection.IsExist(value) == true)
                    {
                        return;
                    }
                }

                _group.Name = value;

                OnPropertyChanged("Name");
                OnPropertyChanged("HierarchyName");
            }
        }

        public string Type
        {
            get
            {
                return _group.Type.ToString();
            }
        }

        public Group Parent
        {
            get
            {
                return _parent;
            }
        }

        public ValueType[] Childs
        {
            get
            {
                return _childs;
            }
        }

        public StaticData.Define.IGroup ModelGroup
        {
            get
            {
                return _group;
            }
        }


        public Command.RelayCommand<string> AddChildGroup { get; private set; }

        public Command.RelayCommand<string> AddChild { get; private set; }

        public string RegistedChildName
        {
            private get
            {
                return _registedChildName;
            }
            set
            {
                _registedChildName = value;
                CheckCommand();
            }
        }


        public void CheckCommand()
        {
            foreach (var item in _childs)
            {
                if (string.Equals(item.Name, _registedChildName) == true)
                {
                    AddChild.IsEnabled = false;
                    AddChildGroup.IsEnabled = false;
                    return;
                }
            }
            AddChild.IsEnabled = true;
            AddChildGroup.IsEnabled = true;

        }


        public Group(StaticData.Define.IGroup group)
        {
            _parent = null;
            _group = group;
            AddChild = new Command.RelayCommand<string>(_AddChild);
            AddChild.IsEnabled = true;
            AddChild.CanExecuteChanged += _AddChildCanExecute;

            AddChildGroup = new Command.RelayCommand<string>(_AddChildGroup);
            AddChildGroup.IsEnabled = true;
            _childs = new ValueType[0];

        }

        public Group(Group parent, StaticData.Define.IGroup group)
        {
            this._parent = parent;
            _group = group;
            AddChild = new Command.RelayCommand<string>(_AddChild);
            AddChild.IsEnabled = true;

            AddChildGroup = new Command.RelayCommand<string>(_AddChildGroup);
            AddChildGroup.IsEnabled = true;
            _childs = new ValueType[0];

        }

        public void _AddChildCanExecute(object sender, System.EventArgs args)
        {

        }

        public void _AddChild(string name)
        {
            if(IsExist(name) == true)
            {
                return;
            }
            
            var value = new StaticData.Define.Integer(name);
            _group.AddChild(value);
            Array.Resize(ref _childs, _childs.Length + 1);
            var valuetype = new ValueType(this, _group, value, _childs.Length - 1);
            _childs[_childs.Length - 1] = valuetype;

            onPropertyChangeParent("Childs");
            AddChild.IsEnabled = false;
            AddChildGroup.IsEnabled = false;

            Static.Inst.Collection.ValueList.Add(valuetype);
        }

        public Group AddValue(int type , string name)
        {
            if (IsExist(name) == true)
            {
                return null;
            }

            Array.Resize(ref _childs, _childs.Length + 1);
            var valuetype = new ValueType(this, _group, type, name, _childs.Length - 1);
            _childs[_childs.Length - 1] = valuetype;

            onPropertyChangeParent("Childs");
            AddChild.IsEnabled = false;
            AddChildGroup.IsEnabled = false;

            Static.Inst.Collection.ValueList.Add(valuetype);

            return valuetype.SubGroup;
        }



        public void _AddChildGroup(string name)
        {
            foreach (var item in _childs)
            {
                if (item.Name == name)
                {
                    return;
                }
            }

            var value = new StaticData.Define.SubGroup(name);
            
            Array.Resize(ref _childs, _childs.Length + 1);

            var valuetype = new ValueType(this, _group, value, _childs.Length - 1);
            _childs[_childs.Length - 1] = valuetype;

            onPropertyChangeParent("Childs");

            Static.Inst.Collection.ValueList.Add(valuetype);
        }

        public bool IsExist(string Name)
        {
            return Childs.Any(x => { return string.Compare(x.Name, Name) == 0; });
        }

        private void onPropertyChangeParent(string name)
        {
            if (_parent != null)
            {
                _parent.onPropertyChangeParent(name);
            }
            OnPropertyChanged(name);
            foreach (var child in _childs)
            {
                child.OnPropertyChangedChild();
            }
        }

        public IEnumerator<Group> GetEnumerator()
        {
            
            yield return this;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Remove(ValueType child)
        {
            for(int i = 0; i < _childs.Length; i++)
            {
                if(_childs[i] == child)
                {
                    
                    Array.Copy(_childs, i, _childs, i + 1, _childs.Length - (i + 1) );
                }
            }

            Array.Resize(ref _childs, _childs.Length - 1);

            
            onPropertyChangeParent("Childs");

            Static.Inst.Collection.ValueList.Remove(child);

            CheckCommand();

            Static.Inst.Collection.SelectedGroup = Static.Inst.Collection.SelectedRootGroup;
        }

        public void Update()
        {
            onPropertyChangeParent("Childs");
        }
    }


}
