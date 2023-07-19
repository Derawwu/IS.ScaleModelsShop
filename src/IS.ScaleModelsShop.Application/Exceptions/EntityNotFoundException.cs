namespace IS.ScaleModelsShop.Application.Exceptions;

/// <summary>
/// An exception that represents that entity is not found.
/// </summary>
public class EntityNotFoundException : Exception
{
    /// <summary>
    /// Initializes new instance of <see cref="EntityNotFoundException"/>
    /// </summary>
    /// <param name="name">Name of the property that not found.</param>
    /// <param name="key">Value of the property that not found.</param>
    public EntityNotFoundException(string name, object key)
        : base($"{name} ({key}) is not found")
    {
    }
}