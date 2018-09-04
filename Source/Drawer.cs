using UnityEngine;
using Verse;

namespace PawnHUD
{
    internal static class Drawer
    {
        private static Hud _selected;

        public static bool ShowHud { get; set; } = true;

        public static void Clear() => _selected = null;
        public static void Prepare(Rect rect, Pawn pawn) => _selected = new Hud(rect, pawn);
        public static void Draw()
        {
            if (ShowHud) { _selected?.Draw(); }
        }
    }
}
