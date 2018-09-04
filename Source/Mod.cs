using Harmony;
using UnityEngine;
using Verse;

namespace PawnHUD
{
    [StaticConstructorOnStartup]
    internal static class Mod
    {
        public static readonly Texture2D ToggleHudIcon = ContentFinder<Texture2D>.Get("UI/ToggleHUD");
        static Mod() => HarmonyInstance.Create("PawnHUD").PatchAll();
    }
}
