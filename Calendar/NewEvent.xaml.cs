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
    /// Interaction logic for NewEvent.xaml
    /// </summary>
    public partial class NewEvent : Window
    {

        internal Event Value { get; private set; }
        internal NewEvent(Event prevEvent)
        {
            InitializeComponent();
            if (prevEvent != null)
            {
                Value = prevEvent;
                EventLoad(prevEvent);
            }
        }
        private void EventLoad(Event prevEvent)
        {
            txtName.Text = prevEvent.Name;
            txtDesc.Text = prevEvent.Description;
            txtDate.Text = prevEvent.Date.ToString();
            chbRepeat.IsChecked = prevEvent.Repeat;
        }

        private void BtnConfirm_Click(object sender, RoutedEventArgs e)
        {
            if (!Kontrola()) return;

            if (Value == null)
                this.Value = new Event(txtName.Text, txtDesc.Text, DateTime.Now, chbRepeat.IsChecked.Value);
            else
            {
                this.Value.ChangeEvent(txtName.Text, txtDesc.Text, DateTime.Now, chbRepeat.IsChecked.Value);
            }
            this.DialogResult = true;
            this.Close();
        }

        private bool Kontrola()
        {
            StringBuilder errors = new StringBuilder();
            if (Value == null)
            {
                if (string.IsNullOrWhiteSpace(txtName.Text))
                    errors.AppendLine("Vyplňte název");
                if (string.IsNullOrWhiteSpace(txtDate.Text))
                    errors.AppendLine("Vyplňte datum");
                if (string.IsNullOrWhiteSpace(txtDesc.Text))
                    errors.AppendLine("Vyplňte popis");
            }

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
