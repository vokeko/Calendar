using System;
using System.Windows;
using System.Windows.Input;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.Win32;
using System.Linq;
using Calendar.Resources.Localization;

namespace Calendar
{
    /// <summary>
    /// Interaction logic for Options.xaml
    /// </summary>
    public partial class Options : Window
    {
        private List<CultureInfo> cultures { get; set; } = new List<CultureInfo>();

        public Options()
        {
            InitializeComponent();
            Dictionary<EventList.BackupFrequency, string> dicFreq = new Dictionary<EventList.BackupFrequency, string>();
            foreach(EventList.BackupFrequency enumFreq in Enum.GetValues(typeof(EventList.BackupFrequency)))
            {
                dicFreq.Add(enumFreq, enumFreq.GetFrequencyString());
            }

            cmbBackupFreq.ItemsSource = dicFreq;
            cmbBackupFreq.SelectedValuePath = "Key";
            cmbBackupFreq.DisplayMemberPath = "Value";
            cmbBackupFreq.SelectedValue = EventList.BackupFreq;

            cultures.Add(new CultureInfo("en-US"));
            cultures.Add(new CultureInfo("cs-CZ"));

            cmbLanguage.ItemsSource = cultures;
            cmbLanguage.DisplayMemberPath = "DisplayName";
            cmbLanguage.SelectedItem = CultureInfo.CurrentCulture;

            txtColor.Text = EventList.HighlightBrush.Color.ToString().Remove(1, 2);
            if (EventList.BackupFreq != EventList.BackupFrequency.None) txtBackupPath.Text = EventList.BackupPath;
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
                Filter = Strings.BinExtension + " *.bin|*.bin",
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
            EventList.SetBackupInfo((EventList.BackupFrequency)cmbBackupFreq.SelectedValue, txtBackupPath.Text);
            CultureInfo selectedCulture = (CultureInfo)cmbLanguage.SelectedItem;

            if (cultures.Any(x => x.LCID == selectedCulture.LCID))
            {
                CultureInfo.CurrentCulture = selectedCulture;
                CultureInfo.DefaultThreadCurrentCulture = selectedCulture;
                CultureInfo.CurrentUICulture = selectedCulture;
            }

            this.Close();
        }

        private void txtColor_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            ColorPicker form = new ColorPicker();
            {
                Activate();
            }
            form.Owner = this;
            form.ShowDialog();
            if (form.PickedColor != null)
            {
                this.txtColor.Text = form.PickedColor;
            }
        }
    }
}
