using UnityEngine;
using System.Collections;

public class StartScherm: MonoBehaviour
{
    public KeyCode startKey;
    public string level;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        StartGame();
    }

    void StartGame()
    {
        if (!Input.GetKeyDown(startKey))
        {
            return;
        }
        Application.LoadLevel(level);
    }
}
