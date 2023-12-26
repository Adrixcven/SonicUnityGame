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
    /// Plays the sound for rings being lost.
    /// </summary>
    public void PlayRingsLost()
    {
        ringsLost.Play();
    }
    public void PlayRingsGot()
    {
        ringsGot.Play();
    }
    public void PlayLifeUp()
    {
        lifeUp.Play();
    }
    /// <summary>
    /// Plays the sound for decrementing a life.
    /// </summary>
    public void PlayLifeDown()
    {
        lifeDown.Play();
    }
    /// <summary>
    /// Plays the sound for increasing the score.
    /// </summary>
    public void PlayScoreUpSound()
    {
        scoreUpSound.Play();
    }
    /// <summary>
    /// Plays the sound for the total score.
    /// </summary>
    public void PlayScoreTotalSound()
    {
        scoreTotalSound.Play();
    }
    /// <summary>
    /// Plays the sound for opening a menu.
    /// </summary>
    public void PlayMenuOpenSound()
    {
        menuOpenSound.Play();
    }
    /// <summary>
    /// Plays the sound for accepting a menu action.
    /// </summary>
    public void PlayMenuAcceptSound()
    {
        menuAcceptSound.Play();
    }
    /// <summary>
    /// Plays a bleep sound for menu interactions.
    /// </summary>
    public void PlayMenuBleepSound()
    {
        menuBleepSound.Play();
    }
    /// <summary>
    /// Plays the sound for turning on a shield.
    /// </summary>
    public void PlayShieldOnSound()
    {
        shieldOnSound.Play();
    }
    /// <summary>
    /// Plays the sound for a shield being popped.
    /// </summary>
    public void PlayShieldPopSound()
    {
        shieldPopSound.Play();
    }
    /// <summary>
    /// Plays the background music.
    /// </summary>
    public void PlayBGMusic()
    {
        bgMusic.Play();
    }
    /// <summary>
    /// Pauses the background music.
    /// </summary>
    public void PauseBGMusic()
    {
        bgMusic.Pause();
    }
}
