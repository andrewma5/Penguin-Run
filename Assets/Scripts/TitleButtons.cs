using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleButtons : MonoBehaviour
{
    [SerializeField] Text HighScoreText;
    // Start is called before the first frame update
    void Start()
    {
        HighScoreText.text = "Highscore: " + PlayerPrefs.GetInt("HighScore").ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame()
    {
        GameManager.instance.StartGame();
    }

    public void Options()
    {
        //
    }

    public void Help()
    {
        GameManager.instance.HowToPlay();
    }
}
