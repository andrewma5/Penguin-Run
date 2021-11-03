using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static GameManager instance;

    float highScore = 0;
    float prevScore = 0;

    public float iceCooldown = 2f;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }

    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void EndScreen()
    {
        SceneManager.LoadScene("End");
    }

    public void TitleScreen()
    {
        SceneManager.LoadScene("Title");
    }

    public void HowToPlay()
    {
        SceneManager.LoadScene("HowToPlay");
    }

    public void LoseGame(float curscore)
    {
        if (curscore > highScore)
        {
            highScore = curscore;
            PlayerPrefs.SetInt("HighScore", (int)highScore);
        }
        prevScore = curscore;
        SceneManager.LoadScene("End");
    }

    public float GetPrevScore()
    {
        return prevScore;
    }
}
