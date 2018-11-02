using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePlayerBullet : MonoBehaviour {

    public int damageAmount; //Damage the bullet will do
    PlayerHealth playerHealth; //Reference to player's health script

    void Awake()
    {
        playerHealth = FindObjectOfType<PlayerHealth>();
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.tag == "Player")
        {
            playerHealth.PlayerTakeDamage(damageAmount);
        }
    }
}
