using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace Wordki.Controls {
  /// <summary>
  /// Interaction logic for Chart.xaml
  /// </summary>
  public partial class Chart : UserControl {

    public static readonly DependencyProperty ValuesProperty = DependencyProperty.Register(
      "Values", typeof(List<double?>), typeof(Chart), new UIPropertyMetadata(new List<double?>()));

    public List<double?> Values {
      get { return GetValue(ValuesProperty) as List<double?>; }
      set { SetValue(ValuesProperty, value); }
    }

    public static readonly DependencyProperty MaxValueProperty = DependencyProperty.Register(
      "MaxValue", typeof(double?), typeof(Chart), new UIPropertyMetadata(0.0));

    public double? MaxValue {
      get { return GetValue(MaxValueProperty) as double?; }
      set { SetValue(MaxValueProperty, value); }
    }

    private Point MousePosition { get; set; }
    private Point BeginPosition { get; set; }
    private Point EndPosition { get; set; }
    private SolidColorBrush Background { get; set; }
    private SolidColorBrush RingBackground { get; set; }
    private Pen RingBorder { get; set; }
    private Pen Pen { get; set; }
    private Rect Border { get; set; }

    public Chart() {
      InitializeComponent();
      Background = new SolidColorBrush(Color.FromRgb(255, 152, 152));
      Pen = new Pen(new SolidColorBrush(Color.FromRgb(255, 0, 0)), 1);
      BeginPosition = new Point(0, 0);
      Border = new Rect(0, 0, Width, Height);

      RingBackground = new SolidColorBrush(Color.FromRgb(128, 155, 155));
      RingBorder = new Pen(new SolidColorBrush(Color.FromRgb(0, 225, 0)), 1);
    }

    public override void EndInit() {
      base.EndInit();
      if (MaxValue.HasValue && Math.Abs(MaxValue.Value - 0.0) < 0.00001) {
        if (Values != null && Values.Count > 0) {
          MaxValue = Values.Sum();
        }
      }
    }

    protected override void OnRender(DrawingContext drawingContext) {
      base.OnRender(drawingContext);
      drawingContext.DrawRectangle(Background, Pen, Border);
      //drawingContext.DrawLine(Pen, BeginPosition, MousePosition);
      DrawRing(drawingContext);
    }

    protected override Size MeasureOverride(Size constraint) {
      double size = Math.Max(ActualWidth, ActualHeight);
      Console.WriteLine("Chart size - {0}", size);

      Border = new Rect(0, 0, size, size);
      EndPosition = new Point(size, size);
      InvalidateVisual();
      return base.MeasureOverride(constraint);
    }

    private void UserControl_MouseMove(object sender, MouseEventArgs e) {
      MousePosition = e.GetPosition(this);
      InvalidateVisual();
    }

    private void DrawRing(DrawingContext pContext) {

      double lOuterRadius = EndPosition.X / 2;
      double lInnerRadius = EndPosition.X / 4;
      
      double startDegrees = 0;
      double sweepDegrees = 0;

      if (Values == null || MaxValue == null) {
        return;
      }

      for (int i = 0; i < Values.Count; i++) {

        double lDegree = Values[i].Value;
        StreamGeometry streamGeom = new StreamGeometry();
        using (StreamGeometryContext ctx = streamGeom.Open()) {
          startDegrees += sweepDegrees;
          sweepDegrees = GetSweepDegrees(MaxValue.Value, lDegree);
          double startRadians = startDegrees * Math.PI / 180.0;
          double sweepRadians = sweepDegrees * Math.PI / 180.0;

          // determine the start point 
          double xso = (Math.Cos(startRadians) * lOuterRadius) + lOuterRadius;
          double yso = (Math.Sin(startRadians) * lOuterRadius) + lOuterRadius;

          // determine the end point 
          double xeo = (Math.Cos(startRadians + sweepRadians) * lOuterRadius) + lOuterRadius;
          double yeo = (Math.Sin(startRadians + sweepRadians) * lOuterRadius) + lOuterRadius;



          bool isLargeArc = Math.Abs(sweepDegrees) > 180;
          SweepDirection sweepDirection = sweepDegrees < 0 ? SweepDirection.Counterclockwise : SweepDirection.Clockwise;

          ctx.BeginFigure(new Point(xso, yso), true, false);
          ctx.ArcTo(new Point(xeo, yeo), new Size(lOuterRadius, lOuterRadius), 0, isLargeArc, sweepDirection, true, false);

          double xsi = (Math.Cos(startRadians) * lInnerRadius) + lOuterRadius;
          double ysi = (Math.Sin(startRadians) * lInnerRadius) + lOuterRadius;

          // determine the end point 
          double xei = (Math.Cos(startRadians + sweepRadians) * lInnerRadius) + lOuterRadius;
          double yei = (Math.Sin(startRadians + sweepRadians) * lInnerRadius) + lOuterRadius;

          ctx.LineTo(new Point(xei, yei), false, false);
          sweepDirection = -sweepDegrees < 0 ? SweepDirection.Counterclockwise : SweepDirection.Clockwise;
          ctx.ArcTo(new Point(xsi, ysi), new Size(lInnerRadius, lInnerRadius), 0, isLargeArc, sweepDirection, true, false);


        }

        //streamGeom.Transform = new RotateTransform(-90.0, 100.0, 100.0);
        streamGeom.Freeze();
        pContext.DrawGeometry(GetBrush(i), GetPen(i), streamGeom);

        //ctx.BeginFigure(new Point(50.0, 0.0), true, false);
        //ctx.ArcTo(new Point(50.0, 100.0), lOuterSize, 0.0, false, SweepDirection.Clockwise, false, true);
        //ctx.ArcTo(new Point(50.0, 0.0), lOuterSize, 0.0, false, SweepDirection.Clockwise, false, true);

        //ctx.LineTo(new Point(50.0, 25.0), false, true);
        //ctx.ArcTo(new Point(50.0, 75.0), lInnerSize, 0.0, false, SweepDirection.Clockwise, false, true);
        //ctx.ArcTo(new Point(50.0, 25.0), lInnerSize, 0.0, false, SweepDirection.Clockwise, false, true);
      }

    }

    private double GetSweepDegrees(double pMaxValue, double pPartValue) {
      return pPartValue / pMaxValue * 360;
    }

    private Pen GetPen(int pIndex) {
      switch (pIndex) {
        case 0:
          return new Pen(new SolidColorBrush(Color.FromRgb(255, 0, 0)), 2);
        case 1:
          return new Pen(new SolidColorBrush(Color.FromRgb(255, 128, 0)), 2);
        case 2:
          return new Pen(new SolidColorBrush(Color.FromRgb(128, 0, 255)), 2);
        case 3:
          return new Pen(new SolidColorBrush(Color.FromRgb(128, 255, 255)), 2);
        case 4:
          return new Pen(new SolidColorBrush(Color.FromRgb(255, 255, 0)), 2);
      }
      return new Pen(new SolidColorBrush(Color.FromRgb(0, 0, 0)), 2);
    }

    private Brush GetBrush(int index) {
      switch (index) {
        case 0:
          return new SolidColorBrush(Color.FromRgb(255, 100, 100));
        case 1:
          return new SolidColorBrush(Color.FromRgb(255, 180, 128));
        case 2:
          return new SolidColorBrush(Color.FromRgb(128, 128, 255));
        case 3:
          return new SolidColorBrush(Color.FromRgb(200, 255, 255));
        case 4:
          return new SolidColorBrush(Color.FromRgb(255, 255, 128));
      }
      return new SolidColorBrush(Color.FromRgb(0, 0, 0));
    }

    private class ChartElement {
      SolidColorBrush Background { get; set; }
      Pen Border { get; set; }
    }

  }
}
