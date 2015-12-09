namespace HSA.RehaGame.Logging
{
    using System;
    using System.IO;

    public class FileAppender : ILogAppender
    {
        private string path;

        public FileAppender(Type name)
        {
            path = @"Log\" + name + ".log";

            if (!File.Exists(path))
            {
                // Create a file to write to.
                using (StreamWriter sw = File.CreateText(this.path))
                {
                    sw.WriteLine("Logfile for " + name + " created on " + DateTime.Now);
                }
            }
        }

        public void Append(string message, LogLevels level)
        {
            using (StreamWriter sw = File.AppendText(this.path))
            {
                sw.WriteLine(message);
            }
        }
    }
}
