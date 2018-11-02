using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerShootController : MonoBehaviour {
 
    public float shootDistance; //This is how far the bullet / hit detection goes.
    public int gunDamage; //This is how much damage each shot will do.
    public int maxAmnoToHold; //The maximum amount of amno you can hold.
    public int maxAmnoMag; //Maximum amount of amno in a magazine.
    public float timeBetweenShots; //Time it takes for the player to shoot again after shooting.
    public float timeBetweenReload; //Time it takes for the player to reload.
   
    public float maxPitch; //Highest pitch for the audio
    public float minPitch; //Lowest pitch for the audio
    public GameObject bulletHole; //Bullet hole will be placed on an object that the player hit.

    public Text amnoAmountUI; //The amno amount displayed to the player
    public Slider reloadTimer; //A visual display of the remaining reload time.
    public AudioClip reloadSound; //Reload sound goes here.
    public AudioClip gunShot; //Sound of the gun shooting
    public AudioClip gunEmptyClick; //Sound of an empty gun shooting.
    public AudioSource gunSource; //The sounds coming from the gun
    public AudioClip amnoPickupSound; //Player will make a sound when picking up the amno pick up.


    Rigidbody throwingGunBody; //Rigidbody of the throwing gun.
    bool isReloading = false; //Checks if the player is reloading
    float nextFire; //Time allowed for the player to shoot again.
    int currentAmno; //Current magazine amno
    int remainingAmno;// Other remaining bullets
    LineRenderer laserLine; //The line of the player's laser.
    RaycastHit hitInfo; //This holds the information gathered from what the player shot.
    Camera mainCamera; //This holds the main camera
    Transform gunEnd; //This is the transform of the end of the gun.
    EnemyHealth enemyHealth; //This holds the reference to the enemy health script
    WaitForSeconds shotDurration = new WaitForSeconds(.05f); //Time it takes for the bullet to fade out
    WaitForSeconds reloadDurration; //Holds the time it takes for the player to reload in the coroutine


    // Variables used in my testing.
    // public GameObject playerGun; //The player's gun.
    // public GameObject throwingGun; //The gun the player will throw
    // public float throwDistance; //This is how good the player's throwing arm is
    // public float explosionPower; //How powerful in terms of force the explosion is
    // public float explosionRadius; //The radius of the explosion
    // public float throwDistance; //This is how good the player's throwing arm is.
    //public AudioClip gunExplosion; //Sound of the gun exploding
    //public AudioClip gunExplosionDud; //Sound of gun not exploding due to lack of bullets.
    //public float timeBetweenGunRespawn;//Time it takes to respawn the gun.
    // WaitForSeconds respawnGun; //Timer for respawning the gun.




    void Awake()
    {
        //Always finds the MainCamera game object and uses the camera component of the MainCamera object.
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").gameObject.GetComponent<Camera>();
        gunEnd = GameObject.FindGameObjectWithTag("RayOrigin").gameObject.GetComponent<Transform>();
        enemyHealth = FindObjectOfType<EnemyHealth>();
        laserLine = GetComponentInChildren<LineRenderer>();
    }

     void Start()
    {
        
        remainingAmno = maxAmnoToHold; //Make the player start with max amno.
        currentAmno = maxAmnoMag; //Their current amno will be the max they can hold in their magazine.
        amnoAmountUI.text = currentAmno + "/" + remainingAmno; //Updates the current amount of amno the player has at the start.
        reloadDurration = new WaitForSeconds(timeBetweenReload); //Updates the reload durration.

 


    }

    void Update ()
    {
        //When the player left clicks, call the player shooting function
		if(Input.GetMouseButtonDown(0) && Time.time > nextFire)
        {
            nextFire = Time.time + timeBetweenShots;
            if (!isReloading) //If the player is reloading, they cannot shoot.
                PlayerPrimaryFire();
        }

        //When the player presses R, call the player's reload function
        if(Input.GetKeyDown(KeyCode.R))
        {
            if (!isReloading) //If the player is reloading, they cannot reload while reloading.
                PlayerReload();
        }

       /* if(Input.GetKeyDown(KeyCode.G)) //Did not happen
        {
            if(!isReloading)
            {
                PlayerThrowGun();
            }
        } */

        if(isReloading) //When reloading, call the visual manager for the reload timer.
        {
            ReloadTimerManager();
        }
	}

    //This function when called makes the player shoot.
    void PlayerPrimaryFire()
    {
        gunSource.pitch = Random.Range(minPitch, maxPitch); //Randomize the pitch

        if (currentAmno > 0) //When there's amno available
        {
            Vector3 rayOrigin = mainCamera.ViewportToWorldPoint(new Vector3(.5f, .5f, 0));
            Vector3 fireDirection = mainCamera.transform.forward; //Direction of the shot, shoots forward from the camera

            StartCoroutine(PlayerShootEffect());

            laserLine.SetPosition(0, gunEnd.position);

            currentAmno -= 1;
            gunSource.PlayOneShot(gunShot);
            amnoAmountUI.text = currentAmno + "/" + remainingAmno; //Refresh amno display on the UI

            if (Physics.Raycast(rayOrigin, fireDirection, out hitInfo, shootDistance))
            {

                laserLine.SetPosition(1, hitInfo.point);
                Vector3 offsetPos = new Vector3(hitInfo.point.x, hitInfo.point.y, hitInfo.point.z + (hitInfo.normal.z * .1f));
                Instantiate(bulletHole);
                bulletHole.transform.position = offsetPos;

                if (hitInfo.transform.tag == "Enemy")
                {
                    enemyHealth.EnemyTakeDamage(gunDamage);
                }
            }
            else //If the raycast hits nothing
            {
                laserLine.SetPosition(1, rayOrigin + (fireDirection * shootDistance));
            }
        }
        else //When there's no amno available
            gunSource.PlayOneShot(gunEmptyClick);

    }

    //This function displays the player's shooting effect
    IEnumerator PlayerShootEffect()
    {
        laserLine.enabled = true;
        yield return shotDurration;
        laserLine.enabled = false;
    }

    

    //This function refills the amno inside the gun by using their reserves/remaining amno
    void PlayerReload()
    {
        gunSource.pitch = Random.Range(minPitch, maxPitch); //Randomize the pitch
        if (currentAmno < maxAmnoMag && remainingAmno > 0) //If the player CAN reload (Current amno is less than a full magazine and has reserves)
        {
            gunSource.PlayOneShot(reloadSound);

            //Slider has the starting value for the reload time and it's max value.
            reloadTimer.maxValue = timeBetweenReload;
            reloadTimer.value = timeBetweenReload;

            StartCoroutine(PlayerReloadCooldown());
            int bulletsToReload; //Will hold the amount of bullets that will be removed from the player's reserves and added to their current bullets.
            bulletsToReload = maxAmnoMag - currentAmno; //Only the bullets missing will be replaced, the player will not lose their current bullets.

            if (bulletsToReload <= remainingAmno) //Player has enough bullets remaining to reload their gun
            {
                remainingAmno -= bulletsToReload;
                currentAmno += bulletsToReload; 
            }
            else //Player can only reload with what they have left.
            {
                currentAmno += remainingAmno;
                remainingAmno -= remainingAmno;
            }
        }
    }

    //This function stalls when the player's fire and reload until it is finished reloading.
    IEnumerator PlayerReloadCooldown()
    {
        isReloading = true;
        reloadTimer.gameObject.SetActive(true); //Display reload timer
        yield return reloadDurration;
        isReloading = false;
        reloadTimer.gameObject.SetActive(false);
        amnoAmountUI.text = currentAmno + "/" + remainingAmno; //Refresh amno display on the UI after the player is done reloading.
        reloadTimer.value = timeBetweenReload; //Refresh the timer's display after reloading.
    }

    
    void ReloadTimerManager()
    {
        reloadTimer.value -= Time.deltaTime;
    }

    //This function is called whenever the player picks up an amno pack
    public void gainAmno(int amnoRecieved)
    {
        gunSource.PlayOneShot(amnoPickupSound);
        remainingAmno += amnoRecieved;

        //Make sure the player can't get more amno than they're supposed to hold.
        if (remainingAmno > maxAmnoToHold)
            remainingAmno = maxAmnoToHold;

        amnoAmountUI.text = currentAmno + "/" + remainingAmno; //Refresh amno display on the UI

    }


    //This part of the code is where I tried to impliment the exploding gun.
   /* //Player will throw explosive gun and deal damage in an area based on the number of bullets in the gun.
    void PlayerThrowGun()
    {
        gunSource.pitch = Random.Range(minPitch, maxPitch);

        //Calculate the explosion damage
        int explosionDamage;
        explosionDamage = currentAmno * gunDamage;
        currentAmno = 0;

        reloadTimer.maxValue = timeBetweenGunRespawn;
        reloadTimer.value = timeBetweenGunRespawn;
        StartCoroutine(GunRespawnTimer());
        playerGun.SetActive(false);

        Instantiate(throwingGun, transform);
        throwingGunBody = throwingGun.GetComponent<Rigidbody>();
        throwingGunBody.isKinematic = false;
        throwingGunBody.AddForce(transform.forward * throwDistance);
        

        if(explosionDamage < 0)
        {
            gunSource.PlayOneShot(gunExplosionDud);
        }
        else
        {
            gunSource.PlayOneShot(gunExplosion);
            throwingGunBody.AddExplosionForce(explosionPower, throwingGun.transform.position, explosionRadius, 3f);
        }
    }

    IEnumerator GunRespawnTimer()
    {
        isReloading = true;
        reloadTimer.gameObject.SetActive(true); //Display reload timer
        yield return GunRespawnTimer();
        reloadTimer.gameObject.SetActive(false);
        playerGun.SetActive(true);
    }
    */
}
