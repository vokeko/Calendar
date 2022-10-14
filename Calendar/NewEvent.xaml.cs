using System;
using System.Text;
using System.Windows;
using Calendar.Resources.Localization;

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
                    errors.AppendLine(Strings.EnterTitle);
                if (!dtPicker.SelectedDate.HasValue)
                    errors.AppendLine(Strings.EnterDate);
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
                        errors.AppendLine(Strings.MinutesRange);
                }
                else
                    errors.AppendLine(Strings.MinutesNaN);
            }

            if (!string.IsNullOrWhiteSpace(txtHour.Text))
            {
                bool success = int.TryParse(txtHour.Text, out intHour);
                if (success)
                {
                    if (intHour < 0 || intHour > 23)
                        errors.AppendLine(Strings.HoursRange);
                }
                else
                    errors.AppendLine(Strings.HoursNaN);
            }

            if (dtPicker.SelectedDate.HasValue)
            {
                if (dtPicker.SelectedDate.Value < DateTime.Today)
                    errors.AppendLine(Strings.OlderThanToday);
            }

            if (errors.Length == 0)
            {
                DateTime tempdt = dtPicker.SelectedDate.HasValue ? dtPicker.SelectedDate.Value : Value.Date;
                int tempHour = txtHour.Text == null ? Value.Date.Hour : intHour;
                int tempMin = txtMin.Text == null ? Value.Date.Minute : intMin;
                bool fSuccess = DateTime.TryParse(new DateTime(tempdt.Year, tempdt.Month, tempdt.Day, tempHour, tempMin, 0).ToString(), out DateTime tempDt);
                if (!fSuccess)
                    errors.AppendLine(Strings.WrongDate);
                else if (tempDt < DateTime.Now)
                    errors.AppendLine(Strings.OlderThanNow);
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
