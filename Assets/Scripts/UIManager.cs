using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Text scoreText;
    private Player player;
    private Image TimerBar;
    private float score = 0;

    private float PowerupTime = 10f;
    private float timeLeft;
    private bool timing = false;

    [SerializeField] GameObject life1, life2, life3, life4, life5;

    [SerializeField] GameObject oneUp, Speed, Flight;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        TimerBar = transform.GetChild(1).GetComponent<Image>();
        timeLeft = 0f;
        TimerBar.fillAmount = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (player.GetMaxZ() - 4 < 0)
        {
            scoreText.text = "Score: 0";
        }
        else
        {
            score = (Mathf.Ceil(player.GetMaxZ() - 4) / 2f) * 100;
            scoreText.text = "Score: " + ((int)score);
        }

        if (timing && timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;
            TimerBar.fillAmount = timeLeft / PowerupTime;
        }

        if (timing && timeLeft <= 0)
        {
            timing = false;
            player.ActivateNothing();
            Speed.SetActive(false);
            Flight.SetActive(false);
            timeLeft = 0;
        }
    }

    public float GetScore()
    {
        return score;
    }

    public void StartTimer(string s)
    {
        timeLeft = PowerupTime;
        TimerBar.fillAmount = 1f;
        if (s == "flight")
        {
            Flight.SetActive(true);
        }
        else if (s == "speed")
        {
            Speed.SetActive(true);
        }
        timing = true;
    }

    public void UpdateLives(int i)
    {
        switch (i)
        {
            case 0:
                life1.SetActive(false);
                life2.SetActive(false);
                life3.SetActive(false);
                life4.SetActive(false);
                life5.SetActive(false);
                break;
            case 1:
                life1.SetActive(true);
                life2.SetActive(false);
                life3.SetActive(false);
                life4.SetActive(false);
                life5.SetActive(false);
                break;
            case 2:
                life1.SetActive(true);
                life2.SetActive(true);
                life3.SetActive(false);
                life4.SetActive(false);
                life5.SetActive(false);
                break;
            case 3:
                life1.SetActive(true);
                life2.SetActive(true);
                life3.SetActive(true);
                life4.SetActive(false);
                life5.SetActive(false);
                break;
            case 4:
                life1.SetActive(true);
                life2.SetActive(true);
                life3.SetActive(true);
                life4.SetActive(true);
                life5.SetActive(false);
                break;
            case 5:
                life1.SetActive(true);
                life2.SetActive(true);
                life3.SetActive(true);
                life4.SetActive(true);
                life5.SetActive(true);
                break;
        }
    }

    public void OneUp()
    {
        StartCoroutine("StopOneUp");
        oneUp.SetActive(true);
    }

    IEnumerator StopOneUp()
    {
        yield return new WaitForSeconds(2);
        oneUp.SetActive(false);
    }
}
