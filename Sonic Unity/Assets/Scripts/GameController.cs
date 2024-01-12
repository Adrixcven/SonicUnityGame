using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
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
    /// Se suscribe al evento sceneLoaded del SceneManager, invocando el método OnSceneLoaded.
    /// </summary>
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    /// <summary>
    /// Se cancela la suscripción al evento sceneLoaded del SceneManager para evitar pérdidas de memoria.
    /// </summary>
    private void OnDisable()
    {
        // Unsubscribe from the sceneLoaded event to avoid memory leaks
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    /// <summary>
    /// Manejador de eventos para el evento sceneLoaded del SceneManager.
    /// Maneja acciones basadas en la escena cargada, como la inicialización o destrucción.
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
    /// Asegura que solo haya una instancia de este script y persista a través de las escenas.
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
    /// Inicializa las variables del juego y actualiza los elementos de la interfaz de usuario durante el inicio.
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
    /// Actualiza el temporizador, verifica la entrada del usuario para abrir el menú de pausa.
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
    /// Abre o cierra el menú de pausa, controlando la escala de tiempo y la visibilidad del cursor.
    /// </summary>
    public void ChangeToPause()
    {
        PauseMenu();
    }
    /// <summary>
    /// Alterna el menú de pausa, ajustando el audio, la escala de tiempo y la visibilidad del cursor.
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
    /// Respawn al jugador en el punto de respawn designado. Freezea al jugador y lo desfreezea para evitar bug de rebote
    /// al respawnear.
    /// </summary>
    public async void Respawn()
    {
        Animator anim = player.GetComponent<Animator>();
        anim.Play("idle");
        Rigidbody playerRigidbody = player.GetComponent<Rigidbody>();
        Vector3 spawnPosition = respawnPoint.transform.position;
        Debug.Log(Vector3ToString(spawnPosition));
        player.transform.position = spawnPosition;
        if (playerRigidbody != null)
        {
            playerRigidbody.velocity = Vector3.zero;
            playerRigidbody.constraints = RigidbodyConstraints.FreezeAll;
            await Task.Delay(1);
            playerRigidbody.constraints &= ~RigidbodyConstraints.FreezePositionX;
            playerRigidbody.constraints &= ~RigidbodyConstraints.FreezePositionY;
            playerRigidbody.constraints &= ~RigidbodyConstraints.FreezePositionZ;
        }
        Vector3 playerPosition = player.transform.position;
        Debug.Log(Vector3ToString(playerPosition));
    }
    string Vector3ToString(Vector3 vector)
    {
        return "(" + vector.x.ToString() + ", " + vector.y.ToString() + ", " + vector.z.ToString() + ")";
    }
    /// <summary>
    /// Agrega la cantidad especificada de anillos y actualiza el HUD en consecuencia.
    /// </summary>
    /// <param name="quantity">La cantidad de anillos que se agregarán.</param>
    public void AddRings(int quantity)
    {
        rings += quantity;
        HUDControl.instance.UpdateRings(rings);
    }
    /// <summary>
    /// Agrega la cantidad especificada de vidas y actualiza el HUD en consecuencia.
    /// </summary>
    /// <param name="quantity">La cantidad de vidas que se agregarán.</param>
    public void AddLives(int quantity)
    {
        actualLives += quantity;
        HUDControl.instance.UpdateLives(actualLives);
    }
    /// <summary>
    /// Maneja la pérdida de anillos, reproduce efectos de sonido y actualiza el HUD.
    /// Si no quedan anillos, llama al método LoseLives; de lo contrario, disminuye la cantidad de anillos.
    /// </summary>
    /// <param name="quantity">La cantidad de anillos que se perderán.</param>
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
    /// Habilita el estado de invencibilidad, desencadena la animación y programa rutinas para deshabilitar la invencibilidad y la animación.
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
    /// Rutina que deshabilita la invencibilidad después de un enfriamiento especificado.
    /// </summary>
    /// <returns>Instrucción de espera para WaitForSeconds.</returns>
    IEnumerator InvincDisableRoutine()
    {
        yield return new WaitForSeconds(invincibleCooldown);
        invincible = false;
    }
    /// <summary>
    /// Rutina que deshabilita la animación de golpe después de un breve retraso.
    /// </summary>
    /// <returns>Instrucción de espera para WaitForSeconds.</returns>
    IEnumerator InvincAnimationDisableRoutine()
    {
        Animator anim = player.GetComponent<Animator>();
        yield return new WaitForSeconds(0.5f);
        anim.SetBool("IsHit", false);

    }
    /// <summary>
    /// Alterna el estado del escudo y reproduce efectos de sonido correspondientes.
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
    /// Maneja la pérdida de vidas, reproduce efectos de sonido, actualiza el HUD y activa la animación de muerte.
    /// Si no quedan vidas, llama al método LoseGame; de lo contrario, disminuye la cantidad de vidas.
    /// </summary>
    /// <param name="quantity">La cantidad de vidas que se perderán.</param>
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
    /// Espera 0.1 segundos para ejecutar el metodo Respawn()
    /// </summary>
    /// <returns>Instrucción de espera para WaitForSeconds.</returns>
    public async void DelayedRespawnRoutine()
    {
        await Task.Delay(100);
        Respawn();
    }
    /// <summary>
    /// Agrega puntos al puntaje por derrotar a un enemigo.
    /// </summary>
    public void AddPointsEnemy()
    {
        score += 1000;
        HUDControl.instance.UpdateScores(score);
    }
    /// <summary>
    /// Agrega puntos al puntaje por recoger un anillo.
    /// </summary>
    public void AddPointsRing()
    {
        score += 100;
        HUDControl.instance.UpdateScores(score);
    }

    /// <summary>
    /// Calcula puntos de bonificación según los anillos recogidos y el tiempo transcurrido,
    /// actualiza el HUD con valores de bonificación y el puntaje al final del nivel.
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
    /// Obtiene los puntos de recompensa basados en el tiempo total transcurrido.
    /// </summary>
    /// <param name="totalSeconds">El tiempo total transcurrido en segundos.</param>
    /// <returns>Los puntos de recompensa basados en el tiempo total transcurrido.</returns>
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
    /// Agrega puntos de bonificación de anillos y tiempo al puntaje continuamente hasta que ambos sean cero,
    /// reproduciendo un sonido de aumento de puntaje por cada incremento.
    /// </summary>
    public void BonusInScore()
    {
        while (ringBonus > 0 || timeBonus > 0)
        {
            // Reduce ringBonus y timeBonus, y acumula los valores reducidos
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
    /// Muestra la pantalla de fin de juego y registra un mensaje.
    /// </summary>
    private void LoseGame()
    {
        HUDControl.instance.OnGameOver();
        Debug.Log("Game Over");
    }
}
