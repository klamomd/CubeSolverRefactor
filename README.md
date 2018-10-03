# CubeSolverRefactor
A utility for calculating the solution to a disassembled puzzle cube, with UI for ease of use.

Building:
Simply open the project in Visual Studio and run it.

Usage:
On the left side of the window is a grid of 16 ToggleButtons surrounding a square. This is how a user enters puzzle pieces into the utility. Simply check the ToggleButtons to indicate where on the puzzle piece there are pegs, and leave untouched the areas where pegs do not exist. Underneath this grid is a textbox showing the string representation of the piece mask (where a 0 indicates no peg and a 1 indicates a peg); an "Add" button, which adds the piece to the list of puzzle pieces to solve and resets the ToggleButtons; and a "Reset" button, which simply resets all the ToggleButtons.

In the center panel is a list of all added pieces, represented as mask strings. Below this are 3 buttons. The "Delete Selected" button will remove the currently selected piece mask from the list. The "Clear List" button will remove all piece masks from the list. The "Solve" button will only function if 6 pieces have been added to the list, and will call the CubeSolver library to determine the solution (if one exists) to the provided pieces. (Note: If in Debug, and if no solution exists, the program will hang for a while since it will recurse down every single possible solution, all the while dumping out lines to the Console via Debug.WriteLine().) The CubeSolver library will brute-force finding the first valid solution for the pieces. A solution is not valid if any pegs are overlapping or if any unfilled spaces exist.

Upon pressing the "Solve" button, a dialog window will show up stating whether a solution has been found, and if one has been found, it will also show the arrangement of pieces in the solution (in text format). The text format of the solution will show which piece goes in which slot, and if it must be flipped and/or rotated. Note that each slot has a specific location in the final (unfolded) cube. The unfolded cube is represented in a T shape. Slots 0-3 are the center line of the T, with 0 being at the top and 3 at the bottom. Slot 4 is the top-left of the T, and slot 5 is the top-right of the T. When orienting pieces before placing them in their slot, if a piece must be flipped then it must be flipped horizontally, and it must be flipped before performing any rotations.

Because this representation of the solution is difficult to understand, I've added a visual representation as well. This can be found on the right side of the window. Once a valid solution is found, each of these 6 ToggleButton grids will show the corresponding piece in the solution and its orientation. If no valid solution is found, these 6 grids will be reset to show nothing.
