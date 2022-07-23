using System;

namespace Utilities.Async
{
    public interface IMainThreadDispatcher
    {
        void Init();
        void Enqueue(Action action);
    }
}
