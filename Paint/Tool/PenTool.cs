using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Paint2.Paint
{
    public class PenTool : Tool
    {
        public override void MouseDown(Point point)
        {
            ToolsBar.Figures.Add(new Pencil(point));
        }

        public override void MouseMove(Point point)
        {
            ToolsBar.Figures[ToolsBar.Figures.Count - 1].AddCord(point);
        }
    }
}