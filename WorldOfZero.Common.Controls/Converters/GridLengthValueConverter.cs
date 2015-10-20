using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace WorldOfZero.Common.Controls.MVVM.Converters
{
    public class GridLengthValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is double)
            {
                return new GridLength((double)value);
            }
            else return new GridLength();
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return 0; //NOT IMPLEMENTED!
        }
    }
}
