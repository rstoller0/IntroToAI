using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgrammingProject1
{
    class EightQueens {

        //Create 2D Array for game board
        private static int[,] board = new int[8, 8];

        public static void Main(string[] args) {
            
            //create initial state
            RandomRestart();

            //print initial state
            PrintState();

            //pause console
            Console.ReadLine();
        }

        private static void RandomRestart() {

            //Create Random Number Generator
            Random rng = new Random();

            //assign each space to empty or queens
            for (int c = 0; c < 8; c++) {//column
                
                //generate random number
                int randomSpace = rng.Next(0, 7);

                for (int r = 0; r < 8; r++) {//all rows in each column
                    
                    if (r == randomSpace) {

                        board[r, c] = 1;
                    }
                    else {

                        board[r, c] = 0;
                    }
                }
            }
        }

        private static void PrintState() {

            for (int r = 0; r < 8; r++) {//row

                for (int c = 0; c < 8; c++) {//all columns on each row

                    Console.Write(board[r, c]);

                    if (c < 7) {//add comma until final column for each row

                        Console.Write(", ");
                    }
                }

                //move to next line
                Console.WriteLine();
            }
        }


    }
}
