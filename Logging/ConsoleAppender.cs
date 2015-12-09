namespace HSA.RehaGame.Logging
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class ConsoleAppender : ILogAppender
    {
        public ConsoleAppender(Type name)
        {

        }

        private static readonly Dictionary<LogLevels, string[]> colors = new Dictionary<LogLevels, string[]>
        {
            {LogLevels.info, new string[] {"<color=green>", "</color>"} },
            {LogLevels.debug, new string[] {"<color=magenta>", "</color>"} },
            {LogLevels.warning, new string[] {"<color=yellow>", "</color>"} },
            {LogLevels.error, new string[] {"<color=orange>", "</color>"} },
            {LogLevels.fatal, new string[] {"<color=red>", "</color>"} },
        };

        public void Append(string message, LogLevels level)
        {
            var color = colors[level];
            UnityEngine.Debug.Log(color[0] + message + color[1]);
        }
    }
}
