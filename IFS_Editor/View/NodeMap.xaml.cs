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
        List<ConnectionArrow> arrows = new List<ConnectionArrow>();
        public bool weightedRs = true;//

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
            foreach(XForm xf in flame.GetXForms())
            {//osszes xformhoz View-t rendelunk
                Node node = new Node(xf);
                Children.Add(node);
                node.Map = this;
            }
            updateConnections();
        }

        public Node AddXForm()
        {
            Node node = new Node(flame.AddXForm(true));
            Children.Add(node);
            node.Map = this;
            SelectedNode = node;
            updateConnections();
            return node;
        }

        public Node AddXForm(XForm xf)
        {
            Node node = new Node(xf);
            Children.Add(node);
            node.Map = this;
            SelectedNode = node;
            updateConnections();
            return node;
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

        public Node GetNodeFromXF(XForm xf)
        {
            List<Node> l = GetNodeList();
            foreach (Node n in l)
            {
                if (n.xf == xf)
                    return n;
            }
            return AddXForm(xf);//elv ilyen nincs
        }

        public void updateConnections()
        {
            //remove old curves
            foreach (ConnectionArrow ca in arrows)
            {
                List<Path> p = ca.GetPaths();
                Children.Remove(p[0]);
                Children.Remove(p[1]);
                Children.Remove(p[2]);
            }
            arrows.Clear();

            //calculate new curves
            List<Node> nodes = GetNodeList();
            foreach (Node n in nodes)
            {
                foreach (Conn c in n.xf.GetConns()) //nem szep, valami mast kene kitalalni
                {
                    foreach (Node To in nodes)
                    {
                        if (To.xf == c.ConnTo)
                        {
                            arrows.Add(new ConnectionArrow(n, To, (n == SelectedNode)));
                            //nem tudjuk egybol itt hozzaadni a Childrenhez, mert a foreachekkel osszeakad
                            break;
                        }
                    }
                }
            }

            //add new curves
            foreach (ConnectionArrow ca in arrows)
            {
                List<Path> p = ca.GetPaths();
                Children.Insert(0, p[0]);//elejere, hogy a Nodeok mogott legyen
                Children.Insert(0, p[1]);
                Children.Insert(0, p[2]);
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

    }
}
