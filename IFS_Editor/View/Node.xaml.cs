﻿using IFS_Editor.Model;
using IFS_Editor.ViewModel;
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
using System.Windows.Media.Animation;
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
        //public XForm xf;
        NodeViewModel nvm;
        NodeMap map;//parent

        public Node()
        {
            nvm = new NodeViewModel();
            DataContext = nvm;

            InitializeComponent();

            Random r = new Random();
            PosX = r.NextDouble() * 600;
            PosY = r.NextDouble() * 600;
            Width = 100;
            Height = 100;

        }

        public Node(XForm _xform)
        {
            nvm = new NodeViewModel(_xform);
            DataContext = nvm;

            InitializeComponent();

            Random r = new Random();
            PosX = r.NextDouble() * 600;
            PosY = r.NextDouble() * 600;
            Width = 100;
            Height = 100;

        }

        //kor sugarat figyelembe veve a kozeppont
        public double PosX
        {
            get { return (Double)GetValue(Canvas.LeftProperty) + WeightedR; }
            set { SetValue(Canvas.LeftProperty, value - WeightedR); }
        }
        public double PosY
        {
            get { return (Double)GetValue(Canvas.TopProperty) + WeightedR; }
            set { SetValue(Canvas.TopProperty, value - WeightedR); }
        }

        public Point Pos { get { return new Point(PosX, PosY); } }

        //public float WeightedR { get { return (float)(((map.weightedRs) ? (0.5f + Math.Sqrt(xf.baseWeight < 10 ? xf.baseWeight : 10)) : 1.0f) * this.Width); } }
        public double WeightedR { get { return 100 / 2.0; } }//TODO weightedR megold

        public NodeMap Map { get => map ?? (NodeMap)this.Parent; set => map = value; }

        public XForm xf { get => nvm._XF; }

        public void EnableEffects(bool b)
        {//nodemap hivja, amikor ki van valasztva vagy nem
            DoubleAnimation appearAnimation;
            if (b) //appear
                appearAnimation = new DoubleAnimation(0.0, 0.5, TimeSpan.FromSeconds(0.2));
            else //disappear
                appearAnimation = new DoubleAnimation(0.5, 0.0, TimeSpan.FromSeconds(0.2));
            this.MyDropShadowEffect.BeginAnimation(System.Windows.Media.Effects.DropShadowEffect.OpacityProperty, appearAnimation);
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            e.Handled = true;//zoombox ne kapja meg

            //map.SelectedNode = this;
        }

        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonUp(e);
            e.Handled = true;//zoombox ne kapja meg

            if (map.SelectedNode != this)
                map.SelectedNode = this;
            else
                map.SelectedNode = null;

            map.endConnecting(this);
        }

        protected override void OnMouseRightButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseRightButtonDown(e);
            e.Handled = true;//zoombox ne kapja meg

            Map.BringNodeToFront(this);
            
            Map.dx = PosX - e.GetPosition(Map).X;
            Map.dy = PosY - e.GetPosition(Map).Y;

        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            
            if (e.RightButton == MouseButtonState.Pressed)
            {
                e.Handled = true;//zoombox ne kapja meg
                PosX = e.GetPosition(Map).X + Map.dx;
                PosY = e.GetPosition(Map).Y + Map.dy;
                Map.updateConnections();
            }
        }

        protected override void OnMouseLeave(MouseEventArgs e)
        {
            base.OnMouseLeave(e);
            e.Handled = true;//zoombox ne kapja meg
            if(e.LeftButton == MouseButtonState.Pressed)
                map.beginConnecting(this);
        }

    }
}
