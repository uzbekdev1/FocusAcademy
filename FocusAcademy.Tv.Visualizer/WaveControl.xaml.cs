using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using CSCoreWaveform.Annotations;

namespace FocusAcademy.Tv.Waveform
{
    /// <summary>
    ///     Interaction logic for Waveform.xaml
    /// </summary>
    public partial class WaveControl : UserControl, INotifyPropertyChanged
    {
        // Using a DependencyProperty as the backing store for ChannelDataFloats.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ChannelDataProperty =
            DependencyProperty.Register("ChannelData", typeof(IList<float>), typeof(WaveControl),
                new PropertyMetadata(null, async (d, e) => { await ((WaveControl) d).UpdateWaveformAsync(); }));

        // Using a DependencyProperty as the backing store for PositionInPerc.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PositionInPercProperty =
            DependencyProperty.Register("PositionInPerc", typeof(double), typeof(WaveControl),
                new PropertyMetadata(0.0, (d, e) => { ((WaveControl) d).UpdatePosition((double) e.NewValue); }));

        private LineGeometry _positionGeometry;

        public WaveControl()
        {
            InitializeComponent();
        }

        public IList<float> ChannelData
        {
            get { return (IList<float>) GetValue(ChannelDataProperty); }
            set { SetValue(ChannelDataProperty, value); }
        }

        public double PositionInPerc
        {
            get { return (double) GetValue(PositionInPercProperty); }
            set { SetValue(PositionInPercProperty, value); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private async Task UpdateWaveformAsync()
        {
            if (!Dispatcher.CheckAccess())
            {
                await Dispatcher.InvokeAsync(async () => await UpdateWaveformAsync());
                return;
            }

            var points = new List<Point>();
            var centerHeight = PART_Canvas.RenderSize.Height / 2d;
            double x = 0;
            var channelData = ChannelData;
            double minValue = 0;
            var maxValue = 1.5;
            var dbScale = maxValue - minValue;

            points.Add(new Point(0, centerHeight));
            points.Add(new Point(0, centerHeight));

            var iOffset = 0;
            for (var i = 0; i < channelData.Count; i++)
            {
                if (i == channelData.Count / 2)
                {
                    iOffset = channelData.Count / 2;
                    points.Add(new Point(x, centerHeight));
                    points.Add(new Point(0, centerHeight));
                }

                x = PART_Canvas.RenderSize.Width / (channelData.Count / 2) * (i - iOffset);
                Debug.Assert(i - iOffset >= 0);
                var height = (channelData[i] - minValue) / dbScale * centerHeight;
                height += centerHeight;
                points.Add(new Point(x, height));
            }

            points.Add(new Point(x, centerHeight));
            points.Add(new Point(0, centerHeight));

            var lineSegment = new PolyLineSegment(points, false);

            var figure = new PathFigure();
            figure.Segments.Add(lineSegment);

            var geometry = new PathGeometry();
            geometry.Figures.Add(figure);

            //var path = new Path
            //{
            //    Data = geometry,
            //    Fill = Brushes.Green,
            //    Stroke = Brushes.Transparent,
            //    StrokeThickness = PART_Canvas.RenderSize.Width / (channelData.Count / 2)
            //};

            PART_Path.Data = geometry;
            PART_Path.StrokeThickness = PART_Canvas.RenderSize.Width / (channelData.Count / 2);

            var centerLinePath = new Path
            {
                Data = new LineGeometry(new Point(points.First().X, centerHeight), new Point(x, centerHeight)),
                Fill = Brushes.Transparent,
                Stroke = Brushes.Black,
                StrokeThickness = 1
            };

            _positionGeometry = new LineGeometry();
            var positionPath = new Path
            {
                Data = _positionGeometry,
                Fill = Brushes.Transparent,
                Stroke = Brushes.Red,
                StrokeThickness = 1
            };

            geometry.FillRule = FillRule.Nonzero;

            ClearWaveform();
            PART_Canvas.CacheMode = new BitmapCache(1.0);
            PART_Canvas.Children.Add(PART_Path);
            PART_Canvas.Children.Add(centerLinePath);
            PART_Canvas.Children.Add(positionPath);
        }

        public void UpdatePosition(double perc)
        {
            if (Dispatcher.CheckAccess() && _positionGeometry != null)
            {
                var x = perc * PART_Canvas.RenderSize.Width;
                var centerLineGeometry = (LineGeometry) ((Path) PART_Canvas.Children[1]).Data;
                x = centerLineGeometry.StartPoint.X +
                    perc * (centerLineGeometry.EndPoint - centerLineGeometry.StartPoint).X;

                _positionGeometry.StartPoint = new Point(x, 0);
                _positionGeometry.EndPoint = new Point(x, PART_Canvas.RenderSize.Height);
            }
            else
            {
                Dispatcher.InvokeAsync(() => UpdatePosition(perc));
            }
        }

        public void ClearWaveform()
        {
            PART_Canvas.Children.Clear();
        }

        protected override async void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);
            var cache = PART_Canvas.CacheMode as BitmapCache;
            if (cache != null)
            {
                cache.RenderAtScale = 1.0;
                await UpdateWaveformAsync();
            }
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }

        private void PART_Canvas_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var x = e.GetPosition(PART_Canvas).X;
            var perc = x / PART_Canvas.RenderSize.Width;
            if (PositionChanged != null)
                PositionChanged(this, new PositionChangedEventArgs(perc));
        }

        public event EventHandler<PositionChangedEventArgs> PositionChanged;
    }

    public class PositionChangedEventArgs : EventArgs
    {
        public PositionChangedEventArgs(double percentage)
        {
            Percentage = percentage;
        }

        public double Percentage { get; }
    }
}