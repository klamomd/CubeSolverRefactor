using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using Prism;
using Prism.Commands;
using Prism.Mvvm;

namespace PieceMaskGenerator
{
    public class MainViewModel : BindableBase
    {
        // CONSTRUCTOR
        public MainViewModel()
        {
            _pieceMaskString = "0000000000000000";
            _togglePegExistsCommand = new DelegateCommand<string>(TogglePegExists);
        }


        // VARIABLES
        private string _pieceMaskString;
        private DelegateCommand<string> _togglePegExistsCommand;


        // PROPERTIES
        public string PieceMaskString
        {
            get { return _pieceMaskString; }
            set
            {
                if (value != _pieceMaskString)
                {
                    _pieceMaskString = value;
                    RaisePropertyChanged("PieceMaskString");
                }
            }
        }

        public DelegateCommand<string> TogglePegExistsCommand
        {
            get { return _togglePegExistsCommand; }
        }


        // FUNCTIONS
        // Takes in a string pegIndexString which should represent a peg index from 0-15, then toggles the peg at that index in PieceMaskString.
        public void TogglePegExists(string pegIndexString)
        {
            int pegIndex;

            // Do nothing if pegIndexString is null, not an int, or out of range.
            if (pegIndexString == null) return;
            if (!int.TryParse(pegIndexString, out pegIndex)) return;
            if (pegIndex < 0 || pegIndex > 15) return;

            // Flip the index's value.
            string newMask = "";
            for (int i = 0; i < 16; i++)
            {
                // Add swapped peg if we're at the pegIndex.
                if (i == pegIndex)
                {
                    newMask += (_pieceMaskString[i] == '0') ? "1" : "0";
                }
                // Otherwise, copy over the mask's existing value for that peg.
                else
                {
                    newMask += _pieceMaskString[i];
                }
            }

            // Assign the new piece mask string.
            PieceMaskString = newMask;
        }
    }
}
