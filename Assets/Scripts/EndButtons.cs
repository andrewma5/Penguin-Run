using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndButtons : MonoBehaviour
{
    [SerializeField] Text ScoreText;

    // Start is called before the first frame update
    void Start()
    {
        ScoreText.text = GameManager.instance.GetPrevScore().ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayAgain()
    {
        GameManager.instance.StartGame();
    }

    public void TitleScreen()
    {
        GameManager.instance.TitleScreen();
    }
}
