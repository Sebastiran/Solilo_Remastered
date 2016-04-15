using UnityEngine;
using System.Collections;

public class AnimationScript_scene1 : MonoBehaviour
{
	GameObject girl;
    GameObject w;

    public string level;
    public KeyCode skipKey;

	// Use this for initialization
	void Start () 
	{
        GetComponent<Animation>().Play("scene1_boy_ani");

        girl = GameObject.Find("Girl");
        girl.GetComponent<Animation>().Play("scene1_girl_ani");
	}

    void Update()
    {
        if (Input.GetKey(skipKey))
            Application.LoadLevel(level);
    }

    public void Why()
    {
        w = GameObject.Find("why");
        w.GetComponent<Animation>().Play("scene1_why_ani");
    }

    public void Audio(AudioClip aud)
    {
        GetComponent<AudioSource>().clip = aud;
        GetComponent<AudioSource>().Play();
    }

    public void Finish()
    {
        Application.LoadLevel(level);
    }
}
