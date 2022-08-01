using UnityEngine;
using TMPro;

public class DailyRewardManager : MonoBehaviour
{
    private float timeRemaining = 0;
    private bool timerIsRunning = false;
    [SerializeField] private TMP_Text timeText;
    [SerializeField] private int numberOfCoinsToAdd = 100;
    [SerializeField] private float countDownTimeInSec = 60;
    private void Start()
    {
        timeRemaining = countDownTimeInSec;
    }
    void Update()
    {
        if (timerIsRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                DisplayTime(timeRemaining);
            }
            else
            {
                timeText.text = "Collect";
                timeRemaining = 0;
                timerIsRunning = false;
            }
        }
    }
    void DisplayTime(float timeToDisplay)
    {
        timeToDisplay += 1;
        float minutes = Mathf.FloorToInt(timeToDisplay / 60); 
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void CollectCoins()
    {
        if (!timerIsRunning)
        {
            GameManager.gamemanagerInstance.SetPlayercoins(numberOfCoinsToAdd);
            timeRemaining = countDownTimeInSec;
            timerIsRunning = true;
        }
    }
}
