using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EvaluateFuntions : MonoBehaviour{

    List<Transform> platforms;
    public float minPosY, maxPosY;
    public float Density;
    public float Leniency;

    public void RunEvaluation()
    { 
        var children = GetComponentsInChildren<Transform>();
        platforms = new List<Transform>(children.Length);
        minPosY = children[0].position.y;
        maxPosY = children[0].position.y;
        float totalDistance = 0;
        for (int i = 0; i < children.Length; i++)
        {
            CheckLiniearity(children[i].transform.position);
            for (int j = 0; j < children.Length; j++)
            {
                if(i != j)
                {
                    totalDistance += Vector3.Distance(children[i].transform.position, children[j].transform.position);
                }
            }
            platforms.Add(children[i]);
        }

        Density = GetDensity(children.Length - 1);
        Leniency = GetLeniency(totalDistance, children.Length - 1);
    }

    void CheckLiniearity(Vector3 position)
    {
        if (position.y < minPosY)
        {
            minPosY = position.y;
        }
        if (position.y > maxPosY)
        {
            maxPosY = position.y;
        }
    }

    float GetDensity(int ChildrenAmount)
    {
        return 1f - (1f / ChildrenAmount);
    }

    float GetLeniency(float totalDistance, int childrenAmount)
    {
        return totalDistance/childrenAmount;
    }
}
