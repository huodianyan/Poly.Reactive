using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Poly.Reactive
{
    public interface IReadOnlyReactiveProperty<T> : IObservable<T>, IDisposable
    {
        T Value { get; }
        bool HasValue { get; }
    }
    public interface IReactiveProperty<T> : IReadOnlyReactiveProperty<T>
    {
        new T Value { get; set; }
    }
    [Serializable]
    public class ReactiveProperty<T> : IReactiveProperty<T>
    {
        private static readonly IEqualityComparer<T> defaultEqualityComparer = EqualityComparer<T>.Default;

        [NonSerialized] private bool canPublishValueOnSubscribe = false;
        [NonSerialized] private bool isDisposed = false;
        private T value = default(T);
        [NonSerialized] private Subject<T> publisher = null;
        [NonSerialized] private IDisposable sourceConnection = null;
        [NonSerialized] private Exception lastException = null;

        protected virtual IEqualityComparer<T> EqualityComparer => defaultEqualityComparer;
        public bool HasValue => canPublishValueOnSubscribe;
        public T Value
        {
            get => value;
            set
            {
                if (!canPublishValueOnSubscribe)
                {
                    canPublishValueOnSubscribe = true;
                    SetValueAndForceNotify(value);
                    return;
                }
                if (!EqualityComparer.Equals(this.value, value))
                    SetValueAndForceNotify(value);
            }
        }

        public ReactiveProperty() : this(default(T))
        {
        }
        public ReactiveProperty(T initialValue)
        {
            SetValue(initialValue);
            canPublishValueOnSubscribe = true;
        }
        public ReactiveProperty(IObservable<T> source)
        {
            canPublishValueOnSubscribe = false;
            sourceConnection = source.Subscribe(new ReactivePropertyObserver(this));
        }
        public ReactiveProperty(IObservable<T> source, T initialValue)
        {
            canPublishValueOnSubscribe = false;
            Value = initialValue; // Value set canPublishValueOnSubscribe = true
            sourceConnection = source.Subscribe(new ReactivePropertyObserver(this));
        }
        public override string ToString() => (value == null) ? "(null)" : value.ToString();
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected virtual void SetValue(T value) => this.value = value;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValueAndForceNotify(T value)
        {
            SetValue(value);
            if (isDisposed) return;
            publisher?.OnNext(this.value);
        }

        #region IObservable
        public IDisposable Subscribe(IObserver<T> observer)
        {
            if (lastException != null)
            {
                observer.OnError(lastException);
                return EmptyDisposable.Instance;
            }
            if (isDisposed)
            {
                observer.OnCompleted();
                return EmptyDisposable.Instance;
            }
            if (publisher == null)
            {
                // Interlocked.CompareExchange is bit slower, guarantee thread safety is overkill.
                // System.Threading.Interlocked.CompareExchange(ref publisher, new Subject<T>(), null);
                publisher = new Subject<T>();
            }
            if (publisher != null)
            {
                var subscription = publisher.Subscribe(observer);
                if (canPublishValueOnSubscribe)
                    observer.OnNext(value); // raise latest value on subscribe
                return subscription;
            }
            else
            {
                observer.OnCompleted();
                return EmptyDisposable.Instance;
            }
        }
        #endregion

        #region IDisposable
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (isDisposed) return;
            isDisposed = true;
            if (sourceConnection != null)
            {
                sourceConnection.Dispose();
                sourceConnection = null;
            }
            if (publisher != null)
            {
                try
                {
                    publisher.OnCompleted();
                }
                finally
                {
                    publisher.Dispose();
                    publisher = null;
                }
            }
        }
        #endregion

        private class ReactivePropertyObserver : IObserver<T>
        {
            readonly ReactiveProperty<T> parent;
            int isStopped = 0;
            public ReactivePropertyObserver(ReactiveProperty<T> parent) => this.parent = parent;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public void OnNext(T value) => parent.Value = value;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public void OnError(Exception error)
            {
                if (Interlocked.Increment(ref isStopped) != 1) { return; }
                parent.lastException = error;
                parent.publisher?.OnError(error);
                parent.Dispose(); // complete subscription
            }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public void OnCompleted()
            {
                if (Interlocked.Increment(ref isStopped) != 1) { return; }
                // source was completed but can publish from .Value yet.
                parent.sourceConnection?.Dispose();
                parent.sourceConnection = null;
            }
        }
    }

    public static class IReactivePropertyExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IReactiveProperty<T> ToReactiveProperty<T>(this IObservable<T> source) => new ReactiveProperty<T>(source);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IReactiveProperty<T> ToReactiveProperty<T>(this IObservable<T> source, T initialValue) => new ReactiveProperty<T>(source, initialValue);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IReadOnlyReactiveProperty<T> ToReadOnlyReactiveProperty<T>(this IObservable<T> source) => new ReactiveProperty<T>(source);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IReadOnlyReactiveProperty<T> ToReadOnlyReactiveProperty<T>(this IObservable<T> source, T initialValue) => new ReactiveProperty<T>(source, initialValue);
    }
}
