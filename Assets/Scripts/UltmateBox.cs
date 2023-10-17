using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using nn.hid;

public class UltmateBox : MonoBehaviour
{

    public GameObject arena;
    ControlArena cA;

    void Start()
    {
        cA = arena.GetComponent<ControlArena>();
    }

    void OnCollisionEnter(Collision other)
    {
        //Destroy(other.gameObject);
        if(other.transform.tag == "Player")
        {
            if(other.gameObject.name == "spinningtop1")
            {
                cA.Nid = NpadId.No1;
            }
            else if (other.gameObject.name == "spinningtop2")
            {
                cA.Nid = NpadId.No2;
            }
            else if (other.gameObject.name == "spinningtop3")
            {
                cA.Nid = NpadId.No3;
            }
            else if (other.gameObject.name == "spinningtop4")
            {
                cA.Nid = NpadId.No4;
            }
        }
    }
}
