using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIGetter : MonoSingleTon<UIGetter>
{
    [SerializeField]
    private HeroInfoUI _heroInfoUI = null;

    public void GetHeroInfoUI(Vector3 position, Hero hero, int upgradeCost, int remainingEvolutionCount, float shootDelay, int damage, float bulletSpeed)
    {
        _heroInfoUI.gameObject.SetActive(true);
        _heroInfoUI.GetComponent<RectTransform>().position
            = UtilClass.mainCamera.WorldToScreenPoint(position);
        _heroInfoUI.SetUpHeroInfomation(hero, upgradeCost, remainingEvolutionCount, shootDelay, damage, bulletSpeed);
    }

    public GameObject GetUI(UIType type)
    {
        switch (type)
        {
            case UIType.None:
                break;
            case UIType.HeroInfoUI:
                return _heroInfoUI.gameObject;
            default:
                break;
        }
        return null;
    }

    public void PushUI(UIType type)
    {
        GetUI(type).SetActive(false);
    }
}

public enum UIType
{
    None,
    HeroInfoUI
}