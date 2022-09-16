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
                Title = "Nová událost";
                Activate();
                Owner = this;
            }

            if (!form.ShowDialog().GetValueOrDefault()) return;

            EventList.Events.Add(form.Value);
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
        }

        private void BtnRemove_Click(object sender, RoutedEventArgs e)
        {
        }

        private void BtnShowAll_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder stringg = new StringBuilder();
            foreach (Event oneEvent in EventList.Events)
            {
                stringg.AppendLine(string.Format("{0}, {1}, {2}, {3}", oneEvent.Name, oneEvent.Description, oneEvent.Date.ToString(), oneEvent.Repeat));
            }
            MessageBox.Show(stringg.ToString());
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DispatcherTimer_Tick(sender, e);
            EventList.LoadData();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            EventList.SaveData();
        }
    }
}
