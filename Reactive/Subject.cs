using System;

namespace Poly.Reactive
{
    public interface ISubject<in TSource, out TResult> : IObserver<TSource>, IObservable<TResult>
    {
    }
    public interface ISubject<T> : ISubject<T, T>
    {
    }
    public class Subject<T> : ISubject<T>, IDisposable
    {
        private readonly object lockObj = new object();

        private bool isStopped;
        private bool isDisposed;
        //private byte flag;
        private Exception lastError;
        private IObserver<T>[] observers;
        private int observerCount;

        public bool HasObservers => !isStopped && !isDisposed && observerCount > 0;

        public void Dispose()
        {
            lock (lockObj)
            {
                isDisposed = true;
                observers = null;
                observerCount = 0;
            }
        }

        #region IObserver
        public void OnCompleted()
        {
            if (isDisposed) throw new ObjectDisposedException("");
            if (isStopped) return;
            lock (lockObj)
            {
                isStopped = true;
                for (int i = 0; i < observerCount; i++) observers[i].OnCompleted();
            }
        }
        public void OnError(Exception error)
        {
            if (error == null) throw new ArgumentNullException(nameof(error));
            if (isDisposed) throw new ObjectDisposedException("");
            if (isStopped) return;
            lock (lockObj)
            {
                isStopped = true;
                lastError = error;
                for (int i = 0; i < observerCount; i++) observers[i].OnError(error);
            }
        }
        public void OnNext(T value)
        {
            lock (lockObj)
                for (int i = 0; i < observerCount; i++) observers[i].OnNext(value);
        }
        #endregion

        #region IObservable
        public IDisposable Subscribe(IObserver<T> observer)
        {
            if (observer == null) throw new ArgumentNullException(nameof(observer));
            if (isDisposed) throw new ObjectDisposedException("");
            if (isStopped)
            {
                if (lastError != null) observer.OnError(lastError);
                else observer.OnCompleted();
                return EmptyDisposable.Instance;
            }
            lock (lockObj)
            {
                if (observerCount == 0) observers = new IObserver<T>[2];
                else if (observerCount == observers.Length) Array.Resize(ref observers, observerCount << 1);
                observers[observerCount++] = observer;
                return new Subscription(this, observer);
            }
        }
        private void Unsubscribe(IObserver<T> observer)
        {
            if (observerCount == 0) return;
            lock (lockObj)
            {
                for (int i = 0; i < observerCount; i++)
                {
                    if (observers[i] != observer) continue;
                    if(i != observerCount - 1)
                        observers[i] = observers[observerCount - 1];
                    observerCount--;
                    break;
                }
            }
        }
        #endregion

        private struct Subscription : IDisposable
        {
            //private readonly object _gate = new object();
            private readonly Subject<T> subject;
            private readonly IObserver<T> observer;

            public Subscription(Subject<T> subject, IObserver<T> observer)
            {
                this.subject = subject;
                this.observer = observer;
            }

            public void Dispose()
            {
                subject.Unsubscribe(observer);
            }
        }
    }

    internal class EmptyDisposable : IDisposable
    {
        public static readonly EmptyDisposable Instance = new EmptyDisposable();
        private EmptyDisposable() { }
        public void Dispose() { }
    }
}