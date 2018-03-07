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

        private XFormSideBar sidebar;



        public double dx;//node mozgataskor mennyivel kattintottunk felre
        public double dy;

        private Node sn = null;
        public Node SelectedNode { get => sn; set {
                sn = value;
                if (sn != null)
                {
                    BringNodeToFront(sn);
                    sidebar.Show(sn.xf);
                }
                updateConnections();
            } }

        public XFormSideBar Sidebar { get => sidebar; set => sidebar = value; }

        public NodeMap()
        {
            InitializeComponent();
            flame = new Flame();
        }

        public NodeMap(Flame f)
        {
            InitializeComponent();
            flame = f;
            foreach(XForm xf in flame.XForms)
            {//osszes xformhoz View-t rendelunk
                Node node = new Node(xf);
                Children.Add(node);
                node.Map = this;
            }
            updateConnections();
        }

        public void AddXForm()
        {
            Node node = new Node(flame.AddXForm(true));
            Children.Add(node);
            node.Map = this;
            SelectedNode = node;
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

        public void updateConnections()
        {
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
                foreach (Conn c in n.xf.Conns) //nem szep, valami mast kene kitalalni
                {
                    foreach (Node To in nodes)
                    {
                        if (To.xf == c.ConnTo)
                        {
                            curves.AddRange(CalcCurvePath(n, To));
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

            InvalidateVisual();
        }

        public void BringNodeToFront(Node node)
        {
            List<Node> nl = GetNodeList();
            foreach (Node n in nl)
            {
                int prev = (int)n.GetValue(Canvas.ZIndexProperty);
                if (prev > (int)node.GetValue(Canvas.ZIndexProperty))
                    n.SetValue(Canvas.ZIndexProperty, prev - 1);
            }
            node.SetValue(Canvas.ZIndexProperty, nl.Count);
        }

        public List<Path> CalcCurvePath(Node e1, Node e2)
        {
            List<Path> output_list = new List<Path>();

            Path curve = new Path
            {
                Stroke = Brushes.Silver,
                StrokeThickness = 1
            };
            if(e1==SelectedNode)
            {//legyen mas, ha a selectedet nezzuk eppen
                curve.StrokeThickness = 4.0;
                curve.Stroke = Brushes.Black;
            }
            Point p1 = new Point(e1.PosX + e1.Width / 2, e1.PosY + e1.Height / 2);
            Point p2 = new Point(e2.PosX + e2.Width / 2, e2.PosY + e2.Height / 2);

            double xdir = p2.X - p1.X;
            double ydir = p2.Y - p1.Y;
            /*double tmpLen = Math.Sqrt(xdir * xdir + ydir * ydir);
            xdir /= tmpLen; //nem is kell normalizalni az atan2 nek
            ydir /= tmpLen;*/
            double angle = Math.Atan2(ydir, xdir) + Math.PI / 4;//TODO: ez lehetne egy setting


            PathSegmentCollection seg = new PathSegmentCollection(1);
            seg.Add(new PolyBezierSegment(new PointCollection(3) {
                new Point((p1.X * 2 + p2.X) / 3 + 30*Math.Cos(angle), (p1.Y * 2 + p2.Y) / 3 + 30*Math.Sin(angle)),
                new Point((p1.X + p2.X * 2) / 3 + 30*Math.Cos(angle), (p1.Y + p2.Y * 2) / 3 + 30*Math.Sin(angle)), p2 }, true));
            curve.Data = new PathGeometry(new PathFigureCollection { new PathFigure(p1, seg, false) });

            PathGeometry flattened = curve.Data.GetFlattenedPathGeometry();//bezier -> line path
            double minL = 9999;
            PointCollection ffig = ((PolyLineSegment)flattened.Figures[0].Segments[0]).Points;
            double halfX = (ffig[0].X + ffig[ffig.Count - 1].X) / 2;
            double halfY = (ffig[0].Y + ffig[ffig.Count - 1].Y) / 2;
            int iP = 0;
            for (; iP < ffig.Count; iP++)
            {//a bezier kozepet megkeressuk
                double nextL = Math.Min(minL, Math.Sqrt(Math.Pow(halfX - ffig[iP].X, 2) + Math.Pow(halfY - ffig[iP].Y, 2)));
                if (nextL < minL)
                    minL = nextL;
                else
                    break;
            }

            output_list.Add(curve);

            //ket nyil kiszamolasa
            if(e1!=e2)
            {//valaki mashoz mutat a nyil
                Point mid = ffig[iP];
                Point prev = ffig[iP-1];
                Point dir = new Point(mid.X - prev.X, mid.Y - prev.Y);
                angle = Math.Atan2(dir.Y, dir.X);
                Path nyilbal = new Path()
                {
                    Stroke = curve.Stroke,
                    StrokeThickness = curve.StrokeThickness
                };
                Path nyiljobb = new Path()
                {
                    Stroke = curve.Stroke,
                    StrokeThickness = curve.StrokeThickness
                };
                double guiSize = 75;
                nyilbal.Data = new LineGeometry(mid, new Point(mid.X - Math.Cos(angle + 0.5) * guiSize / 5.0f, mid.Y - Math.Sin(angle + 0.5) * guiSize / 5.0f));
                nyiljobb.Data = new LineGeometry(mid, new Point(mid.X - Math.Cos(angle - 0.5) * guiSize / 5.0f, mid.Y - Math.Sin(angle - 0.5) * guiSize / 5.0f));
                output_list.Add(nyilbal);
                output_list.Add(nyiljobb);
            }
            else
            {
                //TODO
            }


            return output_list;
        }

    }
}
