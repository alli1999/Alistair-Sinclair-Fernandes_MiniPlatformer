using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerBehaviour : MonoBehaviour
{
    [SerializeField] HealthBar healthBar;
    
    private Rigidbody2D rigidBody;
    private float strength = 100f;
    private float delay = 0.15f;
    private Vector2 startPosition;
    private AudioManager audioManager;

    public UnityEvent OnBegin;
    public UnityEvent OnDone;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    private void Start()
    {
        startPosition = transform.position;
        rigidBody = GetComponent<Rigidbody2D>();
    }

    public void PlayerDamage(int damage)
    {
        GameManager.gameManager._playerHealth.DealDamage(damage);
        healthBar.SetHealth(GameManager.gameManager._playerHealth.Health);
    }

    public void PlayerHeal(int heal)
    {
        GameManager.gameManager._playerHealth.Heal(heal);
        healthBar.SetHealth(GameManager.gameManager._playerHealth.Health);
    }
    
    //knockback effect
    public void PlayFeedback(GameObject sender)
    {
        StopAllCoroutines();
        OnBegin?.Invoke();
        audioManager.PlaySFX(audioManager.enemyDamage);
        Vector2 direction = (transform.position - sender.transform.position).normalized;
        rigidBody.AddForce(direction * strength, ForceMode2D.Impulse);
        StartCoroutine(Reset());
        if (CheckForDeath())
            Die(0.5f);
    }

    private bool CheckForDeath()
    {
        if(GameManager.gameManager._playerHealth.Health <= 0)
        {
            return true;
        }
        return false;
    }

    public void Die(float duration)
    {
        audioManager.PlaySFX(audioManager.death);
        StartCoroutine(Respawn(duration));
    }

    IEnumerator Respawn(float duration)
    {
        transform.localScale = new Vector3(0, 0, 0);
        GetComponent<Rigidbody2D>().velocity = new Vector3(0, 0, 0);

        yield return new WaitForSeconds(duration);

        transform.localScale = new Vector3(1, 1, 1);
        transform.position = startPosition;
        PlayerHeal(100);
        GetComponent<PlayerController2D>().Reset();
    }

    private IEnumerator Reset()
    {
        yield return new WaitForSeconds(delay);
        rigidBody.velocity = Vector2.zero;
        OnDone?.Invoke();
    }
}
