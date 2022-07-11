namespace KeeP;

internal class Task : INotifyPropertyChanged
{
    private string _name = string.Empty;
    [JsonProperty(PropertyName = "name")]
    public string Name
    {
        get => _name;
        set
        {
            _name = value;
            OnPropertyChanged(nameof(Name));
        }
    }

    private string _description = string.Empty;
    [JsonProperty(PropertyName = "description")]
    public string Description
    {
        get => _description;
        set
        {
            _description = value;
            OnPropertyChanged(nameof(Description));
        }
    }

    [JsonProperty(PropertyName = "creationDate")]
    public DateTime CreationDate { get; } = DateTime.Now;
    //подразумевается, что дату создания нельзя изменить

    private DateTime _deadlinedate = DateTime.Now;
    [JsonProperty(PropertyName = "deadlineDate")]
    public DateTime DeadlineDate
    {
        get => _deadlinedate;
        set
        {
            _deadlinedate = value;
            OnPropertyChanged(nameof(DeadlineDate));
        }
    }

    private TaskStatus _status = TaskStatus.Created;
    [JsonProperty(PropertyName = "status")]
    public TaskStatus Status
    {
        get => _status;
        set
        {
            _status = value;
            OnPropertyChanged("Status");
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
