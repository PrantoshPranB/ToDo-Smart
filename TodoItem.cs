using System;
using System.ComponentModel;

public class TodoItem : INotifyPropertyChanged
{
    public DateTime Date { get; set; }
    public DateTime Time { get; set; }
    public string Item { get; set; }
    public string Status { get; set; }

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}