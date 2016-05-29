using UnityEngine;
using System.Collections;

public class CPManager : MonoBehaviour {

    public GameObject[] ConstructivePrimitives;
    // Use this for initialization
    void Start () {
        foreach(GameObject obj in ConstructivePrimitives)
        {
            var script = obj.GetComponent<EvaluateFuntions>();
            if (script == null)
            {
                script = obj.AddComponent<EvaluateFuntions>();
            }
            script.RunEvaluation();
            var obj2 = (GameObject)Instantiate(obj, new Vector3(10000, 10000, 10000), Quaternion.Euler(Vector3.zero));
        }
	}
}
