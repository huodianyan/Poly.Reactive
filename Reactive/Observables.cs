using System;

namespace Poly.Reactive
{
    //internal static class Stubs
    //{
    //    public static readonly Action Nop = () => { };
    //    public static readonly Action<Exception> Throw = ex => throw ex;
    //}
    //internal static class Stubs<T>
    //{
    //    public static readonly Action<T> Ignore = (T t) => { };
    //}
    //public static class IObservableExtensions
    //{
    //    public static IDisposable Subscribe<T>(this IObservable<T> source, Action<T> onNext)
    //    {
    //        return source.Subscribe(CreateSubscribeObserver(onNext, Stubs.Throw, Stubs.Nop));
    //    }
    //    private static IObserver<T> CreateSubscribeObserver<T>(Action<T> onNext, Action<Exception> onError, Action onCompleted)
    //    {
    //        // need compare for avoid iOS AOT
    //        if (onNext == Stubs<T>.Ignore)
    //        { return new StubbedSubscribeObserver<T>(onError, onCompleted); }

    //        return new SubscribeObserver<T>(onNext, onError, onCompleted);
    //    }
    //}

    //[Serializable]
    //public struct Unit : IEquatable<Unit>
    //{
    //    private static readonly Unit @default = new Unit();
    //    public static Unit Default => @default;
    //    public static bool operator ==(Unit first, Unit second) => true;
    //    public static bool operator !=(Unit first, Unit second) => false;
    //    public bool Equals(Unit other) => true;
    //    public override bool Equals(object obj) => obj is Unit;
    //    public override int GetHashCode() => 0;
    //    public override string ToString() => "()";
    //}
}
