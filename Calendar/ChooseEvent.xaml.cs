using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

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

            lstEvents.SelectedIndex = 0;
        }

        private void LstEvents_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lstEvents.SelectedIndex < 0) lstEvents.SelectedIndex = 0;

            if (lstEvents.SelectedItem is Event selected)
            {
                txtName.Text = selected.Name;
                txtDesc.Text = selected.Description;
                txtDate.Text = selected.Date.ToString();
                chbRepeat.IsChecked = selected.Repeat;
            }
        }

        private void BtnConfirm_Click(object sender, RoutedEventArgs e)
        {
            this.Value = EventList.Events[lstEvents.SelectedIndex];

            this.DialogResult = true;
            this.Close();
        }
    }
}
