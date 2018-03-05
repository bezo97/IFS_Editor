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
        NodeMap map;//parent

        public Node()
        {
            InitializeComponent();

            Random r = new Random();
            PosX = r.NextDouble() * 600;
            PosY = r.NextDouble() * 600;
            Width = 100;
            Height = 100;

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

        public NodeMap Map { get => map?? (NodeMap)this.Parent; set => map = value; }

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

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);

        }

        protected override void OnMouseRightButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseRightButtonDown(e);

            Map.BringNodeToFront(this);
            
            Map.dx = PosX - e.GetPosition(Map).X;
            Map.dy = PosY - e.GetPosition(Map).Y;

        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (e.RightButton == MouseButtonState.Pressed)
            {
                PosX = e.GetPosition(Map).X + Map.dx;
                PosY = e.GetPosition(Map).Y + Map.dy;
                Map.updateConnections();
            }
        }

    }
}
