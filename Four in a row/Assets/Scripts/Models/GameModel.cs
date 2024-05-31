using System.Collections;
using System.Collections.Generic;

public class GameModel
{
    // Имена игроков
    private string player1Name;
    private string player2Name;
    // Двумерный массив для хранения состояния игрового поля
    private SlotState[,] board = new SlotState[7, 6];

    // Конструктор класса GameModel
    public GameModel()
    {
        // Сброс игрового поля
        ResetBoard();
    }

    // Метод для получения имени игрока по текущему состоянию слота
    public string GetPlayerName(SlotState currentPlayer)
    {
        if (currentPlayer == SlotState.RED)
            return player1Name;
        if (currentPlayer == SlotState.WHITE)
            return player2Name;
        return "";
    }

    // Метод для установки имен игроков
    public void SetPlayerNames(string _player1Name, string _player2Name)
    {
        player1Name = _player1Name;
        player2Name = _player2Name;
    }

    // Метод для установки состояния слота
    public void SetUsedSlot(int col, int row, SlotState slotState)
    {
        board[col, row] = slotState;
    }

    // Метод для сброса игрового поля
    public void ResetBoard()
    {
        for (int i = 0; i < board.GetLength(0); i++)
        {
            for (int j = 0; j < board.GetLength(1); j++)
            {
                board[i, j] = SlotState.EMPTY;
            }
        }
    }

    // Метод для получения следующего доступного слота в колонке
    public int GetNextAvilableSlot(int col)
    {
        for (int i = 0; i < board.GetLength(1); i++)
        {
            if (board[col, i] == SlotState.EMPTY)
            {
                return i;
            }
        }
        return -1;
    }

    // Метод для проверки состояния выигрыша
    public bool GetGameWinState(int col, int row, SlotState slotColor)
    {
        return SequenceFinder.IsAWin(board, col, row, slotColor);
    }

    // Метод для проверки заполненности игрового поля
    public bool GetIsBoardFull()
    {
        for (int i = 0; i < board.GetLength(0); i++)
        {
            for (int j = 0; j < board.GetLength(1); j++)
            {
                if (board[i, j] == SlotState.EMPTY)
                {
                    return false;
                }
            }
        }
        return true;
    }
}
