using UnityEngine;
using System.Collections;

public class Asteroid : Planet

{
    public float scrollSpeed = 14f;
    float currentZRot = 0.0f;
    // reference to particles that trigger on collision
    public ParticleSystem asteroidParticles;

    // asteriods move down y axis with some rotation
    public override void Move()
    {
        Vector3 tempPos = pos;
        tempPos.y -= scrollSpeed * Time.deltaTime;
        pos = tempPos;

        currentZRot += Time.deltaTime * scrollSpeed;

        Vector3 rot = transform.rotation.eulerAngles;
        rot.z = currentZRot;
        transform.rotation = Quaternion.Euler(rot);
    }

    // use Utils bounds functions to check offscree and destroy if offscreen
    protected override void CheckOffscreen()
    {
        if (bounds.size == Vector3.zero)
        {
            bounds = Utils.CombineBoundsOfChildren(this.gameObject);
            boundsCenterOffset = bounds.center - transform.position;
        }

        bounds.center = transform.position + boundsCenterOffset;
        Vector3 off = Utils.ScreenBoundsCheck(bounds, BoundsTest.offScreen);
        if (off != Vector3.zero && this.tag == "Planet")
        {
            if (off.y < 0)
            {
                Destroy(this.gameObject);
            }  
        }
    }

}
