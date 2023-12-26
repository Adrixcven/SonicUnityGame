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
    /// Subscribes to the SceneManager's sceneLoaded event to handle scene loading.
    /// </summary>
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    /// <summary>
    /// Unsubscribes from the SceneManager's sceneLoaded event to prevent memory leaks.
    /// </summary>
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    /// <summary>
    /// Destroys the GameObject if the loaded scene has a build index of 3 or 4.
    /// </summary>
    /// <param name="scene">The loaded Scene.</param>
    /// <param name="mode">The LoadSceneMode used to load the scene.</param>
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex == 3 || scene.buildIndex == 4)
        {
            Destroy(gameObject);
        }
    }
    /// <summary>
    /// Sets up the singleton pattern to ensure only one instance exists across scenes.
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
    /// Starts playing the main menu OST audio source.
    /// </summary>
    private void Start()
    {
        mainMenuOST.Play();
    }
    /// <summary>
    /// Plays the menu accept sound.
    /// </summary>
    public void PlayMenuAcceptSound()
    {
        menuAcceptSound.Play();
    }
    /// <summary>
    /// Plays the menu bleep sound.
    /// </summary>
    public void PlayMenuBleepSound()
    {
        menuBleepSound.Play();
    }
}
