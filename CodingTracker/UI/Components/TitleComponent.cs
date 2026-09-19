using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Text;

namespace CodingTracker.UI.Components
{
    public static class TitleComponent
    {
        public static void Show()
        {
            AnsiConsole.MarkupLine(@"[green]         ___                      _        ___          _ _            _____                _             [/]");
            AnsiConsole.MarkupLine(@"[green]        / __\___  _ __  ___  ___ | | ___  / __\___   __| (_)_ __   __ /__   \_ __ __ _  ___| | _____ _ __ [/]");
            AnsiConsole.MarkupLine(@"[green]       / /  / _ \| '_ \/ __|/ _ \| |/ _ \/ /  / _ \ / _` | | '_ \ / _` |/ /\/ '__/ _` |/ __| |/ / _ \ '__|[/]");
            AnsiConsole.MarkupLine(@"[green]      / /__| (_) | | | \__ \ (_) | |  __/ /__| (_) | (_| | | | | | (_| / /  | | | (_| | (__|   <  __/ |   [/]");
            AnsiConsole.MarkupLine(@"[green]      \____/\___/|_| |_|___/\___/|_|\___\____/\___/ \__,_|_|_| |_|\__, \/   |_|  \__,_|\___|_|\_\___|_|   [/]");
            AnsiConsole.MarkupLine(@"[green]                                                                  |___/                                   [/]");
        }
    }
}
