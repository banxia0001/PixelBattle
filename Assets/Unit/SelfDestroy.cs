using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestroy : MonoBehaviour
{
    public float timer;
    public void Start()
    {
        Destroy(this.gameObject, timer);
    }
}
