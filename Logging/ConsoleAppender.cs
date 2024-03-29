﻿namespace HSA.RehaGame.Logging
{
    using System;
    using System.Collections.Generic;

    public class ConsoleAppender : ILogAppender
    {
        public ConsoleAppender(Type name)
        {

        }

        private static readonly Dictionary<LogLevels, string[]> colors = new Dictionary<LogLevels, string[]>
        {
            {LogLevels.info, new string[] {"<color=green>", "</color>"} },
            {LogLevels.debug, new string[] {"<color=blue>", "</color>"} },
            {LogLevels.warning, new string[] {"<color=yellow>", "</color>"} },
            {LogLevels.error, new string[] {"<color=red>", "</color>"} },
            {LogLevels.fatal, new string[] {"<color=magenta>", "</color>"} },
        };

        public void Append(string message, LogLevels level)
        {
            var color = colors[level];
            UnityEngine.Debug.Log(color[0] + message + color[1]);
        }
    }
}
