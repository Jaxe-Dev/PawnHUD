using Harmony;
using RimWorld;

namespace PawnHUD.Patch
{
    [HarmonyPatch(typeof(ColonistBar), nameof(ColonistBar.ColonistBarOnGUI))]
    internal static class RimWorld_ColonistBar_ColonistBarOnGUI
    {
        private static void Prefix() => Drawer.Clear();
        private static void Postfix() => Drawer.Draw();
    }
}
