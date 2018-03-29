using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace CubeSolver
{
    public class Slot
    {
        // CONSTRUCTOR
        public Slot()
        {

        }


        /*
         * 
         * 
         *              |||||
         *              |||||
         *              |||||
         *              |||||
         *           \|||||||||/
         *            \|||||||/
         *             \|||||/
         *              \|||/
         *               \|/
         *  LEFTOFF - REORGANIZE CODE
         * 
         * 
         */



        // VARIABLES

        // PROPERTIES

        // FUNCTIONS
        // Returns whether there exists a solution. Also passes out the solution through newSolution if it exists. Must pass in list of remaining pieces and solved pieces. Recursive function.
        public bool FindSolution(List<Piece> pieces, out List<Piece> newSolution, List<Piece> solvedPieces = null)
        {
            // Set newSolution so we can return quickly if false.
            newSolution = new List<Piece>();


            // Loop through each remaining piece.
            foreach (Piece p in pieces)
            {
                // Loop through each piece's 8 possible orientations.
                List<Piece> orientations = p.GetAllPieceOrientations();
                foreach (Piece oriented in orientations)
                {
                    // If it does not fit, it is a dead end. Move on.
                    if (!DoesPieceFit(oriented, solvedPieces)) continue;

                    // If it does fit, make updated piece lists to recurse with.
                    var newPieces = pieces.Where(pi => !pi.Equals(p)).Select(pi => new Piece(pi)).ToList();
                    var newSolvedPieces = solvedPieces.Select(sp => new Piece(sp)).ToList();
                    newSolvedPieces.Add(oriented);

                    // If a solution exists, we return true. Otherwise we continue.
                    bool DoesSolutionExist = FindSolution(newPieces, out newSolution, newPieces);
                    if (DoesSolutionExist) return true;
                }
            }


            // If end reached, no solution exists.
            return false;
        }


        public bool DoesPieceFit(Piece piece, List<Piece> solvedPieces)
        {
            int slotIndex = solvedPieces.Count;
            string slotMask = "";

            switch (slotIndex)
            {
                case 0:
                    return true;
                case 1:
                    if (piece.DoesSideConflict(PieceSide.Top, solvedPieces[0].BottomSideMaskString)) return false;
                    else if (AreCornersImpossible(solvedPieces[0], piece)) return false;
                    else return true;
                case 2:
                    if (piece.DoesSideConflict(PieceSide.Top, solvedPieces[1].BottomSideMaskString)) return false;
                    else if (AreCornersImpossible(solvedPieces[1], piece)) return false;
                    else return true;
                case 3:
                    if (piece.DoesSideConflict(PieceSide.Top, solvedPieces[1].BottomSideMaskString)) return false;
                    else if (AreCornersImpossible(solvedPieces[2], piece)) return false;
                    else if (AreCornersImpossible(piece, solvedPieces[9])) return false;
                    else return true;
                case 4:
                    slotMask = BuildSlotMask(solvedPieces[3].LeftSideMaskString, solvedPieces[0].LeftSideMaskString, solvedPieces[1].LeftSideMaskString, solvedPieces[2].LeftSideMaskString);
                    return DoesPieceFitSlotMask(piece, slotMask);
                case 5:
                    slotMask = BuildSlotMask(solvedPieces[3].RightSideMaskString, solvedPieces[0].RightSideMaskString, solvedPieces[1].RightSideMaskString, solvedPieces[2].RightSideMaskString);
                    return DoesPieceFitSlotMask(piece, slotMask);
                default:
                    throw new Exception();
            }
        }

        private bool AreCornersImpossible(Piece upperPiece, Piece lowerPiece)
        {
            string top = upperPiece.PieceMaskString;
            string bottom = lowerPiece.PieceMaskString;

            // Check upperPiece / bottom-left and lowerPiece / upper-left
            // If (top 11 || bot 1) && (top 13 || bot 15) && !top 12 && !bot 0
            if ((top[11] == '1' || bottom[1] == '1') && (top[13] == '1' || bottom[15] == '1') && !(top[12] == '1') && !(bottom[0] == '1'))
            {
                return true;
            }

            // Check upperPiece / bottom-right and lowerPiece / upper-right
            // If (top 7 || bot 5) && (top 9 || bot 3) && !top 8 && !bot 4
            if ((top[7] == '1' || bottom[5] == '1') && (top[9] == '1' || bottom[3] == '1') && !(top[8] == '1') && !(bottom[4] == '1'))
            {
                return true;
            }

            return false;
        }

        private bool DoesPieceFitSlotMask(Piece piece, string slotMask)
        {
            if (piece == null) throw new ArgumentNullException();
            ThrowIfNullOrBadLength(slotMask, 16);

            string pieceMask = piece.PieceMaskString;
            ThrowIfNullOrBadLength(pieceMask, 16);

            // Check for collisions.
            for (int i = 0; i < 16; i++)
            {
                if (pieceMask[i] == '1' && slotMask[i] == '1') return false;
            }

            // Check that corners exists:
            for (int i = 0; i < 4; i++)
            {
                if (pieceMask[i*4] != '1' && slotMask[i*4] != '1') return false;
            }

            return true;
        }

        // LEFTOFF: FINISHED METHOD BELOW! COMMENT PROPERLY: EACH SIDE MASK IS THE DIRECT SIDE MASK FROM THE RELEVANT PIECES (EX: Slot 4 means T:R:B:L = 3L:0L:1L:2L)
        // Returns existing peg pask to check collision against. TODO: need function that compares entire 16 mask, or just call side collision 4 times and check for all 4 corners to check fit.
        private string BuildSlotMask(string topSideMask, string rightSideMask, string bottomSideMask, string leftSideMask)
        {
            // Error-checking inputs.
            ThrowIfNullOrBadLength(topSideMask, 5);
            ThrowIfNullOrBadLength(rightSideMask, 5);
            ThrowIfNullOrBadLength(bottomSideMask, 5);
            ThrowIfNullOrBadLength(leftSideMask, 5);

            // Create reverse masks.
            string reversedTop = ReverseString(topSideMask);
            string reversedRight = ReverseString(rightSideMask);
            string reversedBottom = ReverseString(bottomSideMask);
            string reversedLeft = ReverseString(leftSideMask);


            List<char> slotMaskList = new List<char>(16);

            // Copy over all centers
            for (int i = 1; i < 4; i++)
            {
                slotMaskList[0 + i] = reversedTop[i];
                slotMaskList[4 + i] = reversedRight[i];
                slotMaskList[8 + i] = reversedBottom[i];
                slotMaskList[12 + i] = reversedLeft[i];
            }

            // Check both sides of each corner to determine if a peg exists in that corner. Set it if so:

            // Top left corner.
            slotMaskList[0] = (reversedLeft[4] == '1' || reversedTop[0] == '1') ? '1' : '0'; 
            // Top right corner.
            slotMaskList[4] = (reversedTop[4] == '1' || reversedRight[0] == '1') ? '1' : '0';
            // Bottom right corner.
            slotMaskList[8] = (reversedRight[4] == '1' || reversedBottom[0] == '1') ? '1' : '0';
            // Bottom left corner.
            slotMaskList[12] = (reversedBottom[4] == '1' || reversedLeft[0] == '1') ? '1' : '0';


            // Build and return slot mask.
            StringBuilder slotMaskBuilder = new StringBuilder();
            slotMaskBuilder.Append(slotMaskList.ToArray());
            return slotMaskBuilder.ToString();
        }

        private string ReverseString(string str)
        {
            string rStr = "";
            for (int i = str.Length - 1; i > 0; i++)
            {
                rStr += str[i];
            }
            return rStr;
        }

        private void ThrowIfNull(string str)
        {
            if (str == null) throw new Exception();
        }

        private void ThrowIfNullOrEmpty(string str)
        {
            ThrowIfNullOrBadLength(str);
        }

        private void ThrowIfNullOrBadLength(string str, int length = 0)
        {
            ThrowIfNull(str);
            if (str.Length != length) throw new Exception();
        }

        
    }
}
