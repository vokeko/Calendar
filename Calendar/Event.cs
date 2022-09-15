using System;

namespace Calendar
{
    [Serializable]
    internal class Event
    {
        internal string Name { get; private set; }
        internal string Description { get; private set; }
        internal DateTime Date { get; private set; }
        internal bool Repeat { get; private set; }
        internal Event(string _name, string _description, DateTime _date, bool _repeat)
        {
            Name = _name;
            Description = _description;
            Date = _date;
            Repeat = _repeat;
        }

        internal void ChangeEvent(string _name, string _description, Nullable<DateTime> _date, Nullable<bool> _repeat)
        {
            if (!string.IsNullOrWhiteSpace(_name))
                Name = _name;
            if (!string.IsNullOrWhiteSpace(_description))
                Description = _description;
            if (_date.HasValue)
                Date = _date.Value;
            if (_repeat.HasValue)
                Repeat = _repeat.Value;
        }
    }
}
