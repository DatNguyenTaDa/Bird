using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Defend : MonoBehaviour
{
    public float defend;
    public float hide_time;
    public GameObject effect;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.relativeVelocity.magnitude>defend)
        {
            Destroy(gameObject, hide_time);
            if(effect!=null)
            {
                Instantiate(effect, transform.position, Quaternion.identity);

            }
        }
        else
        {
            defend -= collision.relativeVelocity.magnitude;
        }
    }
}
