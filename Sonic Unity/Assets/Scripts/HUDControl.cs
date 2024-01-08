using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
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
    /// Inicializa la instancia única y encuentra el objeto SFXManager por etiqueta.
    /// </summary>
    private void Awake()
    {
        instance = this;
        sfxManager = GameObject.FindGameObjectWithTag("SFX");
    }
    /// <summary>
    /// Actualiza el contador de vidas mostrado.
    /// </summary>
    /// <param name="lives">El número de vidas para mostrar.</param>
    public void UpdateLives(int lives)
    {
        livesText.text = lives.ToString();
    }
    /// <summary>
    /// Actualiza el contador de anillos mostrado.
    /// </summary>
    /// <param name="rings">El número de anillos para mostrar.</param>
    public void UpdateRings(int rings)
    {
        ringText.text = rings.ToString("000");
    }
    /// <summary>
    /// Actualiza el contador de puntuación mostrado.
    /// </summary>
    /// <param name="score">La puntuación a mostrar.</param>
    public void UpdateScores(int score)
    {
        scoreText.text = score.ToString("000000");
    }
    /// <summary>
    /// Actualiza el tiempo mostrado.
    /// </summary>
    /// <param name="time">El tiempo a mostrar.</param>
    public void UpdateTimes(string Time)
    {
        timeText.text = Time;
    }
    /// <summary>
    /// Actualiza la pantalla de puntuación al finalizar el nivel.
    /// </summary>
    /// <param name="score">La puntuación al finalizar el nivel.</param>
    public void UpdateEndScores(int score)
    {
        endScoreText.text = score.ToString("000000");
    }
    /// <summary>
    /// Actualiza el texto al finalizar el nivel según la identificación de la escena.
    /// </summary>
    /// <param name="scene">La identificación de la escena.</param>
    public void UpdateEndLevelText(int scene)
    {
        if (scene == 4)
            endLevelText.text = "Sonic got throught Act 2";
        else
            endLevelText.text = "Sonic got throught Act 1";
    }
    /// <summary>
    /// Actualiza la visualización del bono de tiempo.
    /// </summary>
    /// <param name="timeBonus">El bono de tiempo a mostrar.</param>
    public void UpdateTimeBonus(int TimeBonus)
    {
        endTimeBonusText.text = TimeBonus.ToString();
    }
    /// <summary>
    /// Actualiza la visualización del bono de anillos.
    /// </summary>
    /// <param name="ringBonus">El bono de anillos a mostrar.</param>
    public void UpdateRingBonus(int RingBonus)
    {
        endRingsBonusText.text = RingBonus.ToString();
    }

    /// <summary>
    /// Cambia la visibilidad de la pantalla de pausa.
    /// </summary>
    /// <param name="pause">True para activar la pantalla de pausa, false para desactivarla.</param>
    public void ChangeStatesPauseScreen(bool pause)
    {
        pauseScreen.SetActive(pause);
    }
    /// <summary>
    /// Cambia la visibilidad de la pantalla de puntuación final.
    /// </summary>
    /// <param name="ended">True para activar la pantalla de puntuación final, false para desactivarla.</param>
    public void ChangeStatesEndScoreScreen(bool ended)
    {
        endScoreScreen.SetActive(ended);
    }
    /// <summary>
    /// Bloquea el cursor, lo oculta e invoca el método PauseMenu de la instancia GameController.
    /// </summary>
    public void OnButtonContinue()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        GameController.instance.PauseMenu();

    }
    /// <summary>
    /// Recarga la escena actual.
    /// </summary>
    public void OnButtonRestart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    /// <summary>
    /// Carga la escena "Menu".
    /// </summary>
    public void OnButtonExit()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SceneManager.LoadScene("Menu");
        Debug.Log("Funciona");
    }
    /// <summary>
    /// Carga la escena "PlayMenu".
    /// </summary>
    public void OnButtonBeginning()
    {
        SceneManager.LoadScene("PlayMenu");
        Debug.Log("Funciona");
    }
    /// <summary>
    /// Carga la escena "OptionsMenu".
    /// </summary>
    public void OnButtonOptions()
    {
        SceneManager.LoadScene("OptionsMenu");
        Debug.Log("Funciona");
    }
    /// <summary>
    /// Cierra la aplicación.
    /// </summary>
    public void OnButtonExitGame()
    {
        Application.Quit();
        Debug.Log("Funciona");
    }
    /// <summary>
    /// Activa una pantalla de transición, establece la dificultad y carga el nivel especificado después de un retraso.
    /// </summary>
    public void OnButtonOriginalMode()
    {
        Difficulty.instance.SetDifficulty(0);
        NextLevel(3);
        Debug.Log("Funciona Original");
    }
    /// <summary>
    /// Activa una pantalla de transición, establece la dificultad y carga el nivel especificado después de un retraso.
    /// </summary>
    public void OnButtonModernMode()
    {
        Difficulty.instance.SetDifficulty(1);
        NextLevel(3);
        Debug.Log("Funciona Moder");
    }
    /// <summary>
    /// Inicia la carga del próximo nivel con una pantalla de transición y un retraso.
    /// </summary>
    /// <param name="id">La identificación de la escena del próximo nivel.</param>
    public void NextLevel(int id)
    {
        fadeScreen.SetActive(true);
        fadeScreen.GetComponent<Animation>().Play("FadeAnim");
        LoadLevel(id);
    }
    /// <summary>
    /// Carga asincrónicamente una escena de Unity con el ID especificado, mostrando una pantalla de carga durante el proceso.
    /// Este método incluye una demora, asegura un tiempo de carga mínimo y gestiona la visibilidad del cursor.
    /// </summary>
    /// <param name="id">La identificación de la escena del nivel a cargar.</param>
    
    public async void LoadLevel(int id)
    {
        await Task.Delay(1000);
        
        AsyncOperation operation = SceneManager.LoadSceneAsync(id);
        float elapsedLoadTime = 0f;

        currentScreen.SetActive(false);
        loadingScreen.SetActive(true);
        
        while (!operation.isDone)
        {
            elapsedLoadTime += Time.deltaTime;
            await Task.Yield();
        }

        while (elapsedLoadTime < minLoadTime)
        {
            elapsedLoadTime += Time.deltaTime;
            await Task.Yield();
        }
        
    }
    /// <summary>
    /// Inicia una animación de pantalla de transición para la muerte o reaparición.
    /// </summary>
    public void DeathFade()
    {
        fadeScreen.SetActive(true);
        fadeScreen.GetComponent<Animation>().Play("FadeAnimRespawn");
    }

    /// <summary>
    /// Maneja el escenario de fin de juego cargando la escena "GameOver".
    /// </summary>
    public void OnGameOver()
    {
        SceneManager.LoadScene("GameOver");
        Debug.Log("Funciona");
    }
}
