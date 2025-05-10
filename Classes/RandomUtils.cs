using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public static class RandomUtils
{
    private static Random rng = new();

    public static List<T> PickWeighted<T>(IEnumerable<T> items, Func<T, int> weightSelector, int count)
    {
        var weightedList = items.SelectMany(item =>
            Enumerable.Repeat(item, weightSelector(item))
        ).ToList();

        return weightedList.OrderBy(_ => rng.Next()).Distinct().Take(count).ToList();
    }
}
