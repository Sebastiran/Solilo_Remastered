using UnityEngine;
using System.Collections;

public class AnimationScript_scene2 : MonoBehaviour
{
	GameObject girl;
    GameObject h;

    public string level;
    public KeyCode skipKey;

	// Use this for initialization
	void Start () 
	{
        GetComponent<Animation>().Play("scene2_boy_ani");

        girl = GameObject.Find("Girl");
        girl.GetComponent<Animation>().Play("scene2_girl_ani");
	}

    void Update()
    {
        if (Input.GetKey(skipKey))
            Application.LoadLevel(level);
    }

    public void 
        Heart()
    {
        h = GameObject.Find("heart");
        h.GetComponent<Animation>().Play("scene2_heart_ani");
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
