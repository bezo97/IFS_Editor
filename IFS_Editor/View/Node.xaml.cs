using IFS_Editor.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace IFS_Editor.View
{
    /// <summary>
    /// Interaction logic for Node.xaml
    /// </summary>
    public partial class Node : UserControl
    {
        public XForm xf;

        public Node()
        {
            InitializeComponent();
            xf = new XForm();
        }

        public double PosX
        {
            get { return (Double) GetValue(Canvas.LeftProperty); }
            set { SetValue(Canvas.LeftProperty, value); }
        }
        public double PosY
        {
            get { return (Double)GetValue(Canvas.TopProperty); }
            set { SetValue(Canvas.TopProperty, value); }
        }

        public Node(XForm _xform)
        {
            InitializeComponent();
            Random r = new Random();
            PosX = r.NextDouble()*600;
            PosY = r.NextDouble()*600;
            Width = 100;
            Height = 100;
            xf = _xform;
        }

        protected override void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseLeftButtonDown(e);
            //mdown = true;
            //dx = ellipse.Margin.Left - e.GetPosition(this).X;
            //dy = ellipse.Margin.Top - e.GetPosition(this).Y;

            //pr
            Visibility = Visibility.Collapsed;
        }

        protected override void OnPreviewMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseLeftButtonDown(e);
            //mdown = false;
        }

        protected override void OnPreviewMouseMove(MouseEventArgs e)
        {
            base.OnPreviewMouseMove(e);

            /*if (mdown)
            {
                Margin = new Thickness(e.GetPosition(this).X + dx, e.GetPosition(this).Y + dy, Margin.Right, Margin.Bottom);
                calcCurve();
            }*/
        }

        private void calcCurve()
        {
            /*Point p1 = new Point(e1.Margin.Left + e1.Width / 2, e1.Margin.Top + e1.Height / 2);
            Point p2 = new Point(e2.Margin.Left + e2.Width / 2, e2.Margin.Top + e2.Height / 2);
            PathSegmentCollection seg = new PathSegmentCollection(1);
            seg.Add(new PolyBezierSegment(new PointCollection(3) { new Point((p1.X * 2 + p2.X) / 3, (p1.Y * 2 + p2.Y) / 3 + 30), new Point((p1.X + p2.X * 2) / 3, (p1.Y + p2.Y * 2) / 3 + 30), p2 }, true));
            curve.Data = new PathGeometry(new PathFigureCollection { new PathFigure(p1, seg, false) });
        */
    }

    }
}
