using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    private static MusicManager instance;
    public AudioSource ringsLost;
    public AudioSource ringsGot;
    public AudioSource lifeUp;
    public AudioSource lifeDown;
    public AudioSource scoreUpSound;
    public AudioSource scoreTotalSound;
    public AudioSource menuOpenSound;
    public AudioSource menuAcceptSound;
    public AudioSource menuBleepSound;
    public AudioSource shieldOnSound;
    public AudioSource shieldPopSound;
    public AudioSource bgMusic;
    /// <summary>
    /// Reproduce el sonido de pérdida de anillos.
    /// </summary>
    public void PlayRingsLost()
    {
        ringsLost.Play();
    }
    /// <summary>
    /// Reproduce el sonido de obtención de anillos.
    /// </summary>
    public void PlayRingsGot()
    {
        ringsGot.Play();
    }
    /// <summary>
    /// Reproduce el sonido de aumento de vida.
    /// </summary>
    public void PlayLifeUp()
    {
        lifeUp.Play();
    }
    /// <summary>
    /// Reproduce el sonido de decremento de vida.
    /// </summary>
    public void PlayLifeDown()
    {
        lifeDown.Play();
    }
    /// <summary>
    /// Reproduce el sonido de aumento de puntuación.
    /// </summary>
    public void PlayScoreUpSound()
    {
        scoreUpSound.Play();
    }
    /// <summary>
    /// Reproduce el sonido de puntuación total.
    /// </summary>
    public void PlayScoreTotalSound()
    {
        scoreTotalSound.Play();
    }
    /// <summary>
    /// Reproduce el sonido de apertura de menú.
    /// </summary>
    public void PlayMenuOpenSound()
    {
        menuOpenSound.Play();
    }
    /// <summary>
    /// Reproduce el sonido de aceptación de acción de menú.
    /// </summary>
    public void PlayMenuAcceptSound()
    {
        menuAcceptSound.Play();
    }
    /// <summary>
    /// Reproduce un sonido de pitido para interacciones de menú.
    /// </summary>
    public void PlayMenuBleepSound()
    {
        menuBleepSound.Play();
    }
    /// <summary>
    /// Reproduce el sonido de activación de escudo.
    /// </summary>
    public void PlayShieldOnSound()
    {
        shieldOnSound.Play();
    }
    /// <summary>
    /// Reproduce el sonido de desaparición de escudo.
    /// </summary>
    public void PlayShieldPopSound()
    {
        shieldPopSound.Play();
    }
    /// <summary>
    /// Reproduce la música de fondo.
    /// </summary>
    public void PlayBGMusic()
    {
        bgMusic.Play();
    }
    /// <summary>
    /// Pausa la música de fondo.
    /// </summary>
    public void PauseBGMusic()
    {
        bgMusic.Pause();
    }
}
