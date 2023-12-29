using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuMusic : MonoBehaviour
{
    private static MainMenuMusic instance;
    public AudioSource mainMenuOST;
    public AudioSource menuAcceptSound;
    public AudioSource menuBleepSound;

    /// <summary>
    /// Se suscribe al evento sceneLoaded del SceneManager para manejar la carga de escenas.
    /// </summary>
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    /// <summary>
    /// Se desuscribe del evento sceneLoaded del SceneManager para evitar fugas de memoria.
    /// </summary>
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    /// <summary>
    /// Destruye el GameObject si la escena cargada tiene un índice de construcción de 3 o 4.
    /// </summary>
    /// <param name="scene">La escena cargada.</param>
    /// <param name="mode">El LoadSceneMode utilizado para cargar la escena.</param>
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex == 3 || scene.buildIndex == 4)
        {
            Destroy(gameObject);
        }
    }
    /// <summary>
    /// Configura el patrón singleton para garantizar que solo exista una instancia en todas las escenas.
    /// </summary>
    private void Awake()
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
    /// Inicia la reproducción del audio de la música del menú principal.
    /// </summary>
    private void Start()
    {
        mainMenuOST.Play();
    }
    /// <summary>
    /// Reproduce el sonido de aceptación del menú.
    /// </summary>
    public void PlayMenuAcceptSound()
    {
        menuAcceptSound.Play();
    }
    /// <summary>
    /// Reproduce el sonido de pitido del menú.
    /// </summary>
    public void PlayMenuBleepSound()
    {
        menuBleepSound.Play();
    }
}
