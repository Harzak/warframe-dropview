namespace warframe_dropview.Backend.DropTableParser.Interfaces;

internal interface IDropTableParser<T> : IDisposable where T : class 
{
    ReadOnlyCollection<T>? Parse();
    bool IsValid { get; }
}