using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroGhost : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer = null;

    private void Start()
    {
        _spriteRenderer = transform.Find("sprite").GetComponent<SpriteRenderer>();
    }

    public void SetUpGhost(Sprite sprite)
    {
        _spriteRenderer.sprite = sprite;
    }
}
