using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDControl : MonoBehaviour
{

    public GameObject player;

    [Header("HUD")]
    public TMPro.TextMeshProUGUI scoreText;
    public TMPro.TextMeshProUGUI timeText;
    public TMPro.TextMeshProUGUI ringText;
    public TMPro.TextMeshProUGUI livesText;

    [Header("Pause")]
    public GameObject pauseScreen;
    

    [Header("End Score")]
    public GameObject endScoreScreen;
    public TMPro.TextMeshProUGUI endScoreText;
    public TMPro.TextMeshProUGUI endTimeBonusText;
    public TMPro.TextMeshProUGUI endRingsBonusText;

    public static HUDControl instance;

    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    public void UpdateLives(int lives)
    {
        livesText.text = lives.ToString();
    }
    public void UpdateRings(int rings)
    {
        ringText.text = rings.ToString("000");
    }
    public void UpdateScores(int score)
    {
        scoreText.text = score.ToString();
    }
    public void UpdateTimes(string Time)
    {
        timeText.text = Time;
    }
    public void ChangeStatesPauseScreen(bool pause)
    {
        pauseScreen.SetActive(pause);
    }
    public void ChangeStatesEndScoreScreen(bool ended)
    {
        endScoreScreen.SetActive(ended);
    }
    
    public void OnButtonContinue()
    {

    }
    public void OnButtonRestart()
    {

    }
    public void OnButtonExit()
    {

    }
    
}
