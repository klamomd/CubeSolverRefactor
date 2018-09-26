using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PieceMaskGenerator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
    }

    public class PegToggleButtonConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool isChecked = (bool)value;
            int pegIndex = 0;

            // Try to parse the parameter as a string holding an int value representing the peg index. Throw an appropriate exception if we fail.
            if (!int.TryParse((string)parameter, out pegIndex))
            {
                string paramString = (parameter == null) ? "null" : parameter.ToString();
                throw new Exception("PegToggleButtonConverter failed to parse parameter: " + paramString);
            }


            Console.WriteLine("Stepped into Convert");
            throw new NotImplementedException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
