using System.Collections;
using UnityEngine;

namespace Assets.EverlastingDungeons.Scripts.Extensions
{
    public static class Colors
    {
        public static string ToHex(this Color color, bool includeAlpha = false)
        {
            return "#" + (includeAlpha
                ? ColorUtility.ToHtmlStringRGBA(color)
                : ColorUtility.ToHtmlStringRGB(color));
        }
    }
}