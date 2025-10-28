using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using warframe_dropview.Backend.DropTableParser.Parsers;

namespace warframe_dropview.Backend.DropTableParser.Interfaces;

internal interface IDropParser
{
    ReadOnlyCollection<MissionDrop>? MissionDrops { get; }
    ReadOnlyCollection<RelicDrop>? RelicDrops { get; }
    ReadOnlyCollection<EnemyDrop>? EnemyDrops { get; }

    Task ParseAsync();
}
