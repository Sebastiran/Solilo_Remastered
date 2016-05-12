using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class PlatformBehaviour : MonoBehaviour {
	public bool boy;
	public bool girl;
	
	public GameObject despawnPrefab;
	public GameObject colorParticles;
	public UnityEvent afterDespawn;

	// Update is called once per frame
	void Update () {
		
		if (boy == true && girl == true) 
		{
            Instantiate(despawnPrefab, transform.position, Quaternion.identity);
            gameObject.SetActive(false);
            afterDespawn.Invoke();
		}
	}

	public void boyTrue()
	{
		boy = true;
		GameObject a = Instantiate(colorParticles, transform.position, Quaternion.identity) as GameObject;
		ParticleSystem b = a.GetComponent<ParticleSystem> ();
		b.startColor = new Color (0.5f, 0.5f, 1, 1f);
	}

	public void girlTrue()
	{
		girl = true;
		GameObject a = Instantiate(colorParticles, transform.position, Quaternion.identity) as GameObject;
		ParticleSystem b = a.GetComponent<ParticleSystem> ();
		b.startColor = new Color (1, 0.5f, 1, 1f);
	}
}
