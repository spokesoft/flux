using Microsoft.Extensions.DependencyInjection;
using Spectre.Console.Cli;

namespace Flux.Console.Infrastructure;

/// <summary>
/// Implements <see cref="ITypeRegistrar"/> to integrate with Microsoft's dependency injection.
/// </summary>
/// <remarks>
/// Acts as an adapter between Spectre.Console's type registration system and .NET's dependency
/// injection services, enabling commands to resolve dependencies from the application's service
/// container.
/// </remarks>
public sealed class TypeRegistrar(IServiceCollection services) : ITypeRegistrar
{
    private readonly IServiceCollection _services = services ?? throw new ArgumentNullException(nameof(services));

    /// <summary>
    /// Builds a type resolver from the registered services.
    /// </summary>
    /// <returns>
    /// An <see cref="ITypeResolver"/> that resolves types from the built service provider.
    /// </returns>
    public ITypeResolver Build()
    {
        return new TypeResolver(_services.BuildServiceProvider());
    }

    /// <summary>
    /// Registers a service type with its corresponding implementation type.
    /// </summary>
    /// <param name="service">The service type to register.</param>
    /// <param name="implementation">The implementation type to instantiate.</param>
    public void Register(Type service, Type implementation)
    {
        _services.AddSingleton(service, implementation);
    }

    /// <summary>
    /// Registers a service type with a specific implementation instance.
    /// </summary>
    /// <param name="service">The service type to register.</param>
    /// <param name="implementation">The implementation instance to use.</param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="implementation"/> is null.
    /// </exception>
    public void RegisterInstance(Type service, object implementation)
    {
        _services.AddSingleton(service, implementation ?? throw new ArgumentNullException(nameof(implementation)));
    }

    /// <summary>
    /// Registers a service type with a factory function for lazy instantiation.
    /// </summary>
    /// <param name="service">The service type to register.</param>
    /// <param name="factory">The factory function that creates service instances.</param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="factory"/> is null.
    /// </exception>
    /// <remarks>
    /// The factory is invoked on first request and the resulting instance is cached for subsequent
    /// requests (singleton lifecycle).
    /// </remarks>
    public void RegisterLazy(Type service, Func<object> factory)
    {
        ArgumentNullException.ThrowIfNull(factory);
        _services.AddSingleton(service, _ => factory());
    }
}