namespace warframe_dropview.Backend.DropTableParser.Interfaces;

/// <summary>
/// Defines a contract for parsing data into a collection of objects of type <typeparamref name="T"/>.
/// </summary>
internal interface IDropTableParser<T> : IDisposable where T : class 
{
    /// <summary>
    /// Gets a value indicating whether the current state is valid.
    /// </summary>
    bool IsValid { get; }

    /// <summary>
    /// Parses the input data and returns a read-only collection of parsed elements.
    /// </summary>
    ReadOnlyCollection<T>? Parse();
}