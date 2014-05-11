using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace SkjaTextar.Helpers
{
    public class Logger
    {
        private static Logger theInstance = null;
        public Logger()
        {
        }

        public static Logger Instance
        {
            get
            {
                if (theInstance == null)
                {
                    theInstance = new Logger();
                }
                return theInstance;
            }
        }

        public void LogException(Exception ex)
        {
            string message = DateTime.Now + Environment.NewLine + ex.ToString();
            string LogFilePath = Path.GetTempPath() + "skjatextar_logfile.txt";
            using (StreamWriter sw = new StreamWriter(LogFilePath, true))
            {
                sw.WriteLine(message);
            }
        }
    }
}