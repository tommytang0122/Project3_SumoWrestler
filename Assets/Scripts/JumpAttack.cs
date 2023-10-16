using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpAttack : MonoBehaviour
{
    [SerializeField]
    float forceMagnitude = 1000.0f;

    [SerializeField]
    float radius = 4.0f;

    [SerializeField]
    GameObject collisionVFX;
    [SerializeField]
    GameObject smashVFX;

    GameObject impact_obj;

    public bool isJumping = false;

    //[SerializeField]
    //BlastWave _blastWave;
    
    void Start()
    {
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Player")
        {
            Vector3 location = this.transform.position;
            Vector3 closestPoint = collision.collider.ClosestPoint(location);
            impact_obj = Instantiate(collisionVFX);
            impact_obj.transform.position = closestPoint;
            impact_obj.transform.localScale = Vector3.one;
            //Destroy(impact_obj);
            Invoke("DeleteVFX", 0.2f);
        }

        if (isJumping && collision.transform.tag == "arena")
        {
            Vector3 location = this.transform.position;
            Vector3 closestPoint = collision.collider.ClosestPoint(location);
            impact_obj = Instantiate(smashVFX);
            impact_obj.transform.position = closestPoint;
            impact_obj.transform.localScale = Vector3.one;
            //Destroy(impact_obj);
            Invoke("DeleteVFX", 0.2f);

            Collider[] colliders = Physics.OverlapSphere(transform.position, radius);
            foreach (Collider item in colliders)
            {
                if (item.gameObject == gameObject || item.transform.tag == "arena")
                    continue;

                float ForceN = forceMagnitude / (item.transform.position - transform.position).magnitude;
                item.gameObject.GetComponent<Rigidbody>().AddForce((item.transform.position - transform.position).normalized * ForceN);
            }
            Invoke("SetJumpfalse", 2.5f);
        }
    }
    void SetJumpfalse()
    {
        isJumping = false;
    }

    void DeleteVFX()
    {
        Destroy(impact_obj);
    }
}
