using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class starfield : MonoBehaviour
{
    //private Vector3 position;
    private Quaternion rotation;
    public bool isLeft = true;
    //private Transform tx;
    /*private ParticleSystem.Particle[] stars;
    public int starsMax = 1000;
    public float starSize = 0.2f;
    public float starDistance = 100f;
    private float starDistanceSqr;*/

    // Start is called before the first frame update
    void Start()
    {
        setStarsMovement();

        //tx = transform;
        //starDistanceSqr = starDistance * starDistance;
    }

    /*private void createStars()
    {
        stars = new ParticleSystem.Particle[starsMax];
        for(int i=0; i<starsMax; i++)
        {
            stars[i].position = Random.insideUnitSphere * starDistance; //+ tx.position;
            //stars[i].color = new Color(1, 1, 1, 1);
            stars[i].size = starSize;
        }
    }*/

    private void setStarsMovement()
    {
        //position = transform.position;
        rotation = transform.rotation;
        //determine the transform position and rotation according to the given trajectory
        //TODO: add the ability to accept a trajectory input
        //movement left
        if (isLeft)
        {
            //position.x -= 20;
            rotation = Quaternion.Euler(rotation.x, rotation.y - 231, rotation.z);
        }
        else
        {
            //movement right
            //position.x += 20;
            rotation = Quaternion.Euler(rotation.x, rotation.y - 161, rotation.z);
        }

        //transform.SetPositionAndRotation(position, rotation);
        //transform.position = position;
        transform.rotation = rotation;
    }

    // Update is called once per frame
    void Update()
    {
        //if (stars == null) createStars();

        /*for (int i = 0; i < starsMax; i++)
        {
            if((stars[i].position - tx.position).sqrMagnitude > starDistanceSqr)
            {
                stars[i].position = Random.insideUnitSphere * starDistance + tx.position;
            }
        }*/

            //ParticleSystem.SetParticles(stars, stars.Length);
        }
}
