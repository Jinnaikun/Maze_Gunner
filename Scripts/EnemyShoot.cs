using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShoot : MonoBehaviour {

    public GameObject bullet; //Enemy will shoot bullet
    public float bulletSpeed = 500f; //Speed of enemy bullet
 

    public void ShootBullet()
    {
        GameObject shootThis = Instantiate(bullet, transform.position, transform.rotation) as GameObject;
        Rigidbody bulletBody;
        bulletBody = shootThis.GetComponent<Rigidbody>();
        bulletBody.AddRelativeForce(new Vector3(0, 0, bulletSpeed));
    }
}
