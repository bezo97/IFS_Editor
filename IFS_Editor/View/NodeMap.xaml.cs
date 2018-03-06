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
                            Path p = CalcCurvePath(n, To);
                            if (SelectedNode!=n)
                            {//legyen mas, ha a selectedet nezzuk eppen
                                p.StrokeThickness *= 0.25;
                            }
                            curves.Add(p);
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
