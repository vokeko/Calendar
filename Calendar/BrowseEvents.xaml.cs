using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Calendar
{
    /// <summary>
    /// Interaction logic for BrowseEvents.xaml
    /// </summary>
    public partial class BrowseEvents : Window
    {
        public BrowseEvents()
        {
            InitializeComponent();
        }

        private void CldBrowse_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            RefreshEvents();
            Mouse.Capture(null);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            cldBrowse.BlackoutDates.AddDatesInPast();
            RefreshEvents();
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (cldBrowse.SelectedDate != null)
            {
                NewEvent form = new NewEvent(null);
                {
                    Title = "Nová událost";
                    Activate();
                }
                form.Owner = this;
                form.dtPicker.SelectedDate = cldBrowse.SelectedDate;

                if (!form.ShowDialog().GetValueOrDefault()) return;

                EventList.Events.Add(form.Value);
                RefreshEvents();
            }
            else
                MessageBox.Show("Prosím vyberte datum");
        }
        private void BtnRemove_Click(object sender, RoutedEventArgs e)
        {
            if (lstChosenDate.SelectedItem is Event chosenEvent)
            {
                EventList.Events.Remove(chosenEvent);
                RefreshEvents();
            }
            else
                MessageBox.Show("Prosím vyberte položku");
        }
        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (lstChosenDate.SelectedItem is Event chosenEvent)
            {
                NewEvent form = new NewEvent(chosenEvent);
                {
                    Title = "Edit události";
                    Activate();
                }
                form.Owner = this;

                if (!form.ShowDialog().GetValueOrDefault()) return;

                MessageBox.Show("Událost úspěšně upravena");
                RefreshEvents();
            }
            else
                MessageBox.Show("Prosím vyberte položku");
        }
        private void LstChosenDate_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (((FrameworkElement)e.OriginalSource).DataContext is Event item)
            {
                ShowEvent form = new ShowEvent(item);
                {
                    Activate();
                }
                form.Owner = this;
                form.ShowDialog();
            }
        }

        private void RefreshEvents()
        {
            if (cldBrowse.SelectedDate != null)
                lstChosenDate.ItemsSource = EventList.Events.Where(x => x.Date >= cldBrowse.SelectedDate && x.Date < cldBrowse.SelectedDate.Value.AddDays(1)).ToList();
        }

        private void CalendarDayButton_Loaded(object sender, RoutedEventArgs e)
        {
            CalendarDayButton button = (CalendarDayButton)sender;
            DateTime date = (DateTime)button.DataContext;
            HighlightDay(button, date);
            button.DataContextChanged += new DependencyPropertyChangedEventHandler(CalendarButton_DataContextChanged);
        }

        private void HighlightDay(CalendarDayButton button, DateTime date)
        {
            if (EventList.Events.Any(x => x.Date == date))
                button.Background = EventList.HighlightBrush;
            else
                button.Background = Brushes.Transparent;
            //(SolidColorBrush)new BrushConverter().ConvertFrom("#eeeeee");
        }

        private void CalendarButton_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            CalendarDayButton button = (CalendarDayButton)sender;
            DateTime date = (DateTime)button.DataContext;
            HighlightDay(button, date);
        }
    }
}
