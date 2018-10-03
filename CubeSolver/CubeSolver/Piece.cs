using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CubeSolver
{
    public class Piece : IEquatable<Piece>
    {
        // CONSTRUCTOR

        //  Default constructor
        public Piece() : this("0000000000000000", 0, false, -1)
        {

        }


        public Piece(string newPieceMaskString, int rotations, bool isFlipped, int newPieceID)
        {
            _pieceMaskString = newPieceMaskString;
            _rotations = rotations;
            _isFlipped = isFlipped;
            _pieceID = newPieceID;
        }

        public Piece(Piece piece) : this(piece.PieceMaskString, piece.Rotations, piece.IsFlipped, piece.PieceID)
        {

        }

        // VARIABLES
        private string _pieceMaskString;
        private int _rotations;
        private int _pieceID;
        private bool _isFlipped;

        // PROPERTIES
        public long PieceMask
        {
            get { return Convert.ToInt64(_pieceMaskString, 2); }
        }

        public string PieceMaskString
        {
            get { return _pieceMaskString; }
        }

        public int Rotations
        {
            get { return _rotations; }
        }

        public int PieceID
        {
            get { return _pieceID; }
        }

        public bool IsFlipped
        {
            get { return _isFlipped; }
        }

        public string TopSideMaskString
        {
            get { return PieceMaskString.Substring(0, 5); }
        }

        public string RightSideMaskString
        {
            get { return PieceMaskString.Substring(4, 5); }
        }

        public string BottomSideMaskString
        {
            get { return PieceMaskString.Substring(8, 5); }
        }

        public string LeftSideMaskString
        {
            get { return PieceMaskString.Substring(12, 4) + PieceMaskString[0]; }
        }

        public bool TopLeftCornerExists
        {
            get { return PieceMaskString[0] == '0'; }
        }

        public bool TopRightCornerExists
        {
            get { return PieceMaskString[4] == '0'; }
        }

        public bool BottomRightCornerExists
        {
            get { return PieceMaskString[8] == '0'; }
        }

        public bool BottomLeftCornerExists
        {
            get { return PieceMaskString[12] == '0'; }
        }

        // FUNCTIONS
        public List<Piece> GetAllPieceOrientations()
        {
            List<Piece> allOrientations = new List<Piece>();

            Piece flipped = FlippedPiece();
            for (int i=0; i<4; i++)
            {
                allOrientations.Add(RotatePiece(i));
                allOrientations.Add(flipped.RotatePiece(i));
            }

            return allOrientations;
        }

        // Rotates piece 90 degrees clockwise * any number between 0 and 3.
        private Piece RotatePiece(int rotateTimes90Degrees)
        {
            // Ensure rotateTimes90Degrees is between 0 and 3 before proceeding.
            if (rotateTimes90Degrees < 0) return new Piece(this);
            rotateTimes90Degrees %= 4;

            // Kick out a duplicate piece of no rotation specified.
            if (rotateTimes90Degrees == 0) return new Piece(this);

            int pegShift = rotateTimes90Degrees * 4;
            string newMaskString = PieceMaskString.Substring(16 - pegShift, pegShift);
            newMaskString += PieceMaskString.Substring(0, 16 - pegShift);

            return new Piece(newMaskString, rotateTimes90Degrees, IsFlipped, PieceID);
        }

        // Flips piece horizontally.
        private Piece FlippedPiece()
        {
            StringBuilder newPieceMask = new StringBuilder();
            //string newPieceMask = "";

            // Top side
            newPieceMask.Append(PieceMaskString.Substring(0, 5).ToCharArray().Reverse().ToArray());

            // Right center
            newPieceMask.Append(PieceMaskString.Substring(13, 3).ToCharArray().Reverse().ToArray());

            // Bottom side
            newPieceMask.Append(PieceMaskString.Substring(8, 5).ToCharArray().Reverse().ToArray());

            // Left center
            newPieceMask.Append(PieceMaskString.Substring(5, 3).ToCharArray().Reverse().ToArray());


            //TODO: IS it incorrect to use the opposite of this.IsFlipped in the call below? Does that check out if things are rotated?
            return new Piece(newPieceMask.ToString(), Rotations, !IsFlipped, PieceID);
        }

        public string GetSideMask(PieceSide side)
        {
            switch (side)
            {
                case PieceSide.Bottom:
                    return BottomSideMaskString;
                case PieceSide.Left:
                    return LeftSideMaskString;
                case PieceSide.Right:
                    return RightSideMaskString;
                case PieceSide.Top:
                    return TopSideMaskString;
                default:
                    throw new Exception();
            }
        }

        public bool Equals(Piece other)
        {
            if (other == null) return false;
            if (other.PieceID != PieceID || other.PieceMask != PieceMask || other.Rotations != Rotations || other.IsFlipped != IsFlipped) return false;
            return true;
        }

        public bool DoesSideConflict(PieceSide thisSide, string otherSideMaskString)
        {
            // Get the relevant side's mask string. Reverse it get it aligned with the other mask string.
            StringBuilder maskBuilder = new StringBuilder();
            maskBuilder.Append(GetSideMask(thisSide).ToCharArray().Reverse().ToArray());
            string thisSideMaskString = maskBuilder.ToString();

            // Ensure our sideMasks 
            if (thisSideMaskString.Length != 5 || otherSideMaskString.Length != 5) throw new Exception();

            for (int i = 0; i < 5; i++)
            {
                if (thisSideMaskString[i] == '1' && otherSideMaskString[i] == '1') return true;

                // Check the center 3 pegs to see if any are empty on both pieces.
                else if (i >= 1 && i <= 3)
                {
                    if (thisSideMaskString[i] == '0' && otherSideMaskString[i] == '0') return true;
                }
            }

            return false;
        }

        public string ToShortString()
        {
            return string.Format("{0}{1}{2}", PieceID, (IsFlipped ? "F" : ""), Rotations);
        }
    }
}
