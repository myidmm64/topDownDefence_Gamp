using System.Collections;
using System.Collections.Generic;
//using UnityEditor.ShaderKeywordFilter;
using UnityEngine;

public class Hero : MonoBehaviour
{

    [SerializeField]
    private HeroSO _heroSO = null;
    public HeroSO heroSO => _heroSO;


    private HeroData _curData = null;
    public HeroData CurData => _curData;
    private Transform _idleTextingTrm = null;
    private Vector3 projectileSpawnPosition;
    private Enemy targetEnemy = null;

    [SerializeField]
    private float _cantSpawnRadius = 5f;
    public float CantSpawnRadius => _cantSpawnRadius;
    [SerializeField]
    private float _targetSearchRadius = 20f;
    private int _evolutionCount = 0;
    private int _upgradeCost = 0;
    private float _shootingDelay = 0f;
    private int _damage = 0;
    private int _attackCount = 0;

    private int _evolutionIndex = 0;

    private SpriteRenderer _spriteRenderer = null;
    private Animator _animator = null;
    private Coroutine _shootingCoroutine = null;

    #region 피버
    [SerializeField]
    private Transform _feverBarTrm = null;
    #endregion
    private PlayerBuff _playerBuff = null;

    private void Start()
    {
        _playerBuff = GetComponent<PlayerBuff>();
        _spriteRenderer = transform.Find("renderer").GetComponent<SpriteRenderer>();
        _animator = _spriteRenderer.GetComponent<Animator>();
        projectileSpawnPosition = transform.Find("projectileSpawnPosition").position;
        _idleTextingTrm = transform.Find("idleTextingTrm");
        DataSet(_evolutionIndex);
        StartCoroutine(ShootingCoroutine());
    }

    private void OnMouseDown()
    {
        UIGetter.Instance.GetHeroInfoUI(transform.position, this, _upgradeCost, _curData.newEvolutionCount - _evolutionCount, _shootingDelay, _damage, _curData.bulletSpeed);
    }

    private void IdleTexting()
    {
        PopupPoolObject pool = PopupManager.Instance.Popup(_curData.idleTextData, _curData.idleTexts[Random.Range(0, _curData.idleTexts.Count)], _idleTextingTrm.position);
        pool.ColorSet(_curData.uiTextColor);
    }

    private IEnumerator ShootingCoroutine()
    {
        while (true)
        {
            yield return new WaitUntil(() => targetEnemy != null);
            if (targetEnemy == null)
                continue;
            else
            {
                if (targetEnemy.gameObject.activeSelf == false)
                {
                    targetEnemy = null;
                    continue;
                }
            }
            if ((_attackCount % _curData.specialAttackCount) == 0) // 스페셜
            {
                ArrowProjectile arrow = PoolManager.Pop(_curData.specialArrow).GetComponent<ArrowProjectile>();
                arrow.Init(projectileSpawnPosition, _curData.bulletSpeed, Mathf.RoundToInt(_curData.damage * 1.3f), _curData.criticalPercent, targetEnemy.transform);
            }
            else
            {
                ArrowProjectile arrow = PoolManager.Pop(_curData.normalArrow).GetComponent<ArrowProjectile>();
                arrow.Init(projectileSpawnPosition, _curData.bulletSpeed, _curData.damage, _curData.criticalPercent, targetEnemy.transform);
            }
            _attackCount++;
            FeverUISet();
            _animator.SetTrigger("Shoot");
            if (_playerBuff.BuffCheck(PlayerBuffType.Fever))
                yield return new WaitForSeconds(_shootingDelay * 0.5f);
            else
                yield return new WaitForSeconds(_shootingDelay);
        }
    }

    private void FeverUISet()
    {
        if (_playerBuff.BuffCheck(PlayerBuffType.Fever))
            return;

        _feverBarTrm.localScale = new Vector3((float)_attackCount / _curData.feverAttackCount, 1f, 1f);
        if (_attackCount == _curData.feverAttackCount)
        {
            Fever();
        }
    }

    private void Fever()
    {
        PopupPoolObject popup = PopupManager.Instance.Popup(_curData.idleTextData, _curData.feverDialog, _idleTextingTrm.position);
        popup.ColorSet(Color.red);
        FeverFire fire = PoolManager.Pop(PoolType.FeverFire).GetComponent<FeverFire>();
        fire.Init(transform.position + Vector3.up * 1.6f, _curData.feverColor, 5f);
        fire.transform.SetParent(transform);
        _feverBarTrm.localScale = Vector3.one;
        _playerBuff.AddBuff(PlayerBuffType.Fever);
        StartCoroutine(FeverCoroutine());
    }

    private IEnumerator FeverCoroutine()
    {
        float time = 1f;
        while (time >= 0f)
        {
            _feverBarTrm.localScale = new Vector3(time, 1f, 1f);
            time -= Time.deltaTime * (1f / 5f);
            yield return null;
        }
        _feverBarTrm.localScale = new Vector3(0f, 1f, 1f);
        _playerBuff.DeleteBuff(PlayerBuffType.Fever);
        _attackCount = 0;
    }

    private void DataSet(int index)
    {
        DataSet(_heroSO.heroDatas[index]);
    }

    private void DataSet(HeroData newData)
    {
        StopAllCoroutines();
        _feverBarTrm.localScale = new Vector3(0f, 1f, 1f);
        _playerBuff.DeleteBuff(PlayerBuffType.Fever);
        _attackCount = 0;

        _curData = newData;
        _spriteRenderer.sprite = _curData.sprite;
        _evolutionCount = 0;
        _upgradeCost = _curData.originUpgradeCost;
        _shootingDelay = _curData.shootingDelay;
        _damage = _curData.damage;
        CancelInvoke();
        InvokeRepeating("IdleTexting", _curData.idleTextRepeatTime, _curData.idleTextRepeatTime);
        StartCoroutine(ShootingCoroutine());
    }

    /// <summary>
    /// 히어로 강화
    /// </summary>
    public void UpgradeHero()
    {
        if (ResourceManager.Instance.Cost < _upgradeCost)
        {
            PopupPoolObject popup = PopupManager.Instance.Popup(null, $"돈 부족, 필요한 돈 {_upgradeCost}", transform.position, null);
            popup.ColorSet(Color.red);
            UIGetter.Instance.PushUI(UIType.HeroInfoUI);
            return;
        }
        else if (_curData.newEvolutionCount == 0)
        {
            PopupPoolObject popup = PopupManager.Instance.Popup(null, $"최고 단계 강화", transform.position, null);
            popup.ColorSet(Color.red);
            UIGetter.Instance.PushUI(UIType.HeroInfoUI);
            return;
        }
        Instantiate(GameAssets.Instance.pfEnemyDieParticles, transform.position, Quaternion.identity);
        ResourceManager.Instance.Cost -= _curData.originUpgradeCost;
        _shootingDelay *= 0.9f;
        _damage = Mathf.RoundToInt(_damage * 1.2f);
        _upgradeCost = Mathf.RoundToInt(_upgradeCost * 1.2f);
        _evolutionCount++;
        if (_evolutionCount >= _curData.newEvolutionCount)
        {
            _evolutionCount = 0;
            _evolutionIndex++;
            DataSet(_evolutionIndex);
        }
        UIGetter.Instance.GetHeroInfoUI(transform.position, this, _upgradeCost, _curData.newEvolutionCount - _evolutionCount, _shootingDelay, _damage, _curData.bulletSpeed);
    }

    private void FixedUpdate()
    {
        LookForTargets();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _targetSearchRadius);
    }

    private void LookForTargets()
    {
        Collider2D[] collider2DArray = Physics2D.OverlapCircleAll(transform.position, _targetSearchRadius);
        int enemyCount = 0;

        foreach (Collider2D collider2D in collider2DArray)
        {
            Enemy enemy = collider2D.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemyCount++;
                if (targetEnemy == null)
                {
                    targetEnemy = enemy;
                }
                else
                {
                    if (Vector3.Distance(transform.position, enemy.transform.position) <
                        Vector3.Distance(transform.position, targetEnemy.transform.position))
                    {
                        targetEnemy = enemy;
                    }
                }
            }
        }
        if (enemyCount == 0)
            targetEnemy = null;
    }
}
