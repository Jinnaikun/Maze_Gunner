using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmnoPickup : MonoBehaviour {

    public int amountAmnoToGive; //The amount of amno the player will receive upon pick up

    PlayerShootController playerShootController; //Holds reference to player's shooting/gun script


    void Awake()
    {
        //Create references
        playerShootController = FindObjectOfType<PlayerShootController>();
        
    }


    //Whenever the player enters the trigger for the pick up, they will get amno
     void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerShootController.gainAmno(amountAmnoToGive);
            gameObject.SetActive(false);
        }
    }


}
