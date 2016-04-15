using UnityEngine;
using System.Collections;

public class CourageMove : MonoBehaviour {
    int counter = 0;
    public bool b = true;
    public GameObject lerpPoint;

    void Awake()
    {

    }

	
    void FixedUpdate()
    {
        if (b)
        {

        }

        if (!b)
        {
                transform.position = new Vector3(Mathf.Lerp(transform.position.x, lerpPoint.transform.position.x, 0.25f), Mathf.Lerp(transform.position.y, lerpPoint.transform.position.y, 0.25f), 0);
            counter++;
        }

        if (counter >= 20)
            Destroy(gameObject);
    }
}
