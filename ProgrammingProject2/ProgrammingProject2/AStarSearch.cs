using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgrammingProject2
{
    class AStarSearch {

        static void Main(string[] args) {

            Node[,] boardSpace = new Node[15, 15];//the 2D array to hold the tile board
            int spaceCounter = 0;//used to ensure blocked tiles are correctly added
            Random rng = new Random();//random number generator
            int[] blockedNodeRow = new int[22];//array for the row numbers for each of the 10% that will be blocked
            int[] blockedNodeColumn = new int[22];//array for the column numbers for each of the 10% that will be blocked
            bool wasBlocked = false;//variable to allow the blocked tiles to stay blocked
            String userInput = "";//variable to capture user input
            int startingRow = -1, startingColumn = -1, goalRow = -1, goalColumn = -1;//variables used to hold and calculate path

            for (int i = 0; i < 22; i++) {//create randomly blocked nodes
                blockedNodeRow[i] = rng.Next(0, 14);
                blockedNodeColumn[i] = rng.Next(0, 14);
            }

            //create board and each tile
            for (int r = 0; r < 15; r++) {//each row
                for (int c = 0; c < 15; c++) {//each column

                    for (int i = 0; i < 22; i++) {//to add blocked nodes
                        if (r == blockedNodeRow[i] && c == blockedNodeColumn[i])
                        {
                            boardSpace[r, c] = new Node(r, c, 1);//blocked
                            wasBlocked = true;
                        }
                    }

                    if(wasBlocked) {//to stop from having node that was blocked from being overwritten
                        wasBlocked = false;
                    }
                    else {
                        boardSpace[r, c] = new Node(r, c, 0);//not blocked
                    }

                    spaceCounter++;//add the space counter
                }
            }

            //display board
            for (int r = 0; r < 15; r++) {//row

                for (int c = 0; c < 15; c++) {//all columns on each row

                    boardSpace[r, c].displayTile();
                }

                //move to next line
                Console.WriteLine();
            }

            while (string.Equals(userInput, "No", StringComparison.OrdinalIgnoreCase) != true) {//while user is still inputing new starting/goal positions

                startingRow = startingColumn = goalRow = goalColumn = -1;//reset all positions to null (-1)

                Console.Write("Enter the Row of the starting position: ");
                userInput = Console.ReadLine();

                if (Int32.TryParse(userInput, out int sRow)) {//capture starting row number
                    startingRow = sRow;
                } else {
                    /* No, input could not be parsed to an integer */
                }

                Console.Write("Enter the Column of the starting position: ");
                userInput = Console.ReadLine();

                if (Int32.TryParse(userInput, out int sColumn)) {//capture starting column number
                    startingColumn = sColumn;
                } else {
                    /* No, input could not be parsed to an integer */
                }

                Console.Write("Enter the Row of the goal position: ");
                userInput = Console.ReadLine();

                if (Int32.TryParse(userInput, out int gRow)) {//capmture goal row number 
                    goalRow = gRow;
                } else {
                    /* No, input could not be parsed to an integer */
                }

                Console.Write("Enter the Column of the goal position: ");
                userInput = Console.ReadLine();

                if (Int32.TryParse(userInput, out int gColumn)) {//capture goal column number
                    goalColumn = gColumn;
                } else {
                    /* No, input could not be parsed to an integer */
                }

                Console.WriteLine("THIS IS WHERE MAGIC NEEDS TO HAPPEN!");

                while (string.Equals(userInput, "No", StringComparison.OrdinalIgnoreCase) != true && string.Equals(userInput, "Yes", StringComparison.OrdinalIgnoreCase) != true) {//while user hasn't input a correct command

                    Console.Write("Would you like to input another starting and goal position? (Yes or No) ");
                    userInput = Console.ReadLine();
                }
            }

            Console.WriteLine("Starting Position: " + startingRow + ", " + startingColumn);//display starting postions row and column
            Console.WriteLine("Goal Position: " + goalRow + ", " + goalColumn);//display goal positions row and column

            Console.ReadLine();
        }
    }
}
