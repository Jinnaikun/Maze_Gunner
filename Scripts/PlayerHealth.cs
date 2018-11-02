using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;

public class PlayerHealth : MonoBehaviour {

    public int playerHealthMax; //Player's starting/maximum health.
    public int currentPlayerHealth; //Player's current health through out the game.
    public Slider healthSlider; //The visual for the player's health

    public Image damageImage; //The visual feedback when the player gets hit
    public float damageFlashSpeed; //Speed in which the damage flash shows
    public Color damageFlashColor; //Color of the damage flash.
    public AudioClip playerHealSound; //Sound that plays when the player picks up a health orb.
    public AudioClip playerHurtSound; //Sounds that play when the player gets hit.
    public AudioClip deathSound; //Quick sound before dying.
    public AudioSource playerSource; //Player's audio source


    LevelController levelController; //Holds the reference to the level controller for the game over screen
    FirstPersonController firstPersonController; //Reference to movement script, needed to disable movement when dead.
    PlayerShootController playerShootController; //Reference to player shooting, neede to disable fire when dead.
    bool isDead = false; //Player's dead state
    

    bool isDamaged = false; //If the player is currently taking damage.



    void Awake()
    {
        levelController = FindObjectOfType<LevelController>();
        firstPersonController = FindObjectOfType<FirstPersonController>();
        playerShootController = FindObjectOfType<PlayerShootController>();
    }
    
    void Start ()
    {
        //Set player's health to max at the beginning
        currentPlayerHealth = playerHealthMax;
        healthSlider.value = currentPlayerHealth;
	}

    void Update()
    {
        if(isDamaged) //When the player is damaged, display the flash by setting the image color to be the same as the flash.
        {
            damageImage.color = damageFlashColor;
        }
        else //After the isDamaged is set to false, have the damage image color fade back to clear.
        {
            damageImage.color = Color.Lerp(damageImage.color, Color.clear, damageFlashSpeed * Time.deltaTime);
        }

        if(currentPlayerHealth > 0)
        isDamaged = false; //The damage state is always being turned off.
    }

    //Decreases the player's health when getting hit
    public void PlayerTakeDamage(int damageTaken)
    {
        playerSource.PlayOneShot(playerHurtSound);
        currentPlayerHealth -= damageTaken;
        healthSlider.value = currentPlayerHealth;
        isDamaged = true;

        if(currentPlayerHealth <= 0)
        {
            PlayerDead();
        }
    }


    //Increase player health when called by a health pick up.
    public void PlayerHeal(int healthHealed)
    
    {
        currentPlayerHealth += healthHealed;
        playerSource.PlayOneShot(playerHealSound);

        //Check if player's health goes over the max
        if(currentPlayerHealth > playerHealthMax)
            {
                currentPlayerHealth = playerHealthMax;
            }

        healthSlider.value = currentPlayerHealth;

    }

    /*Deactivate the player when the player is dead and send position to a script that will teleport and empty object with an audio source
      that can play the death sound while the player is dead.*/
    void PlayerDead()
    {
        levelController.GameOver();
        playerSource.PlayOneShot(deathSound);
        firstPersonController.enabled = false;
        playerShootController.enabled = false;
        if(isDead)
            playerSource.enabled = false;
        isDead = true;
        
    }
}
