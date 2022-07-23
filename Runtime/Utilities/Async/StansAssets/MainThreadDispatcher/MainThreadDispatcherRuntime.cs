namespace Utilities.Async
{
    internal class MainThreadDispatcherRuntime : MainThreadDispatcherBase
    {
        public override void Init()
        {
            MonoBehaviourCallback.Instantiate();
            MonoBehaviourCallback.OnUpdate += Update;
        }
    }
}
