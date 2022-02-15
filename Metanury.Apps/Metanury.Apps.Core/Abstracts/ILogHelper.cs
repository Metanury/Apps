using System;

namespace Metanury.Apps.Core
{
    public interface ILogHelper
    {
        void Debug(string msg);

        void Debug<T>(T target);

        void Error(string msg);

        void Error(Exception ex);

        void Warn(string msg);

        void Info(string msg);

        void Fatal(string msg);
    }
}
