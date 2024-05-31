using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameView : BaseView
{

    public BoardView board;

    public override void Show()
    {
        base.Show();
        board.Reset();
    }
}
