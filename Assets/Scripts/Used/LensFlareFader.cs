using UnityEngine;
using System.Collections;

[RequireComponent(typeof(LensFlare))]
public class LensFlareFader : MonoBehaviour
{
	public float startValue = 1f;
	public float endValue = 0f;
	public float fadeTime = 1f;

	public void PerformFadeAction()
	{
		StartCoroutine(Fade());
	}

	IEnumerator Fade()
	{
		int nSteps;
		float deltaValue;
		LensFlare lensFlare;

		nSteps = (int)(fadeTime / Time.fixedDeltaTime);
		deltaValue = (endValue - startValue) / nSteps;
		lensFlare = GetComponent<LensFlare>();
		lensFlare.brightness = startValue;

		for (int i = 0; i < nSteps; i++)
		{
			lensFlare.brightness += deltaValue;
			yield return new WaitForFixedUpdate();
		}

		lensFlare.brightness = endValue;
	}
}
