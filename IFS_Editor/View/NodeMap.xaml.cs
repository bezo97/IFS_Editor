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
    /// Interaction logic for NodeMap.xaml
    /// </summary>
    public partial class NodeMap : Canvas
    {
        private Flame flame = new Flame();
        List<Path> curves = new List<Path>();

        public NodeMap()
        {
            InitializeComponent();
        }

        public void AddXForm()
        {
            Node node = new Node(flame.AddXForm(true));
            Children.Add(node);
            InvalidateVisual();
        }

        public List<Node> GetNodeList()
        {
            List<Node> l = new List<Node>();
            foreach (UIElement n in Children)
            {
                if (n is Node)//JAJ
                    l.Add((Node)n);
            }
            return l;
        }

        protected override void OnRender(DrawingContext dc)
        {
            base.OnRender(dc);

            //remove old curves
            foreach (Path p in curves)
            {
                Children.Remove(p);
            }
            curves.Clear();

            //calculate new curves
            List<Node> nodes = GetNodeList();
            foreach (Node n in nodes)
            {
                foreach(Conn c in n.xf.Conns) //nem szep, valami mast kene kitalalni
                {
                    foreach (Node To in nodes)
                    {
                        if(To.xf == c.ConnTo)
                        {
                            curves.Add(CalcCurvePath(n,To));
                            //nem tudjuk egybol itt hozzaadni a Childrenhez, mert a foreachekkel osszeakad
                            break;
                        }
                    }
                }
            }

            //add new curves
            foreach (Path p in curves)
            {
                Children.Insert(0, p);//elejere, hogy a Nodeok mogott legyen
            }

        }

        public Path CalcCurvePath(Node e1, Node e2)
        {
            
            Path curve = new Path();
            curve.Stroke = Brushes.Black;
            curve.StrokeThickness = 3;
            Point p1 = new Point(e1.PosX + e1.Width / 2, e1.PosY + e1.Height / 2);
            Point p2 = new Point(e2.PosX + e2.Width / 2, e2.PosY + e2.Height / 2);
            PathSegmentCollection seg = new PathSegmentCollection(1);
            seg.Add(new PolyBezierSegment(new PointCollection(3) { new Point((p1.X * 2 + p2.X) / 3, (p1.Y * 2 + p2.Y) / 3 + 30), new Point((p1.X + p2.X * 2) / 3, (p1.Y + p2.Y * 2) / 3 + 30), p2 }, true));
            curve.Data = new PathGeometry(new PathFigureCollection { new PathFigure(p1, seg, false) });
            return curve;
        }

    }
}
