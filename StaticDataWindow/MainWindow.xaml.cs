using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Windows.UI.Notifications;
using Newtonsoft.Json;

using System.Linq;

namespace StaticDataWindow
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    /// 

    public class testAttribute : Attribute
    {
        public testAttribute(string value)
        {

        }
    }

    public class www
    {
        public int wwwwww;
        public string wwwwww2;
    }

    public class weff
    {
        [test("Name")]
        public string wer;
        public www werrr;
    }
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var c = StaticDataViewModel.Static.Inst.Collection;
            DataContext = c;
            //var flyout = this.Flyouts.Items[0] as Flyout;
            //flyout.IsOpen = false;
            //this.LeftWindowCommandsOverlayBehavior = WindowCommandsOverlayBehavior.HiddenTitleBar;


            weff w = new weff();
            w.wer = "";
            test(w);

            GetValue<weff>(null);

        }


        public void test(weff w)
        {
            var type = w.GetType();
            string name = type.Name;
            var itme = type.GetCustomAttributesData();
            var Fields = type.GetFields(BindingFlags.Instance
            | BindingFlags.Public);
            var property = type.GetProperties(System.Reflection.BindingFlags.Public);
            var type3 = typeof(weff);
            var type2 = w.wer.GetType();

            Fields[0].SetValue(w, "stttt");

            Console.WriteLine(name);
        }

        public T GetValue<T>(Dictionary<string, string> row)
        {
            var type = typeof(T);
            var newObj = default(T);

            var Fields = type.GetFields(BindingFlags.Instance
            | BindingFlags.Public);

            var attr = Fields[0].GetCustomAttribute<testAttribute>();
            var attr2 = Fields[1].GetCustomAttribute<testAttribute>();


            var type2 = Fields[1].FieldType.IsPrimitive;
            var Fields2 = Fields[1].FieldType.GetFields(BindingFlags.Instance
            | BindingFlags.Public);
            var Primitive = Fields2[0].FieldType.IsPrimitive;

            var Primitive2 = Fields2[1].FieldType.IsPrimitive;

            return newObj;
        }




        private void TreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            var tex = sender as TreeView;
            var selectGroup = tex.SelectedItem as StaticDataViewModel.ViewModel.Group;
            var selectValue = tex.SelectedItem as StaticDataViewModel.ViewModel.ValueType;

            if (selectValue != null && selectValue.SubGroup != null)
            {
                StaticDataViewModel.Static.Inst.Collection.SelectedGroup = selectValue.SubGroup;
            }
            else
            {
                StaticDataViewModel.Static.Inst.Collection.SelectedGroup = selectGroup;
            }

            StaticDataViewModel.Static.Inst.Collection.SelectedValue = selectValue;
        }

        private void ValueName_TextChanged(object sender, TextChangedEventArgs e)
        {
            var tex = sender as TextBox;

            var group = StaticDataViewModel.Static.Inst.Collection.SelectedGroup;
            if (group == null)
            {
                tex.Background = Brushes.White;
                return;
            }
            group.RegistedChildName = tex.Text;

            if (group.IsExist(tex.Text) == true)
            {
                tex.Background = Brushes.OrangeRed;
            }
            else
            {
                tex.Background = Brushes.White;
            }
        }

        private void GroupName_TextInput(object sender, TextCompositionEventArgs e)
        {
            var tex = sender as TextBox;

            var collection = StaticDataViewModel.Static.Inst.Collection;
            collection.SelectedGroup.RegistedChildName = tex.Text;

            if (collection.IsExist(tex.Text) == true)
            {
                tex.Background = Brushes.OrangeRed;
            }
            else
            {
                tex.Background = Brushes.White;
            }
        }

        private const String APP_ID = "YourCompanyName.YourAppName";

        public static void CreateToast()
        {
            Windows.Data.Xml.Dom.XmlDocument toastXml = ToastNotificationManager.GetTemplateContent(
                ToastTemplateType.ToastImageAndText02);

            // Fill in the text elements
            Windows.Data.Xml.Dom.XmlNodeList stringElements = toastXml.GetElementsByTagName("text");
            stringElements[0].AppendChild(toastXml.CreateTextNode("This is my title!!!!!!!!!!"));
            stringElements[1].AppendChild(toastXml.CreateTextNode("This is my message!!!!!!!!!!!!"));

            // Specify the absolute path to an image
            string filePath = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86) + @"\Your Path To File\Your Image Name.png";
            Windows.Data.Xml.Dom.XmlNodeList imageElements = toastXml.GetElementsByTagName("image");
            imageElements[0].Attributes.GetNamedItem("src").NodeValue = filePath;

            // Change default audio if desired - ref - https://docs.microsoft.com/en-us/uwp/schemas/tiles/toastschema/element-audio
            Windows.Data.Xml.Dom.XmlElement audio = toastXml.CreateElement("audio");
            //audio.SetAttribute("src", "ms-winsoundevent:Notification.Reminder");
            //audio.SetAttribute("src", "ms-winsoundevent:Notification.IM");
            //audio.SetAttribute("src", "ms-winsoundevent:Notification.Mail"); // sounds like default
            //audio.SetAttribute("src", "ms-winsoundevent:Notification.Looping.Call7");  
            audio.SetAttribute("src", "ms-winsoundevent:Notification.Looping.Call2");
            audio.SetAttribute("loop", "true");
            // Add the audio element
            toastXml.DocumentElement.AppendChild(audio);

            Windows.Data.Xml.Dom.XmlElement actions = toastXml.CreateElement("actions");
            toastXml.DocumentElement.AppendChild(actions);

            // Create a simple button to display on the toast
            Windows.Data.Xml.Dom.XmlElement action = toastXml.CreateElement("action");
            actions.AppendChild(action);
            action.SetAttribute("content", "Show details");
            action.SetAttribute("arguments", "viewdetails");

            // Create the toast 
            ToastNotification toast = new ToastNotification(toastXml);

            // Show the toast. Be sure to specify the AppUserModelId
            // on your application's shortcut!
            ToastNotificationManager.CreateToastNotifier("Static").Show(toast);
        }

        private void Toast_Click(object sender, RoutedEventArgs e)
        {
            CreateToast();
        }

        private void MyCalendar_ChildChanged(object sender, EventArgs e)
        {

        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var grid = sender as DataGrid;
            var selectGroup = grid.SelectedItem as StaticDataViewModel.ViewModel.Group;
            StaticDataViewModel.Static.Inst.Collection.SelectedGroup = selectGroup;
            StaticDataViewModel.Static.Inst.Collection.SelectedRootGroup = selectGroup;

            var group = StaticDataViewModel.Static.Inst.Collection.SelectedGroup;
            if (group == null)
            {
                return;
            }

            group.RegistedChildName = this.ValueName.Text;

            if (group.IsExist(this.ValueName.Text) == true)
            {
                this.ValueName.Background = Brushes.OrangeRed;
            }
            else
            {
                this.ValueName.Background = Brushes.White;
            }
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DataGrid_AddingNewItem(object sender, AddingNewItemEventArgs e)
        {
           
        }

        private void DataGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if(e.Column.Header.ToString() == "HierarchyName")
            {

            }
            else if(e.Column.Header.ToString() == "Name")
            {

            }
            else if(e.Column.Header.ToString() == "Type")
            {

            }
            else
            {
                e.Cancel = true;
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            JsonSerializerSettings setting = new JsonSerializerSettings()
            {
                Formatting = Formatting.Indented,
                TypeNameHandling = TypeNameHandling.None,
            };

            List<StaticData.Define.IGroup> groups = new List<StaticData.Define.IGroup>(); ;
            foreach(var tt in StaticDataViewModel.Static.Inst.Collection.GroupList)
            {
                groups.Add(tt.ModelGroup);
            }

            var data = JsonConvert.SerializeObject(groups, setting);
            System.Diagnostics.Debug.Write(data);
            Console.WriteLine(data);

            var item11 = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            System.Diagnostics.Debug.Write(item11);

            System.IO.File.WriteAllText($"{item11}/save.json", data);

            foreach (var item in StaticDataViewModel.Static.Inst.Collection.GroupList)
            {
                var group = item.ModelGroup;
                
                data = JsonConvert.SerializeObject(group , setting);

                System.Diagnostics.Debug.Write(data);
                Console.WriteLine(data);

                System.Diagnostics.Debug.Write(System.AppDomain.CurrentDomain.BaseDirectory);
                
            }
            
        }

        private static void OnChanged(object sender, System.IO.FileSystemEventArgs e)
        {
            if (e.ChangeType != System.IO.WatcherChangeTypes.Changed)
            {
                return;
            }
            Console.WriteLine($"Changed: {e.FullPath}");
        }

        private void DetailView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var grid = sender as System.Windows.Controls.DataGrid;
            //grid.CommitEdit();
            var column = grid.CurrentColumn;
            if(column != null && string.Equals(column.Header.ToString() , "Type")==false)
            {
                grid.CommitEdit();
            }
            if (grid.SelectedItem != null)
            {
                var selectValue = grid.SelectedItem as StaticDataViewModel.ViewModel.ValueType;

                if (selectValue != null && selectValue.SubGroup != null)
                {
                    StaticDataViewModel.Static.Inst.Collection.SelectedGroup = selectValue.SubGroup;
                }
                else
                {
                    StaticDataViewModel.Static.Inst.Collection.SelectedGroup = StaticDataViewModel.Static.Inst.Collection.SelectedRootGroup;
                }

                StaticDataViewModel.Static.Inst.Collection.SelectedValue = selectValue;
            }


            var group = StaticDataViewModel.Static.Inst.Collection.SelectedGroup;
            if (group == null)
            {
                return;
            }

            group.RegistedChildName = this.ValueName.Text;

            if (group.IsExist(this.ValueName.Text) == true)
            {
                this.ValueName.Background = Brushes.OrangeRed;
            }
            else
            {
                this.ValueName.Background = Brushes.White;
            }
        }

        private void DetailView_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            var grid = sender as System.Windows.Controls.DataGrid;
            
            if (grid.SelectedItem != null)
            {
                var selectValue = grid.SelectedItem as StaticDataViewModel.ViewModel.ValueType;
                if(selectValue != null)
                {
                    var type = selectValue.Type;
                    selectValue.Type = type;
                }

                if (selectValue != null && selectValue.SubGroup != null)
                {
                    StaticDataViewModel.Static.Inst.Collection.SelectedGroup = selectValue.SubGroup;
                }

                StaticDataViewModel.Static.Inst.Collection.SelectedValue = selectValue;
            }
        }

        private void Load_Click(object sender, RoutedEventArgs e)
        {
            JsonSerializerSettings setting = new JsonSerializerSettings()
            {
                Formatting = Formatting.Indented,
                TypeNameHandling = TypeNameHandling.None,
            };

            var myDocuments = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            System.Diagnostics.Debug.Write(myDocuments);

            var readText = System.IO.File.ReadAllText($"{myDocuments}/save.json");

            var json = Newtonsoft.Json.Linq.JArray.Parse(readText);

            StaticDataViewModel.Static.Inst.Collection.GroupList.Clear();

            foreach (var item in json)
            {
                var value = item as Newtonsoft.Json.Linq.JObject;

                var Name = value["Name"] as Newtonsoft.Json.Linq.JValue;
                var Type = value["Type"] as Newtonsoft.Json.Linq.JValue;
                var child = value["Childs"] as Newtonsoft.Json.Linq.JArray;

                
                StaticDataViewModel.Static.Inst.Collection.AddGroup.Execute(Name.Value);
                var grouplist = StaticDataViewModel.Static.Inst.Collection.GroupList;
                StaticDataViewModel.ViewModel.Group group = null;
                foreach (var groupItem in grouplist)
                {
                    if(string.Equals(groupItem.Name , Name.Value))
                    {
                        group = groupItem;
                        break;
                    }
                }
                ChildArray(group, child);
            }
            StaticDataViewModel.Static.Inst.Collection.SelectedRootGroup = StaticDataViewModel.Static.Inst.Collection.GroupList.First();
        }

        public static void ChildArray(StaticDataViewModel.ViewModel.Group group , Newtonsoft.Json.Linq.JArray array)
        {
            foreach( var item in array)
            {
                var Name = item["Name"] as Newtonsoft.Json.Linq.JValue;
                var Type = item["Type"] as Newtonsoft.Json.Linq.JValue;
                var childGroup = group.AddValue((int)(long)Type.Value, Name.Value as string);

                var child = item["Childs"] as Newtonsoft.Json.Linq.JArray;

                if(childGroup != null && child != null)
                {
                    ChildArray(childGroup, child);
                }
            }
            
        }
    }
}
