using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletProjectileRaycast : MonoBehaviour
{
    public GameObject impactParticle; // Effect spawned when projectile hits a collider
    public GameObject projectileParticle; // Effect attached to the gameobject as child
    public GameObject muzzleParticle; // Effect instantly spawned when gameobject is spawned
    [SerializeField] private float shootSpeed = 200f;

    private Vector3 targetPosition;

    private void Start()
    {
        projectileParticle = Instantiate(projectileParticle) as GameObject;
        projectileParticle.transform.SetParent(transform, false);

        if (muzzleParticle)
        {
            muzzleParticle = Instantiate(muzzleParticle, transform.position, transform.rotation) as GameObject;
            Destroy(muzzleParticle, 1.5f); // 2nd parameter is lifetime of effect in seconds
        }
    }

    public void Setup(Vector3 targetPosition)
    {
        this.targetPosition = targetPosition;
    }

    private void Update()
    {
        var distanceBefore = Vector3.Distance(transform.position, targetPosition);

        Vector3 moveDirection = (targetPosition - transform.position).normalized;
        transform.position += moveDirection * shootSpeed * Time.deltaTime;


        var distanceAfter = Vector3.Distance(transform.position, targetPosition);
        
        if (distanceBefore < distanceAfter)
        {
            GameObject impactP = Instantiate(impactParticle, targetPosition, Quaternion.identity);
            Destroy(impactP, 3f);
            Destroy(gameObject);
        }
    }
}
