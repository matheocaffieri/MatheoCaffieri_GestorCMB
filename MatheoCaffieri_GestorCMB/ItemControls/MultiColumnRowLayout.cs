using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace MatheoCaffieri_GestorCMB.ItemControls
{
    internal static class MultiColumnRowLayout
    {
        public class ColumnSpec
        {
            public Label Label;
            public int MinWidth;
            public int Weight;
            public ContentAlignment Align;
        }

        public static void Layout(
            IList<ColumnSpec> cols,
            IList<Label> separators,
            int totalWidth,
            int paddingX,
            int sepWidth,
            int rowY,
            int rowHeight)
        {
            if (cols == null || cols.Count == 0) return;
            int sepCount = separators?.Count ?? 0;

            int totalSepWidth = sepCount * sepWidth;
            int sumMin = 0;
            int sumWeights = 0;
            for (int i = 0; i < cols.Count; i++)
            {
                sumMin += cols[i].MinWidth;
                sumWeights += cols[i].Weight;
            }

            int available = totalWidth - paddingX * 2 - totalSepWidth;
            int extra = available - sumMin;

            int[] widths = new int[cols.Count];

            if (extra >= 0 && sumWeights > 0)
            {
                int distributed = 0;
                for (int i = 0; i < cols.Count - 1; i++)
                {
                    int add = extra * cols[i].Weight / sumWeights;
                    widths[i] = cols[i].MinWidth + add;
                    distributed += add;
                }
                widths[cols.Count - 1] = cols[cols.Count - 1].MinWidth + (extra - distributed);
            }
            else
            {
                // Ancho insuficiente para los mínimos: comprimimos proporcionalmente
                // así nada se va fuera del control y los separadores siguen visibles.
                int positive = available > 0 ? available : 0;
                int distributed = 0;
                if (sumMin > 0)
                {
                    for (int i = 0; i < cols.Count - 1; i++)
                    {
                        int w = positive * cols[i].MinWidth / sumMin;
                        widths[i] = w;
                        distributed += w;
                    }
                    widths[cols.Count - 1] = positive - distributed;
                }
            }

            int x = paddingX;
            for (int i = 0; i < cols.Count; i++)
            {
                var lbl = cols[i].Label;
                int w = widths[i] < 0 ? 0 : widths[i];
                lbl.Location = new Point(x, rowY);
                lbl.Size = new Size(w, rowHeight);
                lbl.TextAlign = cols[i].Align;
                x += w;

                if (i < sepCount)
                {
                    var sep = separators[i];
                    sep.Location = new Point(x, rowY);
                    sep.Size = new Size(sepWidth, rowHeight);
                    sep.TextAlign = ContentAlignment.MiddleCenter;
                    x += sepWidth;
                }
            }
        }
    }
}
