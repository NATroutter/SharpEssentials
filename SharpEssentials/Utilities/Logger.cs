using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpEssentials {
    public static class Logger {

        public static void log(string message) {
            Console.WriteLine("[SharpEssentials] " + message);
        }
        public static void error(string message) {
            Console.WriteLine("[SharpEssentials][ERROR] " + message);
        }
        public static void info(string message) {
            Console.WriteLine("[SharpEssentials][INFO] " + message);
        }
        public static void debug(string message) {
            Console.WriteLine("[SharpEssentials][DEBUG] " + message);
        }

    }
}
