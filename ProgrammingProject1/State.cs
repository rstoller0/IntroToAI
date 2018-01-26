using System;

public class State
{
    private int heuristic;
    private int[,] boardSpace = new int[8, 8];

    public State()
    {

    }

    public int getHeuristic()
    {
        return heuristic;
    }

    public void setBoardSpace(int r, int c, int space)
    {
        boardSpace[r, c] = space;
    }

    public int getBoardSpace(int r, int c)
    {
        return boardSpace[r, c];
    }
}