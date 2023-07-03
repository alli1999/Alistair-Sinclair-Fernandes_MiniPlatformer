using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager gameManager { get; private set; }

    public HealthClass _playerHealth = new HealthClass(100, 100);

    [SerializeField] Animator transitionAnim;

    [SerializeField] GameObject youWinText;

    private AudioManager audioManager;

    private void Awake()
    {
        if(gameManager == null)
        {
            gameManager = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this.gameObject);
        }

        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    public void NextLevel()
    {
        StartCoroutine(LoadLevel());
    }

    IEnumerator LoadLevel()
    {      
        if(SceneManager.GetActiveScene().buildIndex == 2)
        {
            youWinText.SetActive(true);

            yield return new WaitForSeconds(5f);

            transitionAnim.SetTrigger("End");
            youWinText.SetActive(false);

            yield return new WaitForSeconds(1f);

            if (audioManager.isMusicPlaying)
                audioManager.StopMusic();
            SceneManager.LoadSceneAsync(0);
        }
        else
        {
            transitionAnim.SetTrigger("End");

            yield return new WaitForSeconds(1f);

            SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
        }
        _playerHealth.Heal(100);
        transitionAnim.SetTrigger("Start");
    }

    public IEnumerator LoadLevel(int level)
    {   
        transitionAnim.SetTrigger("End");

        yield return new WaitForSeconds(1f);

        SceneManager.LoadSceneAsync(level);
        _playerHealth.Heal(100);
        transitionAnim.SetTrigger("Start");
    }
}
