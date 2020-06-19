using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatrixEntry : MonoBehaviour
{
    public int titleType;    //  0 -> empty | 1-> ground | 2 -> half pass

    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "Ground":
                titleType = 1;
                break;
            case "HalfPass":
                titleType = 2;
                break;
            case "Enemy":
                titleType = 3;
                break;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        titleType = 0;
    }

}
