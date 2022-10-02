 using System;
using System.Collections.Generic;
using System.Globalization;
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

namespace Calendar
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(DispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            dispatcherTimer.Start();
        }

        private void DispatcherTimer_Tick(object sender, EventArgs e)
        {
            lblTime.Content = DateTime.Now.ToString();
            lblDay.Content = DateTime.Now.ToString("dddd", CultureInfo.CurrentCulture).FirstCharToUpper();
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            NewEvent form = new NewEvent(null);
            {
                Activate();
            }
            form.Owner = this;

            if (!form.ShowDialog().GetValueOrDefault()) return;

            EventList.Events.Add(form.Value);

            MessageBox.Show("Událost úspěšně přidána");
            Refresh_EventLists();
        }
        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            ChooseEvent formC = new ChooseEvent();
            {
                Activate();
            }
            formC.Owner = this;

            if (!formC.ShowDialog().GetValueOrDefault()) return;


            NewEvent form = new NewEvent(formC.Value);
            {
                Activate();
            }
            form.Owner = this;

            if (!form.ShowDialog().GetValueOrDefault()) return;

            MessageBox.Show("Událost úspěšně upravena");
            Refresh_EventLists();
        }
        private void BtnRemove_Click(object sender, RoutedEventArgs e)
        {
            ChooseEvent form = new ChooseEvent();
            {
                Activate();
            }
            form.Owner = this;
            if (!form.ShowDialog().GetValueOrDefault()) return;

            EventList.Events.Remove(form.Value);

            MessageBox.Show("Událost úspěšně vymazána");
            Refresh_EventLists();
        }
        private void BtnShowAll_Click(object sender, RoutedEventArgs e)
        {
            BrowseEvents form = new BrowseEvents();
            {
                Activate();
            }
            form.Owner = this;
            form.ShowDialog();
            Refresh_EventLists();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DispatcherTimer_Tick(sender, e);
            string chyba = EventList.LoadData();
            if (chyba != null)
                MessageBox.Show(chyba);
            Refresh_EventLists();
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            bool success = EventList.SaveData();
            if (!success)
                MessageBox.Show("Chyba při ukládání dat");
        }

        private void Refresh_EventLists()
        {
            EventList.SortEvents();
            lstTodayEvents.ItemsSource = EventList.Events.Where(x => x.Date >= DateTime.Today && x.Date < DateTime.Today.AddDays(1)).ToList();
            lstNextEvents.ItemsSource = EventList.Events.Where(x => x.Date >= DateTime.Today.AddDays(1) && x.Date < DateTime.Today.AddDays(6)).ToList();
        }

        private void Lst_DoubleClick(object sender, MouseButtonEventArgs e)
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
    }
}
