using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PawnHUD
{
    internal static class Extensions
    {
        public static string Italic(this string self) => "<i>" + self + "</i>";
        public static string Bold(this string self) => "<b>" + self + "</b>";

        public static int LastIndex(this IList self) => self.Count - 1;

        public static Rect AdjustedBy(this Rect self, float x, float y, float width, float height) => new Rect(self.x + x, self.y + y, self.width + width, self.height + height);

        public static float Half(this float self) => self / 2f;

        public static Rect[] GetGrid(this Rect self, float spacing, int rows, int columns)
        {
            var grids = new List<Rect>();

            for (var row = 0; row < rows; row++)
            {
                var vGrid = self.GetVGrid(spacing, Enumerable.Repeat(0f, rows).ToArray());
                grids.AddRange(vGrid.SelectMany(rect => rect.GetHGrid(spacing, Enumerable.Repeat(0f, columns).ToArray())));
            }

            return grids.ToArray();
        }

        public static Rect[] GetHGrid(this Rect self, float spacing, params float[] widths)
        {
            var unfixedCount = 0;
            var currentX = self.x;
            var fixedWidths = 0f;
            var rects = new Rect[widths.Length];

            for (var index = 0; index < widths.Length; index++)
            {
                var width = widths[index];
                if (width > 0) { fixedWidths += width; }
                else { unfixedCount++; }

                if (index != widths.LastIndex()) { fixedWidths += spacing; }
            }

            var unfixedWidth = unfixedCount > 0 ? (self.width - fixedWidths) / unfixedCount : 0;

            for (var index = 0; index < widths.Length; index++)
            {
                var width = widths[index];
                float newWidth;
                if (width > 0)
                {
                    newWidth = width;
                    rects[index] = new Rect(currentX, self.y, newWidth, self.height);
                }
                else
                {
                    newWidth = unfixedWidth;
                    rects[index] = new Rect(currentX, self.y, newWidth, self.height);
                }
                currentX += newWidth + spacing;
            }

            return rects;
        }

        public static Rect[] GetVGrid(this Rect self, float spacing, params float[] heights)
        {
            var unfixedCount = 0;
            var currentY = self.y;
            var fixedHeights = 0f;
            var rects = new Rect[heights.Length];

            for (var index = 0; index < heights.Length; index++)
            {
                var height = heights[index];
                if (height > 0) { fixedHeights += height; }
                else { unfixedCount++; }

                if (index != heights.LastIndex()) { fixedHeights += spacing; }
            }

            var unfixedWidth = unfixedCount > 0 ? (self.height - fixedHeights) / unfixedCount : 0;

            for (var index = 0; index < heights.Length; index++)
            {
                var height = heights[index];
                float newHeight;
                if (height > 0)
                {
                    newHeight = height;
                    rects[index] = new Rect(self.x, currentY, self.width, newHeight);
                }
                else
                {
                    newHeight = unfixedWidth;
                    rects[index] = new Rect(self.x, currentY, self.width, newHeight);
                }
                currentY += newHeight + spacing;
            }

            return rects;
        }
    }
}
