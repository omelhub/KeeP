namespace KeeP;

internal enum TaskStatus
{
    Created,
    Started,
    Paused,
    Completed
}

internal class Logics
{
    private const string Path = "tasks.json";

    public Logics()
    {
        Application.Current.MainWindow.Closing += MainWindow_Closing;
        TasksSaveBtn = new RelayCommand(SaveToFile);
        TasksCreateBtn = new RelayCommand(() => EditTask(true));
        TaskDeleteBtn = new RelayCommand(DeleteTask);
        TaskEditBtn = new RelayCommand(() => EditTask(false));
        if (!File.Exists(Path)) File.WriteAllText(Path, "[]");
        ReadFromFile();
    }

    public ObservableCollection<Task> Tasks { get; set; } = new();
    public Task ListViewSelected { get; set; }

    public RelayCommand TasksSaveBtn { get; set; }
    public RelayCommand TasksCreateBtn { get; set; }
    public RelayCommand TaskDeleteBtn { get; set; }
    public RelayCommand TaskEditBtn { get; set; }
    public RelayCommand FilterSaveBtn { get; set; }

    private bool Saved { get; set; } = true;


    public string EditorName { get; set; }
    public string EditorDescription { get; set; }
    public TaskStatus EditorStatus { get; set; }
    public List<TaskStatus> EditorStatuses { get; set; } = new();
    public DateTime EditorEndDate { get; set; }

    public RelayCommand EditorClose { get; set; }
    public RelayCommand EditorSave { get; set; }

    private void DeleteTask()
    {
        Saved = false;
        Tasks.Remove(ListViewSelected);
    }

    private void EditTask(bool create)
    {
        var edit = new Edit();
        edit.DataContext = this;
        EditorClose = new RelayCommand(() => { edit.Close(); });
        EditorSave = new RelayCommand(() =>
        {
            var ShowError = (string msg) =>
            {
                MessageBox.Show(msg, "Ошибка!", MessageBoxButton.OK,
                    MessageBoxImage.Warning);
            };
            if (create)
            {

                if (EditorName == "")
                {
                    ShowError("Укажите имя задачи");
                    return;
                }

                if (EditorDescription == "")
                {
                    ShowError("Укажите описание задачи");
                    return;
                }

                if (Tasks.Select(task => task.Name).Count(str => str == EditorName) > 0)
                {
                    ShowError("Данная задача уже есть в списке!");
                    return;
                }

                Tasks.Add(new Task
                {
                    Name = EditorName,
                    Description = EditorDescription,
                    //CreationDate = DateTime.Now,
                    DeadlineDate = EditorEndDate,
                    Status = EditorStatus
                });
            }
            else
            {
                if (ListViewSelected.Name != EditorName && Tasks.Select(task => task.Name).Count(str => str == EditorName) > 0)
                {
                    ShowError("Данная задача уже есть в списке!");
                    return;

                }
                ListViewSelected.Name = EditorName;
                ListViewSelected.Description = EditorDescription;
                ListViewSelected.Status = EditorStatus;
                ListViewSelected.DeadlineDate = EditorEndDate;
            }

            Saved = false;
            edit.Close();
        });
        EditorStatuses.Clear();
        EditorStatuses.Add(TaskStatus.Created);
        EditorStatuses.Add(TaskStatus.Started);
        if (!create)
        {
            if (ListViewSelected == null) return;

            if (ListViewSelected.Status != TaskStatus.Created)
            {
                EditorStatuses.Add(TaskStatus.Paused);
                EditorStatuses.Add(TaskStatus.Completed);
            }

            EditorName = ListViewSelected.Name;
            EditorDescription = ListViewSelected.Description;
            EditorStatus = ListViewSelected.Status;
            EditorEndDate = ListViewSelected.DeadlineDate;
        }
        else
        {
            EditorName = "";
            EditorDescription = "";
            EditorStatus = TaskStatus.Created;
            EditorEndDate = DateTime.Now;
        }

        edit.ShowDialog();
    }

    private void SaveToFile()
    {
        File.WriteAllText(Path, JsonConvert.SerializeObject(Tasks));
        Saved = true;
    }

    private void MainWindow_Closing(object? sender, CancelEventArgs e)
    {
        if (Saved) return;

        switch (MessageBox.Show("Ваши изменения не сохранены, сохранить или отменить и выйти?", "Пятая лабы ХЫ",
                    MessageBoxButton.YesNoCancel, MessageBoxImage.Question))
        {
            case MessageBoxResult.Yes:
                SaveToFile();
                break;

            case MessageBoxResult.No:
                break;

            case MessageBoxResult.Cancel:
                e.Cancel = true;
                break;
        }
    }

    private void ReadFromFile()
    {
        (JsonConvert.DeserializeObject<List<Task>>(File.ReadAllText(Path)) ?? new List<Task>()).ForEach(Tasks.Add);
    }
}
