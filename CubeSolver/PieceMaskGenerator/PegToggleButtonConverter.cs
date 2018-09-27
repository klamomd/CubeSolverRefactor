using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace PieceMaskGenerator
{
    public class PegToggleButtonConverter : IValueConverter
    {
        // Keep track of the latestPieceMaskString to pass through for when we need to convert back (horribly clunky, I know, but it works!).
        private string latestPieceMaskString = "0000000000000000";

        // Value is piece mask string.
        // Parameter is peg index in that string.
        // Returns true if the character at that index in the piece mask string is a 1.
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string pieceMaskString = (string)value;
            int pegIndex = 0;

            // Try to parse the parameter as a string holding an int value representing the peg index. Throw an appropriate exception if we fail or get an out of range index.
            if (!int.TryParse((string)parameter, out pegIndex))
            {
                string paramString = (parameter == null) ? "null" : parameter.ToString();
                throw new Exception("PegToggleButtonConverter failed to parse parameter: " + paramString);
            }
            if (pegIndex < 0 || pegIndex > 15) throw new ArgumentOutOfRangeException("parameter");

            // Save the pieceMaskString locally so we can access it in the ConvertBack method.
            latestPieceMaskString = pieceMaskString;

            // Return true if the peg at the given peg index is 1.
            return pieceMaskString[pegIndex] == '1';
        }

        // Value is true or false.
        // Parameter is peg index in that string.
        // Returns an updated piece mask string with the given peg toggled on or off (1 or 0).
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int pegIndex = 0;

            // Try to parse the parameter as a string holding an int value representing the peg index. Throw an appropriate exception if we fail or get an out of range index.
            if (!int.TryParse((string)parameter, out pegIndex))
            {
                string paramString = (parameter == null) ? "null" : parameter.ToString();
                throw new Exception("PegToggleButtonConverter failed to parse parameter: " + paramString);
            }
            if (pegIndex < 0 || pegIndex > 15) throw new ArgumentOutOfRangeException("parameter");

            // Update the piece mask string by taking the substring up to the given peg index, adding on a 1 or 0 as indicated by value, and then appending everything after the given peg index.
            StringBuilder updatedPieceMaskString = new StringBuilder();
            updatedPieceMaskString.Append(latestPieceMaskString.Substring(0, pegIndex));
            updatedPieceMaskString.Append((bool)value ? '1' : '0');
            updatedPieceMaskString.Append(latestPieceMaskString.Substring(pegIndex + 1));

            // Store the updated piece mask string in our latest piece mask string variable and return it.
            latestPieceMaskString = updatedPieceMaskString.ToString();
            return latestPieceMaskString;
        }
    }
}
