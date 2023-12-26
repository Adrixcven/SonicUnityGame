using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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

    public TMPro.TextMeshProUGUI endLevelText;
    public TMPro.TextMeshProUGUI endScoreText;
    public TMPro.TextMeshProUGUI endTimeBonusText;
    public TMPro.TextMeshProUGUI endRingsBonusText;

    public GameObject loadingScreen;
    public GameObject currentScreen;
    public GameObject fadeScreen;

    public float minLoadTime;
    public GameObject sfxManager;

    public static HUDControl instance;
    /// <summary>
    /// Initializes the singleton instance and finds the SFXManager GameObject by tag.
    /// </summary>
    private void Awake()
    {
        instance = this;
        sfxManager = GameObject.FindGameObjectWithTag("SFX");
    }
    /// <summary>
    /// Updates the displayed lives count.
    /// </summary>
    /// <param name="lives">The number of lives to display.</param>
    public void UpdateLives(int lives)
    {
        livesText.text = lives.ToString();
    }
    /// <summary>
    /// Updates the displayed ring count.
    /// </summary>
    /// <param name="rings">The number of rings to display.</param>
    public void UpdateRings(int rings)
    {
        ringText.text = rings.ToString("000");
    }
    /// <summary>
    /// Updates the displayed score count.
    /// </summary>
    /// <param name="score">The score to display.</param>
    public void UpdateScores(int score)
    {
        scoreText.text = score.ToString("000000");
    }
    /// <summary>
    /// Updates the displayed time.
    /// </summary>
    /// <param name="time">The time to display.</param>
    public void UpdateTimes(string Time)
    {
        timeText.text = Time;
    }
    /// <summary>
    /// Updates the end-level score display.
    /// </summary>
    /// <param name="score">The end-level score to display.</param>
    public void UpdateEndScores(int score)
    {
        endScoreText.text = score.ToString("000000");
    }
    /// <summary>
    /// Updates the end-level text based on the scene ID.
    /// </summary>
    /// <param name="scene">The scene ID.</param>
    public void UpdateEndLevelText(int scene)
    {
        if (scene == 4)
            endLevelText.text = "Sonic got throught Act 2";
        else
            endLevelText.text = "Sonic got throught Act 1";
    }
    /// <summary>
    /// Updates the time bonus display.
    /// </summary>
    /// <param name="timeBonus">The time bonus to display.</param>
    public void UpdateTimeBonus(int TimeBonus)
    {
        endTimeBonusText.text = TimeBonus.ToString();
    }
    /// <summary>
    /// Updates the ring bonus display.
    /// </summary>
    /// <param name="ringBonus">The ring bonus to display.</param>
    public void UpdateRingBonus(int RingBonus)
    {
        endRingsBonusText.text = RingBonus.ToString();
    }

    /// <summary>
    /// Changes the visibility of the pause screen.
    /// </summary>
    /// <param name="pause">True to activate the pause screen, false to deactivate.</param>
    public void ChangeStatesPauseScreen(bool pause)
    {
        pauseScreen.SetActive(pause);
    }
    /// <summary>
    /// Changes the visibility of the end score screen.
    /// </summary>
    /// <param name="ended">True to activate the end score screen, false to deactivate.</param>
    public void ChangeStatesEndScoreScreen(bool ended)
    {
        endScoreScreen.SetActive(ended);
    }
    /// <summary>
    /// Locks the cursor, hides it, and invokes the PauseMenu method from the GameController instance.
    /// </summary>
    public void OnButtonContinue()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        GameController.instance.PauseMenu();

    }
    /// <summary>
    /// Reloads the current scene.
    /// </summary>
    public void OnButtonRestart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    /// <summary>
    /// Loads the "Menu" scene.
    /// </summary>
    public void OnButtonExit()
    {
        SceneManager.LoadScene("Menu");
        Debug.Log("Funciona");
    }
    /// <summary>
    /// Loads the "PlayMenu" scene.
    /// </summary>
    public void OnButtonBeginning()
    {
        SceneManager.LoadScene("PlayMenu");
        Debug.Log("Funciona");
    }
    /// <summary>
    /// Loads the "OptionsMenu" scene.
    /// </summary>
    public void OnButtonOptions()
    {
        SceneManager.LoadScene("OptionsMenu");
        Debug.Log("Funciona");
    }
    /// <summary>
    /// Quits the application.
    /// </summary>
    public void OnButtonExitGame()
    {
        Application.Quit();
        Debug.Log("Funciona");
    }
    /// <summary>
    /// Activates a fade screen, sets the difficulty, and loads the specified level after a delay.
    /// </summary>
    public void OnButtonOriginalMode()
    {
        fadeScreen.SetActive(true);
        fadeScreen.GetComponent<Animation>().Play("FadeAnim");
        Difficulty.instance.SetDifficulty(0);
        StartCoroutine(LoadLevel(3));
        Debug.Log("Funciona Original");
    }
    /// <summary>
    /// Activates a fade screen, sets the difficulty, and loads the specified level after a delay.
    /// </summary>
    public void OnButtonModernMode()
    {
        fadeScreen.SetActive(true);
        fadeScreen.GetComponent<Animation>().Play("FadeAnim");
        Difficulty.instance.SetDifficulty(1);
        StartCoroutine(LoadLevel(3));
        Debug.Log("Funciona Moder");
    }
    /// <summary>
    /// Initiates the loading of the next level with a fade screen and delay.
    /// </summary>
    /// <param name="id">The scene ID of the next level.</param>
    public void NextLevel(int id)
    {
        fadeScreen.SetActive(true);
        fadeScreen.GetComponent<Animation>().Play("FadeAnim");
        StartCoroutine(LoadLevel(id));
    }
    /// <summary>
    /// Coroutine to load a level with a delay and display a loading screen.
    /// </summary>
    /// <param name="id">The scene ID of the level to load.</param>
    IEnumerator LoadLevel(int id)
    {
        yield return new WaitForSeconds(1);
        AsyncOperation operation = SceneManager.LoadSceneAsync(id);
        float elapsedLoadTime = 0f;

        currentScreen.SetActive(false);
        loadingScreen.SetActive(true);
        while (!operation.isDone)
        {
            elapsedLoadTime += Time.deltaTime;
            yield return null;
        }

        while (elapsedLoadTime < minLoadTime)
        {
            elapsedLoadTime += Time.deltaTime;
            yield return null;
        }
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    /// <summary>
    /// Initiates a fade screen animation for death or respawn.
    /// </summary>
    public void DeathFade()
    {
        fadeScreen.SetActive(true);
        fadeScreen.GetComponent<Animation>().Play("FadeAnimRespawn");
    }

    /// <summary>
    /// Handles the game over scenario by loading the "GameOver" scene.
    /// </summary>
    public void OnGameOver()
    {
        SceneManager.LoadScene("GameOver");
        Debug.Log("Funciona");
    }

}
