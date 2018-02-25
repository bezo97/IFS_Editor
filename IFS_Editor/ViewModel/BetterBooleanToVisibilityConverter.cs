using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace IFS_Editor.View
{
    public /*sealed*/ class BetterBooleanToVisibilityConverter : BooleanConverter<Visibility>
    {
        public BetterBooleanToVisibilityConverter() :
            base(Visibility.Visible, Visibility.Collapsed)
        { }
    }
}
