using UnityEngine;
using System.Collections;

public class OWCPlatformBehaviour : MonoBehaviour
{

    bool girl = false;
    bool boy = false;

    public Sprite blue, pink, neutral;

    int timer = 0;

    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Physics2D.IgnoreCollision(this.GetComponent<Collider2D>(), other, true);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        Physics2D.IgnoreCollision(this.GetComponent<Collider2D>(), other, false);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.transform.tag == "Boy")
        {
            spriteRenderer.sprite = blue;
            boy = true;
        }
        else if (other.transform.tag == "Girl")
        {
            spriteRenderer.sprite = pink;
            girl = true;
        }
    }

    void Update()
    {
        if (boy && girl)
        {
            if (GameObject.Find("Player Character (Boy)").GetComponent<PlatformerControls>().activateNeutral)
            {
                spriteRenderer.sprite = neutral;
                boy = false;
                girl = false;
            }
            else
            {
                timer++;
                if (timer > 10)
                {
                    gameObject.SetActive(false);
                }
            }
        }
    }
}
