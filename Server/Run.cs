using System;
using System.Collections.Generic;
using System.Text;

namespace Socket
{
    public static class Run
    {
        public static void Init()
        {
            Static.Static.CreateInstance();
        }

        public static void CleanUp()
        {
            Static.Static.CleanUp();
        }
    }
}
