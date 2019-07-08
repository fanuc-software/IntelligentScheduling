using System;
using System.Windows.Data;
using System.Globalization;

namespace WPF.RightCarryUI
{
    [ValueConversion(typeof(int), typeof(string))]
    public class BoolFinStringConverter : IValueConverter
    {
        public object Convert(object value,
            Type targetType, object parameter, CultureInfo culture)
        {
            int temp = (int)value;
            if (temp == 1) return "等待执行";
            else if (temp == 2) return "正常结束";
            else if (temp == 3) return "执行错误";
            else return "";
        }
        //反转换方法，将字符串转换为日期类型
        public object ConvertBack(object value,
            Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    [ValueConversion(typeof(int), typeof(string))]
    public class BoolFinColorConverter : IValueConverter
    {
        public object Convert(object value,
            Type targetType, object parameter, CultureInfo culture)
        {
            int temp = (int)value;
            if (temp == 1) return "Yellow";
            else if (temp == 2) return "Green";
            else if (temp == 3) return "Red";
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
