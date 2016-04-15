using UnityEngine;
using System.Collections;

public class AnimationScript : MonoBehaviour
{
	GameObject girl;
	GameObject bg;
	GameObject w;
	GameObject t;
	GameObject h;
	GameObject h2;
	GameObject zin1;
	GameObject zin2;
	GameObject zin3;
	GameObject zin4;
	GameObject zin5;
	GameObject zin6;
	GameObject zin7;
	GameObject zin8;
	GameObject zin9;
	GameObject zin10;
	GameObject zin11;
	GameObject zin12;
	GameObject zin13;
	GameObject zin14;
	GameObject zin15;
	GameObject zin16;

	public string level;
    public KeyCode skipKey;

	// Use this for initialization
	void Start () 
	{
		GetComponent<Animation>().Play("Boy");

		zin1 = GameObject.Find("1");
		zin1.GetComponent<Animation>().Play("animation_1");

		girl = GameObject.Find("Girl");
		girl.GetComponent<Animation>().Play("Girl");

	}

	public void Zin2()
	{
		zin2 = GameObject.Find("2");
		zin2.GetComponent<Animation>().Play("animation_2");
	}

	public void Zin3()
	{
		zin3 = GameObject.Find("3");
		zin3.GetComponent<Animation>().Play("animation_3");

		zin4 = GameObject.Find("4");
		zin4.GetComponent<Animation>().Play("animation_4");
	}
	public void Zin4()
	{
		zin5 = GameObject.Find("5");
		zin5.GetComponent<Animation>().Play("animation_5");
	}
	public void Zin5()
	{
		zin6 = GameObject.Find("6");
		zin6.GetComponent<Animation>().Play("animation_6");
	}
	public void Zin6()
	{
		zin7 = GameObject.Find("7");
		zin7.GetComponent<Animation>().Play("animation_7");
	}
	public void Zin7()
	{
		zin8 = GameObject.Find("8");
		zin8.GetComponent<Animation>().Play("animation_8");
	}
	public void Zin8()
	{
		zin9 = GameObject.Find("9");
		zin9.GetComponent<Animation>().Play("animation_9");
		zin10 = GameObject.Find("10");
		zin10.GetComponent<Animation>().Play("animation_10");
	}
	public void Zin9()
	{
		zin11 = GameObject.Find("11");
		zin11.GetComponent<Animation>().Play("animation_11");
	}
	public void Zin10()
	{
		zin12 = GameObject.Find("12");
		zin12.GetComponent<Animation>().Play("animation_12");
	}
	public void Zin11()
	{
		zin13 = GameObject.Find("13");
		zin13.GetComponent<Animation>().Play("animation_13");
	}
	public void Zin12()
	{
		zin14 = GameObject.Find("14");
		zin14.GetComponent<Animation>().Play("animation_14");
	}
	public void Zin13()
	{
		zin15 = GameObject.Find("15");
		zin15.GetComponent<Animation>().Play("animation_15");
	}
	public void Zin14()
	{
		zin16 = GameObject.Find("16");
		zin16.GetComponent<Animation>().Play("animation_16");
	}
	public void Background()
	{
		bg = GameObject.Find("Background");
		bg.GetComponent<Animation>().Play("backG_move");
	}
	public void Titel()
	{
		t = GameObject.Find("titel");
		t.GetComponent<Animation>().Play("titel");
	}
	public void Heart()
	{
		h = GameObject.Find("heart");
		h.GetComponent<Animation>().Play("Heart");
	}

	public void Heart2()
	{
		h2 = GameObject.Find("heart");
		h2.GetComponent<Animation>().Play("Heart2");
	}

	public void Why()
	{
		w = GameObject.Find("why");
		w.GetComponent<Animation>().Play("Why");
	}

	public void Audio(AudioClip aud)
	{
		GetComponent<AudioSource>().clip = aud;
		GetComponent<AudioSource>().Play();
	}

    void Update()
    {
        if (Input.GetKey(skipKey))
            Application.LoadLevel(level);
    }

	public void Finish()
	{
		Application.LoadLevel(level);
	}
}
