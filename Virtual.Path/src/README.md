# Bearz.Virtual.Path

## Description

The default implementations of  `IVirtualPath` that represents a contract for
System.IO.Path. Its useful for dependency injection, adding
extension methods to the `IVirtualPath` interface, and for switching out implementations.

The methods adapt to the version of .NET that is used. For example, `Join` is available
.Net Core but not available on netstandard and the full framework version.

## Features

- `DefaultVirtualPath` - The default implementation of `IVirtualPath` that
  wraps `System.IO.Path`.


## License 

MIT License
