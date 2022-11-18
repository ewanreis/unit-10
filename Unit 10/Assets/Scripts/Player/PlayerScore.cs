using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerScore : MonoBehaviour
{
    //private int score;
    private int time;
    public int rings;

    public TMP_Text ringText, scoreText, timeText;
    private void Awake()
    {
        //score = 0;
        rings = 0;
    }
    private void FixedUpdate()
    {
        ringText.text = $"{rings}";
    }
}
