using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;

public class Player : Agent
{
    public float speed;
    public float jumpForce;
    public GameObject trainMatObj;
    public GameObject groundDetectObj;
    public GameObject rightWallDetectObj;
    public GameObject leftWallDetectObj;

    public int way = 0; // numpat yerleşimiyle 1 : <- | 2 : -> | 3 : stop
    public bool jump;

    Rigidbody rg;
    PlayerMat trainMat;
    GroundDetect touchGround;
    GroundDetect touchRightWall;
    GroundDetect touchLeftWall;

    bool randomStart = false;
    bool moveStart;
    bool levelEnd = false;
    bool died = false;

    public Transform[] startPos;
    public Vector3[] strPos;
    Vector3 gameStart;

    float curr_x;

    // Start is called before the first frame update
    void Start()
    {
        rg = GetComponent<Rigidbody>();
        gameStart = transform.position;
        curr_x = transform.position.x;
        trainMat = trainMatObj.GetComponent<PlayerMat>();
        touchGround = groundDetectObj.GetComponent<GroundDetect>();
        touchRightWall= rightWallDetectObj.GetComponent<GroundDetect>();
        touchLeftWall= leftWallDetectObj.GetComponent<GroundDetect>();

        strPos = new Vector3[startPos.Length];
        for(int i = 0; i < strPos.Length; ++i)
        {
            strPos[i] = startPos[i].position;
        }

    }

    public override void CollectObservations(VectorSensor sensor)
    {
        for (int l = 0; l < PlayerMat.L; ++l)
        {
            for (int s = 0; s < PlayerMat.S; ++s)
            {
                sensor.AddObservation(trainMat.mat[s, l]);
            }
        }
        
    }

    public override void OnActionReceived(float[] vectorAction)
    {

        if (Mathf.FloorToInt(vectorAction[0]) == 1)
        {
            jump = true;
        }
        else
        {
            jump = false;
        }
        way = Mathf.FloorToInt(vectorAction[1]);


        if (jump)
        {
            if (touchGround.isTouchGround)
            {
                if (way == 2)
                {
                    rg.velocity = new Vector3((touchRightWall.isTouchGround) ? 0 : speed, jumpForce, 0);
                }
                else if (way == 1)
                {
                    rg.velocity = new Vector3((touchLeftWall.isTouchGround) ? 0 : -speed, jumpForce, 0);
                }
                else
                {
                    rg.velocity = new Vector3(0, jumpForce, 0);
                }
            }
            else
            {
                if (way == 2)
                {
                    rg.velocity = new Vector3((touchRightWall.isTouchGround) ? 0 : speed, rg.velocity.y, 0);
                }
                else if (way == 1)
                {
                    rg.velocity = new Vector3((touchLeftWall.isTouchGround) ? 0 : -speed, rg.velocity.y, 0);
                }
            }
            moveStart = true;
        }
        else if (way == 2)
        {
            rg.velocity = new Vector3((touchRightWall.isTouchGround) ? 0 : speed, rg.velocity.y, 0);
            moveStart = true;
        }
        else if (way == 1)
        {
            rg.velocity = new Vector3((touchLeftWall.isTouchGround) ? 0 : -speed, rg.velocity.y, 0);
            moveStart = true;
        }
        else
        {
            if (moveStart)
            {
                moveStart = false;
                rg.velocity = new Vector3(0, 0, 0);
            }
        }

        if(curr_x < transform.position.x)
        {
            curr_x = transform.position.x;
            //AddReward(0.1f);
            SetReward(0.2f);
        }

        if (levelEnd)
        {
            //AddReward(5);
            SetReward(10);
            EndEpisode();
        }

        if(died)
        {
            //AddReward(-5);
            SetReward(-0.2f);
            EndEpisode();
        }

    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (Input.GetKey(KeyCode.Space))
        {
            if (touchGround.isTouchGround)
            {
                if (Input.GetKey(KeyCode.RightArrow))
                {
                    rg.velocity = new Vector3((touchRightWall.isTouchGround) ? 0 : speed, jumpForce, 0);
                }
                else if (Input.GetKey(KeyCode.LeftArrow))
                {
                    rg.velocity = new Vector3((touchLeftWall.isTouchGround) ? 0 : -speed, jumpForce, 0);
                }
                else
                {
                    rg.velocity = new Vector3(0, jumpForce, 0);
                }
            }
            else
            {
                if (Input.GetKey(KeyCode.RightArrow))
                {
                    rg.velocity = new Vector3((touchRightWall.isTouchGround) ? 0 : speed, rg.velocity.y, 0);
                }
                else if (Input.GetKey(KeyCode.LeftArrow))
                {
                    rg.velocity = new Vector3((touchLeftWall.isTouchGround) ? 0 : -speed, rg.velocity.y, 0);
                }
            }
            moveStart = true;
        }else if (Input.GetKey(KeyCode.RightArrow))
        {
            rg.velocity = new Vector3((touchRightWall.isTouchGround) ? 0 : speed, rg.velocity.y, 0);
            moveStart = true;
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            rg.velocity = new Vector3((touchLeftWall.isTouchGround) ? 0 : -speed, rg.velocity.y, 0);
            moveStart = true;
        }
        else
        {
            if (moveStart)
            {
                moveStart = false;
                rg.velocity = new Vector3(0, 0, 0);
            }
        }
        */

    }

    public override void OnEpisodeBegin()
    {
        rg.velocity = new Vector3(0, 0, 0);
        levelEnd = false;
        died = false;
        if (randomStart)
            transform.position = strPos[Random.Range(0, 100) % startPos.Length];
        else
            transform.position = gameStart;
        curr_x = transform.position.x;
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Finish")
        {
            levelEnd = true;
        }if(other.tag == "Enemy")
        {
            died = true;
        }
    }

}
