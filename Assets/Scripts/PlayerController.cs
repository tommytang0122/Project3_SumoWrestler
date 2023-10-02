using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField]
    GameObject player;
    [SerializeField]
    GameObject player2;


    Rigidbody rig1;
    Rigidbody rig2;

    // Start is called before the first frame update
    void Start()
    {
        rig1 = player.GetComponent<Rigidbody>();
        rig2 = player2.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {



        //Player1
        player.transform.Rotate(0f, 2.0f, 0f, Space.Self);
        if (Input.GetKey(KeyCode.W))
        {
            player.transform.forward  = new Vector3(-0.1f, 0.0f, 0.0f);
            player.transform.position -= new Vector3(0.1f, 0.0f, 0.0f);
        }
        if (Input.GetKey(KeyCode.A))
        {
            player.transform.forward = new Vector3(0.0f, 0.0f, -0.1f);
            player.transform.position -= new Vector3(0.0f, 0.0f, 0.1f);
        }
        if (Input.GetKey(KeyCode.S))
        {
            player.transform.forward = new Vector3(0.1f, 0.0f, 0.0f);
            player.transform.position += new Vector3(0.1f, 0.0f, 0.0f);
        }

        if (Input.GetKey(KeyCode.D))
        {
            player.transform.forward = new Vector3(0.0f, 0.0f, 0.1f);
            player.transform.position += new Vector3(0.0f, 0.0f, 0.1f);
        }

        //player 1 rush
        if (Input.GetKeyDown(KeyCode.Q))
        {
            rig1.AddForce(player.transform.forward * 15f, ForceMode.Impulse);
        }


        //Player2
        player2.transform.Rotate(0f, 2.0f, 0f, Space.Self);
        if (Input.GetKey(KeyCode.UpArrow))
        {
            player2.transform.forward = new Vector3(-0.1f, 0.0f, 0.0f);
            player2.transform.position -= new Vector3(0.1f, 0.0f, 0.0f);
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            player2.transform.forward = new Vector3(0.0f, 0.0f, -0.1f);
            player2.transform.position -= new Vector3(0.0f, 0.0f, 0.1f);
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            player2.transform.forward = new Vector3(0.1f, 0.0f, 0.0f);
            player2.transform.position += new Vector3(0.1f, 0.0f, 0.0f);
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            player2.transform.forward = new Vector3(0.0f, 0.0f, 0.1f);
            player2.transform.position += new Vector3(0.0f, 0.0f, 0.1f);
        }


        //player 2 rush
        if (Input.GetKeyDown(KeyCode.P))
        {
            rig2.AddForce(player2.transform.forward * 15f, ForceMode.Impulse);
        }
    }
}
