using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace WheelTest
{
    public static class AppUtility
    {
        public static string ExecFile { get; private set; }

        public static string ExecPath { get; private set; }
        static AppUtility()
        {
            Assembly entryAssembly = Assembly.GetEntryAssembly();
            ExecFile = entryAssembly.Location;
            ExecPath = Path.GetDirectoryName(ExecFile);
        }
    }
}
