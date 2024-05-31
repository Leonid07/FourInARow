using System;
using UnityEngine;
public static class SequenceFinder
{
    public static bool IsAWin(SlotState[,] board, int col, int row, SlotState slotColor)
    {
        int winningLength = 4;
        foreach(Line line in Enum.GetValues(typeof(Line)))
        {
            int sum = CountSequence(board, col, row, slotColor, line, Direction.FORWARD) + CountSequence(board, col, row, slotColor, line, Direction.BACKWARDS) + 1;
            if (sum >= winningLength) return true;
        }
        return false;
    }

    private static int CountSequence(SlotState[,] board, int col, int row, SlotState slotColor, Line line, Direction direction)
    {
        int rowDelta = GetRowDelta(line, (int)direction);
        int colDelta = GetColDelta(line, (int)direction);
        col += colDelta;
        row += rowDelta;
        int count = 0;
        while (IsInRange(board,col,row) && board[col, row] == slotColor)
        {
            ++count;
            col += colDelta;
            row += rowDelta;
        }
        return count;
    }

    private static bool IsInRange(SlotState[,] board, int col, int row)
    {
        return (board.GetLength(0) > col) && (board.GetLength(1) > row) && (col >= 0) && (row >= 0);
    }

    private static int GetRowDelta(Line line, int direction)
    {
        switch (line)
        {
            case Line.HORIZONTAL:
                return 0 * direction;
            default:
            case Line.FIRST_DIAGONAL:
            case Line.VERTICAL:
            case Line.SECOND_DIAGONAL:
                return 1 * direction;
        }
    }

    private static int GetColDelta(Line line, int direction)
    {
        switch (line)
        { 
            case Line.VERTICAL:
                return 0 * direction;
            case Line.SECOND_DIAGONAL:
                return -1 * direction;
            default:
            case Line.FIRST_DIAGONAL:
            case Line.HORIZONTAL:
                return 1 * direction;
        } 
    }




}
