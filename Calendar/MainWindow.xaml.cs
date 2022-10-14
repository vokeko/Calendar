using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Calendar.Resources.Localization;

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
            lblTime.Content = DateTime.Now.ToString(CultureInfo.CurrentCulture);
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

            MessageBox.Show(Strings.SuccessfulAdd);
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

            MessageBox.Show(Strings.SuccessfulEdit);
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

            MessageBox.Show(Strings.SuccessfulDelete);
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
        private void BtnSettings_Click(object sender, RoutedEventArgs e)
        {
            Options form = new Options();
            {
                Activate();
            }
            form.Owner = this;
            form.ShowDialog();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DispatcherTimer_Tick(sender, e);
            string chyba = EventList.LoadData(null);
            if (chyba != null)
                MessageBox.Show(chyba);
            Refresh_EventLists();
            SetButtons();
            EventList.FileBackup();
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            bool success = EventList.SaveData(null);
            if (!success)
                MessageBox.Show(Strings.LoadError);
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

        private void SetButtons()
        {
            ContextMenu menu = new ContextMenu();
            MenuItem item = new MenuItem();
            item.Header = Strings.ShowItem;
            //item.Icon = Properties.Resources.view;
            item.Click += Menu_View;
            menu.Items.Add(item);

            item = new MenuItem();
            item.Header = Strings.EditItem;
            //item.Icon = Properties.Resources.edit;
            item.Click += Menu_Edit;
            menu.Items.Add(item);

            item = new MenuItem();
            item.Header = Strings.DeleteItem;
            //item.Icon = Properties.Resources.del;
            item.Click += Menu_Delete;
            menu.Items.Add(item);

            ContextMenu = menu;
        }

        private void Menu_Delete(object sender, RoutedEventArgs e)
        {
            if (this.lstNextEvents.SelectedItem is Event next)
            {
                EventList.Events.Remove(next);
            }
            else if (this.lstTodayEvents.SelectedItem is Event today)
            {
                EventList.Events.Remove(today);
            }
            else
            {
                return;
            }
            MessageBox.Show(Strings.SuccessfulDelete);
            Refresh_EventLists();
        }
        private void Menu_Edit(object sender, RoutedEventArgs e)
        {
            Event ev;
            if (this.lstNextEvents.SelectedItem is Event next)
            {
                ev = next;
            }
            else if (this.lstTodayEvents.SelectedItem is Event today)
            {
                ev = today;
            }
            else
            {
                return;
            }

            NewEvent form = new NewEvent(ev);
            {
                Activate();
            }
            form.Owner = this;

            if (!form.ShowDialog().GetValueOrDefault()) return;

            MessageBox.Show(Strings.SuccessfulEdit);
            Refresh_EventLists();
        }
        private void Menu_View(object sender, RoutedEventArgs e)
        {
            Event ev;
            if (this.lstNextEvents.SelectedItem is Event next)
            {
                ev = next;
            }
            else if (this.lstTodayEvents.SelectedItem is Event today)
            {
                ev = today;
            }
            else
            {
                return;
            }

            ShowEvent form = new ShowEvent(ev);
            {
                Activate();
            }
            form.Owner = this;
            form.ShowDialog();
        }
    }
}
