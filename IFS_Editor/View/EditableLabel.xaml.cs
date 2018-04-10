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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace IFS_Editor.View
{
    /// <summary>
    /// Interaction logic for EditableLabel.xaml
    /// </summary>
    public partial class EditableLabel : UserControl
    {

        public EditableLabel()
        {
            InitializeComponent();
            //DataContext = vm;
        }

        private string EscapeName = "Unnamed Flame";

        private void DisplayText_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            EscapeName = EditText.Text;
            DisplayText.Visibility = Visibility.Collapsed;
            EditText.Visibility = Visibility.Visible;
            EditText.SelectAll();

            /*https://stackoverflow.com/questions/13955340/keyboard-focus-does-not-work-on-text-box-in-wpf
             * WPF can have multiple Focus Scopes,
             * so multiple elements can have Logical Focus (IsFocused = true),
             * however only one element can have Keyboard Focus 
             * */
            Dispatcher.BeginInvoke(DispatcherPriority.Input,
               new Action(delegate ()
               {
                   EditText.Focus();         // Set Logical Focus
                   Keyboard.Focus(EditText); // Set Keyboard Focus
               }));
        }


        private void EditText_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (EditText.Text.Equals(""))
                    EditText.Text = EscapeName;
                DisplayText.Visibility = Visibility.Visible;
                EditText.Visibility = Visibility.Collapsed;
            }
            if (e.Key == Key.Escape)
            {
                EditText.Text = EscapeName;
                DisplayText.Visibility = Visibility.Visible;
                EditText.Visibility = Visibility.Collapsed;
            }
        }

        private void EditText_LostFocus(object sender, RoutedEventArgs e)
        {
            if (EditText.Visibility == Visibility.Visible)
            {
                EditText.Text = EscapeName;
                DisplayText.Visibility = Visibility.Visible;
                EditText.Visibility = Visibility.Collapsed;
            }
        }
    }
}
