using System.Text.RegularExpressions;

namespace warframe_dropview.Backend.DropTableParser.Parsers;

internal abstract partial class BaseHtmlDropParser<T> : IDropTableParser<T> where T : class
{
    [GeneratedRegex(@"^(.+?)\s*\(([\d.]+)%\)$")]
    protected static partial Regex DropInfoFormat();

    protected readonly ILogger Logger;
    protected readonly List<HtmlNode> Drops;

    public bool IsValid { get; protected set; }

    protected BaseHtmlDropParser(List<HtmlNode> drops, ILogger logger)
    {
        this.Drops = drops;
        this.Logger = logger;
        this.IsValid = this.ParseHeader(this.Drops[0]);
    }

    public ReadOnlyCollection<T>? Parse()
    {
        this.IsValid = this.ParseHeader(this.Drops[0]);
        if (!IsValid)
        {
            this.Logger.LogSkippingInvalidDrops(this.Drops[0].InnerText);
            return default;
        }
        return ParseInternal().AsReadOnly();
    }

    protected abstract List<T> ParseInternal();

    protected abstract bool ParseHeader(HtmlNode node);


    private bool _disposed;
    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            this.Drops.Clear();
            _disposed=true;
        }
    }
    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
