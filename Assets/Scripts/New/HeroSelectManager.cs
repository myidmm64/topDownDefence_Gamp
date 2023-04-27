using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;

public class HeroSelectManager : MonoSingleTon<HeroSelectManager>
{
    private Dictionary<HeroQualityType, List<Hero>> _herosDic = new Dictionary<HeroQualityType, List<Hero>>();

    [SerializeField]
    private float _canSpawnRadius = 10f;
    [SerializeField]
    private List<Hero> _heros = new List<Hero>();
    [SerializeField]
    private List<int> _costs = new List<int>();

    public List<Hero> Heros => _heros;

    private HeroGhost _heroGhost = null;

    private HeroQualityType _curType = HeroQualityType.None;

    private void Start()
    {
        _heroGhost = transform.Find("HeroGhost").GetComponent<HeroGhost>();
        foreach(var hero in _heros)
        {
            HeroQualityType type = hero.heroSO.heroQuality;
            if (_herosDic.ContainsKey(type))
            {
                _herosDic[type].Add(hero);
            }
            else
            {
                _herosDic.Add(type, new List<Hero>());
                _herosDic[type].Add(hero);
            }
        }
    }

    public bool Costable()
    {
        return ResourceManager.Instance.Cost >= _costs[(int)_curType - 1];
    }

    public bool OtherHeroCheck()
    {
        Collider2D[] collider2DArray = Physics2D.OverlapCircleAll(UtilClass.GetMouseWorldPosition(), _canSpawnRadius);
        int count = 0;
        foreach (var c in collider2DArray)
        {
            if (c.GetComponentInParent<Hero>() != null)
            {
                count++;
                return false;
            }
        }
        return true;
    }

    private void Update()
    {
        if (_curType != HeroQualityType.None)
        {
            _heroGhost.gameObject.transform.position = UtilClass.GetMouseWorldPosition();
            if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
            {
                if (Costable() == false)
                {
                    PopupPoolObject popup = PopupManager.Instance.Popup(null, $"돈 부족, 필요한 돈 {_costs[(int)_curType - 1]}", UtilClass.GetMouseWorldPosition(), null);
                    popup.ColorSet(Color.red);
                    return;
                }
                else if (OtherHeroCheck() == false)
                {
                    PopupPoolObject popup = PopupManager.Instance.Popup(null, $"근처에 다른 hero 존재", UtilClass.GetMouseWorldPosition(), null);
                    popup.ColorSet(Color.red);
                    return;
                }
                Instantiate(GameAssets.Instance.pfBuildingPlacedParticles, UtilClass.GetMouseWorldPosition(), Quaternion.identity);
                List<Hero> heroList = _herosDic[_curType];
                GameObject hero = Instantiate(heroList[Random.Range(0, heroList.Count)], Vector2.zero, Quaternion.identity).gameObject;
                hero.transform.position = UtilClass.GetMouseWorldPosition();
                ResourceManager.Instance.Cost -= _costs[(int)_curType - 1];
            }

            if (Input.GetMouseButtonDown(1) && !EventSystem.current.IsPointerOverGameObject())
            {
                _curType = HeroQualityType.None;
                _heroGhost.SetUpGhost(null);
            }
        }
        else
        {
        }
    }

    public void QualityTypeSet(HeroQualityType type)
    {
        _curType = type;
        _heroGhost.SetUpGhost(_herosDic[_curType][0].heroSO.heroDatas[0].sprite);
    }
}
