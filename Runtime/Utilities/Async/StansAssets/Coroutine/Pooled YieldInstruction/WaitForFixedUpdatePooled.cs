namespace Utilities.Async
{
    public sealed class WaitForFixedUpdatePooled : PooledYieldInstruction
    {
        public override bool keepWaiting => false;
    }
}
