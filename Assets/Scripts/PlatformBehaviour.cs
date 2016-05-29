using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class PlatformBehaviour : MonoBehaviour {
    public bool boyStart;
    public bool girlStart;

    protected bool boy;
	protected bool girl;

    public Sprite blue, pink, neutral;
    protected SpriteRenderer spriteRenderer;

    public GameObject despawnPrefab;
	public GameObject colorParticles;
	public UnityEvent afterDespawn;

    protected void Start ()
	{
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        if (boyStart)
            boyTrue();
        if (girlStart)
            girlTrue();
        CheckDespawn();
	}

    public virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Boy")
        {
            boyTrue();
            CheckDespawn();
        }
        else if (other.tag == "Girl")
        {
            girlTrue();
            CheckDespawn();
        }
    }

    protected void CheckDespawn()
    {
        if (boy == true && girl == true)
        {
            StartCoroutine(Despawn());
        }
    }

    protected IEnumerator Despawn()
    {
        yield return new WaitForSeconds(0.1f);
        Instantiate(despawnPrefab, transform.position, Quaternion.identity);
        gameObject.SetActive(false);
        afterDespawn.Invoke();
    }

    protected void ChangeSprite(Sprite sprite)
    {
        if (spriteRenderer.sprite != sprite)
        {
            spriteRenderer.sprite = sprite;
        }
    }

    protected void boyTrue()
	{
        if (!boy)
        {
            boy = true;
            GameObject a = Instantiate(colorParticles, transform.position, Quaternion.identity) as GameObject;
            ParticleSystem b = a.GetComponent<ParticleSystem>();
            b.startColor = new Color(0.5f, 0.5f, 1, 1f);
            ChangeSprite(blue);
        }
	}

    protected void girlTrue()
	{
        if (!girl)
        {
            girl = true;
		    GameObject a = Instantiate(colorParticles, transform.position, Quaternion.identity) as GameObject;
		    ParticleSystem b = a.GetComponent<ParticleSystem> ();
		    b.startColor = new Color (1, 0.5f, 1, 1f);
            ChangeSprite(pink);
        }
    }
}
