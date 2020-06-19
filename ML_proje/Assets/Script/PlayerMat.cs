using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMat : MonoBehaviour
{
    public const int L = 5;
    public const int S = 7;

    public MatrixEntry[] boxsObj;
    public int[,] mat = new int[S, L];


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.frameCount % 7 == 0)
        {
            for(int i = 0; i < boxsObj.Length; ++i)
            {
                mat[i % S, (i - i % S) / S] = boxsObj[i].titleType;
            }
        }
    }

}
