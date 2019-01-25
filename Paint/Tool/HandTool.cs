using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Paint2.Paint
{
    class HandTool : Tool
    {

        public override void MouseDown(Point point)
        {
            ToolsBar.Figures.Add(new HandLine(point));
            ToolsBar.HandScrollX = point.X;
            ToolsBar.HandScrollY = point.Y;
        }

        public override void MouseMove(Point point)
        {
            ToolsBar.HandScrollX += ToolsBar.Figures[ToolsBar.Figures.Count - 1].Coordinates[0].X - ToolsBar.Figures[ToolsBar.Figures.Count - 1].Coordinates[1].X;
            ToolsBar.HandScrollY += ToolsBar.Figures[ToolsBar.Figures.Count - 1].Coordinates[0].Y - ToolsBar.Figures[ToolsBar.Figures.Count - 1].Coordinates[1].Y;
            ToolsBar.Figures[ToolsBar.Figures.Count - 1].AddCord(point);
        }

        public override void MouseUp(Point point)
        {
            ToolsBar.Figures.Remove(ToolsBar.Figures[ToolsBar.Figures.Count - 1]);
        }
    }
}   