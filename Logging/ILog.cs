namespace HSA.RehaGame.Logging
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public interface ILog
    {
        void Info(params object[] objects);

        void Debug(params object[] objects);

        void Warning(params object[] objects);

        void Error(params object[] objects);

        void Fatal(params object[] objects);
    }
}
