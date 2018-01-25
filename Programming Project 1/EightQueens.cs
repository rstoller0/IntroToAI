using System;

public class EightQueens
{
	public EightQueens()
	{
        void Main()
        {
            int[,] board = new int[8, 8];

            for (int r = 0; r < board.Length; r++)
            {
                for (int c = 0; c < board.Length; c++)
                {
                    board[i, c] = 0;
                }
            }
            Console.WriteLine(board[0, 0] + ", " + board[0, 1] + ", " + board[0, 2] + ", " + board[0, 3] + ", " + board[0, 4] + ", " + board[0, 5] + ", " + board[0, 6] + ", " + board[0, 7] + ", " + board[0, 8] + ", ");
        }
	}
}