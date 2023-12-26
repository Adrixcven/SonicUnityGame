using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{

    [Header("Atributes")]
    public int rings = 0;
    public int actualLives = 3;
    public GameObject player;
    public GameObject shield;
    public bool invincible = false;
    public float invincibleCooldown = 2.0f;
    public bool shieldOn = false;

    private float timer = 0f;
    public int minutes;
    public int seconds;

    public int score = 0;

    [Header("Respawn")]
    public GameObject respawnPoint;

    [Header("Bonus Points")]
    public int timeBonus = 0;
    public int ringBonus = 0;

    [Header("Sounds")]
    public GameObject sfxManager;
    public bool isTimerRunning;

    public bool gamePaused;

    public static GameController instance;
    /// <summary>
    /// Subscribes to the SceneManager's sceneLoaded event, invoking the OnSceneLoaded method.
    /// </summary>
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    /// <summary>
    /// Unsubscribes from the SceneManager's sceneLoaded event to avoid memory leaks.
    /// </summary>
    private void OnDisable()
    {
        // Unsubscribe from the sceneLoaded event to avoid memory leaks
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    /// <summary>
    /// Event handler for the SceneManager's sceneLoaded event.
    /// Handles actions based on the loaded scene, such as initialization or destruction.
    /// </summary>
    /// <param name="scene"></param>
    /// <param name="mode"></param>
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex == 0)
        {
            Destroy(gameObject);
        }
        else
        {
            Time.timeScale = 1;
            isTimerRunning = true;
            rings = 0;
            timer = 0;
            respawnPoint = GameObject.FindGameObjectWithTag("Respawn");
            sfxManager = GameObject.FindGameObjectWithTag("SFX");
            player = GameObject.FindGameObjectWithTag("Player");
            shield = GameObject.FindGameObjectWithTag("Shield");
            shield.SetActive(false);
            HUDControl.instance.UpdateLives(actualLives);
            HUDControl.instance.UpdateRings(rings);
            HUDControl.instance.UpdateScores(score);
        }
    }
    /// <summary>
    ///  Ensures that there is only one instance of this script and persists across scenes.
    /// </summary>
    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {

            Destroy(gameObject);
        }

    }
    /// <summary>
    /// Initializes game variables and updates the UI elements during startup.
    /// </summary>
    public void Start()
    {
        if (Difficulty.instance.currentDifficulty == 1) actualLives = 10;
        rings = 0;
        timer = 0;
        HUDControl.instance.UpdateLives(actualLives);
        HUDControl.instance.UpdateRings(rings);
        HUDControl.instance.UpdateScores(score);
    }
    /// <summary>
    /// Updates the timer, checks for user input to open the pause menu.
    /// </summary>
    private void Update()
    {
        if (isTimerRunning)
        {
            timer += Time.deltaTime;
            minutes = Mathf.FloorToInt(timer / 60f);
            seconds = Mathf.FloorToInt(timer % 60f);
            HUDControl.instance.UpdateTimes(string.Format("{0:00}:{1:00}", minutes, seconds));
        }
        if (Input.GetButtonDown("Cancel"))
        {

            ChangeToPause();
        }

    }
    /// <summary>
    /// Opens or closes the pause menu, controlling time scale and cursor visibility.
    /// </summary>
    private void ChangeToPause()
    {
        PauseMenu();
    }
    /// <summary>
    /// Toggles the pause menu, adjusting audio, time scale, and cursor visibility.
    /// </summary>
    public void PauseMenu()
    {

        sfxManager.GetComponent<MusicManager>().PlayMenuOpenSound();
        gamePaused = !gamePaused;
        if (gamePaused)
        {
            sfxManager.GetComponent<MusicManager>().PauseBGMusic();
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            sfxManager.GetComponent<MusicManager>().PlayBGMusic();
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        Time.timeScale = (gamePaused) ? 0.0f : 1f;
        HUDControl.instance.ChangeStatesPauseScreen(gamePaused);
    }
    /// <summary>
    /// Respawns the player at the designated respawn point.
    /// </summary>
    public void Respawn()
    {
        Animator anim = player.GetComponent<Animator>();
        anim.Play("idle");
        player.transform.position = respawnPoint.transform.position;
    }
    /// <summary>
    /// Adds the specified quantity of rings and updates the HUD accordingly.
    /// </summary>
    /// <param name="quantity">The quantity of rings to add.</param>
    public void AddRings(int quantity)
    {
        rings += quantity;
        HUDControl.instance.UpdateRings(rings);
    }
    /// <summary>
    /// Adds the specified quantity of lives and updates the HUD accordingly.
    /// </summary>
    /// <param name="quantity">The quantity of lives to add.</param>
    public void AddLives(int quantity)
    {
        actualLives += quantity;
        HUDControl.instance.UpdateLives(actualLives);
    }
    /// <summary>
    /// Handles the loss of rings, plays sound effects, and updates the HUD.
    /// If no rings are left, calls the LoseLives method; otherwise, decreases the ring count.
    /// </summary>
    /// <param name="quantity">The quantity of rings to lose.</param>
    public void LoseRings(int quantity)
    {
        Animator anim = player.GetComponent<Animator>();
        if (rings <= 0)
            LoseLives(1);
        else
        {
            if (rings > 10)
            {
                for (int i = 0; i < 10; i++)
                {
                    player.GetComponent<PlayerRingSpawn>().SpawnRings();
                    Debug.Log("Spawn1");
                }
            }
            else
            {
                for (int i = 0; i < rings; i++)
                {
                    player.GetComponent<PlayerRingSpawn>().SpawnRings();
                    Debug.Log("Spawn2");
                }
            }
            sfxManager.GetComponent<MusicManager>().PlayRingsLost();
            anim.Play("sn_damage_miss_loop");
            rings -= quantity;
            HUDControl.instance.UpdateRings(rings);
        }

    }
    /// <summary>
    /// Enables the invincibility state, triggers animation, and schedules invincibility and animation disable routines.
    /// </summary>
    public void InvincibleEnabled()
    {
        Animator anim = player.GetComponent<Animator>();
        invincible = true;
        anim.SetBool("IsHit", true);
        StartCoroutine(InvincAnimationDisableRoutine());
        StartCoroutine(InvincDisableRoutine());
    }
    /// <summary>
    /// Coroutine that disables invincibility after a specified cooldown.
    /// </summary>
    /// <returns>Yield instruction for WaitForSeconds.</returns>
    IEnumerator InvincDisableRoutine()
    {
        yield return new WaitForSeconds(invincibleCooldown);
        invincible = false;
    }
    /// <summary>
    /// Coroutine that disables the hit animation after a short delay.
    /// </summary>
    /// <returns>Yield instruction for WaitForSeconds.</returns>
    IEnumerator InvincAnimationDisableRoutine()
    {
        Animator anim = player.GetComponent<Animator>();
        yield return new WaitForSeconds(0.5f);
        anim.SetBool("IsHit", false);

    }
    /// <summary>
    /// Toggles the state of the shield and plays corresponding sound effects.
    /// </summary>
    public void EnableDisableShield()
    {
        shieldOn = !shieldOn;
        shield.SetActive(shieldOn);
        if (shieldOn)
            sfxManager.GetComponent<MusicManager>().PlayShieldOnSound();
        else
            sfxManager.GetComponent<MusicManager>().PlayShieldPopSound();
    }
    /// <summary>
    /// Handles the loss of lives, plays sound effects, updates the HUD, and triggers death animation.
    /// If no lives are left, calls the LoseGame method; otherwise, decreases the lives count.
    /// </summary>
    /// <param name="quantity">The quantity of lives to lose.</param>
    public void LoseLives(int quantity)
    {
        Animator anim = player.GetComponent<Animator>();
        sfxManager.GetComponent<MusicManager>().PlayLifeDown();
        anim.Play("sn_death_loop");
        if (actualLives <= 0)
            LoseGame();
        else
        {
            actualLives -= quantity;
            HUDControl.instance.UpdateLives(actualLives);
            HUDControl.instance.DeathFade();
        }

    }
    /// <summary>
    /// Coroutine for delayed respawn after a short delay.
    /// </summary>
    /// <returns>Yield instruction for WaitForSeconds.</returns>
    public IEnumerator DelayedRespawnRoutine()
    {
        yield return new WaitForSeconds(0.1f);
        Respawn();
    }
    /// <summary>
    /// Adds points to the score for defeating an enemy.
    /// </summary>
    public void AddPointsEnemy()
    {
        score += 1000;
        HUDControl.instance.UpdateScores(score);
    }
    /// <summary>
    /// Adds points to the score for collecting a ring.
    /// </summary>
    public void AddPointsRing()
    {
        score += 100;
        HUDControl.instance.UpdateScores(score);
    }

    /// <summary>
    /// Calculates bonus points based on rings collected and time taken,
    /// updates the HUD with bonus values, and updates the end-of-level score.
    /// </summary>
    public void CalculateBonus()
    {
        ringBonus = rings * 100;
        int totalSeconds = minutes * 60 + seconds;
        timeBonus = GetReward(totalSeconds);
        HUDControl.instance.UpdateRingBonus(ringBonus);
        HUDControl.instance.UpdateTimeBonus(timeBonus);
        HUDControl.instance.UpdateEndScores(score);
    }
    /// <summary>
    /// Gets the reward points based on the total time taken.
    /// </summary>
    /// <param name="totalSeconds">The total time taken in seconds.</param>
    /// <returns>The reward points based on the total time taken.</returns>
    private static int GetReward(int totalSeconds)
    {
        if (totalSeconds <= 44)
            return 50000;
        else if (totalSeconds <= 60)
            return 10000;
        else if (totalSeconds <= 89)
            return 5000;
        else if (totalSeconds <= 119)
            return 4000;
        else if (totalSeconds <= 179)
            return 3000;
        else if (totalSeconds <= 239)
            return 2000;
        else if (totalSeconds <= 299)
            return 1000;
        else
            return 0;
    }
    /// <summary>
    /// Adds ring and time bonus points to the score continuously until both are zero,
    /// playing a score-up sound for each increment.
    /// </summary>
    public void BonusInScore()
    {
        while (ringBonus > 0 || timeBonus > 0)
        {
            // Decrement num1 and num2, and accumulate the decremented values
            if (ringBonus > 0)
            {
                sfxManager.GetComponent<MusicManager>().PlayScoreUpSound();
                ringBonus -= 100;
                score += 100;
                HUDControl.instance.UpdateRingBonus(ringBonus);
                HUDControl.instance.UpdateScores(score);
                HUDControl.instance.UpdateEndScores(score);
            }
            if (timeBonus > 0)
            {
                sfxManager.GetComponent<MusicManager>().PlayScoreUpSound();
                timeBonus -= 100;
                score += 100;
                HUDControl.instance.UpdateTimeBonus(timeBonus);
                HUDControl.instance.UpdateScores(score);
                HUDControl.instance.UpdateEndScores(score);
            }
        }
        if (ringBonus == 0 && timeBonus == 0)
        {
            sfxManager.GetComponent<MusicManager>().PlayScoreTotalSound();
        }
    }

    /// <summary>
    /// Displays the game over screen and logs a message.
    /// </summary>
    private void LoseGame()
    {
        HUDControl.instance.OnGameOver();
        Debug.Log("Game Over");
    }



}
