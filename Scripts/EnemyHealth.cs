using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour {

    public int enemyMaxHealth; //This is the enemy's max health
    public float enemyDeathTime; //Time enemy has dead.
    public float colorChange = 0f; //Tracks the color change

    int currentEnemyHealth; //This the enemy's current health
    MeshRenderer enemyColor; //This mesh renderer will change the enemy's color as he gets hit
    EnemyNav enemyNav; //Reference to enemy nav to shut if off.
    WaitForSeconds enemyDeathTimer; //Timer for enemy's gameobject to be destroyed.

     void Awake()
    {
        //Fill references
        enemyNav = FindObjectOfType<EnemyNav>();
        enemyColor = GetComponent<MeshRenderer>();  
    }

    void Start()
    {
        //Set enemy's health to max
        currentEnemyHealth = enemyMaxHealth;
        enemyDeathTimer = new WaitForSeconds(enemyDeathTime);
    }

    //Whenever the enemy takes damage, lower their health.
    public void EnemyTakeDamage(int damageTaken)
    {


        if (currentEnemyHealth > 0)
        {
            currentEnemyHealth -= damageTaken;
            enemyColor.material.color = new Color(0, colorChange += .2f, 0f, colorChange+=.2f);

        }
        else
        {
            enemyNav.enabled = false;
            enemyColor.material.color = Color.black;
            StartCoroutine(EnemyDeath());
            
        }
    }

    IEnumerator EnemyDeath()
    {
        yield return enemyDeathTimer;
        gameObject.SetActive(false); //When the enemy at any points hits 0 or below, it dies.
       Destroy(this.gameObject, .5f);
    }
}
