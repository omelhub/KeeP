namespace KeeP;

internal class ConverterStatusToText : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        switch ((TaskStatus)value)
        {
            case TaskStatus.Created: return "Создана";
            case TaskStatus.Started: return "Находится в работе";
            case TaskStatus.Paused: return "Приостановлена";
            case TaskStatus.Completed: return "Выполнена";
        }

        return "";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
