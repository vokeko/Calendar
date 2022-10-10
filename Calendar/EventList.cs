using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Windows.Media;
using System.Threading;
using System.Globalization;
using Microsoft.Win32;

namespace Calendar
{
    static class EventList
    {
        public static List<Event> Events { get; private set; } = new List<Event>();
        internal static SolidColorBrush HighlightBrush { get; private set; } = Brushes.Pink;
        internal static CultureInfo Language { get; private set; } = CultureInfo.CurrentCulture;

        internal static string LoadData(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath)) filePath = "events.bin";

            try
            {
                using (Stream stream = File.Open(filePath, FileMode.OpenOrCreate))
                {
                    if (stream.Length == 0) return "Nový datový objekt úspěšně založen";

                    BinaryFormatter bin = new BinaryFormatter();
                    {
                        object deseri = bin.Deserialize(stream);

                        switch (deseri)
                        {
                            case BinaryTempEvents data:
                                Events.AddRange(data.TempEvents);
                                SetBrush(data.TempBrush);
                                Language = data.Culture;
                                break;
                            case List<Event> events:
                                Events.AddRange(events);
                                break;
                            default:
                                return "Nekompatibilní datový objekt";
                        }
                    }
                }
                DiscardOldEvents();
                return null;
            }
            catch
            {
                return "Chyba při načítání dat";
            }
        }
        internal static bool SaveData(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath)) filePath = "events.bin";

            SortEvents();
            try
            {
                using (Stream stream = File.Open(filePath, FileMode.Create))
                {
                    BinaryFormatter bin = new BinaryFormatter();
                    bin.Serialize(stream, new BinaryTempEvents());
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        internal static void ExportData()
        {
            SortEvents();

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Title = "Exportování souboru";
            saveFileDialog.DefaultExt = "txt";
            saveFileDialog.Filter = "txt files (*.txt)|*.txt";
            saveFileDialog.CheckPathExists = true;

            if (saveFileDialog.ShowDialog().GetValueOrDefault())
            {
                using (StreamWriter writer = new StreamWriter(saveFileDialog.FileName))
                {
                    writer.Write(ToText());
                    writer.Close();
                }
            }
        }

        internal static void SaveDataAs()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Title = "Ukládání dat";
            saveFileDialog.DefaultExt = "bin";
            saveFileDialog.Filter = "Binary files (*.bin)|*.bin";
            saveFileDialog.CheckPathExists = true;
            saveFileDialog.FileName = "events.bin";

            if (saveFileDialog.ShowDialog().GetValueOrDefault())
            {
                bool success = SaveData(saveFileDialog.FileName);
                if (!success)
                    MessageBox.Show("Chyba při ukládání dat");
            }
        }
        internal static void LoadDataFrom()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Načítání dat";
            openFileDialog.DefaultExt = "bin";
            openFileDialog.Filter = "Binary files (*.bin)|*.bin";
            openFileDialog.CheckPathExists = true;
            openFileDialog.FileName = "events.bin";

            if (openFileDialog.ShowDialog().GetValueOrDefault())
            {
                Events.Clear();
                string chyba = LoadData(openFileDialog.FileName);
                if (chyba != null)
                    MessageBox.Show(chyba);
            }
        }

        private static void DiscardOldEvents()
        {
            List<Event> oldEvents = Events.Where(x => x.Date < DateTime.Today).ToList();
            List<Event> newEvents = new List<Event>();
            foreach (Event oldEvent in oldEvents)
            {
                Events.Remove(oldEvent);
                if (oldEvent.Repeat)
                    Events.Add(new Event(oldEvent.Name, oldEvent.Description, oldEvent.Date.AddYears(1), true));
            }
            Events = Events.Where(x => x.Date >= DateTime.Today).OrderBy(x => x.Date).ToList();
        }
        internal static void SortEvents()
        {
            Events = Events.OrderBy(x => x.Date).ToList();
        }

        internal static void SetBrush(string tempbrush)
        {
            if (tempbrush.IsValidHexCode())
                HighlightBrush = (SolidColorBrush)new BrushConverter().ConvertFrom(tempbrush);
        }

        private static string ToText()
        {
            BinaryTempEvents tempEvents = new BinaryTempEvents();
            StringBuilder ret = new StringBuilder();
            ret.AppendLine("Kalendář");
            ret.AppendLine("--------");
            ret.AppendLine("Barva: "+ tempEvents.TempBrush);
            ret.AppendLine("Jazyk: " + tempEvents.Culture);
            ret.AppendLine("--------");
            foreach(Event e in tempEvents.TempEvents)
            {
                ret.AppendLine(string.Format("{0} - {1}, {2}, {3}", e.Name, e.Description, e.Date, e.Repeat));
            }
            return ret.ToString();
        }

        [Serializable]
        private class BinaryTempEvents
        {
            internal List<Event> TempEvents { get; }
            internal string TempBrush { get; }
            internal CultureInfo Culture { get; }
            public BinaryTempEvents()
            {
                TempEvents = Events;
                TempBrush = HighlightBrush.Color.ToString();
                Culture = Language;
            }
        }
    }
}


