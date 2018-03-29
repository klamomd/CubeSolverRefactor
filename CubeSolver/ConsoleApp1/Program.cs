using System;
using System.Collections.Generic;
using System.Diagnostics;
using CubeSolver;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {

            Debug.WriteLine("### GREEN CUBE TEST");

            List<Piece> pieceList = new List<Piece>();

            Piece orangePiece0 = new Piece("0010110100110101", 0, false, 0);
            Piece orangePiece1 = new Piece("0001101011010010", 0, false, 1);
            Piece orangePiece2 = new Piece("0101001000101101", 0, false, 2);
            Piece orangePiece3 = new Piece("1010110110100101", 0, false, 3);
            Piece orangePiece4 = new Piece("0010001000110101", 0, false, 4);
            Piece orangePiece5 = new Piece("0010000101011101", 0, false, 5);


            pieceList.Add(orangePiece0);
            pieceList.Add(orangePiece1);
            pieceList.Add(orangePiece2);
            pieceList.Add(orangePiece3);
            pieceList.Add(orangePiece4);
            pieceList.Add(orangePiece5);

            //Piece greenPiece0 = new Piece("1010010100101101", 0, false, 0);
            //Piece greenPiece1 = new Piece("1010110100100101", 0, false, 1);
            //Piece greenPiece2 = new Piece("0101001000100101", 0, false, 2);
            //Piece greenPiece3 = new Piece("0101110110101101", 0, false, 3);
            //Piece greenPiece4 = new Piece("0010001001010010", 0, false, 4);
            //Piece greenPiece5 = new Piece("0101101000100101", 0, false, 5);


            //pieceList.Add(greenPiece0);
            //pieceList.Add(greenPiece1);
            //pieceList.Add(greenPiece2);
            //pieceList.Add(greenPiece3);
            //pieceList.Add(greenPiece4);
            //pieceList.Add(greenPiece5);

            for (int p = 0; p < pieceList.Count; p++)
            {
                var piece = pieceList[p];

                string pieceString = string.Format("PIECE {0}: {1}", piece.PieceID, piece.PieceMaskString);
                Debug.WriteLine(pieceString + "\n");
            }


            // OLD CODE
            /*
            List<Piece> orderedPiecesList = Calculator.CalculateCubeSolution(pieceList, new List<Piece>());
            if (orderedPiecesList == null) Debug.WriteLine("No solution found! :(");
            else
            {
                Debug.WriteLine("\n\n\n~~~~~~~~~~~~~~~~~~\n\n\n\nSOLUTION:");
                for (int p = 0; p < orderedPiecesList.Count; p++)
                {
                    var piece = orderedPiecesList[p];

                    string pieceString = string.Format("ORDERED PIECE #{0}: {1} - {2} rotation(s). {3}.", p, piece.PieceID, piece.Rotations, piece.IsFlipped ? "Flipped" : "Not Flipped");
                    Debug.WriteLine(pieceString + "\n");
                }
            }
            */
            // END OLD CODE


            List<Piece> solvedPiecesList = new List<Piece>();
            bool solutionExists = Calculator.FindSolution(pieceList, solvedPiecesList, out solvedPiecesList);

            if (!solutionExists) Debug.WriteLine("No solution found! :(");
            else
            {
                Debug.WriteLine("\n\n\n~~~~~~~~~~~~~~~~~~\n\n\n\nSOLUTION:");
                for (int p = 0; p < solvedPiecesList.Count; p++)
                {
                    var piece = solvedPiecesList[p];

                    string pieceString = string.Format("ORDERED PIECE #{0}: {1} - {2} rotation(s). {3}.", p, piece.PieceID, piece.Rotations, piece.IsFlipped ? "Flipped" : "Not Flipped");
                    Debug.WriteLine(pieceString + "\n");
                }
            }


            Console.Read();
        }
    }
}
