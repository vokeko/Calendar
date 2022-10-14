using System;
using System.Windows;
using System.Windows.Input;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.Win32;
using Calendar.Resources.Localization;

namespace Calendar
{
    /// <summary>
    /// Interaction logic for Options.xaml
    /// </summary>
    public partial class Options : Window
    {
        public List<CultureInfo> cultures { get; private set; } = new List<CultureInfo>();

        public Options()
        {
            InitializeComponent();

            cmbBackupFreq.ItemsSource = Enum.GetValues(typeof(EventList.BackupFrequency));
            cmbBackupFreq.SelectedIndex = 0;

            cultures.Add(new CultureInfo("en"));
            cultures.Add(new CultureInfo("cs"));

            cmbLanguage.ItemsSource = cultures;
            //cmbLanguage.SelectedIndex = 0;
        }

        private void BtnLoad_Click(object sender, RoutedEventArgs e)
        {
            EventList.LoadDataFrom();
        }
        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            EventList.SaveDataAs();
        }
        private void BtnExport_Click(object sender, RoutedEventArgs e)
        {
            EventList.ExportData();
        }

        private void TxtBackupPath_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog()
            {
                Title = Strings.BackupPath,
                DefaultExt = "bin",
                Filter = "*.bin|*.bin",
                CheckPathExists = true,
                FileName = "events.bin",
            };

            if (saveFileDialog.ShowDialog().GetValueOrDefault())
            {
                txtBackupPath.Text = saveFileDialog.FileName;
            }
        }

        private void BtnConfirm_Click(object sender, RoutedEventArgs e)
        {
            EventList.SetBrush(txtColor.Text);
            EventList.SetBackupInfo((EventList.BackupFrequency)cmbBackupFreq.SelectedItem, txtBackupPath.Text);
            
            this.Close();
        }
    }
}
