using System;

public class State
{
    private int heuristic;
    private int[,] boardSpace = new int[8, 8];

    //initial constructor
    public State() {

        randomRestart();
        calcHeuristic();
    }

    //neighbor state constructor
    #region
    public State(State s) {

        //duplicate each space to the current state
        for (int c = 0; c < 8; c++) {//column

            for (int r = 0; r < 8; r++) {//all rows in each column

                boardSpace[r, c] = s.boardSpace[r, c];
            }
        }

        //get heuristic value on creation
        calcHeuristic();
    }
    #endregion

    //Get Heuristic Value of this State
    public int getHeuristic() {

        return heuristic;
    }

    //Set a Space of the Board
    public void setBoardSpace(int r, int c, int space) {

        boardSpace[r, c] = space;
    }

    //Get a Space of the Board
    public int getBoardSpace(int r, int c) {

        return boardSpace[r, c];
    }

    //Calculate Heuristic Value
    #region
    private void calcHeuristic() {

        //initialize Heuristic Value as 0
        heuristic = 0;

        for(int r = 0; r < 8; r++) {//row

            for(int c = 0; c < 8; c++) {

                if (boardSpace[r, c] == 1) {

                    //variable to check spaces to the right of each queen found
                    int spaceCheck = c + 1;

                    //while space to the right is still on the board
                    while (spaceCheck < 8) {

                        if (boardSpace[r, spaceCheck] == 1) {

                            heuristic++;
                        }

                        //move to next right space
                        spaceCheck++;
                    }

                    //varibles to cheack spaces downwardly diagonal to the left of each queen
                    int rowCheck = r + 1;
                    int columnCheckLeft = c - 1;

                    //while space to the downward left is still on the board
                    while (rowCheck < 8 && columnCheckLeft > -1) {

                        if (boardSpace[rowCheck, columnCheckLeft] == 1) {

                            heuristic++;
                        }

                        //move to next diagonal spaces
                        rowCheck++;
                        columnCheckLeft--;
                        
                    }

                    //reset variables to cheack spaces downwardly diagonal to the right of each queen
                    rowCheck = r + 1;
                    int columnCheckRight = c + 1;

                    //while space to the downward left is still on the board
                    while (rowCheck < 8 && columnCheckRight < 8) {

                        if (boardSpace[rowCheck, columnCheckRight] == 1) {

                            heuristic++;
                        }

                        //move to next diagonal spaces
                        rowCheck++;
                        columnCheckRight++;
                    }
                }
            }
        }
    }
    #endregion

    //Random Reset of the Board
    #region
    public void randomRestart() {

        //Create Random Number Generator
        Random rng = new Random();

        //assign each space to empty or queens
        for (int c = 0; c < 8; c++) {//column

            //generate random number
            int randomSpace = rng.Next(0, 7);

            for (int r = 0; r < 8; r++) {//all rows in each column

                if (r == randomSpace) {

                    boardSpace[r, c] = 1;
                } else {

                    boardSpace[r, c] = 0;
                }
            }
        }

        //recalculate heuristic value
        calcHeuristic();
    }
    #endregion

    //Print this State
    #region
    public void printState() {

        //printing state and information
        Console.WriteLine("Current State");

        for (int r = 0; r < 8; r++) {//row

            for (int c = 0; c < 8; c++) {//all columns on each row

                Console.Write(boardSpace[r, c]);

                if (c < 7) {//add comma until final column for each row

                    Console.Write(", ");
                }
            }

            //move to next line
            Console.WriteLine();
        }
    }
    #endregion

    //get the state of another state
    #region
    public void newState(State s) {

        //duplicate each space to the current state
        for (int c = 0; c < 8; c++) {//column

            for (int r = 0; r < 8; r++) {//all rows in each column

                boardSpace[r, c] = s.boardSpace[r, c];
            }
        }

        //recalculate heuristic value
        calcHeuristic();
    }
    #endregion

    //set new state
    #region
    public void setState(int rowSet, int columnSet) {

        for (int r = 0; r < 8; r++)
        {//find queen location

            if (boardSpace[r, columnSet] == 1)
            {//set queen location to empty

                boardSpace[r, columnSet] = 0;
            }
        }

        //set new desired queen location
        boardSpace[rowSet, columnSet] = 1;

        //recalculate heuristic
        calcHeuristic();
    }
    #endregion
}