using Harmony;
using RimWorld;
using Verse;

namespace PawnHUD.Patch
{
    [HarmonyPatch(typeof(PlaySettings), nameof(PlaySettings.DoPlaySettingsGlobalControls))]
    internal static class RimWorld_PlaySettings_DoPlaySettingsGlobalControls
    {
        private static void Postfix(WidgetRow row, bool worldView)
        {
            if (worldView) { return; }

            if (row == null) { return; }

            var showHud = Drawer.ShowHud;
            row.ToggleableIcon(ref showHud, Mod.ToggleHudIcon, "PawnHUD.ToggleHUD".Translate(), SoundDefOf.Mouseover_ButtonToggle);
            Drawer.ShowHud = showHud;
        }
    }
}
