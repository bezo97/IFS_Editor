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

using GraphVizWrapper;


namespace IFS_Editor.View
{
    /// <summary>
    /// Interaction logic for NodeMap.xaml
    /// </summary>
    public partial class NodeMap : Canvas
    {
        Flame flame = new Flame();
        List<ConnectionArrow> arrows = new List<ConnectionArrow>();
        public bool weightedRs = true;//

        private XFormSideBar sidebar;

        private Node connectingStart;
        private List<Path> ArrowToMouse;
        public void beginConnecting(Node n)
        {
            if (connectingStart == null)
            {//csak akkor kezdunk ujba, ha eppen meg nem kotunk ossze
                connectingStart = n;
            }

        }
        public void endConnecting(Node n)
        {
            if(connectingStart!=null)
            {//eppen osszekotessel vegzunk
                if(n!=null)//nodehoz kotottuk, nem a semmibe
                    connectingStart.xf.SetConn(new Conn(n.xf, 0.5));
                connectingStart = null;//vege az osszekotesnek
                if (ArrowToMouse != null)
                    foreach (Path p in ArrowToMouse)
                        Children.Remove(p);//remove old arrow
                updateConnections();
            }
        }

        public double dx;//node mozgataskor mennyivel kattintottunk felre
        public double dy;

        private Node sn = null;
        public Node SelectedNode { get => sn; set {
                if (sn == value)
                    return;//mar ez van kijelolve, nem kell csinalni semmit
                if (sn != null)//nem onmaga, akkor eltunik az effekt
                    sn.EnableEffects(false);
                sn = value;
                if (sn != null)
                {
                    sn.EnableEffects(true);
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
            SetFlame(f);
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

        public Flame GetFlame()
        {
            return flame;
        }

        public void SetFlame(Flame f)
        {
            //SelectedNode = null;
            sidebar.Close();//TODO: ennek be kene zarodnia a SelectedNode=null;-val

            RemoveNodes();
            flame = f;
            foreach (XForm xf in flame.GetXForms())
            {//osszes xformhoz View-t rendelunk
                Node node = new Node(xf);
                Children.Add(node);
                node.Map = this;
            }
            GenerateLayout(Enums.RenderingEngine.Sfdp);//TODO: ötlet: user pref, hogy melyik a default elrendezés algo
            updateConnections();
        }

        private void RemoveConnections()
        {
            foreach (ConnectionArrow ca in arrows)
            {
                List<Path> p = ca.GetPaths();
                Children.Remove(p[0]);
                Children.Remove(p[1]);
                Children.Remove(p[2]);
            }
            arrows.Clear();
        }

        private void RemoveNodes()
        {
            RemoveConnections();
            foreach (Node n in GetNodeList())
            {
                Children.Remove(n);
            }
        }

        public void updateConnections()
        {
            RemoveConnections();

            //calculate new ones
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

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            e.Handled = true;
            if (connectingStart != null)
            {
                if (e.LeftButton == MouseButtonState.Pressed)
                {
                    if (ArrowToMouse != null)
                        foreach (Path p in ArrowToMouse)
                            Children.Remove(p);//remove old arrow
                    ArrowToMouse = ConnectionArrow.CalcByPoints(connectingStart.Pos, e.GetPosition(this), true);
                    foreach (Path p in ArrowToMouse)
                        Children.Insert(0, p);
                }
                else
                    endConnecting(null);
            }
        }


        public void GenerateLayout(GraphVizWrapper.Enums.RenderingEngine layoutType)
        {
            List<Node> Nodes = GetNodeList();
            //Cursor = Cursors.WaitCursor;
            double NodeR = 200.0 / Math.Sqrt(Nodes.Count);

            //put all connections into digraph, read by graphviz
            string digraph = "digraph{";
            foreach (Node n in Nodes)
            {
                foreach (Conn To in n.xf.GetConns())
                {
                    digraph += Nodes.IndexOf(n) + " -> " + Nodes.IndexOf(GetNodeFromXF(To.ConnTo)) + ";";
                }
            }
            digraph += "}";

            //graphviz library to generate layout
            var getStartProcessQuery = new GraphVizWrapper.Queries.GetStartProcessQuery();
            var getProcessStartInfoQuery = new GraphVizWrapper.Queries.GetProcessStartInfoQuery();
            var registerLayoutPluginCommand = new GraphVizWrapper.Commands.RegisterLayoutPluginCommand(getProcessStartInfoQuery, getStartProcessQuery);
            var wrapper = new GraphVizWrapper.GraphGeneration(getStartProcessQuery, getProcessStartInfoQuery, registerLayoutPluginCommand);
            wrapper.RenderingEngine = layoutType;//twopi vagy sfdp nez ki legjobban
            string[] output = System.Text.Encoding.Default.GetString(wrapper.GenerateGraph(digraph, GraphVizWrapper.Enums.GraphReturnType.Plain)).Replace('.', ',').Split('\n');

            int rowNr = 0;
            string[] firstRow = output[rowNr].Split(' ');
            double outWidth = Double.Parse(firstRow[2]);
            double outHeight = Double.Parse(firstRow[3]);
            while (++rowNr < output.Length)
            {
                string[] row = output[rowNr].Split(' ');
                if (row[0] == "node")
                {
                    Node n = Nodes[int.Parse(row[1])];
                    n.PosX = NodeR + (Double.Parse(row[2]) / outWidth) * (Width - 2 * NodeR);
                    n.PosY = NodeR + (Double.Parse(row[3]) / outHeight) * (Height - 2 * NodeR);
                }
                else
                    break;
            }

            updateConnections();
            //Cursor = Cursors.Default;
        }

        public RenderTargetBitmap GenerateImage()
        {//http://brianlagunas.com/wpf-copy-uielement-as-image-to-clipboard/
            double width = this.ActualWidth;
            double height = this.ActualHeight;
            RenderTargetBitmap bmpCopied = new RenderTargetBitmap((int)Math.Round(width), (int)Math.Round(height), 96, 96, PixelFormats.Default);
            DrawingVisual dv = new DrawingVisual();
            using (DrawingContext dc = dv.RenderOpen())
            {
                VisualBrush vb = new VisualBrush(this);
                dc.DrawRectangle(vb, null, new Rect(new Point(), new Size(width, height)));
            }
            bmpCopied.Render(dv);
            return bmpCopied;
        }


    }
}
