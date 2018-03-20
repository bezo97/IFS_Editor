using IFS_Editor.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace IFS_Editor.View
{
    public class ConnectionArrow
    {//nyil ami osszekot ket node ot
        //nem lehet a Path-bol szarmazni, mert sealed, ezert wrapper osztalyt keszitunk
        Path curve;
        Path nyilbal;
        Path nyiljobb;

        Node e1;
        Node e2;
        bool selected;

        static double guiSize = 75;//

        public ConnectionArrow(Node _e1, Node _e2, bool _selected)
        {
            e1 = _e1;
            e2 = _e2;
            selected = _selected;
        }

        public List<Path> GetPaths()
        {
            if (curve != null/* || !recalculate*/)
                return new List<Path>() { curve, nyilbal, nyiljobb };
            else
            {
                List<Path> res = CalcPaths();
                return res;
            }
                
        }

        public List<Path> CalcPaths()
        {
            /*List<Path> init = InitPaths(selected);
            curve = init[0];
            nyilbal = init[1];
            nyiljobb = init[2];*/

            if (e1 != e2)
            {//valaki mashoz mutat a nyil
             /*Point p1 = e1.Pos;
             Point p2 = e2.Pos;

             double xdir = p2.X - p1.X;
             double ydir = p2.Y - p1.Y;
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

             //ket nyil kiszamolasa
             Point mid = ffig[iP];
             Point prev = ffig[iP - 1];
             Point dir = new Point(mid.X - prev.X, mid.Y - prev.Y);
             angle = Math.Atan2(dir.Y, dir.X);
             nyilbal.Data = new LineGeometry(mid, new Point(mid.X - Math.Cos(angle + 0.5) * guiSize / 5.0, mid.Y - Math.Sin(angle + 0.5) * guiSize / 5.0));
             nyiljobb.Data = new LineGeometry(mid, new Point(mid.X - Math.Cos(angle - 0.5) * guiSize / 5.0, mid.Y - Math.Sin(angle - 0.5) * guiSize / 5.0));
         */
                List<Path> res = CalcByPoints(e1.Pos, e2.Pos, selected);
                curve = res[0];
                nyilbal = res[1];
                nyiljobb = res[2];
            }
            else
            {//sajat maga
                List<Path> init = InitPaths(selected);
                curve = init[0];
                nyilbal = init[1];
                nyiljobb = init[2];

                //calc loopback angle
                double dirx = 0;
                double diry = 0;
                foreach (Conn c in e1.xf.GetConns())
                {
                    Node Toa = e1.Map.GetNodeFromXF(c.ConnTo);
                    if (e1.xf != c.ConnTo)
                    {
                        dirx += Toa.PosX - e1.PosX;
                        diry += Toa.PosY - e1.PosY;
                    }
                }
                double loopbackAngle = Math.Atan2(-diry, -dirx);



                double r = e1.WeightedR / 5.0 * 4.0;
                double cosa = Math.Cos(loopbackAngle);
                double sina = Math.Sin(loopbackAngle);
                Point mid = new Point(e1.PosX + r * cosa, e1.PosY + r * sina);
                curve.Data = new EllipseGeometry(mid, r, r);
                mid = new Point(e1.PosX + 2 * r * cosa, e1.PosY + 2 * r * sina);
                double a = Math.Atan2(sina, cosa) - 3.1415 / 4.0;
                cosa = Math.Cos(a);
                sina = Math.Sin(a);
                nyilbal.Data = new LineGeometry(mid, new Point(mid.X - guiSize / 5.0 * cosa, mid.Y - guiSize / 5.0 * sina));
                nyiljobb.Data = new LineGeometry(mid, new Point(mid.X - guiSize / 5.0 * sina, mid.Y + guiSize / 5.0 * cosa));
                

            }

            //

            curve.MouseDown += OnClick;
            nyilbal.MouseDown += OnClick;
            nyiljobb.MouseDown += OnClick;

            return new List<Path>() { curve, nyilbal, nyiljobb };
        }

        private void OnClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            XForm xf1 = e1.xf;
            XForm xf2 = e2.xf;
            xf1.SetConn(new Conn(xf2, 0));
            e1.Map.updateConnections();
        }

        private static List<Path> InitPaths(bool selected)
        {
            Path curve = new Path
            {
                Stroke = Brushes.Silver,
                StrokeThickness = 1
            };
            if (selected)
            {//legyen mas, ha a selectedet nezzuk eppen
                curve.StrokeThickness = 4.0;
                curve.Stroke = Brushes.Black;
            }
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
            return new List<Path>() { curve, nyilbal, nyiljobb };
        }

        public static List<Path> CalcByPoints(Point p1, Point p2, bool selected)
        {
            List<Path> init = InitPaths(selected);
            Path curve = init[0];
            Path nyilbal = init[1];
            Path nyiljobb = init[2];

            double xdir = p2.X - p1.X;
            double ydir = p2.Y - p1.Y;
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

            //ket nyil kiszamolasa
            Point mid = ffig[iP];
            Point prev = ffig[iP - 1];
            Point dir = new Point(mid.X - prev.X, mid.Y - prev.Y);
            angle = Math.Atan2(dir.Y, dir.X);
            nyilbal.Data = new LineGeometry(mid, new Point(mid.X - Math.Cos(angle + 0.5) * guiSize / 5.0, mid.Y - Math.Sin(angle + 0.5) * guiSize / 5.0));
            nyiljobb.Data = new LineGeometry(mid, new Point(mid.X - Math.Cos(angle - 0.5) * guiSize / 5.0, mid.Y - Math.Sin(angle - 0.5) * guiSize / 5.0));

            return new List<Path>() { curve, nyilbal, nyiljobb };
        }
    }
}
