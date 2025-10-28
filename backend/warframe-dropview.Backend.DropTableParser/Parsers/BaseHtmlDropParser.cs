using System.Text.RegularExpressions;

namespace warframe_dropview.Backend.DropTableParser.Parsers;

internal abstract partial class BaseHtmlDropParser<T> : IDropTableParser<T> where T : class
{
    [GeneratedRegex(@"^(.+?)\s*\(([\d.]+)%\)$")]
    protected static partial Regex DropInfoFormat();

    protected readonly List<HtmlNode> Drops;
    private bool disposedValue;

    public bool IsValid { get; protected set; }

    protected BaseHtmlDropParser(List<HtmlNode> drops)
    {
        this.Drops = drops;
        this.IsValid = this.ParseHeader(this.Drops[0]);
    }

    public ReadOnlyCollection<T>? Parse()
    {
        this.IsValid = this.ParseHeader(this.Drops[0]);
        if (!IsValid)
        {
            Console.WriteLine("Skipping invalid relic drops: {0}", this.Drops[0].InnerText);
            return default;
        }
        return ParseInternal().AsReadOnly();
    }

    protected abstract List<T> ParseInternal();

    protected abstract bool ParseHeader(HtmlNode node);

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            this.Drops.Clear();
            disposedValue=true;
        }
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
