using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour {

    PlayerHealth playerHealth; //Holds reference to playerHealth script

    public int amountToHeal; //The amount the player will heal on pick up

     void Awake()
    {
        //Create references
        playerHealth = FindObjectOfType<PlayerHealth>();  
    }

    //When the player enters the trigger volume for the health pick up, heal the player
     void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            playerHealth.PlayerHeal(amountToHeal);
            gameObject.SetActive(false);
        }
    }
}
