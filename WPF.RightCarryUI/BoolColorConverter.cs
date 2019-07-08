using System;
using System.Windows.Data;
using System.Globalization;

namespace WPF.RightCarryUI
{
    [ValueConversion(typeof(bool), typeof(string))]
    public class BoolStringConverter : IValueConverter
    {
        public object Convert(object value,
            Type targetType, object parameter, CultureInfo culture)
        {
            bool temp = (bool)value;
            if (temp == true) return "出库";
            else return "入库";
        }
        //反转换方法，将字符串转换为日期类型
        public object ConvertBack(object value,
            Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    [ValueConversion(typeof(bool), typeof(string))]
    public class BoolAlarmColorConverter : IValueConverter
    {
        public object Convert(object value,
            Type targetType, object parameter, CultureInfo culture)
        {
            bool temp = (bool)value;
            if (temp == true) return "Red";
            else return "Transparent";
        }
        //反转换方法，将字符串转换为日期类型
        public object ConvertBack(object value,
            Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
