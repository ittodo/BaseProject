using System;
using System.Collections.Generic;
using System.Text;

namespace Socket
{
    public static class Run
    {
        static bool isinit = false;
        public static void Init()
        {
            if(isinit == true)
            {
                return;
            }
            Static.Static.CreateInstance();
            isinit = true;
        }

        public static void CleanUp()
        {
            if (isinit == false)
            {
                return;
            }
            Static.Static.CleanUp();
            isinit = false;
        }
    }
}
