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
using System.Windows.Shapes;

namespace Calendar
{
    /// <summary>
    /// Interaction logic for ShowEvent.xaml
    /// </summary>
    public partial class ShowEvent : Window
    {
        internal Event Value { get; }
        internal ShowEvent(Event prevEvent)
        {
            InitializeComponent();
            Value = prevEvent;
            Load();
        }

        private void Load()
        {
            txtName.Text = Value.Name;
            txtDesc.Text = Value.Description;
            txtDate.Text = Value.Date.ToString();
            chbRepeat.IsChecked = Value.Repeat;
        }
    }
}
