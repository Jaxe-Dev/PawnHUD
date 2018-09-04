using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;
using Verse;

namespace PawnHUD
{
    [StaticConstructorOnStartup]
    internal class Hud
    {
        private const float BarsPadding = 1f;

        private const float SkillsWindowOffset = 2f;
        private const float SkillsWindowPadding = 4f;
        private const float SkillsWindowWidth = 160f;

        private const int FontSize = 8;
        private const float MarkerOffset = 6f;

        private static readonly Texture2D BarInstantMarkerTex = ContentFinder<Texture2D>.Get("UI/Misc/BarInstantMarker");

        private static readonly GUIStyle FontStyle = new GUIStyle(Text.fontStyles[(int) GameFont.Small]) { fontSize = FontSize };
        private static readonly float FontHeight = FontStyle.CalcHeight(new GUIContent("W"), 999f);

        private static readonly Color BarBackgroundColor = new Color(0.2f, 0.2f, 0.2f);
        private static readonly Color BarHealthColor = new Color(0.6f, 0f, 0.1f);
        private static readonly Color BarMoodColor = new Color(0f, 0.6f, 0.6f);
        private static readonly Color BarRestColor = new Color(0.1f, 0f, 0.6f);
        private static readonly Color BarFoodColor = new Color(0.1f, 0.6f, 0f);
        private static readonly Color BarJoyColor = new Color(0.6f, 0.6f, 0f);

        private static readonly Color SkillDisabledColor = new Color(0.5f, 0.5f, 0.5f);
        private static readonly Dictionary<Passion, Color> SkillPassionColor = new Dictionary<Passion, Color>
                                                                               {
                                                                                           { Passion.None, new Color(0.9f, 0.9f, 0.9f) },
                                                                                           { Passion.Minor, new Color(1.0f, 0.9f, 0.7f) },
                                                                                           { Passion.Major, new Color(1.0f, 0.7f, 0.4f) }
                                                                               };

        public Rect Rect { get; }
        public Pawn Pawn { get; }

        public Hud(Rect rect, Pawn pawn)
        {
            Rect = rect;
            Pawn = pawn;
        }

        private static IEnumerable<SkillDef> GetShownSkills() => new List<SkillDef>
                                                                 {
                                                                             SkillDefOf.Shooting,
                                                                             SkillDefOf.Melee,
                                                                             SkillDefOf.Construction,
                                                                             SkillDefOf.Mining,
                                                                             SkillDefOf.Cooking,
                                                                             SkillDefOf.Plants,
                                                                             SkillDefOf.Animals,
                                                                             SkillDefOf.Crafting,
                                                                             SkillDefOf.Artistic,
                                                                             SkillDefOf.Medicine,
                                                                             SkillDefOf.Social,
                                                                             SkillDefOf.Intellectual
                                                                 };

        public void Draw()
        {
            var curY = DrawMarker();
            curY = DrawNeeds(curY);
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) { DrawSkills(curY); }
        }

        private float DrawMarker()
        {
            var rect = new Rect((Rect.x + Rect.width.Half()) - ((float) BarInstantMarkerTex.width).Half(), Rect.yMax + MarkerOffset, BarInstantMarkerTex.width, BarInstantMarkerTex.height);

            GUI.DrawTexture(rect, BarInstantMarkerTex);
            return rect.yMax;
        }

        private static void DrawBar(Rect rect, float percentage, Color color)
        {
            Widgets.DrawBoxSolid(rect, BarBackgroundColor);
            Widgets.DrawBoxSolid(rect.LeftPart(percentage), color);
        }

        private float DrawNeeds(float y)
        {
            var rect = new Rect((Rect.x + Rect.width.Half()) - SkillsWindowWidth.Half(), y, SkillsWindowWidth, 20f);

            Widgets.DrawShadowAround(rect);
            Widgets.DrawBoxSolid(rect, Color.black);

            var barGrid = rect.ContractedBy(BarsPadding).GetGrid(BarsPadding, 3, 2);

            var healthBar = barGrid[0];
            healthBar.xMax = barGrid[1].xMax;

            var moodBar = barGrid[2];
            var restBar = barGrid[3];
            var foodBar = barGrid[4];
            var joyBar = barGrid[5];

            DrawBar(healthBar, Pawn.health.summaryHealth.SummaryHealthPercent, BarHealthColor);
            DrawBar(moodBar, Pawn.needs.mood.CurLevelPercentage, BarMoodColor);
            DrawBar(foodBar, Pawn.needs.food.CurLevelPercentage, BarFoodColor);
            DrawBar(restBar, Pawn.needs.rest.CurLevelPercentage, BarRestColor);
            DrawBar(joyBar, Pawn.needs.joy.CurLevelPercentage, BarJoyColor);

            return rect.yMax;
        }

        private void DrawSkills(float y)
        {
            var skills = GetShownSkills();
            var rect = new Rect((Rect.x + Rect.width.Half()) - SkillsWindowWidth.Half(), y + SkillsWindowOffset, SkillsWindowWidth, (float) ((FontHeight * Math.Ceiling(skills.Count() / 2d)) + (SkillsWindowPadding * 2)));

            Widgets.DrawShadowAround(rect);
            Widgets.DrawWindowBackground(rect);

            var pos = new Vector2(rect.x + SkillsWindowPadding, rect.y + SkillsWindowPadding);
            var lineFeed = false;

            foreach (var skill in skills)
            {
                var record = Pawn.skills.GetSkill(skill);

                pos = DrawLabel(pos, (SkillsWindowWidth - (SkillsWindowPadding * 2)).Half(), $"{skill.label.ToUpper()} {GetSkillValue(record)}", lineFeed ? TextAnchor.MiddleRight : TextAnchor.MiddleLeft, GetSkillColor(record), lineFeed);
                lineFeed = !lineFeed;
            }
        }

        private static Vector2 DrawLabel(Vector2 pos, float width, string text, TextAnchor anchor, Color color, bool lineFeed)
        {
            var rect = new Rect(pos, new Vector2(width, FontHeight));

            var previousColor = GUI.color;
            GUI.color = color;

            GUI.Label(rect, text, new GUIStyle(FontStyle) { alignment = anchor });

            GUI.color = previousColor;

            return lineFeed ? new Vector2(pos.x - width, pos.y + FontHeight) : new Vector2(pos.x + width, pos.y);
        }
        private static string GetSkillValue(SkillRecord record) => record.TotallyDisabled ? "-" : record.levelInt.ToString();
        private static Color GetSkillColor(SkillRecord record) => record.TotallyDisabled ? SkillDisabledColor : SkillPassionColor[record.passion];
    }
}
