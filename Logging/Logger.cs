// ------------------------------------------------------------------------
// <copyright file="logger.cs" company="HSA.RehaGame">
//     Copyright statement. All right reserved
// </copyright>
// ------------------------------------------------------------------------
namespace HSA.RehaGame.Logging
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Handles requests for logger for a specific class,
    /// creates the configuration if there is none.
    /// </summary>
    public class Logger<T> : ILog
    {
        private Dictionary<Type, ILogAppender> logAppenders = new Dictionary<Type, ILogAppender>();

        private Type type;

        public Logger()
        {
            this.type = typeof(T);
        }

        /// <summary>
        /// Adds the logger.
        /// </summary>
        /// <param name="name">The name of the logger.</param>
        public void AddLogAppender<L>() where L : ILogAppender
        {
            if(logAppenders.ContainsKey(typeof(L)) == false)
            {
                logAppenders.Add(typeof(L), (L)Activator.CreateInstance(typeof(L), new object[] { this.type }));
            }
        }

        private string ObjectsToString(object[] gameObjects)
        {
            string tmp = "({0}{1}";

            for(int i = 0; i < gameObjects.Length; i++)
            {
                if (i < gameObjects.Length - 1)
                    tmp = string.Format(tmp, gameObjects[i].ToString(), ", {0}{1}");
                else
                    tmp = string.Format(tmp, gameObjects[i].ToString(), ")");
            }

            return tmp;
        }

        /// <summary>
        /// Gets the logger and add a new logger if not exist.
        /// </summary>
        /// <param name="name">The name of the new logger.</param>
        /// <returns>The ILog logger.</returns>
        private void Log(LogLevels level, object[] gameObjects)
        {
            string log = string.Format("{0} IN {1} AT {2}: {3}", level.ToString().ToUpper(), this.type.Name, DateTime.Now, this.ObjectsToString(gameObjects));

            foreach(var logAppender in this.logAppenders)
            {
                logAppender.Value.Append(log, level);
            }
        }

        /// <summary>
        /// Gets the logger and add a new logger if not exist.
        /// </summary>
        /// <param message="message">The name of the new logger.</param>
        /// <param name="name">The name of the new logger.</param>
        /// <returns>The ILog logger.</returns>
        private void Log(LogLevels level, string message, object[] gameObjects)
        {
            string log = string.Format("{0} IN {1} AT {2}: {3}:\n{4}", level.ToString().ToUpper(), this.type.Name, DateTime.Now, message, this.ObjectsToString(gameObjects));

            foreach (var logAppender in this.logAppenders)
            {
                logAppender.Value.Append(log, level);
            }
        }

        public void Debug(params object[] objects)
        {
            this.Log(LogLevels.debug, objects);
        }
        public void Debug(string message, params object[] objects)
        {
            this.Log(LogLevels.debug, message, objects);
        }

        public void Error(params object[] objects)
        {
            this.Log(LogLevels.error, objects);
        }
        public void Error(string message, params object[] objects)
        {
            this.Log(LogLevels.error, message, objects);
        }

        public void Fatal(params object[] objects)
        {
            this.Log(LogLevels.fatal, objects);
        }
        public void Fatal(string message, params object[] objects)
        {
            this.Log(LogLevels.fatal, message, objects);
        }

        public void Info(params object[] objects)
        {
            this.Log(LogLevels.info, objects);
        }
        public void Info(string message, params object[] objects)
        {
            this.Log(LogLevels.info, message, objects);
        }

        public void Warning(params object[] objects)
        {
            this.Log(LogLevels.warning, objects);
        }
        public void Warning(string message, params object[] objects)
        {
            this.Log(LogLevels.warning, message, objects);
        }
    }
}