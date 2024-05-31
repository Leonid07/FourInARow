using TMPro;
using System;
using UnityEngine;

public class StartView : BaseView
{


    public Action<string, string> onStartGame;
    public Action<bool> onAiToggled;

    public void StartGameClicked()
    {
        onStartGame?.Invoke("Player1", "Player2");
    }

    public void OnToggle(bool isAi)
    {
        Debug.Log(isAi);
        onAiToggled?.Invoke(isAi);
    }

}
