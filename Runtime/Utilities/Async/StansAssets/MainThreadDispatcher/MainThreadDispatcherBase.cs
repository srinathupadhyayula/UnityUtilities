using System;
using System.Collections.Concurrent;

namespace Utilities.Async
{
    public abstract class MainThreadDispatcherBase : IMainThreadDispatcher
    {
        private static readonly ConcurrentQueue<Action> ExecutionQueue = new();

        public abstract void Init();

        public void Enqueue(Action action) => ExecutionQueue.Enqueue(action);
        //TODO: Implement generic update method like generic implementation : MainThreadDispatcher<T> where T : IDispatcherUpdate
        protected virtual void Update()
        {
            if(ExecutionQueue.TryDequeue(out var action))
                action.Invoke();
        }
    }
}
