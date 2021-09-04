using System.Collections.Generic;
using Newtonsoft.Json;

namespace StaticData
{
    public class Static
    {

        public static Static Inst
        {
            get;
            private set;
        } = new Static();


        public Dictionary<string, Define.IGroup> Groups
        {
            get;
            private set;
        } = new Dictionary<string, Define.IGroup>();


        public Static()
        {

        }

        public (Define.IGroup, bool) NewGroup(string Name)
        {
            var group = new Define.Group();
            return (group, true);
        }

        public bool DeleteGroup(string Name)
        {
            return true;
        }

        public string Save()
        {
            foreach(var item in Groups)
            {
               
            }

            return string.Empty;
        }

        public void Load(string Value)
        {

        }

        public void Export()
        {

        }

    }
}
