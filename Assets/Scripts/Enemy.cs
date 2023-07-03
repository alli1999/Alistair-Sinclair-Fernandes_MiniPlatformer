using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private int damage = 20;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerBehaviour>().PlayerDamage(damage);
            collision.gameObject.GetComponent<PlayerBehaviour>().PlayFeedback(gameObject.transform.parent.gameObject);
        }
    }
}
