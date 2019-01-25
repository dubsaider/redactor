using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using Paint2.Paint;

namespace Paint2.Paint
{
    class ZoomTool : Tool
    {
        public override void MouseDown(Point point)
        {
            ToolsBar.Figures.Add(new ZoomRect(point));
        }

        public override void MouseMove(Point point)
        {
            ToolsBar.Figures[ToolsBar.Figures.Count - 1].AddCord(point);
        }

        public override void MouseUp(Point point)
        {
            if (Point.Subtract(ToolsBar.Figures[ToolsBar.Figures.Count - 1].Coordinates[0], ToolsBar.Figures[ToolsBar.Figures.Count - 1].Coordinates[1]).Length > 50)
            {
                ToolsBar.ScaleRateX = ToolsBar.CanvasWidth / Math.Abs(ToolsBar.Figures[ToolsBar.Figures.Count - 1].Coordinates[0].X - ToolsBar.Figures[ToolsBar.Figures.Count - 1].Coordinates[1].X);

                if (ToolsBar.Figures[ToolsBar.Figures.Count - 1].Coordinates[1].Y > ToolsBar.Figures[ToolsBar.Figures.Count - 1].Coordinates[0].Y)
                {
                    ToolsBar.ScaleRateY = ToolsBar.CanvasHeigth / (ToolsBar.Figures[ToolsBar.Figures.Count - 1].Coordinates[1].Y - ToolsBar.Figures[ToolsBar.Figures.Count - 1].Coordinates[0].Y);
                }
                else
                {
                    ToolsBar.ScaleRateY = ToolsBar.CanvasHeigth / (ToolsBar.Figures[ToolsBar.Figures.Count - 1].Coordinates[0].Y - ToolsBar.Figures[ToolsBar.Figures.Count - 1].Coordinates[1].Y);
                }

                if (ToolsBar.ScaleRateX > ToolsBar.ScaleRateY)
                {
                    ToolsBar.ScaleRateY = ToolsBar.ScaleRateX;
                }
                else
                {
                    ToolsBar.ScaleRateX = ToolsBar.ScaleRateY;
                }

                if (ToolsBar.Figures[ToolsBar.Figures.Count - 1].Coordinates[1].X > ToolsBar.Figures[ToolsBar.Figures.Count - 1].Coordinates[0].X)
                {
                    ToolsBar.DistanceToPointX = ToolsBar.Figures[ToolsBar.Figures.Count - 1].Coordinates[0].X;
                }
                else
                {
                    ToolsBar.DistanceToPointX = ToolsBar.Figures[ToolsBar.Figures.Count - 1].Coordinates[1].X;
                }

                if (ToolsBar.Figures[ToolsBar.Figures.Count - 1].Coordinates[1].Y > ToolsBar.Figures[ToolsBar.Figures.Count - 1].Coordinates[0].Y)
                {
                    ToolsBar.DistanceToPointY = ToolsBar.Figures[ToolsBar.Figures.Count - 1].Coordinates[0].Y;
                }
                else
                {
                    ToolsBar.DistanceToPointY = ToolsBar.Figures[ToolsBar.Figures.Count - 1].Coordinates[1].Y;
                }
            }
            else
            {
                ToolsBar.ScaleRateX = 1;
                ToolsBar.ScaleRateY = 1;
                ToolsBar.DistanceToPointX = 0;
                ToolsBar.DistanceToPointY = 0;
            }
            ToolsBar.Figures.Remove(ToolsBar.Figures[ToolsBar.Figures.Count - 1]);
        }
    }
} 