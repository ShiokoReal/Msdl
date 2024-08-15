using System;
using System.Reflection;

namespace Net.Myzuc.PurpleStainedGlass
{
    public static class Logs
    {
        public static void Warning(string message)
        {
            Any(Assembly.GetCallingAssembly(), message);
        }
        public static void Info(string message)
        {
            Any(Assembly.GetCallingAssembly(), message);
        }
        private static void Any(Assembly assembly, string message)
        {
            string format = $"[{assembly.GetName().Name}] {message}";
            Console.WriteLine(format);
        }
    }
}
