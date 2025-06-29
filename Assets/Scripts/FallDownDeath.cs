using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallDownDeath : MonoBehaviour
{
    private int damage = 100;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerBehaviour>().PlayerDamage(damage);
            collision.gameObject.GetComponent<PlayerBehaviour>().PlayFeedback(this.gameObject.transform.parent.gameObject);
        }
    }
}
