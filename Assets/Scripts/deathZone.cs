using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class deathZone : MonoBehaviour
{

    [SerializeField]
    GameObject systemManagerOBJ;

    SystemManager _systemManager;
    // Start is called before the first frame update
    void Start()
    {
        _systemManager = systemManagerOBJ.GetComponent<SystemManager>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnCollisionEnter(Collision other)
    {
        //Destroy(other.gameObject);
        other.gameObject.SetActive(false);
        _systemManager.playerDie();
    }
}
