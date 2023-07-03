using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelOver : MonoBehaviour
{
    private AudioManager audioManager;
    private Animation anim;
    private Rigidbody2D rigidBody;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    private IEnumerator OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            anim = collision.GetComponent<Animation>();
            rigidBody = collision.GetComponent<Rigidbody2D>();
            rigidBody.simulated = false;
            anim.Play("Portal");
            StartCoroutine(MoveInPortal(collision.gameObject));

            yield return new WaitForSeconds(0.5f);

            audioManager.PlaySFX(audioManager.levelCompleted);
            GameManager.gameManager.NextLevel();
            gameObject.GetComponent<CircleCollider2D>().enabled = false;

            yield return new WaitForSeconds(1f);

            gameObject.GetComponent<CircleCollider2D>().enabled = true;
        }
    }

    private IEnumerator MoveInPortal(GameObject player)
    {
        float timer = 0f;
        while (timer < 0.5f)
        {
            player.transform.position = Vector2.MoveTowards(player.transform.position, transform.position, 3 * Time.deltaTime);
            yield return new WaitForEndOfFrame();
            timer += Time.deltaTime;
        }
    }
}
