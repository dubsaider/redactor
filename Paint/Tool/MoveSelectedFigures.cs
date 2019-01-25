﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Paint2.Paint;

namespace Paint2.Paint
{
    class HandForFigureTool : Tool
    {
        Point StartPoint;
        Point LastPoint;

        public override void MouseDown(Point point)
        {
            StartPoint = point;
        }

        public override void MouseMove(Point point)
        {
            LastPoint = point;
            List<Figure> figureNow = new List<Figure>();
            foreach(Figure figure in ToolsBar.Figures)
            {
                figureNow.Add(figure.Clone());
            }
            ToolsBar.Figures.Clear();
            foreach(Figure figure in figureNow)
            {
                if (figure.Select == true)
                {
                    for (var i = 0; i < figure.Coordinates.Count; i++)
                    {
                        figure.Coordinates[i] = Point.Add(figure.Coordinates[i], Point.Subtract(LastPoint, StartPoint));
                    }

                    for (var i = 0; i < 2; i++)
                    {
                        figure.SelectRect.Coordinates[i] = Point.Add(figure.SelectRect.Coordinates[i], Point.Subtract(LastPoint, StartPoint));
                    }
                }
            }
            ToolsBar.Figures = new List<Figure>();
            foreach (Figure figure in figureNow)
            {
                ToolsBar.Figures.Add(figure.Clone());
            }
            StartPoint = LastPoint;
        }
    }
}