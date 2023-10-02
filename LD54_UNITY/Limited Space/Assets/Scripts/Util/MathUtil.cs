using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MathUtil
{
    public static T GetRandomValue<T>(this List<T> list)
    {
        if (list == null || list.Count == 0)
        {
            return default(T);
        }

        int randomIndex = Random.Range(0, list.Count);
        return list[randomIndex];
    }

    public static float GetRandomValueInRange(this Vector2 range)
    {
        return Random.Range(range.x, range.y);
    }
}
