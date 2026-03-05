using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using warframe_dropview.Backend.DropTableParser.Parsers;

namespace warframe_dropview.Backend.DropTableParser.Interfaces;

/// <summary>
/// Defines the contract for parsing and retrieving drop data, including mission drops, relic drops, and enemy drops.
/// </summary>
internal interface IDropParser
{
    /// <summary>
    /// Gets the collection of all the missions drops in the drop table.
    /// </summary>
    ReadOnlyCollection<MissionDrop>? MissionDrops { get; }

    /// <summary>
    /// Gets the collection of all relics drops in the drop table.
    /// </summary>
    ReadOnlyCollection<RelicDrop>? RelicDrops { get; }

    /// <summary>
    /// Gets the collection of all enemy drops in the drop table.
    /// </summary>
    ReadOnlyCollection<EnemyDrop>? EnemyDrops { get; }


    /// <summary>
    /// Asynchronously parses drop data from an HTML document and populates the collections <see cref="MissionDrops"/>, <see cref="RelicDrops"/>, and <see cref="EnemyDrops"/>.
    /// </summary>
    Task<OperationResult> ParseAsync();

    /// <summary>
    /// Parses drop data from an HTML document and populates the collections <see cref="MissionDrops"/>, <see cref="RelicDrops"/>, and <see cref="EnemyDrops"/>.
    /// </summary>
    OperationResult Parse();
}
