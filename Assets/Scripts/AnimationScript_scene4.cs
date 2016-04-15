using UnityEngine;
using System.Collections;

public class AnimationScript_scene4 : MonoBehaviour
{
	GameObject girl;
    GameObject h;
    GameObject w;

    public string level;
    public KeyCode skipKey;

	// Use this for initialization
	void Start () 
	{
        GetComponent<Animation>().Play("scene4_boy_ani");

        girl = GameObject.Find("Girl");
        girl.GetComponent<Animation>().Play("scene4_girl_ani");
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
        h.GetComponent<Animation>().Play("scene4_heart_ani");
    }

    public void 
        Heart2()
    {
        h = GameObject.Find("heart");
        h.GetComponent<Animation>().Play("scene4_heart2_ani");
    }

    public void
        Heart3()
    {
        h = GameObject.Find("heart2");
        h.GetComponent<Animation>().Play("scene4_heart3_ani");
    }

    public void
        Heart4()
    {
        h = GameObject.Find("heart3");
        h.GetComponent<Animation>().Play("scene4_heart4_ani");
    }

    public void
        Heart5()
    {
        h = GameObject.Find("heart");
        h.GetComponent<Animation>().Play("scene4_heart5_ani");
    }

    public void
        Why()
    {
        w = GameObject.Find("why");
        w.GetComponent<Animation>().Play("scene4_why_ani");
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
