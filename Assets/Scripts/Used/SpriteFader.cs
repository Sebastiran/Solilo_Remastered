using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SpriteRenderer))]
public class SpriteFader : MonoBehaviour
{
	public float startValue = 1f;
	public float endValue = 0f;
	public float fadeTime = 1f;
	public AnimationCurve alphaCurve = AnimationCurve.EaseInOut(0f, 1f, 1f, 0f);

	bool isFading;

	public void PerformFadeAction()
	{
		if (!isFading)
			StartCoroutine(Fade());
	}

	IEnumerator Fade()
	{
		isFading = true;

		SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
		Keyframe lastKeyFrame = alphaCurve.keys[alphaCurve.length - 1];

		for (float t = 0; t < lastKeyFrame.time; t += Time.fixedDeltaTime)
		{
			Color color = spriteRenderer.color;
			color.a = alphaCurve.Evaluate(t);
			spriteRenderer.color = color;
			yield return new WaitForFixedUpdate();
		}

		isFading = false;
	}
}
