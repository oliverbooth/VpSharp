namespace VpSharp.Internal;

internal static class ObjectReferenceCounter
{
    private static readonly ReaderWriterLockSlim Rwl = new();

    private static int s_reference = int.MinValue;

    internal static int GetNextReference()
    {
        int ret;
        Rwl.EnterWriteLock();
        if (s_reference < int.MaxValue)
        {
            ret = s_reference++;
        }
        else
        {
            ret = s_reference = int.MinValue;
        }

        Rwl.ExitWriteLock();
        return ret;
    }
}
