using System.Collections.ObjectModel;
using System.Linq;

namespace StaticDataViewModel.ViewModel
{
    public class Collection : Notify.NotifyBase
    {
        
        public ObservableCollection<Group> GroupList { get; set; } = new ObservableCollection<Group>();

        public Command.RelayCommand<string> AddGroup { get; private set; }

        public Command.RelayCommand<string> ChangeName { get; private set; }

        public Command.RelayCommand RemoveGroup { get; private set; }

        public ObservableCollection<ValueType> ValueList { get; set; } = new ObservableCollection<ValueType>();



        private Group _selectGroup;
        public Group SelectedGroup
        {
            get { return _selectGroup; }
            set
            {
                _selectGroup = value;
                this.OnPropertyChanged("SelectedGroup");

                
            }
        }

        void ValueTypeChildGet(ValueType valuetype)
        {
            ValueList.Add(valuetype);
            var childs = valuetype.Childs;
            if(childs != null)
            {
                foreach(var child in childs )
                {
                    ValueTypeChildGet(child);
                }
            }
        }


        private Group _selectedRootGroup;
        public Group SelectedRootGroup
        {
            get { return _selectedRootGroup; }
            set
            {
                _selectedRootGroup = value;
                this.OnPropertyChanged("SelectedRootGroup");
                if (_selectedRootGroup != null)
                {
                    ValueList.Clear();
                    foreach (var child in _selectedRootGroup.Childs)
                    {
                        ValueTypeChildGet(child);
                    }
                }
            }
        }

        private ValueType _selectedValue;
        public ValueType SelectedValue
        {
            get
            {
                return _selectedValue;
            }
            set
            {
                _selectedValue = value;
                this.OnPropertyChanged("SelectedValue");
            }
        }

        public Collection()
        {
            AddGroup = new Command.RelayCommand<string>(_addGroupCommand);
            AddGroup.IsEnabled = true;

            RemoveGroup = new Command.RelayCommand(_removeGroupCommand);
            RemoveGroup.IsEnabled = false;
        }

        private void _removeGroupCommand()
        {
            if(_selectedRootGroup == null)
            {
                return;
            }

            GroupList.Remove(_selectedRootGroup);
            _selectedRootGroup = null;
        }

        private void _addGroupCommand(string Name)
        {
            if(IsExist(Name) == true)
            {
                return;
            }

            var group = new StaticData.Define.Group(Name);
            var collectiongroup = new Group(group);
            GroupList.Add(collectiongroup);
        }

        public bool IsExist(string Name)
        {
            return GroupList.Any(x => { return string.Compare(x.Name, Name) == 0; });
        }
    }
}
