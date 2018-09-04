using Harmony;
using RimWorld;
using UnityEngine;
using Verse;

namespace PawnHUD.Patch
{
    [HarmonyPatch(typeof(ColonistBarColonistDrawer), nameof(ColonistBarColonistDrawer.DrawColonist))]
    internal static class RimWorld_ColonistBarColonistDrawer_DrawColonist
    {
        private static void Postfix(Rect rect, Pawn colonist)
        {
            if ((Find.Selector.NumSelected != 1) || colonist.Dead || !Find.Selector.SelectedObjects.Contains(colonist)) { return; }

            Drawer.Prepare(rect, colonist);
        }
    }
}
