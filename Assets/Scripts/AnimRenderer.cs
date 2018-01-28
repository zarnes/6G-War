using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimRenderer : MonoBehaviour
{
    public List<Sprite> steps;
    public float frameDuration = 100;
    public bool loop = false;

    private SpriteRenderer spriteRenderer;
    public bool playing;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        playing = true;
        StartCoroutine(Play(loop));
    }

    private void OnDisable()
    {
        playing = false;
        StopAllCoroutines();
    }

    private IEnumerator Play(bool loop)
    {
        do
        {
            foreach(Sprite step in steps)
            {
                if (!playing)
                {
                    loop = false;
                    break;
                }
                spriteRenderer.sprite = step;
                yield return new WaitForSeconds(frameDuration / 1000);
            }

            if (!loop)
            {
                spriteRenderer.enabled = false;
                this.enabled = false;
            }

        } while (loop);
    }
}
