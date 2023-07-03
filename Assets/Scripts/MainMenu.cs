using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private AudioManager audioManager;

    public void PlayGame()
    {
        if(GameManager.gameManager == null)
            SceneManager.LoadSceneAsync(1);
        else
            StartCoroutine(GameManager.gameManager.LoadLevel(1));

        if (GameManager.gameManager != null)
        {
            audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
            if(!audioManager.isMusicPlaying)
                audioManager.StartMusic();
        }
    }

    public void Quit()
    {
        Application.Quit();
    }
}
