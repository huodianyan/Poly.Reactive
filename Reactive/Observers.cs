using System;
using System.Threading;

namespace Poly.Reactive
{
    //public class DisposedObserver<T> : IObserver<T>
    //{
    //    public static readonly DisposedObserver<T> Instance = new DisposedObserver<T>();
    //    private DisposedObserver() { }
    //    public void OnCompleted() => throw new ObjectDisposedException("");
    //    public void OnError(Exception error) => throw new ObjectDisposedException("");
    //    public void OnNext(T value) => throw new ObjectDisposedException("");
    //}
    //public class EmptyObserver<T> : IObserver<T>
    //{
    //    public static readonly EmptyObserver<T> Instance = new EmptyObserver<T>();
    //    private EmptyObserver() { }
    //    public void OnCompleted() { }
    //    public void OnError(Exception error) { }
    //    public void OnNext(T value) { }
    //}
    //public class ListObserver<T> : IObserver<T>
    //{
    //    private readonly ImmutableList<IObserver<T>> observers;
    //    public ListObserver(ImmutableList<IObserver<T>> observers) => this.observers = observers;
    //    public void OnCompleted()
    //    {
    //        var targetObservers = observers.Data;
    //        for (var i = 0; i < targetObservers.Length; i++)
    //            targetObservers[i].OnCompleted();
    //    }
    //    public void OnError(Exception error)
    //    {
    //        var targetObservers = observers.Data;
    //        for (var i = 0; i < targetObservers.Length; i++)
    //            targetObservers[i].OnError(error);
    //    }
    //    public void OnNext(T value)
    //    {
    //        var targetObservers = observers.Data;
    //        for (var i = 0; i < targetObservers.Length; i++)
    //            targetObservers[i].OnNext(value);
    //    }
    //    internal IObserver<T> Add(IObserver<T> observer) => new ListObserver<T>(observers.Add(observer));
    //    internal IObserver<T> Remove(IObserver<T> observer)
    //    {
    //        var i = Array.IndexOf(observers.Data, observer);
    //        if (i < 0) return this;
    //        if (observers.Data.Length == 2) return observers.Data[1 - i];
    //        return new ListObserver<T>(observers.Remove(observer));
    //    }
    //}
    //class StubbedSubscribeObserver<T> : IObserver<T>
    //{
    //    private readonly Action<Exception> onError;
    //    private readonly Action onCompleted;
    //    private int isStopped = 0;
    //    public StubbedSubscribeObserver(Action<Exception> onError, Action onCompleted)
    //    {
    //        this.onError = onError;
    //        this.onCompleted = onCompleted;
    //    }
    //    public void OnNext(T value) { }
    //    public void OnError(Exception error)
    //    {
    //        if (Interlocked.Increment(ref isStopped) == 1)
    //        { onError(error); }
    //    }
    //    public void OnCompleted()
    //    {
    //        if (Interlocked.Increment(ref isStopped) == 1)
    //        { onCompleted(); }
    //    }
    //}
    //public class SubscribeObserver<T> : IObserver<T>
    //{
    //    private readonly Action<T> onNext;
    //    private readonly Action<Exception> onError;
    //    private readonly Action onCompleted;
    //    private int isStopped = 0;
    //    public SubscribeObserver(Action<T> onNext, Action<Exception> onError, Action onCompleted)
    //    {
    //        this.onNext = onNext;
    //        this.onError = onError;
    //        this.onCompleted = onCompleted;
    //    }
    //    public void OnNext(T value)
    //    {
    //        if (isStopped == 0) onNext(value);
    //    }
    //    public void OnError(Exception error)
    //    {
    //        if (Interlocked.Increment(ref isStopped) == 1)
    //        { onError(error); }
    //    }
    //    public void OnCompleted()
    //    {
    //        if (Interlocked.Increment(ref isStopped) == 1)
    //        { onCompleted(); }
    //    }
    //}
}