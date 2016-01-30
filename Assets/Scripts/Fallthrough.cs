using UnityEngine;
using System.Collections;

public class Fallthrough : MonoBehaviour {
    public Collider player; // assign in inspector?
    public Collider tCollider; // assign in inspector?

    void OnTriggerEnter(Collider c)
    {
        if (c.tag == "Player")
        {
            Physics.IgnoreCollision(player, tCollider, true);
        }
    }

    void OnTriggerExit(Collider c)
    {
        if (c.tag == "Player")
        {
            Physics.IgnoreCollision(player, tCollider, false);
        }
    }
}
