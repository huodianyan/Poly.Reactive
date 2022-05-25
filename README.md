# Poly.Reactive
a minimal reimplementation of the .NET Reactive Extensions for Unity and any C# (or .Net) project.

## Features
- Zero dependencies
- Minimal core
- Lightweight and fast
- Subject implementation
- Linq operator implementation (*)
- Adapted to all C# game engine

## Installation

## Overview

```csharp

var source = new Subject<int>();
var subscription1 = source.Subscribe(observer1);
source.OnNext(11);
source.OnError(new Exception("Error"));
source.OnComplete();

subscription1.Dispose();
source.Dispose();

```

## License
The software is released under the terms of the [MIT license](./LICENSE.md).

## FAQ

## References

### Documents
- [ReactiveX](https://reactivex.io/)
- [Why Rx?](http://introtorx.com/Content/v1.0.10621.0/01_WhyRx.html)

### Projects
- [neuecc/UniRx](https://github.com/neuecc/UniRx)
- [dotnet/reactive](https://github.com/dotnet/reactive)
- [EcsRx/SystemsRx](https://github.com/EcsRx/SystemsRx/tree/main/src/SystemsRx.MicroRx)

### Benchmarks
