using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HighScoreItemView : MonoBehaviour
{
    [SerializeField]
    TMP_Text nameText, scoreText;

    public void Init(string name, int score)
    {
        nameText.text = name;
        scoreText.text = score.ToString();
    }
    
}
