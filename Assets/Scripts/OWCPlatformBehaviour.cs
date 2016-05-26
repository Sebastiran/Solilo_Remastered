using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class OWCPlatformBehaviour : PlatformBehaviour
{

    public override void OnTriggerEnter2D(Collider2D other)
    {
        Physics2D.IgnoreCollision(this.GetComponent<Collider2D>(), other, true);
        base.OnTriggerEnter2D(other);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        Physics2D.IgnoreCollision(this.GetComponent<Collider2D>(), other, false);
    }
}
