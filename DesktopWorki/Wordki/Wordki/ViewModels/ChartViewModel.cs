using Repository.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using Util;
using Wordki.Database;
using Wordki.Helpers;
using Wordki.Models;
using Point = System.Windows.Point;

namespace Wordki.ViewModels
{

    public enum XLabel
    {
        Date,
        Id,
    }

    public enum Plots
    {
        Correct,
        Accepted,
        Wrong,
        Unvisibles,
    }

    public class ChartViewModel : ViewModelBase
    {

        private const int ChartHeight = 900;
        private const int ChartWidth = 2000;
        private const int Xmin = ChartWidth * 1 / 20;
        private const int Xmax = ChartWidth * 15 / 20;
        private const int Ymin = ChartHeight * 8 / 9;
        private const int Ymax = ChartHeight * 1 / 9;
        private double MaxXValue;
        private double MaxYValue;
        private double MinXValue;
        private double MinYValue;

        private int _selectedLabel;
        private int _selectedGroup;
        private string _title;
        private BitmapSource _bitmap;
        private Bitmap Bm { get; set; }

        #region Properties
        private Dictionary<ChartElement, Color> ColorDictionary { get; set; }
        private Dictionary<ChartElement, Brush> BrushDictionary { get; set; }
        private Dictionary<ChartElement, Pen> PenDictionary { get; set; }
        private Dictionary<Plots, List<Point>> PlotElements { get; set; }
        public ICommand BackCommand { get; set; }
        public ICommand RefreshChartCommand { get; set; }
        public ICommand MouseMoveCommand { get; set; }
        public ICommand MouseDownCommand { get; set; }
        public ObservableCollection<string> GroupNameList { get; set; }
        public long GroupId { get; set; }
        public int SelectedLabel
        {
            get { return _selectedLabel; }
            set
            {
                if (_selectedLabel != value)
                {
                    _selectedLabel = value;
                    OnPropertyChanged("SelectedLabel");
                    OnDraw();
                }
            }
        }
        public int SelectedGroup
        {
            get { return _selectedGroup; }
            set
            {
                if (_selectedGroup != value)
                {
                    _selectedGroup = value;
                    OnPropertyChanged("SelectedGroup");
                    if (value > 0)
                        GroupId = DatabaseSingleton.Instance.Groups[value].Id;
                    OnDraw();
                }
            }
        }
        public string Title
        {
            get { return _title; }
            set
            {
                if (_title != value)
                {
                    _title = value;
                    OnPropertyChanged("Title");
                }
            }
        }
        public BitmapSource Bitmap
        {
            get { return _bitmap; }
            set
            {
                if (_bitmap != value)
                {
                    _bitmap = value;
                    OnPropertyChanged("Bitmap");
                }
            }
        }
        public ObservableCollection<UIElement> Elements { get; set; }

        #endregion

        public ChartViewModel()
        {


            ColorDictionary = new Dictionary<ChartElement, Color>();
            ColorDictionary.Add(ChartElement.Axises, Color.FromArgb(255, 255, 0, 0));
            ColorDictionary.Add(ChartElement.Net, Color.FromArgb(100, 255, 0, 0));
            ColorDictionary.Add(ChartElement.Marker, Color.FromArgb(150, 255, 0, 0));
            ColorDictionary.Add(ChartElement.Contour, Color.FromArgb(255, 50, 50, 50));
            ColorDictionary.Add(ChartElement.Background, Color.FromArgb(255, 10, 10, 10));
            ColorDictionary.Add(ChartElement.Foreground, Color.FromArgb(255, 150, 150, 150));
            ColorDictionary.Add(ChartElement.Point, Color.FromArgb(255, 0, 200, 0));
            ColorDictionary.Add(ChartElement.PlotCorrect, Color.FromArgb(255, 0, 150, 0));
            ColorDictionary.Add(ChartElement.PlotAccepted, Color.FromArgb(255, 150, 150, 0));
            ColorDictionary.Add(ChartElement.PlotWrong, Color.FromArgb(255, 150, 0, 0));
            ColorDictionary.Add(ChartElement.PlotUnvisibles, Color.FromArgb(255, 150, 150, 150));

            BrushDictionary = new Dictionary<ChartElement, Brush>();
            BrushDictionary.Add(ChartElement.Axises, new SolidBrush(ColorDictionary[ChartElement.Axises]));
            BrushDictionary.Add(ChartElement.Net, new SolidBrush(ColorDictionary[ChartElement.Net]));
            BrushDictionary.Add(ChartElement.Marker, new SolidBrush(ColorDictionary[ChartElement.Marker]));
            BrushDictionary.Add(ChartElement.Contour, new SolidBrush(ColorDictionary[ChartElement.Contour]));
            BrushDictionary.Add(ChartElement.Background, new SolidBrush(ColorDictionary[ChartElement.Background]));
            BrushDictionary.Add(ChartElement.Foreground, new SolidBrush(ColorDictionary[ChartElement.Foreground]));
            BrushDictionary.Add(ChartElement.Point, new SolidBrush(ColorDictionary[ChartElement.Point]));
            BrushDictionary.Add(ChartElement.PlotCorrect, new SolidBrush(ColorDictionary[ChartElement.PlotCorrect]));
            BrushDictionary.Add(ChartElement.PlotAccepted, new SolidBrush(ColorDictionary[ChartElement.PlotAccepted]));
            BrushDictionary.Add(ChartElement.PlotWrong, new SolidBrush(ColorDictionary[ChartElement.PlotWrong]));
            BrushDictionary.Add(ChartElement.PlotUnvisibles, new SolidBrush(ColorDictionary[ChartElement.PlotUnvisibles]));

            PenDictionary = new Dictionary<ChartElement, Pen>();
            PenDictionary.Add(ChartElement.Axises, new Pen(BrushDictionary[ChartElement.Axises], 2));
            PenDictionary.Add(ChartElement.Net, new Pen(BrushDictionary[ChartElement.Net], 1));
            PenDictionary.Add(ChartElement.Marker, new Pen(BrushDictionary[ChartElement.Marker], 1));
            PenDictionary.Add(ChartElement.Contour, new Pen(BrushDictionary[ChartElement.Contour], 1));
            PenDictionary.Add(ChartElement.Background, new Pen(BrushDictionary[ChartElement.Background], 1));
            PenDictionary.Add(ChartElement.Foreground, new Pen(BrushDictionary[ChartElement.Foreground], 1));
            PenDictionary.Add(ChartElement.Point, new Pen(BrushDictionary[ChartElement.Point], 1));
            PenDictionary.Add(ChartElement.PlotCorrect, new Pen(BrushDictionary[ChartElement.PlotCorrect], 1));
            PenDictionary.Add(ChartElement.PlotAccepted, new Pen(BrushDictionary[ChartElement.PlotAccepted], 1));
            PenDictionary.Add(ChartElement.PlotWrong, new Pen(BrushDictionary[ChartElement.PlotWrong], 1));
            PenDictionary.Add(ChartElement.PlotUnvisibles, new Pen(BrushDictionary[ChartElement.PlotUnvisibles], 1));
            PlotElements = new Dictionary<Plots, List<Point>>();
            Bm = new Bitmap(ChartWidth, ChartHeight);

            ActivateCommand();
            GroupNameList = new ObservableCollection<string>();

        }

        public override void InitViewModel()
        {
            IDatabase lDatabase = DatabaseSingleton.Instance;
            SelectedLabel = 1;
            GroupId = (long)PackageStore.Get(0);

            //foreach (Group lGroup in lDatabase.GroupsList)
            //{
            //    GroupNameList.Add(lGroup.Name);
            //}
            //SelectedGroup = GroupNameList.IndexOf(lDatabase.GetGroupById(GroupId).Name);
            OnDraw();
        }

        public override void Back()
        {

        }

        private void ActivateCommand()
        {
            BackCommand = new Util.BuilderCommand(BackAction);
            RefreshChartCommand = new Util.BuilderCommand(RefreshChart);

            MouseMoveCommand = new Util.BuilderCommand(MouseMove);
            MouseDownCommand = new Util.BuilderCommand(MouseDown);
        }



        private void MouseDown(object obj)
        {
            Console.WriteLine("x: {0} y: {1}", ((Point)obj).X, ((Point)obj).Y);
            //OnDraw();
        }

        private void MouseMove(object obj)
        {
            //System.Drawing.Point lMousePosition = (System.Drawing.Point)obj;
            //MouseX = lMousePosition.X;
            //MouseY = lMousePosition.Y;
            ////OnDraw();
            //Console.WriteLine("x: {0} y: {1}", lMousePosition.X, lMousePosition.Y);
        }

        private void RefreshChart(object obj)
        {
            PrepareChartData();
            OnDraw();
        }

        private void BackAction(object obj)
        {
            Back();
            Switcher.Back();
        }

        private void PrepareChartData()
        {
            PlotElements.Clear();
            List<Point> lPointsList = null;
            lPointsList = CreatePoints(GroupId, Plots.Correct);
            if (lPointsList == null || lPointsList.Count == 0)
            {
                LoggerSingleton.LogInfo("{0} - {1}", "ChartViewModel.PrepareChartData", "lPointsList == null || lPointsList.Count == 0");
                return;
            }
            PlotElements.Add(Plots.Correct, lPointsList);
            lPointsList = CreatePoints(GroupId, Plots.Accepted);
            if (lPointsList == null || lPointsList.Count == 0)
            {
                LoggerSingleton.LogInfo("{0} - {1}", "ChartViewModel.PrepareChartData", "lPointsList == null || lPointsList.Count == 0");
                return;
            }
            PlotElements.Add(Plots.Accepted, lPointsList);
            lPointsList = CreatePoints(GroupId, Plots.Wrong);
            if (lPointsList == null || lPointsList.Count == 0)
            {
                LoggerSingleton.LogInfo("{0} - {1}", "ChartViewModel.PrepareChartData", "lPointsList == null || lPointsList.Count == 0");
                return;
            }
            PlotElements.Add(Plots.Wrong, lPointsList);

            lPointsList = CreatePoints(GroupId, Plots.Unvisibles);
            if (lPointsList == null || lPointsList.Count == 0)
            {
                LoggerSingleton.LogInfo("{0} - {1}", "ChartViewModel.PrepareChartData", "lPointsList == null || lPointsList.Count == 0");
                return;
            }
            PlotElements.Add(Plots.Unvisibles, lPointsList);
        }

        private List<Point> CreatePoints(long pGroupId, Plots pMode)
        {
            List<Point> lPointsList = new List<Point>();
            try
            {
                IList<IResult> lResultList = new List<IResult>();/*DatabaseSingleton.Instance.GetResultsList(pGroupId).ToList();*/
                if (lResultList.Count == 0)
                {
                    return lPointsList;
                }
                for (int i = 0; i < lResultList.Count; i++)
                {
                    if (SelectedLabel == (int)XLabel.Date)
                        switch (pMode)
                        {
                            case Plots.Correct:
                                {
                                    lPointsList.Add(new Point(lResultList[i].DateTime.Ticks, lResultList[i].Correct + lResultList[i].Accepted + lResultList[i].Wrong + lResultList[i].Invisibilities));
                                }
                                break;
                            case Plots.Accepted:
                                {
                                    lPointsList.Add(new Point(lResultList[i].DateTime.Ticks, lResultList[i].Accepted + lResultList[i].Wrong + lResultList[i].Invisibilities));
                                }
                                break;
                            case Plots.Wrong:
                                {
                                    lPointsList.Add(new Point(lResultList[i].DateTime.Ticks, lResultList[i].Wrong + lResultList[i].Invisibilities));
                                }
                                break;
                            case Plots.Unvisibles:
                                {
                                    lPointsList.Add(new Point(lResultList[i].DateTime.Ticks, lResultList[i].Invisibilities));
                                }
                                break;
                        }
                    if (SelectedLabel == (int)XLabel.Id)
                        switch (pMode)
                        {
                            case Plots.Correct:
                                {
                                    lPointsList.Add(new Point(i, lResultList[i].Correct + lResultList[i].Accepted + lResultList[i].Wrong + lResultList[i].Invisibilities));
                                }
                                break;
                            case Plots.Accepted:
                                {
                                    lPointsList.Add(new Point(i, lResultList[i].Accepted + lResultList[i].Wrong + lResultList[i].Invisibilities));
                                }
                                break;
                            case Plots.Wrong:
                                {
                                    lPointsList.Add(new Point(i, lResultList[i].Wrong + lResultList[i].Invisibilities));
                                }
                                break;
                            case Plots.Unvisibles:
                                {
                                    lPointsList.Add(new Point(i, lResultList[i].Invisibilities));
                                }
                                break;
                        }
                }
            }
            catch (Exception lException)
            {
                LoggerSingleton.LogError("{0} - {1}", "ChartViewModel.CreatePoint", lException.Message);
            }
            return lPointsList;
        }

        private void OnDrawPlots(Graphics pGraphics, List<Point> pPlot, Plots lPlotEnum)
        {
            List<System.Drawing.Point> lScreenPlot;
            try
            {
                Brush lBrush;
                Pen lPen;
                switch (lPlotEnum)
                {
                    case Plots.Correct:
                        {
                            lBrush = BrushDictionary[ChartElement.PlotCorrect];
                            lPen = PenDictionary[ChartElement.PlotCorrect];
                        }
                        break;
                    case Plots.Accepted:
                        {
                            lBrush = BrushDictionary[ChartElement.PlotAccepted];
                            lPen = PenDictionary[ChartElement.PlotAccepted];
                        }
                        break;
                    case Plots.Wrong:
                        {
                            lBrush = BrushDictionary[ChartElement.PlotWrong];
                            lPen = PenDictionary[ChartElement.PlotWrong];
                        }
                        break;
                    case Plots.Unvisibles:
                        {
                            lBrush = BrushDictionary[ChartElement.PlotUnvisibles];
                            lPen = PenDictionary[ChartElement.PlotUnvisibles];
                        }
                        break;
                    default:
                        throw new ArgumentOutOfRangeException("lPlotEnum");
                }

                lScreenPlot = GetScreenPoint(pPlot);
                foreach (System.Drawing.Point lPoint in lScreenPlot)
                {//rysowanie punktów
                    pGraphics.DrawEllipse(lPen, lPoint.X, lPoint.Y, 5, 5);
                }

                for (int i = 1; i < lScreenPlot.Count; i++)
                {
                    pGraphics.FillPolygon(lBrush, new[] { lScreenPlot[i - 1], lScreenPlot[i], new System.Drawing.Point { X = lScreenPlot[i].X, Y = Ymin }, new System.Drawing.Point { X = lScreenPlot[i - 1].X, Y = Ymin } });
                }
                pGraphics.DrawLines(lPen, lScreenPlot.ToArray());

            }
            catch (Exception lException)
            {
                LoggerSingleton.LogError("{0} - {1}", "ChartViewModel.OnDrawPlot", lException.Message);
            }
        }

        //private void OnDrawPointInfo(DrawingContext pDrawingContext, List<System.Windows.Point> pPlot) {
        //  System.Drawing.Point lDrawPoint = new System.Drawing.Point();
        //  double lMinDistance = double.MaxValue;
        //  double lDistanceCalculate;
        //  List<System.Drawing.Point> lScreenPlot = GetScreenPoint(pPlot);
        //  foreach (System.Drawing.Point lPoint in lScreenPlot) {
        //    lDistanceCalculate = DistanceCalculate(lPoint, MouseClick);
        //    if (lDistanceCalculate > MinInfoDistance)
        //      continue;
        //    if (lDistanceCalculate < lMinDistance) {
        //      lMinDistance = lDistanceCalculate;
        //      lDrawPoint = lPoint;
        //    }
        //  }
        //  Console.WriteLine("X: {0}, Y: {1}", lDrawPoint.X, lDrawPoint.Y);
        //}

        internal static class NativeMethods
        {
            [DllImport("gdi32.dll")]
            [return: MarshalAs(UnmanagedType.Bool)]
            internal static extern bool DeleteObject(IntPtr hObject);
        }

        public static BitmapSource ToBitmapSource(Bitmap source)
        {
            BitmapSource bitSrc = null;

            var hBitmap = source.GetHbitmap();

            try
            {
                bitSrc = Imaging.CreateBitmapSourceFromHBitmap(
                    hBitmap,
                    IntPtr.Zero,
                    Int32Rect.Empty,
                    BitmapSizeOptions.FromEmptyOptions());
            }
            catch (Win32Exception)
            {
                bitSrc = null;
            }
            finally
            {
                NativeMethods.DeleteObject(hBitmap);
            }

            return bitSrc;
        }
        private void OnDraw()
        {
            using (Graphics lGraphics = Graphics.FromImage(Bm))
            {

                //Rysowanie tła
                lGraphics.FillRectangle(BrushDictionary[ChartElement.Background], 0, 0, ChartWidth, ChartHeight);

                //rysowanie siatki
                int lStep = 100;
                for (int i = 0; i < ChartWidth / lStep; i++)
                {
                    lGraphics.DrawLine(PenDictionary[ChartElement.Net], i * lStep, 0, i * lStep, ChartHeight);
                }
                for (int i = 0; i < ChartHeight / lStep; i++)
                {
                    lGraphics.DrawLine(PenDictionary[ChartElement.Net], 0, i * lStep, ChartWidth, i * lStep);
                }

                //rysowanie osi
                lGraphics.DrawLine(PenDictionary[ChartElement.Axises], Xmin, Ymin, Xmax, Ymin);
                lGraphics.DrawLine(PenDictionary[ChartElement.Axises], Xmin, Ymin, Xmin, Ymax);


                //rysowanie wykresu
                PrepareChartData();
                CalculateMaxMinValue();
                foreach (KeyValuePair<Plots, List<Point>> lItem in PlotElements)
                {
                    OnDrawPlots(lGraphics, lItem.Value, lItem.Key);
                }
            }
            Bitmap = ToBitmapSource(Bm);
            OnPropertyChanged("Bitmap");
        }

        private void CalculateMaxMinValue()
        {
            MaxXValue = double.MinValue;
            MaxYValue = double.MinValue;
            MinXValue = double.MaxValue;
            foreach (var lItem in PlotElements)
                foreach (Point lPoint in lItem.Value)
                {
                    double lPointX = lPoint.X;
                    double lPointY = lPoint.Y;
                    if (lPointX < MinXValue)
                        MinXValue = lPointX;
                    if (lPointX > MaxXValue)
                        MaxXValue = lPointX;
                    if (lPointY > MaxYValue)
                        MaxYValue = lPointY;
                }
            MinYValue = 0;
        }

        private List<System.Drawing.Point> GetScreenPoint(List<Point> pPlot)
        {
            List<System.Drawing.Point> lScreenPlot = new List<System.Drawing.Point>();
            double lXValue, lYValue;
            foreach (Point lPoint in pPlot)
            {
                lXValue = (lPoint.X - MinXValue) * (1400 - 200) / (MaxXValue - MinXValue) + 200;
                lYValue = 800 + (lPoint.Y - MinYValue) * (800 - 200) / (MinYValue - MaxYValue);
                lScreenPlot.Add(new System.Drawing.Point((int)lXValue, (int)lYValue));
            }
            return lScreenPlot;
        }

        public override void Loaded()
        {
            throw new NotImplementedException();
        }

        public override void Unloaded()
        {
            throw new NotImplementedException();
        }
    }

    public enum ChartElement
    {
        Axises,
        Net,
        Contour,
        Background,
        Foreground,
        Marker,
        Point,
        PlotCorrect,
        PlotAccepted,
        PlotWrong,
        PlotUnvisibles,
    }
}
