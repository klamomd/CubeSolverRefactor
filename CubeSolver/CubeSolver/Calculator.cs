using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace CubeSolver
{
    public static class Calculator
    {
        // CONSTRUCTOR
        static Calculator()
        {

        }

        // VARIABLES

        // PROPERTIES

        // FUNCTIONS


        #region SLOT CODE

        public static bool FindSolution(List<Piece> pieces, List<Piece> solvedPieces, out List<Piece> newSolution)
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
#if DEBUG
                    string tabbing = "|";
                    for (int i = 0; i < solvedPieces.Count; i++)
                    {
                        tabbing += "--";
                    }


                    Debug.Write(string.Format("{0}Slot {1} -- Trying {2}...\t", tabbing, solvedPieces.Count, oriented.ToShortString()));
#endif

                    // If it does not fit, it is a dead end. Move on.
                    if (!DoesPieceFit(oriented, solvedPieces))
                    {
#if DEBUG
                        Debug.WriteLine("X - Collision, continuing.");
#endif
                        continue;
                    }

#if DEBUG
                    Debug.WriteLine("Fits!");
#endif

                    // If it does fit, make updated piece lists to recurse with.
                    var newPieces = pieces.Where(pi => !pi.Equals(p)).Select(pi => new Piece(pi)).ToList();
                    var newSolvedPieces = solvedPieces.Select(sp => new Piece(sp)).ToList();
                    newSolvedPieces.Add(oriented);

                    // End condition, found solution.
                    if (newSolvedPieces.Count == 6)
                    {
                        newSolution = newSolvedPieces;
                        return true;
                    }

#if DEBUG
                    for (int i = 0; i < solvedPieces.Count; i++)
                    {
                        tabbing += "--";
                    }


                    string pieceListString = StringHelper.PieceListToString(newSolvedPieces);
                    Debug.WriteLine(string.Format("Recursing with solved piece list: {0}...", pieceListString));
#endif

                    // If a solution exists, we return true. Otherwise we continue.
                    bool DoesSolutionExist = FindSolution(newPieces, newSolvedPieces, out newSolution);
                    if (DoesSolutionExist)
                    {

                        return true;
                    }
                }
            }


            // If end reached, no solution exists.
            return false;
        }

        public static bool DoesPieceFit(Piece piece, List<Piece> solvedPieces)
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
                    if (piece.DoesSideConflict(PieceSide.Top, solvedPieces[2].BottomSideMaskString)) return false;
                    else if (piece.DoesSideConflict(PieceSide.Bottom, solvedPieces[0].TopSideMaskString)) return false;
                    else if (AreCornersImpossible(solvedPieces[2], piece)) return false;
                    else if (AreCornersImpossible(piece, solvedPieces[0])) return false;
                    else if (AreCornersImpossible(piece, solvedPieces[0])) return false;
                    else return true;
                case 4:
                    slotMask = BuildSlotMask(solvedPieces[3].LeftSideMaskString, solvedPieces[0].LeftSideMaskString, solvedPieces[1].LeftSideMaskString, solvedPieces[2].LeftSideMaskString);
                    return DoesPieceFitSlotMask(piece, slotMask);
                case 5:
                    slotMask = BuildSlotMask(solvedPieces[3].RightSideMaskString, solvedPieces[2].RightSideMaskString, solvedPieces[1].RightSideMaskString, solvedPieces[0].RightSideMaskString);
                    return DoesPieceFitSlotMask(piece, slotMask);
                default:
                    throw new Exception();
            }
        }

        private static bool AreCornersImpossible(Piece upperPiece, Piece lowerPiece)
        {

            // Impossible corner if:
            //  -   Shared side center-edge peg exists on upper or lower piece (ex: 00010 bottom side for upper; or 01000 top side for lower; if checking top-left corner) AND:
            //  -   Both pieces have their individual side center-edge pegs (ex: 00010 left side for lower; and 01000 left side for upper; if checking top-left corner) AND:
            //  -   No corner peg exists.

            // Check left-side corner:
            //   1) Check both individual side pegs.
            if (lowerPiece.LeftSideMaskString[3] == '1' && upperPiece.LeftSideMaskString[1] == '1')
            {
                //    2) Check for empty corner.
                if (lowerPiece.TopLeftCornerExists && upperPiece.BottomLeftCornerExists)
                {
                    //    3) Check for shared side peg.
                    if (lowerPiece.TopSideMaskString[1] == '1' || upperPiece.BottomSideMaskString[3] == '1')
                    {
                        return true;
                    }
                }
            }

            // Check right-side corner.
            //   1) Check both individual side pegs.
            if (lowerPiece.RightSideMaskString[1] == '1' && upperPiece.RightSideMaskString[3] == '1')
            {
                //    2) Check for empty corner.
                if (lowerPiece.TopRightCornerExists && upperPiece.BottomRightCornerExists)
                {
                    //    3) Check for shared side peg.
                    if (lowerPiece.TopSideMaskString[3] == '1' || upperPiece.BottomSideMaskString[1] == '1')
                    {
                        return true;
                    }
                }
            }




            /*****/

            /*
            // OLD BAD CODE: Tries to check for bad corner, fails to account for corners with 'L' peg corners

            string top = upperPiece.PieceMaskString;
            string bottom = lowerPiece.PieceMaskString;

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
            */
            return false;
        }

        private static bool DoesPieceFitSlotMask(Piece piece, string slotMask)
        {
            if (piece == null) throw new ArgumentNullException();
            slotMask.ThrowIfNullOrBadLength(16);

            string pieceMask = piece.PieceMaskString;
            pieceMask.ThrowIfNullOrBadLength(16);

            // Check for collisions.
            for (int i = 0; i < 16; i++)
            {
                if (pieceMask[i] == '1' && slotMask[i] == '1') return false;
            }

            // Check that corners exists:
            for (int i = 0; i < 4; i++)
            {
                if (pieceMask[i * 4] != '1' && slotMask[i * 4] != '1') return false;
            }

            return true;
        }

        // LEFTOFF: FINISHED METHOD BELOW! COMMENT PROPERLY: EACH SIDE MASK IS THE DIRECT SIDE MASK FROM THE RELEVANT PIECES (EX: Slot 4 means T:R:B:L = 3L:0L:1L:2L)
        // Returns existing peg pask to check collision against. TODO: need function that compares entire 16 mask, or just call side collision 4 times and check for all 4 corners to check fit.
        private static string BuildSlotMask(string topSideMask, string rightSideMask, string bottomSideMask, string leftSideMask)
        {
            // Error-checking inputs.
            topSideMask.ThrowIfNullOrBadLength(5);
            rightSideMask.ThrowIfNullOrBadLength(5);
            bottomSideMask.ThrowIfNullOrBadLength(5);
            leftSideMask.ThrowIfNullOrBadLength(5);

            // Create reverse masks.
            string reversedTop = topSideMask.ReverseString();
            string reversedRight = rightSideMask.ReverseString();
            string reversedBottom = bottomSideMask.ReverseString();
            string reversedLeft = leftSideMask.ReverseString();


            string[] slotMaskList = new string[16];

            // Copy over all centers
            for (int i = 1; i < 4; i++)
            {
                slotMaskList[0 + i] = reversedTop[i].ToString();
                slotMaskList[4 + i] = reversedRight[i].ToString();
                slotMaskList[8 + i] = reversedBottom[i].ToString();
                slotMaskList[12 + i] = reversedLeft[i].ToString();
            }

            // Check both sides of each corner to determine if a peg exists in that corner. Set it if so:

            // Top left corner.
            slotMaskList[0] = (reversedLeft[4] == '1' || reversedTop[0] == '1') ? "1" : "0";
            // Top right corner.
            slotMaskList[4] = (reversedTop[4] == '1' || reversedRight[0] == '1') ? "1" : "0";
            // Bottom right corner.
            slotMaskList[8] = (reversedRight[4] == '1' || reversedBottom[0] == '1') ? "1" : "0";
            // Bottom left corner.
            slotMaskList[12] = (reversedBottom[4] == '1' || reversedLeft[0] == '1') ? "1" : "0";


            // Build and return slot mask.
            StringBuilder slotMaskBuilder = new StringBuilder();
            foreach (string s in slotMaskList) slotMaskBuilder.Append(s);
            //slotMaskBuilder.Append(slotMaskList.ToList());
            return slotMaskBuilder.ToString();
        }
        #endregion



        private static bool DoSidesConflict(string side1MaskString, string side2MaskString)
        {
            // Reverse one side to get them aligned the same way.
            string reversedSide2Mask = side2MaskString.ToCharArray().Reverse().ToString();

            // Return if two spaces conflict.
            for (int i=0; i<side1MaskString.Length; i++)
            {
                if (side1MaskString[i] == '1' && side2MaskString[i] == '1') return true;
            }
            return false;
        }





        public static List<Piece> GetCopyAndRemove(this List<Piece> pieces, Piece toRemove)
        {
            return pieces.Where(p => p != toRemove).Select(p => new Piece(p)).ToList();
        }

        public static List<Piece> GetCopyAndAppend(this List<Piece> pieces, Piece toAdd)
        {
            List<Piece> newPieces = pieces.Select(p => new Piece(p)).ToList();
            newPieces.Add(new Piece(toAdd));
            return newPieces;
        }

        public static List<Piece> GetCopy(this List<Piece> pieces)
        {
            return pieces.Select(p => new Piece(p)).ToList();
        }
    }
}
