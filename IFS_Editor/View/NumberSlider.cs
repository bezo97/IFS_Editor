using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace IFS_Editor.View
{
    public class NumberSlider : TextBox
    {
        private double min;
        private double max;
        private bool textmode = false;

        public NumberSlider()
        {
            //ez arra kell, hogy a textbox visualjat overridolhassuk
            DefaultStyleKey = typeof(NumberSlider);
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);

            drawingContext.DrawRectangle(Brushes.Gray, new Pen(Brushes.DarkGray, 1), new System.Windows.Rect(0, 0, this.ActualWidth, this.ActualHeight));
            drawingContext.DrawText(
               new FormattedText(this.Text,
                  CultureInfo.GetCultureInfo("en-us"),
                  System.Windows.FlowDirection.LeftToRight,
                  new Typeface("Verdana"),
                  12, System.Windows.Media.Brushes.Black),
                  new System.Windows.Point(0, 0));
        }

        protected override void OnPreviewMouseDown(MouseButtonEventArgs e)
        {
            textmode = !textmode;

            if (textmode)
            {
                //DefaultStyleKey = typeof(TextBox);
                base.OnMouseDown(e);
            }
            else
            {
                //DefaultStyleKey = typeof(NumberSlider);
            }

            this.InvalidateVisual();

        }

        protected override void OnTextChanged(TextChangedEventArgs e)
        {
            base.OnTextChanged(e);
            this.InvalidateVisual();
        }
    }
}
