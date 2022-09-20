﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace Calendar
{
    static class EventList
    {
        public static List<Event> Events { get; private set; } = new List<Event>();
        internal static bool LoadData()
        {
            try
            {
                using (Stream stream = File.Open("events.bin", FileMode.OpenOrCreate))
                {
                    if (stream.Length == 0) return false;

                    BinaryFormatter bin = new BinaryFormatter();
                    {
                        object deseri = bin.Deserialize(stream);

                        switch (deseri)
                        {
                            case List<Event> _events:
                                Events.AddRange(_events);
                                break;
                            default:
                                return false;
                        }
                    }
                }
                DiscardOldEvents();
                return true;
            }
            catch
            {
                return false;
            }
        }
        internal static bool SaveData()
        {
            Events = Events.OrderBy(x => x.Date).ToList();
            try
            {
                using (Stream stream = File.Open("events.bin", FileMode.Create))
                {
                    BinaryFormatter bin = new BinaryFormatter();
                    bin.Serialize(stream, Events);
                }
                return true;
            }
            catch
            {
                return false;
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
    }
}


