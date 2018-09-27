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
            _pieceMaskString = _emptyPieceMask;
            AddPieceCommand = new DelegateCommand(AddPiece, CanAddPiece);
            DeleteSelectedPieceCommand = new DelegateCommand(DeleteSelectedPiece, CanDeleteSelectedPiece);
            ClearPieceListCommand = new DelegateCommand(ClearPieceList, CanClearPieceList);
            SolvePuzzleCommand = new DelegateCommand(SolvePuzzle, CanSolvePuzzle);
        }


        // VARIABLES
        private const string _emptyPieceMask = "0000000000000000";
        private string _pieceMaskString;
        private int _selectedPieceMaskIndex = -1;
        private List<string> _pieceMasks = new List<string>();


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

        public List<string> PieceMasks
        {
            get { return _pieceMasks; }
            set
            {
                if (value != _pieceMasks)
                {
                    _pieceMasks = value;
                    RaisePropertyChanged("PieceMasks");
                    AddPieceCommand.RaiseCanExecuteChanged();
                    ClearPieceListCommand.RaiseCanExecuteChanged();
                    SolvePuzzleCommand.RaiseCanExecuteChanged();
                }
            }
        }

        public int SelectedPieceMaskIndex
        {
            get { return _selectedPieceMaskIndex; }
            set
            {
                if (value != _selectedPieceMaskIndex)
                {
                    _selectedPieceMaskIndex = value;
                    RaisePropertyChanged("SelectedPieceMask");
                    DeleteSelectedPieceCommand.RaiseCanExecuteChanged();
                }
            }
        }

        public DelegateCommand AddPieceCommand { get; }
        public DelegateCommand DeleteSelectedPieceCommand { get; }
        public DelegateCommand ClearPieceListCommand { get; }
        public DelegateCommand SolvePuzzleCommand { get; }

        // FUNCTIONS
        // Add the piece to the piece masks list and reset the piece mask string.
        public void AddPiece()
        {
            List<string> newMasksList = new List<string>(PieceMasks);
            newMasksList.Add(PieceMaskString);
            PieceMasks = newMasksList;
            PieceMaskString = _emptyPieceMask;
        }

        // Can only add more pieces when we have less than 6.
        public bool CanAddPiece()
        {
            return PieceMasks.Count < 6;
        }

        // Remove the piece at the selected index.
        public void DeleteSelectedPiece()
        {
            List<string> newMasksList = new List<string>(PieceMasks);
            newMasksList.RemoveAt(SelectedPieceMaskIndex);
            PieceMasks = newMasksList;
        }

        // Can only delete a piece when one is selected.
        public bool CanDeleteSelectedPiece()
        {
            return SelectedPieceMaskIndex > -1;
        }

        // Reset the list of pieces.
        public void ClearPieceList()
        {
            PieceMasks = new List<string>();
        }

        // Can only reset the list of pieces when we have 1 or more pieces.
        public bool CanClearPieceList()
        {
            return PieceMasks.Count > 0;
        }

        // TODO: Implement this so that it passes the piece masks to the CubeSolver and returns/displays the first solution (or all solutions).
        public void SolvePuzzle()
        {
            throw new NotImplementedException();
        }

        // Can only solve the puzzle if we have all 6 pieces.
        public bool CanSolvePuzzle()
        {
            return PieceMasks.Count == 6;
        }
    }
}
