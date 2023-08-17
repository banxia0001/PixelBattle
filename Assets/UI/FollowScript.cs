using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FollowScript : MonoBehaviour
{

    public RectTransform RectThis;
    public RectTransform RectTarget;

    void Update()
    {
        RectThis.transform.position = RectTarget.transform.position;
    }
}
