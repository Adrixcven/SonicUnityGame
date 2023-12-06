using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{

    [Header("Atributes")]
    public int Rings = 0;
    public int ActualLives = 3;
    public GameObject player;

    private float timer = 0f;

    public int Score = 0;

    [Header("Respawn")]

    public GameObject RespawnPoint;



    public bool GamePaused;

    public static GameController instance;

    public void Awake()
    {
        instance = this;
    }

    public void Start()
    {
        HUDControl.instance.UpdateLives(ActualLives);
        HUDControl.instance.UpdateRings(Rings);

    }
    

    private void Update()
    {
        timer += Time.deltaTime;
        int minutes = Mathf.FloorToInt(timer / 60f);
        int seconds = Mathf.FloorToInt(timer % 60f);
        HUDControl.instance.UpdateTimes(string.Format("{0:00}:{1:00}", minutes, seconds));

        if(Input.GetButtonDown("Cancel"))
        {
            ChangeToPause();
        }
        
    }
    private void ChangeToPause()
    {
        GamePaused = !GamePaused;
        Time.timeScale = (GamePaused) ? 0.0f : 1f;
        HUDControl.instance.ChangeStatesPauseScreen(GamePaused);
    }
    public void respawn()
    {
        Animator anim = player.GetComponent<Animator>();
        anim.Play("idle");
        player.transform.position = RespawnPoint.transform.position;
    }
    public void AddRings(int quantity)
    {
        Rings += quantity;
        HUDControl.instance.UpdateRings(Rings);
    }
    public void AddLives(int quantity)
    {
        ActualLives += quantity;
        HUDControl.instance.UpdateLives(ActualLives);
    }
    public void LoseRings(int quantity)
    {
        Rings -= quantity;
        HUDControl.instance.UpdateRings(Rings);

        if(Rings <= 0)
            LoseGame();
    }

    public void LoseLives(int quantity)
    {
        ActualLives -= quantity;
        HUDControl.instance.UpdateLives(ActualLives);

        if(ActualLives < 0)
            LoseGame();
    }
    private void LoseGame()
    {
        Debug.Log("Game Over");
    }

}
