//using IFS_Editor.Model;
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
using IFS_Editor.ViewModel;

namespace IFS_Editor.View
{
    /// <summary>
    /// Interaction logic for NodeMap.xaml
    /// </summary>
    public partial class NodeMap : Canvas
    {
        FLVM flame;
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
                    connectingStart.GetXF().SetConn(new ConnVM(n.GetXF(), 1.0));
                connectingStart = null;//vege az osszekotesnek
                if (ArrowToMouse != null)
                    foreach (Path p in ArrowToMouse)
                        Children.Remove(p);//remove old arrow
                if (n != null)
                    Flame.Selection = n.GetXF();
                //updateConnections();
            }
        }

        public double dx;//node mozgataskor mennyivel kattintottunk felre
        public double dy;

        public XFormSideBar Sidebar { get => sidebar; set => sidebar = value; }
        public FLVM Flame { get => flame; set
            {
                sidebar.Close(false);
                RemoveNodes();
                flame = value;
                DataContext = Flame;
                foreach (XFVM xf in Flame.GetXForms())
                {//osszes ViewModelhez View-t rendelunk
                    Node node = new Node(xf, this);
                    Children.Add(node);
                }
                GenerateLayout(Enums.RenderingEngine.Sfdp);//TODO: Prefs - melyik a default elrendezés algo
                flame.PropertyChanged += PropertyChanged;
                updateConnections();
            }
        }

        public NodeMap()
        {
            InitializeComponent();
            //Flame = new FLVM();
            //Flame.PropertyChanged += PropertyChanged;
            XFVM.StaticPropertyChanged += PropertyChanged;
        }

        public NodeMap(FLVM f)
        {
            InitializeComponent();
            Flame = f;
            XFVM.StaticPropertyChanged += PropertyChanged;
            //Flame.PropertyChanged += PropertyChanged;
            
        }

        private void PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {//sender: flame vagy null (static XFVM)
            if (e.PropertyName == "Selection")
            {
                if (Flame.Selection != null)
                {
                    BringNodeToFront(GetNodeFromXF(Flame.Selection));
                    sidebar.Show(Flame.Selection);
                }
                else
                    sidebar.Close(false);
                updateConnections();
            }
            if (e.PropertyName == "WeightedSize" || e.PropertyName == "BaseSize")
                updateConnections();
        }

        public XFVM GetSelection()
        {
            return Flame.Selection;
        }

        public void SetSelection(XFVM nxf)
        {
            Flame.Selection = nxf;
            
        }

        public Node AddXForm()
        {
            XFVM newxf = Flame.AddXForm(true);
            Node node = new Node(newxf, this);
            Children.Add(node);
            Flame.Selection = newxf;
            updateConnections();
            return node;
        }

        public Node AddXForm(XFVM xf)
        {
            Node node = new Node(xf, this);
            Children.Add(node);
            Flame.Selection = xf;
            updateConnections();
            return node;
        }

        public Node AddLinkedXForm()
        {
            XFVM xf = Flame.AddXForm(false);
            Node node = new Node(xf, this);
            xf.Name = "linked";
            Children.Add(node);
            //linkeles
            Flame.Selection.Opacity = 0.0;
            foreach (ConnVM c in Flame.Selection.GetConns())
            {
                xf.SetConn(c);
            }
            Flame.Selection.ClearConns();
            Flame.Selection.SetConn(new ConnVM(node.GetXF(), 1));

            Flame.Selection = xf;
            updateConnections();
            return node;
        }

        public void RemoveXForm(XFVM f)
        {
            Node n = GetNodeFromXF(f);
            Flame.RemoveXForm(f);
            if (Flame.Selection == f)
                Flame.Selection = null;
            Children.Remove(n);
        }

        public void RemoveSelected()
        {
            if (Flame.Selection == null)
                return;
            Flame.RemoveXForm(Flame.Selection);
            Children.Remove(GetNodeFromXF(Flame.Selection));
            Flame.Selection = null;
            updateConnections();
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

        public Node GetNodeFromXF(XFVM xf)
        {
            List<Node> l = GetNodeList();
            foreach (Node n in l)
            {
                if (n.GetXF() == xf)
                    return n;
            }

            return null;
            //pl ha a Selectionre kerdezunk ra, az lehet null.. de elv nincs ilyen
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

        /// <summary>
        /// nyilak újraszámolása
        /// </summary>
        public void updateConnections()
        {
            RemoveConnections();

            //calculate new ones
            List<Node> nodes = GetNodeList();
            foreach (Node n in nodes)
            {
                foreach (ConnVM c in n.GetXF().GetConns()) //nem szep, valami mast kene kitalalni
                {
                    foreach (Node To in nodes)
                    {
                        if (To.GetXF() == c.ConnTo)
                        {
                            bool isHighlighted = false;
                            if (Flame.Selection != null)
                                isHighlighted = (n.GetXF() == Flame.Selection);
                            arrows.Add(new ConnectionArrow(n, To, isHighlighted));
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

        /// <summary>
        /// GraphViz library használata layout generálásra
        /// </summary>
        /// <param name="layoutType">SFDP a legjobb</param>
        public void GenerateLayout(GraphVizWrapper.Enums.RenderingEngine layoutType)
        {
            List<Node> Nodes = GetNodeList();
            if (Nodes.Count < 1)
                return;
            //Cursor = Cursors.WaitCursor;
            double NodeR = XFVM.BaseSize/2 / Math.Sqrt(Nodes.Count);

            //put all connections into digraph, read by graphviz
            string digraph = "digraph{";
            foreach (Node n in Nodes)
            {
                foreach (ConnVM To in n.GetXF().GetConns())
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

        /// <summary>
        /// képet készít a Contentjéből
        /// </summary>
        /// <returns></returns>
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
