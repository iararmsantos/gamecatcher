using System;
using System.Collections.Generic;
using System.Linq;

public static class GemUtils
{
    // Spawning a Random Gem (non-special)
    public static Gem.GemType GetRandomSpawnableGem()
    {
        var spawnableTypes = Enum.GetValues(typeof(Gem.GemType))
                                 .Cast<Gem.GemType>()
                                 .Where(t => !GemMetadata.IsSpecial(t));

        return PickWeighted(spawnableTypes, 
                            t => GemMetadata.Info[t].Rarity, 
                            1)
              .First();
    }

    // Common gems like Red have a high chance.
    // Special ones like Life/Super appear rarely but not never.
    public static Gem.GemType GetRandomAnyGem()
    {
        var allGemTypes = Enum.GetValues(typeof(Gem.GemType))
                              .Cast<Gem.GemType>();

        return PickWeighted(allGemTypes, 
                            t => GemMetadata.Info[t].Rarity, 
                            1)
              .First();
    }

    // Spawning Multiple at Once
    private static IEnumerable<T> PickWeighted<T>(IEnumerable<T> items, Func<T, int> weightSelector, int count)
    {
        var weightedItems = items.Select(item => new { item, weight = weightSelector(item) }).ToList();
        var totalWeight = weightedItems.Sum(x => x.weight);
        var random = new Random();
        var chosenItems = new List<T>();

        for (int i = 0; i < count; i++)
        {
            var randWeight = random.Next(0, totalWeight);
            var item = weightedItems.First(x => (randWeight -= x.weight) < 0).item;
            chosenItems.Add(item);
        }

        return chosenItems;
    }
}
