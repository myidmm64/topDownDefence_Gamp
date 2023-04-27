using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeverFire : PoolAbleObject
{
    private SpriteRenderer _spriteRenderer = null;

    public void Init(Vector3 position, Color color, float time)
    {
        transform.position = position;
        _spriteRenderer.color = color;
        Invoke("Die", time);
    }

    private void Die()
    {
        PoolManager.Push(poolType, gameObject);
    }

    public override void Init_Pop()
    {
        if(_spriteRenderer == null)
            _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public override void Init_Push()
    {
    }
}
