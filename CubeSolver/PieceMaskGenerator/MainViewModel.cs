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
        }

        // VARIABLES
        private string _pieceMaskString;


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


        // * TODO: MASK STRING BINDING
        // * TODO: TOGGLE BUTTON BINDING + CONVERTER

            /* ---- LEFTOFF HERE ----
             * : Implemented function below.
             * : Need to bind textbox to mask string property.
             * : Need to bind peg buttons with converter that uses function below or similar.
             * 
             * 
             * 
             */ 



        // FUNCTIONS
        public void TogglePegExists(int pegIndex)
        {
            if (pegIndex < 0 || pegIndex > 15) throw new ArgumentOutOfRangeException();

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



        ////private void MyClickFunction(object sender, RoutedEventArgs e)
        ////{
        ////    var tag = ((Button)sender).Tag;
        ////    MessageBox.Show(tag.ToString());
        ////}
    }
}
