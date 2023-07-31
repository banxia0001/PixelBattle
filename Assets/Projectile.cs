using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public int dam;
    public float flySpeed;
    public Vector3 arrowDropLocation;

    void Start()
    {
        
    }

    public void SetUpArror(Vector3 arrowDropLocation, int dam, float flySpeed)
    {
        this.arrowDropLocation = arrowDropLocation;
        this.flySpeed = flySpeed;
        transform.LookAt(arrowDropLocation);
    }

    // Update is called once per frame
    void Update()
    {
        float dist = Vector3.Distance(transform.position, arrowDropLocation);
        if (dist < 0.01f)
        {

        }
        else
        {
            Vector3.MoveTowards(transform.position, arrowDropLocation, 1 * flySpeed);

        }
    }
}
