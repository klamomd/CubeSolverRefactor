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
using CubeSolver;
using System.Windows;

namespace PieceMaskGenerator
{
    public class MainViewModel : BindableBase
    {
        // CONSTRUCTOR
        public MainViewModel()
        {
            _pieceMaskString = _emptyPieceMask;
            AddPieceCommand = new DelegateCommand(AddPiece, CanAddPiece);
            ResetPieceCommand = new DelegateCommand(ResetPiece);
            DeleteSelectedPieceCommand = new DelegateCommand(DeleteSelectedPiece, CanDeleteSelectedPiece);
            ClearPieceListCommand = new DelegateCommand(ClearPieceList, CanClearPieceList);
            SolvePuzzleCommand = new DelegateCommand(SolvePuzzle, CanSolvePuzzle);
        }


        // VARIABLES
        private const string _emptyPieceMask = "0000000000000000";
        private string _pieceMaskString;
        private int _selectedPieceMaskIndex = -1;
        private List<string> _pieceMasks = new List<string>();
        private List<string> _solutionPieceMasks = new List<string>();


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

        public List<string> SolutionPieceMasks
        {
            get { return _solutionPieceMasks; }
            set
            {
                if (value != _solutionPieceMasks)
                {
                    _solutionPieceMasks = value;
                    RaisePropertyChanged("SolutionPieceMasks");
                    RaisePropertyChanged("Piece0MaskString");
                    RaisePropertyChanged("Piece1MaskString");
                    RaisePropertyChanged("Piece2MaskString");
                    RaisePropertyChanged("Piece3MaskString");
                    RaisePropertyChanged("Piece4MaskString");
                    RaisePropertyChanged("Piece5MaskString");
                }
            }
        }

        public string Piece0MaskString
        {
            get
            {
                if (_solutionPieceMasks == null || _solutionPieceMasks.Count != 6) return _emptyPieceMask;
                else return _solutionPieceMasks[0];
            }
        }

        public string Piece1MaskString
        {
            get
            {
                if (_solutionPieceMasks == null || _solutionPieceMasks.Count != 6) return _emptyPieceMask;
                else return _solutionPieceMasks[1];
            }
        }

        public string Piece2MaskString
        {
            get
            {
                if (_solutionPieceMasks == null || _solutionPieceMasks.Count != 6) return _emptyPieceMask;
                else return _solutionPieceMasks[2];
            }
        }

        public string Piece3MaskString
        {
            get
            {
                if (_solutionPieceMasks == null || _solutionPieceMasks.Count != 6) return _emptyPieceMask;
                else return _solutionPieceMasks[3];
            }
        }

        public string Piece4MaskString
        {
            get
            {
                if (_solutionPieceMasks == null || _solutionPieceMasks.Count != 6) return _emptyPieceMask;
                else return _solutionPieceMasks[4];
            }
        }

        public string Piece5MaskString
        {
            get
            {
                if (_solutionPieceMasks == null || _solutionPieceMasks.Count != 6) return _emptyPieceMask;
                else return _solutionPieceMasks[5];
            }
        }

        public DelegateCommand AddPieceCommand { get; }
        public DelegateCommand ResetPieceCommand { get; }
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

        // Reset the piece mask back to all 0s.
        public void ResetPiece()
        {
            PieceMaskString = _emptyPieceMask;
        }

        // TODO: Implement this so that it passes the piece masks to the CubeSolver and returns/displays the first solution (or all solutions).
        public void SolvePuzzle()
        {
            List<Piece> unsolvedPieces = GeneratePiecesFromMasks(PieceMasks);
            List<Piece> solvedPiecesList = new List<Piece>();

            bool solutionExists = Calculator.FindSolution(unsolvedPieces, solvedPiecesList, out solvedPiecesList);

            if (!solutionExists) MessageBox.Show("No solution found :(");
            else
            {
                StringBuilder messageBoxContents = new StringBuilder("Solution found:\n");
                for (int p = 0; p < solvedPiecesList.Count; p++)
                {
                    var piece = solvedPiecesList[p];

                    string transformationString = "";
                    if (piece.IsFlipped) transformationString = string.Format("Flipped, then rotated {0} times.", piece.Rotations);
                    else transformationString = string.Format("Rotated {0} times.", piece.Rotations);

                    string pieceString = string.Format("ORDERED PIECE #{0}:\t{1} - {2}", p, piece.PieceID, transformationString);
                    //string pieceString = string.Format("ORDERED PIECE #{0}: {1} - {2} rotation(s). {3}.", p, piece.PieceID, piece.Rotations, piece.IsFlipped ? "Flipped" : "Not Flipped");
                    messageBoxContents.AppendLine(pieceString);
                }

                // Set the solution piece masks.
                List<string> solutionPieceMasks = new List<string>();
                foreach (var piece in solvedPiecesList)
                {
                    solutionPieceMasks.Add(piece.PieceMaskString);
                }
                SolutionPieceMasks = solutionPieceMasks;

                MessageBox.Show(messageBoxContents.ToString());
            }
        }

        // Can only solve the puzzle if we have all 6 pieces.
        public bool CanSolvePuzzle()
        {
            return PieceMasks.Count == 6;
        }

        // Given a list of piece masks, returns a list of Pieces.
        public List<Piece> GeneratePiecesFromMasks(List<string> pieceMasks)
        {
            if (pieceMasks == null || pieceMasks.Count != 6) throw new ArgumentException("Not enough pieceMasks!");


            List<Piece> pieceList = new List<Piece>();
            int i = 0;
            foreach (var mask in pieceMasks)
            {
                pieceList.Add(new Piece(mask, 0, false, i++));
            }

            return pieceList;
        }
    }
}
