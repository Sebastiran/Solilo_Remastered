using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Courage : MonoBehaviour {
	
	//public int courage;
	static int curCourage = 1;
    public Slider courageBar;
	static int startCourage;
	static int curLevel;
	static int maxValue = 10;

	public static int CurrentValue
	{
		get { return curCourage; }
		set
		{
			if (value < 0)
				curCourage = 0;
			else if (value > MaxValue)
				curCourage = MaxValue;
			else
				curCourage = value;
		}
	}

	public static int MaxValue
	{
		get { return maxValue; }
		set { maxValue = Mathf.Clamp(value, 0, int.MaxValue); }
	}
	
	// Use this for initialization
	void Start ()
	{
		curLevel = Application.loadedLevel;

		if (courageBar != null)
		{
			courageBar.maxValue = MaxValue;
			courageBar.minValue = 0;
			courageBar.value = CurrentValue;
			startCourage = CurrentValue;
		}
	}

	void OnLevelWasLoaded(int level)
	{
		Debug.Log (level);
		if (level == curLevel)
		{
			CurrentValue = startCourage;

			if (courageBar != null)
				courageBar.value = CurrentValue;
		}
		curLevel = level;
		startCourage = CurrentValue;
	}
	
	void OnTriggerEnter2D(Collider2D col){
		
		if (col.tag == "Courage") {
            CourageColl c = col.GetComponent<CourageColl>();
            if (!c.used)
            {
                AddCourage(1);
                c.used = true;
            }
		}
	}

    public int GetCourage()
    {
		return CurrentValue;
    }

    public void AddCourage(int c)
    {
		CurrentValue += c;

		if (courageBar != null)
			courageBar.value += (float)c;
    }

    public void SubtractCourage(int c)
    {
        curCourage -= c;

		if (courageBar != null)
			courageBar.value -= (float)c;
    }
}
