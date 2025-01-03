using System.Reactive.Linq;

namespace VpSharp.Extensions;

/// <summary>
///     Reactive extensions for <see cref="IObservable{T}" />.
/// </summary>
public static class ObservableExtensions
{
    /// <summary>
    ///     Provides a way to subscribe to an observable using an asynchronous on-next handler.
    /// </summary>
    /// <param name="source">The source observable.</param>
    /// <param name="onNextAsync">The asynchronous on-next handler.</param>
    /// <typeparam name="T">The type of the observable.</typeparam>
    /// <returns>A disposable object used to unsubscribe from the observable.</returns>
    public static IDisposable SubscribeAsync<T>(this IObservable<T> source, Func<T, Task> onNextAsync)
    {
        return source.Select(arg => Observable.FromAsync(_ => onNextAsync(arg))).Concat().Subscribe();
    }
}
