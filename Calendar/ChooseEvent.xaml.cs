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
    /// Interaction logic for ChooseEvent.xaml
    /// </summary>
    public partial class ChooseEvent : Window
    {
        internal Event Value { get; private set; }

        public ChooseEvent()
        {
            InitializeComponent();

            foreach (Event oneEvent in EventList.Events)
            {
                lstEvents.Items.Add(oneEvent.Name);
            }
            lstEvents.SelectedIndex = 0;
        }

        private void LstEvents_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lstEvents.SelectedIndex < 0) lstEvents.SelectedIndex = 0;

            txtName.Text = EventList.Events[lstEvents.SelectedIndex].Name;
            txtDesc.Text = EventList.Events[lstEvents.SelectedIndex].Description;
            txtDate.Text = EventList.Events[lstEvents.SelectedIndex].Date.ToString();
            chbRepeat.IsChecked = EventList.Events[lstEvents.SelectedIndex].Repeat;
        }

        private void BtnConfirm_Click(object sender, RoutedEventArgs e)
        {
            if (!Kontrola()) return;
            this.Value = EventList.Events[lstEvents.SelectedIndex];

            this.DialogResult = true;
            this.Close();
        }

        private bool Kontrola()
        {
            StringBuilder errors = new StringBuilder();

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
