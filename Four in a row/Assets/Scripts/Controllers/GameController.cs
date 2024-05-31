using Unity.VisualScripting;
using UnityEngine;

public class GameController : MonoBehaviour
{
    // Это наш синглтон
    private static GameController gameInstance;

    // Представления
    [SerializeField] StartView startView;
    [SerializeField] GameView gameView;
    [SerializeField] GameObject pauseGameView;
    [SerializeField] GameOverView gameOverView;

    public GameObject labelPlayer1, labelPlayer2;

    SlotState currentPlayer;    // Текущий игрок
    bool isVsAi = false;    // Флаг игры против ИИ

    // Модели
    private GameModel gameModel;

    // Метод Awake вызывается при создании объекта
    private void Awake()
    {
        // Проверяем, существует ли уже экземпляр GameController
        if (gameInstance == null)
        {
            // Если нет, то назначаем текущий экземпляр и инициализируем модели
            gameInstance = this;
            gameModel = new GameModel();
            InitializeWindows(); // Инициализация окон
            SetCallbacks(); // Установка обратных вызовов
        }
        else
        {
            // Если экземпляр уже существует, уничтожаем текущий объект
            Destroy(gameObject);
        }
    }

    // Метод для старта игры
    public void StartGame(string player1Name, string player2Name)
    {
        //bool err = true;
        if (!string.IsNullOrEmpty(player1Name))
        {
            if (isVsAi)
            {
                startView.Hide();
                GetPlayerTurn();
                gameModel.SetPlayerNames(player1Name, "Computer");
                gameView.Show();
                PlayComputerTurn();
            }
            else if (!string.IsNullOrEmpty(player2Name))
            {
                startView.Hide();
                GetPlayerTurn();
                gameModel.SetPlayerNames(player1Name, player2Name);
                gameView.Show();
            }
        }
    }

    // Метод для инициализации окон
    private void InitializeWindows()
    {
        startView.Show(); // Показать окно старта
        gameView.Hide(); // Скрыть игровое окно
        gameOverView.Hide(); // Скрыть окно окончания игры
    }

    public void StartNewGame()
    {
        startView.Hide(); // Показать окно старта
        gameView.Show(); // Скрыть игровое окно
        gameOverView.Hide(); // Скрыть окно окончания игры
        gameModel.ResetBoard();
    }

    public void BackToMenu()
    {
        startView.Show(); // Показать окно старта
        gameView.Hide(); // Скрыть игровое окно
        gameOverView.Hide(); // Скрыть окно окончания игры
        pauseGameView.SetActive(false);
        gameModel.ResetBoard();
    }

    // Метод для определения, кто ходит первым
    private void GetPlayerTurn()
    {
            currentPlayer = SlotState.WHITE;
    }

    // Метод для выполнения хода компьютера
    private void PlayComputerTurn()
    {
        if (isVsAi && currentPlayer == SlotState.RED)
        {
            HandleColumnClicked(Random.Range(0, 7));
        }
    }

    // Метод для установки обратных вызовов (коллбэков)
    private void SetCallbacks()
    {
        startView.onStartGame += StartGame; // Назначение функции StartGame для вызова при старте игры
        startView.onAiToggled += HandleAiToggled; // Назначение функции HandleAiToggled для вызова при переключении AI
        gameView.board.onColumnClicked += HandleColumnClicked; // Назначение функции HandleColumnClicked для вызова при клике по колонке
        gameView.board.onColumnHovered += HandleColumnHovered; // Назначение функции HandleColumnHovered для вызова при наведении на колонку
        gameOverView.onNewGame += InitializeWindows; // Назначение функции InitializeWindows для вызова при старте новой игры
        gameOverView.onExit += Application.Quit; // Назначение функции Application.Quit для выхода из игры
    }

    // Метод, вызываемый при уничтожении объекта
    private void OnDestroy()
    {
        startView.onStartGame -= StartGame; // Удаление назначения функции StartGame
        startView.onAiToggled -= HandleAiToggled; // Удаление назначения функции HandleAiToggled
        gameView.board.onColumnClicked -= HandleColumnClicked; // Удаление назначения функции HandleColumnClicked
        gameView.board.onColumnHovered -= HandleColumnHovered; // Удаление назначения функции HandleColumnHovered
        gameOverView.onNewGame += InitializeWindows; // Удаление назначения функции InitializeWindows
        gameOverView.onExit += Application.Quit; // Удаление назначения функции Application.Quit
    }

    // Метод для переключения хода игрока
    private void TogglePlayerTurn(int colIndex)
    {
        int nextAvilableSlot = gameModel.GetNextAvilableSlot(colIndex);
        if (currentPlayer == SlotState.RED)
        {
            currentPlayer = SlotState.WHITE;
            labelPlayer1.SetActive(true);
            labelPlayer2.SetActive(false);
        }
        else
        {
            currentPlayer = SlotState.RED;
            labelPlayer1.SetActive(false);
            labelPlayer2.SetActive(true);
        }
        if (!isVsAi)
        {
            if (nextAvilableSlot >= 0)
            {
                gameView.board.SetHoverSlotState(colIndex, currentPlayer);
            }
            else
            {
                gameView.board.SetHoverSlotState(colIndex, SlotState.EMPTY);
            }
        }
        PlayComputerTurn();
    }

    // Метод для обработки окончания игры
    private void HandleGameOver(bool isTie)
    {

        // Сбрасываем игровую доску
        gameModel.ResetBoard();

        // Определяем имя победителя (если нет ничьей)
        string winningPlayerName = isTie ? null : gameModel.GetPlayerName(currentPlayer);

        // Определяем цвет победителя (если нет ничьей)
        SlotState winningPlayer = isTie ? SlotState.EMPTY : currentPlayer;

        // Показываем экран окончания игры с соответствующей информацией
        gameOverView.ShowGameOverView(winningPlayer, winningPlayerName);
    }

    // Метод для обработки клика по колонке
    public void HandleColumnClicked(int colIndex)
    {
        int nextAvilableSlot = gameModel.GetNextAvilableSlot(colIndex);

        if (nextAvilableSlot >= 0)
        {
            gameModel.SetUsedSlot(colIndex, nextAvilableSlot, currentPlayer);
            gameView.board.SetSlotState(colIndex, nextAvilableSlot, currentPlayer);

            bool isWin = gameModel.GetGameWinState(colIndex, nextAvilableSlot, currentPlayer);
            bool isTie = gameModel.GetIsBoardFull();

            if (!isWin && !isTie)
            {
                TogglePlayerTurn(colIndex);
            }
            else
            {
                HandleGameOver(isTie);
            }
        }
    }

    // Метод для обработки наведения на колонку
    public void HandleColumnHovered(int colIndex)
    {
        int nextAvilableSlot = gameModel.GetNextAvilableSlot(colIndex);

        // Если колонка полная
        if (nextAvilableSlot < 0)
        {
            gameView.board.SetHoverSlotState(colIndex, SlotState.EMPTY);
        }
        else
        {
            gameView.board.SetHoverSlotState(colIndex, currentPlayer);
        }
    }

    // Метод для обработки переключения режима AI
    public void HandleAiToggled(bool isAi)
    {
        isVsAi = isAi;
    }
}
