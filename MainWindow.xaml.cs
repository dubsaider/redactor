using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Paint2.Paint;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Microsoft.Win32;

namespace Paint2
{
    public partial class MainWindow : Window
    {
        public static MainWindow Instance { get; private set; }

        bool ClikOnCanvas = false;

        public MainWindow()
        {
            InitializeComponent();
            Instance = this;
            MyCanvas.Children.Add(ToolsBar.FigureHost);
            ButtonGeneration.Generation();
            ToolsBar.AddCondition();
        }

        private void Invalidate()
        {
            ToolsBar.FigureHost.Children.Clear();
            var drawingVisual = new DrawingVisual();
            var drawingContext = drawingVisual.RenderOpen();
            foreach (var figure in ToolsBar.Figures)
            {
                figure.Draw(drawingContext);
                if (figure.SelectRect != null)
                {
                    figure.SelectRect.Draw(drawingContext);
                }
            }

            drawingContext.Close();
            ToolsBar.FigureHost.Children.Add(drawingVisual);
        }

        private void MyCanvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                ToolsBar.ToolNow.MouseDown(e.GetPosition(MyCanvas));
            }
            if (e.RightButton == MouseButtonState.Pressed)
            {
                ToolsBar.tempBrush = ToolsBar.BrushNow;
                ToolsBar.BrushNow = ToolsBar.ColorNow;
                ToolsBar.ColorNow = ToolsBar.tempBrush;
                ToolsBar.tempStringBrush = ToolsBar.BrushStringNow;
                ToolsBar.BrushStringNow = ToolsBar.ColorStringNow;
                ToolsBar.ColorStringNow = ToolsBar.tempStringBrush;
                ToolsBar.ToolNow.MouseDown(e.GetPosition(MyCanvas));
                ToolsBar.tempBrush = ToolsBar.BrushNow;
                ToolsBar.BrushNow = ToolsBar.ColorNow;
                ToolsBar.ColorNow = ToolsBar.tempBrush;
                ToolsBar.tempStringBrush = ToolsBar.BrushStringNow;
                ToolsBar.BrushStringNow = ToolsBar.ColorStringNow;
                ToolsBar.ColorStringNow = ToolsBar.tempStringBrush;
            }
            ClikOnCanvas = true;
            Invalidate();
        }

        private void MyCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (ClikOnCanvas)
            {
                ToolsBar.ToolNow.MouseMove(e.GetPosition(MyCanvas));
                if (ToolsBar.ToolNow == ToolsBar.Transform["Hand"])
                {
                    ScrollViewerCanvas.ScrollToVerticalOffset(ToolsBar.HandScrollY);
                    ScrollViewerCanvas.ScrollToHorizontalOffset(ToolsBar.HandScrollX);
                }
                Invalidate();
            }
        }
        private void MyCanvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (ClikOnCanvas)
            {
                ToolsBar.ToolNow.MouseUp(e.GetPosition(MyCanvas));

                if (ToolsBar.ToolNow != ToolsBar.Transform["Allotment"] & ToolsBar.ToolNow != ToolsBar.Transform["ZoomRect"] & ToolsBar.ToolNow != ToolsBar.Transform["Hand"])
                {
                    ToolsBar.AddCondition();
                    gotoPastCondition.IsEnabled = true;
                    gotoSecondCondition.IsEnabled = false;
                }
                if (ToolsBar.ToolNow == ToolsBar.Transform["ZoomRect"])
                {
                    MyCanvas.LayoutTransform = new ScaleTransform(ToolsBar.ScaleRateX, ToolsBar.ScaleRateY);
                    ScrollViewerCanvas.ScrollToVerticalOffset(ToolsBar.DistanceToPointY * ToolsBar.ScaleRateY);
                    ScrollViewerCanvas.ScrollToHorizontalOffset(ToolsBar.DistanceToPointX * ToolsBar.ScaleRateX);
                }
                if (ToolsBar.ToolNow == ToolsBar.HandTool)
                {
                    ToolsBar.ToolNow = ToolsBar.Transform["Allotment"];
                }
                ClikOnCanvas = false;
                Invalidate();
            }
        }

        private void MyCanvas_MouseLeave(object sender, MouseEventArgs e)
        {
            if (ClikOnCanvas)
            {
                ToolsBar.ToolNow.MouseUp(e.GetPosition(MyCanvas));

                if (ToolsBar.ToolNow != ToolsBar.Transform["Allotment"] & ToolsBar.ToolNow != ToolsBar.Transform["ZoomRect"] & ToolsBar.ToolNow != ToolsBar.Transform["Hand"] & ToolsBar.ToolNow != ToolsBar.HandTool)
                {
                    ToolsBar.AddCondition();
                    gotoPastCondition.IsEnabled = true;
                    gotoSecondCondition.IsEnabled = false;
                }
                if (ToolsBar.ToolNow == ToolsBar.Transform["ZoomRect"])
                {
                    MyCanvas.LayoutTransform = new ScaleTransform(ToolsBar.ScaleRateX, ToolsBar.ScaleRateY);
                    ScrollViewerCanvas.ScrollToVerticalOffset(ToolsBar.DistanceToPointY * ToolsBar.ScaleRateY);
                    ScrollViewerCanvas.ScrollToHorizontalOffset(ToolsBar.DistanceToPointX * ToolsBar.ScaleRateX);
                }
                ClikOnCanvas = false;
                Invalidate();
            }
        }

        public void ButtonChangeTool(object sender, RoutedEventArgs e)
        {
            ToolsBar.ToolNow = ToolsBar.Transform[(sender as Button).Tag.ToString()];
            if ((sender as Button).Tag.ToString() == "RoundRect")
            {
                textBoxRoundRectX.IsEnabled = true;
                textBoxRoundRectY.IsEnabled = true;
            }
            else
            {
                textBoxRoundRectX.IsEnabled = false;
                textBoxRoundRectY.IsEnabled = false;
            }
            foreach (Figure figure in ToolsBar.Figures)
            {
                figure.UnSelected();
            }
            Invalidate();
            PropToolBarPanel.Children.Clear();
        }

        public void ButtonChangeColor(object sender, RoutedEventArgs e)
        {
            if (ToolsBar.FirstPress == true)
            {
                ToolsBar.ColorNow = ToolsBar.TransformColor[(sender as Button).Tag.ToString()];
                ToolsBar.ColorStringNow = (sender as Button).Tag.ToString();
                if ((sender as Button).Background == null) { button_firstColor.Background = Brushes.Gray; }
                else { button_firstColor.Background = (sender as Button).Background; }

            }
            else
            {
                ToolsBar.BrushNow = ToolsBar.TransformColor[(sender as Button).Tag.ToString()];
                ToolsBar.BrushStringNow = (sender as Button).Tag.ToString();
                if ((sender as Button).Background == null) { button_secondColor.Background = Brushes.Gray; }
                else { button_secondColor.Background = (sender as Button).Background; }
            }
        }

        private void ThiknessSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            ToolsBar.ThicnessNow = ThiknessSlider.Value;
        }

        private void FirstColor(object sender, RoutedEventArgs e)
        {
            ToolsBar.FirstPress = true;
            ToolsBar.SecondPress = false;
            button_firstColor.BorderThickness = new Thickness(5);
            button_secondColor.BorderThickness = new Thickness(0);
        }

        private void SecondColor(object sender, RoutedEventArgs e)
        {
            ToolsBar.FirstPress = false;
            ToolsBar.SecondPress = true;
            button_secondColor.BorderThickness = new Thickness(5);
            button_firstColor.BorderThickness = new Thickness(0);
        }

        private void MyCanvas_Loaded(object sender, RoutedEventArgs e)
        {
            ToolsBar.CanvasHeigth = MyCanvas.Height;
            ToolsBar.CanvasWidth = MyCanvas.Width;
        }

        private void MyCanvas_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ToolsBar.CanvasHeigth = MyCanvas.Height;
            ToolsBar.CanvasWidth = MyCanvas.Width;
        }

        public void CleanMyCanvas(object sender, RoutedEventArgs e)
        {
            ToolsBar.FigureHost.Children.Clear();
            ToolsBar.Figures.Clear();
            ToolsBar.ConditionNumber = 0;
            ToolsBar.ConditionsCanvas.Clear();
            ToolsBar.AddCondition();
            gotoPastCondition.IsEnabled = false;
            gotoSecondCondition.IsEnabled = false;
        }

        public void MinusZoomMyCanvas(object sender, RoutedEventArgs e)
        {
            MyCanvas.LayoutTransform = new ScaleTransform(1, 1);
            ScrollViewerCanvas.ScrollToVerticalOffset(0);
            ScrollViewerCanvas.ScrollToHorizontalOffset(0);
        }

        private void ChangeSelectionDash(object sender, SelectionChangedEventArgs e)
        {
            ToolsBar.DashNow = ToolsBar.TransformDash[comboBoxDash.SelectedIndex.ToString()];
            if (comboBoxDash.SelectedIndex.ToString() == "0")
            {
                ToolsBar.DashStringhNow = "―――――";
            }
            if (comboBoxDash.SelectedIndex.ToString() == "1")
            {
                ToolsBar.DashStringhNow = "— — — — — —";
            }
            if (comboBoxDash.SelectedIndex.ToString() == "2")
            {
                ToolsBar.DashStringhNow = "— ∙ — ∙ — ∙ — ∙ —";
            }
            if (comboBoxDash.SelectedIndex.ToString() == "3")
            {
                ToolsBar.DashStringhNow = "— ∙ ∙ — ∙ ∙ — ∙ ∙ — ";
            }
            if (comboBoxDash.SelectedIndex.ToString() == "4")
            {
                ToolsBar.DashStringhNow = "∙∙∙∙∙∙∙∙∙∙∙∙∙∙∙∙∙∙∙∙∙∙∙";
            }
        }

        private void textBoxRoundRectX_TextChanged(object sender, TextChangedEventArgs e)
        {
            ToolsBar.RoundXNow = Convert.ToDouble(textBoxRoundRectX.Text);
        }

        private void textBoxRoundRectY_TextChanged(object sender, TextChangedEventArgs e)
        {
            ToolsBar.RoundYNow = Convert.ToDouble(textBoxRoundRectY.Text);
        }

        private void gotoPastCondition_Click(object sender, RoutedEventArgs e)
        {
            ToolsBar.gotoPastCondition();
            if (ToolsBar.ConditionNumber == 1)
            {
                gotoPastCondition.IsEnabled = false;
            }
            gotoSecondCondition.IsEnabled = true;
            Invalidate();
        }

        private void gotoSecondCondition_Click(object sender, RoutedEventArgs e)
        {
            ToolsBar.gotoSecondCondition();
            if (ToolsBar.ConditionNumber == ToolsBar.ConditionsCanvas.Count)
            {
                gotoSecondCondition.IsEnabled = false;
            }
            gotoPastCondition.IsEnabled = true;
            Invalidate();
        }

        //Change property figure function

        public void changeRoundX(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            foreach (Figure figure in ToolsBar.Figures)
            {
                if (figure.Select == true)
                {
                    figure.ChangeRoundX(e.NewValue);
                }
            }
            Invalidate();
        }

        public void changeRoundY(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            foreach (Figure figure in ToolsBar.Figures)
            {
                if (figure.Select == true)
                {
                    figure.ChangeRoundY(e.NewValue);
                }
            }
            Invalidate();
        }

        public void ChangeStrokeColor(object sender, RoutedEventArgs e)
        {
            foreach (Figure figure in ToolsBar.Figures)
            {
                if (figure.Select == true)
                {
                    figure.ChangePen(ToolsBar.TransformColor[(sender as Button).Tag.ToString()], (sender as Button).Tag.ToString());
                }
            }
            ToolsBar.AddCondition();
            gotoPastCondition.IsEnabled = true;
            gotoSecondCondition.IsEnabled = false;
            Invalidate();
        }

        public void ChangeBrushColor(object sender, RoutedEventArgs e)
        {
            foreach (Figure figure in ToolsBar.Figures)
            {
                if (figure.Select == true)
                {
                    figure.ChangePen(ToolsBar.TransformColor[(sender as Button).Tag.ToString()], (sender as Button).Tag.ToString(), new bool());
                }
            }
            ToolsBar.AddCondition();
            gotoPastCondition.IsEnabled = true;
            gotoSecondCondition.IsEnabled = false;
            Invalidate();
        }

        public void ChangeDash(object sender, RoutedEventArgs e)
        {
            foreach (Figure figure in ToolsBar.Figures)
            {
                if (figure.Select == true)
                {
                    figure.ChangePen(ToolsBar.TransformDashProp[(sender as Button).Content.ToString()], (sender as Button).Content.ToString());
                }
            }
            ToolsBar.AddCondition();
            gotoPastCondition.IsEnabled = true;
            gotoSecondCondition.IsEnabled = false;
            Invalidate();
        }

        public void ClearSelectedFigure(object sender, RoutedEventArgs e)
        {
            foreach (Figure figure in ToolsBar.Figures.ToArray())
            {
                if (figure.Select == true)
                {
                    ToolsBar.Figures.Remove(figure);
                }
            }
            PropToolBarPanel.Children.Clear();
            ToolsBar.AddCondition();
            gotoPastCondition.IsEnabled = true;
            gotoSecondCondition.IsEnabled = false;
            Invalidate();
        }

        public void RowThicnessChange(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            foreach (Figure figure in ToolsBar.Figures)
            {
                if (figure.Select == true)
                {
                    figure.ChangePen(e.NewValue);
                }
            }
            Invalidate();
        }

        public void HandForSelectedFigure(object sender, RoutedEventArgs e)
        {
            ToolsBar.ToolNow = ToolsBar.HandTool;
        }

        public void SldMouseUp(object sender, MouseButtonEventArgs e)
        {
            ToolsBar.AddCondition();
            gotoPastCondition.IsEnabled = true;
            gotoSecondCondition.IsEnabled = false;
        }

    }
}