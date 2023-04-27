using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Enemy : PoolAbleObject
{
    [SerializeField]
    private EnemyDataSO _dataSO = null;

    public static Enemy Create(Vector3 position)
    {
        Transform pfEnemy = GameAssets.Instance.pfEnemy;
        Transform enemyTransform = Instantiate(pfEnemy, position, Quaternion.identity);
        Enemy enemy = enemyTransform.GetComponent<Enemy>();
        return enemy;
    }

    private Transform targetTransform;
    private Rigidbody2D enemyRigidbody2D;

    private float lookForTargetTimer;
    private float lookForTargetTimerMax = .2f;

    private HealthSystem healthSystem;

    private SpriteRenderer _spriteRenderer = null;
    private TrailRenderer _trailRenderer = null;

    private void Awake()
    {
        enemyRigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void HealthSystem_OnDamaged(object sender, System.EventArgs e)
    {
        SoundManager.Instance.PlaySound(SoundManager.Sound.EnemyHit);
        CinemachineShake.Instance.ShakeCamera(5f, .1f);
        ChromaticAberrationEffect.Instance.SetWeight(.5f);
    }

    private void HealthSystem_OnDied(object sender, System.EventArgs e)
    {
        SoundManager.Instance.PlaySound(SoundManager.Sound.EnemyDie);
        CinemachineShake.Instance.ShakeCamera(7f, .15f);
        ChromaticAberrationEffect.Instance.SetWeight(.5f);
        Instantiate(GameAssets.Instance.pfEnemyDieParticles, transform.position, Quaternion.identity);
        ResourceManager.Instance.Cost += _dataSO.dieCost;
        //PopupPoolObject popup = PopupManager.Instance.Popup(null, $"+{_dataSO.dieCost}", transform.position);
        //popup.ColorSet(new Color(0, 0, 0.25f));
        PoolManager.Push(poolType, gameObject);
    }

    private void JustDie()
    {
        SoundManager.Instance.PlaySound(SoundManager.Sound.EnemyDie);
        CinemachineShake.Instance.ShakeCamera(7f, .15f);
        ChromaticAberrationEffect.Instance.SetWeight(.5f);
        Instantiate(GameAssets.Instance.pfEnemyDieParticles, transform.position, Quaternion.identity);
        PoolManager.Push(poolType, gameObject);
    }

    private void Update()
    {
        HandleMovement();
        HandleTargetting();
    }

    private void HandleMovement()
    {
        if (targetTransform != null)
        {
            Vector3 moveDir = (targetTransform.position - transform.position).normalized;

            enemyRigidbody2D.velocity = moveDir * _dataSO.speed;
        }
        else
        {
            enemyRigidbody2D.velocity = Vector2.zero;
        }
    }

    private void HandleTargetting()
    {
        lookForTargetTimer -= Time.deltaTime;
        if (lookForTargetTimer < 0)
        {
            lookForTargetTimer += lookForTargetTimerMax;
            LookForTargets();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Building building = collision.gameObject.GetComponent<Building>();
        if (building != null)
        {
            HealthSystem healthSystem = building.GetComponent<HealthSystem>();
            healthSystem.Damage(_dataSO.damage);
            JustDie();
        }
    }

    private void LookForTargets()
    {
        float targetMaxRadius = 20f;
        Collider2D[] collider2DArray = Physics2D.OverlapCircleAll(transform.position, targetMaxRadius);

        foreach (Collider2D collider2D in collider2DArray)
        {
            Building building = collider2D.GetComponent<Building>();
            if (building != null)
            {
                if (targetTransform == null)
                {
                    targetTransform = building.transform;
                }
                else
                {
                    if (Vector3.Distance(transform.position, building.transform.position) <
                        Vector3.Distance(transform.position, targetTransform.position))
                    {
                        targetTransform = building.transform;
                    }
                }
            }
        }

        if (targetTransform == null)
        {
            if(BuildingManager.Instance != null)
            {
                Building b = BuildingManager.Instance.GetHqBuilding();
                if(b == null)
                {
                    Debug.Log("鸥百 给茫酒");
                    healthSystem.Damage(999999999);
                    return;
                }
                targetTransform = b.transform;
            }
            else
            {
                Debug.Log("鸥百 给茫酒");
                healthSystem.Damage(999999999);
            }
        }
    }

    public void DataSet(EnemyDataSO data, Vector3 position)
    {
        _dataSO = data;
        if (BuildingManager.Instance.GetHqBuilding() != null)
        {
            targetTransform = BuildingManager.Instance.GetHqBuilding().transform;
        }
        if(healthSystem == null)
        {
            _spriteRenderer = transform.Find("pfEnemyVisual").GetComponent<SpriteRenderer>();
            _trailRenderer = transform.Find("trail").GetComponent<TrailRenderer>();
            healthSystem = GetComponent<HealthSystem>();
            healthSystem.OnDied += HealthSystem_OnDied;
            healthSystem.OnDamaged += HealthSystem_OnDamaged;
            lookForTargetTimer = Random.Range(0f, lookForTargetTimerMax);
        }
        healthSystem.SetHealthAmountMax(_dataSO.maxHp, true);
        healthSystem.HealFull();
        _spriteRenderer.sprite = _dataSO.sprite;
        transform.localScale = Vector3.one * _dataSO.size;

        _trailRenderer.enabled = false;
        transform.position = position;
        _trailRenderer.enabled = true;
    }

    public override void Init_Pop()
    {
    }

    public override void Init_Push()
    {
        targetTransform = null;
        transform.localScale = Vector3.one;
        _trailRenderer.enabled = false;
    }
}
