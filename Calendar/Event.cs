using System;

namespace Calendar
{
    [Serializable]
    internal class Event
    {
        public string Name { get; private set; }
        internal string Description { get; private set; }
        public DateTime Date { get; set; }
        internal bool Repeat { get; private set; }
        internal Event(string _name, string _description, DateTime _date, bool _repeat)
        {
            Name = _name;
            Description = _description;
            Date = _date;
            Repeat = _repeat;
        }

        internal void ChangeEvent(string _name, string _description, Nullable<DateTime> _date, Nullable<bool> _repeat, string _txtHour, string _txtMin)
        {
            if (!string.IsNullOrWhiteSpace(_name))
                Name = _name;
            if (!string.IsNullOrWhiteSpace(_description))
                Description = _description;
            if (_repeat.HasValue)
                Repeat = _repeat.Value;

            if (!string.IsNullOrWhiteSpace(_txtHour) || !string.IsNullOrWhiteSpace(_txtMin) || _date.HasValue)
                Date = new DateTime(_date.HasValue ? _date.Value.Year : Date.Year, _date.HasValue ? _date.Value.Month : Date.Month, _date.HasValue ? _date.Value.Day : Date.Day, !string.IsNullOrWhiteSpace(_txtHour) ? int.Parse(_txtHour) : Date.Hour, !string.IsNullOrWhiteSpace(_txtMin) ? int.Parse(_txtMin) : Date.Minute, 0);
        }
    }
}
