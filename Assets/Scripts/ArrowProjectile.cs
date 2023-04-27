using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowProjectile : PoolAbleObject
{
    private Enemy targetEnemy;
    private Vector3 lastMoveDir;
    private float timeToDie = 2f;

    private int _damage = 0;
    private float _criticalPercent = 0f;
    private Rigidbody2D _rigid = null;

    public void Init(Vector3 position, float speed, int damage, float criticalPercent, Transform target)
    {
        transform.position = position;
        transform.eulerAngles = new Vector3(0f, 0f, UtilClass.GetAngleFromVector(target.position - transform.position));
        _damage = damage;
        _criticalPercent = criticalPercent;
        _rigid.velocity = transform.right * speed;
        StartCoroutine(DieCoroutine());
    }

    private IEnumerator DieCoroutine()
    {
        yield return new WaitForSeconds(timeToDie);
        PoolManager.Push(poolType, gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy enemy = collision.GetComponent<Enemy>();
        if(enemy != null)
        {
            enemy.GetComponent<HealthSystem>().Damage(_damage);
            Destroy(gameObject);
        }
    }

    public override void Init_Pop()
    {
        if (_rigid == null)
            _rigid = GetComponent<Rigidbody2D>();
    }

    public override void Init_Push()
    {
        StopAllCoroutines();
        _rigid.velocity = Vector2.zero;
    }
}
