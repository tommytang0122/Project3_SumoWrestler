using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField]
    GameObject player;
    [SerializeField]
    GameObject player2;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {



        //Player1
        player.transform.Rotate(0f, 2.0f, 0f, Space.Self);
        if (Input.GetKey(KeyCode.W))
        {
            player.transform.position -= new Vector3(0.1f, 0.0f, 0.0f);
        }
        if (Input.GetKey(KeyCode.A))
        {
            player.transform.position -= new Vector3(0.0f, 0.0f, 0.1f);
        }
        if (Input.GetKey(KeyCode.S))
        {
            player.transform.position += new Vector3(0.1f, 0.0f, 0.0f);
        }

        if (Input.GetKey(KeyCode.D))
        {
            player.transform.position += new Vector3(0.0f, 0.0f, 0.1f);
        }


        //Player2
        player2.transform.Rotate(0f, 2.0f, 0f, Space.Self);
        if (Input.GetKey(KeyCode.UpArrow))
        {
            player2.transform.position -= new Vector3(0.1f, 0.0f, 0.0f);
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            player2.transform.position -= new Vector3(0.0f, 0.0f, 0.1f);
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            player2.transform.position += new Vector3(0.1f, 0.0f, 0.0f);
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            player2.transform.position += new Vector3(0.0f, 0.0f, 0.1f);
        }
    }
}
