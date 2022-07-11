namespace KeeP;

internal class ConverterStatusToColor : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        switch ((TaskStatus)value)
        {
            case TaskStatus.Created: return Brushes.Silver;
            case TaskStatus.Started: return Brushes.Goldenrod;
            case TaskStatus.Paused: return Brushes.Crimson;
            case TaskStatus.Completed: return Brushes.Green;
        }

        return Brushes.Black;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
