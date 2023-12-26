using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoalPlate : MonoBehaviour
{

    public float audioLength;
    public int nextLevel;
    public AudioSource stageClearSound;
    public AudioSource musicSound;


    private void OnTriggerEnter(Collider other)
    {
        audioLength = stageClearSound.clip.length;
        GameObject enteringObject = other.transform.parent.gameObject;
        if (enteringObject.CompareTag("Player"))
        {
            GameController.instance.isTimerRunning = false;
            musicSound.Stop();
            Rigidbody rigidbody = enteringObject.GetComponent<Rigidbody>();
            StartCoroutine(StopCharacter(rigidbody));
            StartCoroutine(Rotation());
            stageClearSound.Play();
            GameController.instance.CalculateBonus();
            HUDControl.instance.UpdateEndLevelText(SceneManager.GetActiveScene().buildIndex);
            HUDControl.instance.ChangeStatesEndScoreScreen(true);
            StartCoroutine(ChangeBonusVictory());
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            StartCoroutine(NextLevel());



        }
    }
    IEnumerator ChangeBonusVictory()
    {
        yield return new WaitForSeconds(audioLength / 2);
        GameController.instance.BonusInScore();
    }
    IEnumerator StopCharacter(Rigidbody rigidbody)
    {
        yield return new WaitForSeconds(0.01f);
        if (rigidbody != null)
            {
                rigidbody.velocity = Vector3.zero;
                rigidbody.constraints = RigidbodyConstraints.FreezeAll;
            }
    }
    IEnumerator Rotation()
    {
        yield return new WaitForSeconds(1);

        float timePassed = 0;
        while (timePassed < 5)
        {
            // Code to go left here
            timePassed += Time.deltaTime;
            transform.Rotate(new Vector3(0f, 1f, 0f));
            yield return null;
        }
    }
    IEnumerator NextLevel()
    {
        yield return new WaitForSeconds(8f);
        HUDControl.instance.NextLevel(nextLevel);
    }
}
