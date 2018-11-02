using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyNav : MonoBehaviour {

  
	public float rangeOfAwareness = 5f; //The range in which the enemy will start shooting
	public GameObject target; //The enemy's target to hunt, which will be the player
    public float attackFrequency; //How often the enemy will attack, can change depending on the enemy in the inspector

    EnemyShoot enemyShoot; //Reference to the enemy attack script
    RaycastHit hitInfo; //Information from the enemy's raycast
	NavMeshAgent agent; //Allows the enemy to move around the map
	bool playerInRange = false; //Boolean checks to see if the player is in range or not.
    float timer; //Timer to count for next attacks.

	void Awake()
	{
        //Fill references
		agent = GetComponent <NavMeshAgent>();
		if(target == null)
		target = GameObject.FindGameObjectWithTag ("Player");
        enemyShoot = gameObject.GetComponent<EnemyShoot>(); //This will find the ranged attack script attached to this object.
	}

	void Update()
	{
       
        {
            DetectPlayerRange(); //Always checks if the player is in range.
            timer += Time.deltaTime; //Keeps the timer for attack intervals going.

            if (!playerInRange)
            {
                HuntPlayer();
            }
            else if (timer >= attackFrequency && playerInRange)
            {
                Attack();
            }
        }
        
    }

    //Checks to see if the player is within the enemy's range
	void DetectPlayerRange()
	{

		float currentPlayerDistance = Vector3.Distance (transform.position, target.transform.position);
		//Debug.Log (currentPlayerDistance);

		if (currentPlayerDistance <= rangeOfAwareness) 
		{
			playerInRange = true;

		} 
		else 
		{
			playerInRange = false;

		}


	}

    //Continue to follow player if the player is out of range
	void HuntPlayer()
	{
		agent.SetDestination(target.transform.position);

        if (agent.isStopped)
        {
            agent.isStopped = false;
        }
	}


    //Attack the player if within the range.
	void Attack()
	{
		agent.isStopped = true;
		transform.LookAt(target.transform);

        //When staring at the player's direction, check for a wall
        if(Physics.Raycast(gameObject.transform.position, gameObject.transform.forward, out hitInfo, rangeOfAwareness))
        {
            if(hitInfo.transform.tag == "Wall") //Continue hunting the player if there's a wall in the way.
            {
                HuntPlayer();
            }
            else if(hitInfo.transform.tag == "Player")
            {
                timer = 0f; //Whenever the enemy attacks, reset the timer.
                enemyShoot.ShootBullet();
            }
        }

	}
}
