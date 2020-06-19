using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundDetect : MonoBehaviour
{
    public bool isTouchGround;

    private void OnTriggerEnter(Collider other)
    {
        isTouchGround = true;
    }

    private void OnTriggerExit(Collider other)
    {
        isTouchGround = false;
    }
}
