using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic
{
    public static class Loger
    {
        // Initiate path.
        private static string m_exePath = string.Empty;

        // take message and start loging proccess.
        public static void LogWriter(string logMessage)
        {
            LogWrite(logMessage);
        }

        // create log file if not exist and log message to it.
        private static void LogWrite(string logMessage)
        {
            m_exePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            try
            {
                using (StreamWriter w = File.AppendText(m_exePath + "\\" + "log.txt"))
                {
                    Log(logMessage, w);
                }
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex);
            }
        }

        // perform view of log message.
        private static void Log(string logMessage, TextWriter txtWriter)
        {
            try
            {
                txtWriter.Write("Log Entry: ");
                txtWriter.WriteLine($"{DateTime.Now.ToLongTimeString()} {DateTime.Now.ToLongDateString()}");
                txtWriter.WriteLine($"Message: {logMessage}");
                txtWriter.WriteLine("-------------------------------\n");
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex);
            }
        }
    }
}
