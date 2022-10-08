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
            else
                dtPicker.SelectedDate = DateTime.Today.AddDays(1);

            dtPicker.BlackoutDates.AddDatesInPast();
        }
        private void EventLoad(Event prevEvent)
        {
            txtName.Text = prevEvent.Name;
            txtDesc.Text = prevEvent.Description;
            dtPicker.SelectedDate = prevEvent.Date;
            chbRepeat.IsChecked = prevEvent.Repeat;
            txtHour.Text = prevEvent.Date.Hour.ToString();
            txtMin.Text = prevEvent.Date.Minute.ToString();
        }

        private void BtnConfirm_Click(object sender, RoutedEventArgs e)
        {
            if (!Kontrola()) return;

            if (Value == null)
                this.Value = new Event(txtName.Text, txtDesc.Text, new DateTime(dtPicker.SelectedDate.Value.Year, dtPicker.SelectedDate.Value.Month, dtPicker.SelectedDate.Value.Day, int.Parse(txtHour.Text), int.Parse(txtMin.Text), 0), chbRepeat.IsChecked.Value);
            else
                this.Value.ChangeEvent(txtName.Text, txtDesc.Text, dtPicker.SelectedDate, chbRepeat.IsChecked.Value, txtHour.Text, txtMin.Text);

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
                if (!dtPicker.SelectedDate.HasValue)
                    errors.AppendLine("Vyberte datum");
                if (string.IsNullOrWhiteSpace(txtHour.Text))
                    txtHour.Text = "0";
                if (string.IsNullOrWhiteSpace(txtMin.Text))
                    txtMin.Text = "0";
            }

            int intMin = 0;
            int intHour = 0;

            if (!string.IsNullOrWhiteSpace(txtMin.Text))
            {
                bool success = int.TryParse(txtMin.Text, out intMin);
                if (success)
                {
                    if (intMin < 0 || intMin > 59)
                        errors.AppendLine("Čas minut musí být v rozmezí 0-59");
                }
                else
                    errors.AppendLine("Čas minut musí být číslo");
            }

            if (!string.IsNullOrWhiteSpace(txtHour.Text))
            {
                bool success = int.TryParse(txtHour.Text, out intHour);
                if (success)
                {
                    if (intHour < 0 || intHour > 23)
                        errors.AppendLine("Čas hodin musí být v rozmezí 0-23");
                }
                else
                    errors.AppendLine("Čas hodin musí být číslo");
            }

            if (dtPicker.SelectedDate.HasValue)
            {
                if (dtPicker.SelectedDate.Value < DateTime.Today)
                    errors.AppendLine("Vybrané datum nemůže být starší než dnešní datum");
            }

            if (errors.Length == 0)
            {
                DateTime tempdt = dtPicker.SelectedDate.HasValue ? dtPicker.SelectedDate.Value : Value.Date;
                int tempHour = txtHour.Text == null ? Value.Date.Hour : intHour;
                int tempMin = txtMin.Text == null ? Value.Date.Minute : intMin;
                bool fSuccess = DateTime.TryParse(new DateTime(tempdt.Year, tempdt.Month, tempdt.Day, tempHour, tempMin, 0).ToString(), out DateTime tempDt);
                if (!fSuccess)
                    errors.AppendLine("Chybné datum!");
                else if (tempDt < DateTime.Now)
                    errors.AppendLine("Celkový čas nemůže být starší než dnešní čas");
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
