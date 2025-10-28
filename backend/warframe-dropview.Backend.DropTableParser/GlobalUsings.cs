
//.Net
global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.Extensions.Hosting;
global using System.Globalization;
global using System.Collections.ObjectModel;

//App
global using warframe_dropview.Backend.Models;
global using warframe_dropview.Backend.Models.Enums;
global using warframe_dropview.Backend.Plugin.MongoDB.Registrations;
global using warframe_dropview.Backend.DropTableParser.Interfaces;
global using warframe_dropview.Backend.DropTableParser.Extensions;
global using warframe_dropview.Backend.DropTableParser.Parsers.MissionDrops;
global using warframe_dropview.Backend.DropTableParser.Parsers.RelicDrops;
global using warframe_dropview.Backend.DropTableParser.Parsers.EnemyDrops;
global using warframe_dropview.Backend.DropTableParser.Parsers;

//NuGet
global using HtmlAgilityPack;
global using MongoDB.Driver;
