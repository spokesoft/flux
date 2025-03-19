using Microsoft.Extensions.DependencyInjection;
using Spectre.Console.Cli;

namespace Flux.Console.Infrastructure;

/// <summary>
/// Implements <see cref="ITypeResolver"/> to resolve types from a <see cref="ServiceProvider"/>.
/// </summary>
/// <remarks>
/// Works in conjunction with <see cref="TypeRegistrar"/> to provide dependency injection capabilities
/// for Spectre.Console commands. This class manages the lifecycle of the underlying
/// <see cref="ServiceProvider"/>.
/// </remarks>
public sealed class TypeResolver(ServiceProvider provider) : ITypeResolver, IDisposable
{
    private readonly ServiceProvider _provider = provider ?? throw new ArgumentNullException(nameof(provider));

    /// <summary>
    /// Resolves an instance of the specified type from the service provider.
    /// </summary>
    /// <param name="type">The type to resolve.</param>
    /// <returns>
    /// The resolved instance, or <c>null</c> if either <paramref name="type"/> is <c>null</c> or
    /// the type cannot be resolved.
    /// </returns>
    public object? Resolve(Type? type)
    {
        if (type is null)
        {
            return null;
        }

        return _provider.GetService(type);
    }

    /// <summary>
    /// Disposes the underlying <see cref="ServiceProvider"/>.
    /// </summary>
    /// <remarks>
    /// This releases any resources held by the service provider and the services it has created.
    /// </remarks>
    public void Dispose()
    {
        _provider.Dispose();
    }
}