# Bearz.Virtual.Environment

## Description

The default implementations of  `IVirtualEnvironment` that represents a contract for
a modernized version of `System.Environment`. Its useful for dependency injection, adding
extension methods to the `IVirtualEnvironment` interface, and for switching out implementations.

## Features 

- `DefaultVirtualEnvironment` - The default implementation of `IVirtualEnvironment` that
  wraps `System.Environment`.
- `InMemoryVirtualEnvironment` - An implementation of `IVirtualEnvironment` that stores
  the environment variables in memory. This is useful when taking a snapshot of the environment 
  and does not modify the actual environment variables.

## License 

MIT
