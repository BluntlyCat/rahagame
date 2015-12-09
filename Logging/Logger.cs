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

        private string ObjectsToString(params object[] gameObjects)
        {
            string tmp = "";

            foreach (var gameObject in gameObjects)
            {
                tmp += gameObject.ToString() + " | ";
            }

            return tmp;
        }

        /// <summary>
        /// Gets the logger and add a new logger if not exist.
        /// </summary>
        /// <param name="name">The name of the new logger.</param>
        /// <returns>The ILog logger.</returns>
        private void Log(LogLevels level, params object[] gameObjects)
        {
            string log = level.ToString().ToUpper() + " IN " + this.type + " AT " + DateTime.Now + ": " + this.ObjectsToString(gameObjects);

            foreach(var logAppender in this.logAppenders)
            {
                logAppender.Value.Append(log, level);
            }
        }

        public void Debug(params object[] objects)
        {
            this.Log(LogLevels.debug, objects);
        }

        public void Error(params object[] objects)
        {
            this.Log(LogLevels.error, objects);
        }

        public void Fatal(params object[] objects)
        {
            this.Log(LogLevels.fatal, objects);
        }

        public void Info(params object[] objects)
        {
            this.Log(LogLevels.info, objects);
        }

        public void Warning(params object[] objects)
        {
            this.Log(LogLevels.warning, objects);
        }
    }
}