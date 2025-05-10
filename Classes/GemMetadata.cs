using System.Collections.Generic;

public static class GemMetadata
{
    public static readonly Dictionary<Gem.GemType, GemTypeInfo> Info = new()
    {
        { Gem.GemType.Red, new() { IsSpecial = false, Rarity = 5, DisplayName = "Red", InitialSpeed = 90.0f} },
        { Gem.GemType.Blue, new() { IsSpecial = false, Rarity = 4, DisplayName = "Blue", InitialSpeed = 90.0f } },
        { Gem.GemType.Green, new() { IsSpecial = false, Rarity = 3, DisplayName = "Green", InitialSpeed = 90.0f } },
        { Gem.GemType.Yellow, new() { IsSpecial = false, Rarity = 5, DisplayName = "Yellow", InitialSpeed = 90.0f } },
        { Gem.GemType.Orange, new() { IsSpecial = false, Rarity = 4, DisplayName = "Orange", InitialSpeed = 90.0f } },
        { Gem.GemType.Black, new() { IsSpecial = false, Rarity = 3, DisplayName = "Black", InitialSpeed = 90.0f } },
        { Gem.GemType.Super, new() { IsSpecial = true, Rarity = 1, DisplayName = "Super", InitialSpeed = 90.0f } },
        { Gem.GemType.Life, new() { IsSpecial = true, Rarity = 1, DisplayName = "Life", InitialSpeed = 90.0f } },
        { Gem.GemType.Slow, new() { IsSpecial = true, Rarity = 1, DisplayName = "Slow", InitialSpeed = 90.0f } }
    };

    public static bool IsSpecial(Gem.GemType type) => Info.TryGetValue(type, out var meta) && meta.IsSpecial;
}
