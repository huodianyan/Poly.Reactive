using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poly.Reactive
{
    //public interface ICancelable : IDisposable
    //{
    //    bool IsDisposed { get; }
    //}
    //public class SingleAssignmentDisposable : IDisposable, ICancelable
    //{
    //    private readonly object _gate = new object();
    //    private IDisposable _current;
    //    private bool _disposed;
    //    public bool IsDisposed
    //    {
    //        get { lock (_gate) { return _disposed; } }
    //    }
    //    public IDisposable Disposable
    //    {
    //        get => _current;
    //        set
    //        {
    //            var old = default(IDisposable);
    //            bool alreadyDisposed;
    //            lock (_gate)
    //            {
    //                alreadyDisposed = _disposed;
    //                old = _current;
    //                if (!alreadyDisposed)
    //                {
    //                    if (value == null) { return; }
    //                    _current = value;
    //                }
    //            }

    //            if (alreadyDisposed && value != null)
    //            {
    //                value.Dispose();
    //                return;
    //            }

    //            if (old != null)
    //            { throw new InvalidOperationException("Disposable is already set"); }
    //        }
    //    }
    //    public void Dispose()
    //    {
    //        IDisposable old = null;
    //        lock (_gate)
    //        {
    //            if (!_disposed)
    //            {
    //                _disposed = true;
    //                old = _current;
    //                _current = null;
    //            }
    //        }
    //        old?.Dispose();
    //    }
    //}

    //public sealed class CompositeDisposable : ICollection<IDisposable>, IDisposable
    //{
    //    private readonly object _gate = new object();
    //    private bool _disposed;
    //    private List<IDisposable> disposables;
    //    private int count;
    //    private const int SHRINK_THRESHOLD = 64;

    //    public int Count => count;
    //    public bool IsReadOnly => false;
    //    public bool IsDisposed => _disposed;

    //    public CompositeDisposable() => disposables = new List<IDisposable>();
    //    public CompositeDisposable(int capacity)
    //    {
    //        if (capacity < 0) throw new ArgumentOutOfRangeException(nameof(capacity));
    //        disposables = new List<IDisposable>(capacity);
    //    }
    //    public CompositeDisposable(params IDisposable[] disposables)
    //    {
    //        if (disposables == null) throw new ArgumentNullException(nameof(disposables));
    //        this.disposables = new List<IDisposable>(disposables);
    //        count = this.disposables.Count;
    //    }
    //    public CompositeDisposable(IEnumerable<IDisposable> disposables)
    //    {
    //        if (disposables == null) throw new ArgumentNullException(nameof(disposables));
    //        this.disposables = new List<IDisposable>(disposables);
    //        count = this.disposables.Count;
    //    }
    //    public void Add(IDisposable item)
    //    {
    //        if (item == null) throw new ArgumentNullException(nameof(item)); 

    //        bool shouldDispose;
    //        lock (_gate)
    //        {
    //            shouldDispose = _disposed;
    //            if (!_disposed)
    //            {
    //                disposables.Add(item);
    //                count++;
    //            }
    //        }
    //        if (shouldDispose) item.Dispose();
    //    }
    //    public bool Remove(IDisposable item)
    //    {
    //        if (item == null) throw new ArgumentNullException(nameof(item));
    //        var shouldDispose = false;
    //        lock (_gate)
    //        {
    //            if (!_disposed)
    //            {
    //                var i = disposables.IndexOf(item);
    //                if (i >= 0)
    //                {
    //                    shouldDispose = true;
    //                    disposables[i] = null;
    //                    count--;

    //                    if (disposables.Capacity > SHRINK_THRESHOLD && count < disposables.Capacity / 2)
    //                    {
    //                        var old = disposables;
    //                        disposables = new List<IDisposable>(disposables.Capacity / 2);

    //                        foreach (var d in old)
    //                        {
    //                            if (d != null)
    //                            { disposables.Add(d); }

    //                        }
    //                    }
    //                }
    //            }
    //        }
    //        if (shouldDispose) item.Dispose();
    //        return shouldDispose;
    //    }
    //    public void Dispose()
    //    {
    //        var currentDisposables = default(IDisposable[]);
    //        lock (_gate)
    //        {
    //            if (!_disposed)
    //            {
    //                _disposed = true;
    //                currentDisposables = disposables.ToArray();
    //                disposables.Clear();
    //                count = 0;
    //            }
    //        }

    //        if (currentDisposables != null)
    //        {
    //            foreach (var d in currentDisposables)
    //            {
    //                if (d != null)
    //                { d.Dispose(); }
    //            }
    //        }
    //    }
    //    public void Clear()
    //    {
    //        var currentDisposables = default(IDisposable[]);
    //        lock (_gate)
    //        {
    //            currentDisposables = disposables.ToArray();
    //            disposables.Clear();
    //            count = 0;
    //        }

    //        foreach (var d in currentDisposables)
    //        { d?.Dispose(); }
    //    }
    //    public bool Contains(IDisposable item)
    //    {
    //        if (item == null) throw new ArgumentNullException(nameof(item));
    //        lock (_gate)
    //        {
    //            return disposables.Contains(item);
    //        }
    //    }
    //    public void CopyTo(IDisposable[] array, int arrayIndex)
    //    {
    //        if (array == null)
    //        { throw new ArgumentNullException(nameof(array)); }

    //        if (arrayIndex < 0 || arrayIndex >= array.Length)
    //        { throw new ArgumentOutOfRangeException(nameof(arrayIndex)); }

    //        lock (_gate)
    //        {
    //            var disArray = new List<IDisposable>();
    //            foreach (var item in disposables)
    //            {
    //                if (item != null)
    //                { disArray.Add(item); }
    //            }

    //            Array.Copy(disArray.ToArray(), 0, array, arrayIndex, array.Length - arrayIndex);
    //        }
    //    }
    //    public IEnumerator<IDisposable> GetEnumerator()
    //    {
    //        var res = new List<IDisposable>();

    //        lock (_gate)
    //        {
    //            foreach (var d in disposables)
    //            {
    //                if (d != null)
    //                { res.Add(d); }
    //            }
    //        }

    //        return res.GetEnumerator();
    //    }
    //    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    //    {
    //        return GetEnumerator();
    //    }
    //}
}
