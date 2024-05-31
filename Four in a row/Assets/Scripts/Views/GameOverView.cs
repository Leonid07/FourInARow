using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class GameOverView : BaseView
{

    [SerializeField] Image whoWin;

    public Sprite player1Win;
    public Sprite player2Win;
    public Sprite Draw;// ничья
    public Sprite AIWin;


    
    public Action onNewGame, onExit;

    public void ShowGameOverView(SlotState winingPlayer, string playerName = null)
    {
        base.Show();
        if(winingPlayer == SlotState.EMPTY)
        {
            whoWin.sprite = Draw;
        } else
        {
            switch (playerName)
            {
                case "Computer":
                    whoWin.sprite = AIWin;
                    break;
                case "Player2":
                    whoWin.sprite = player1Win;
                    break;
                case "Player1":
                    whoWin.sprite = player2Win;
                    break;
            }
        }
    }

    public void OnNewGameButtonClicked()
    {
        onNewGame?.Invoke();
    }

    public void OnExitButtonClicked()
    {
        onExit?.Invoke();
    }
}
