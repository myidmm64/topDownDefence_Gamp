using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class HeroInfoUI : MonoBehaviour
{
    [SerializeField]
    private Image _background = null;

    [SerializeField]
    private TextMeshProUGUI _nameText = null;
    [SerializeField]
    private TextMeshProUGUI _subNameText = null;
    [SerializeField]
    private TextMeshProUGUI _upgradeCostText = null;
    [SerializeField]
    private TextMeshProUGUI _needUpgradeText = null;
    [SerializeField]
    private TextMeshProUGUI _shootDelayText = null;
    [SerializeField]
    private TextMeshProUGUI _damageText = null;
    [SerializeField]
    private TextMeshProUGUI _shootSpeedText = null;
    [SerializeField]
    private Image _heroFaceImage = null;

    private Hero _myHero = null;

    public void SetUpHeroInfomation(Hero hero, int upgradeCost, int remainingEvolutionCount, float shootDelay, int damage, float bulletSpeed)
    {
        _myHero = hero;
        _background.color = hero.CurData.uiBackgroundColor;
        _nameText.SetText(hero.CurData.heroName);
        _subNameText.SetText(hero.CurData.heroSubName);
        _subNameText.color = hero.CurData.uiTextColor;
        _nameText.color = hero.CurData.uiTextColor;
        _upgradeCostText.SetText($"<color=#{hero.CurData.uiTextColor.ToHexString()}>강화 코스트 : {upgradeCost}</color>");
        _needUpgradeText.SetText($"<color=#{hero.CurData.uiTextColor.ToHexString()}>진화 필요 강화 : {remainingEvolutionCount}</color>");
        _shootDelayText.SetText($"<color=#{hero.CurData.uiTextColor.ToHexString()}>공격 속도 : {(1f / shootDelay).ToString("N1")}/s</color>");
        _damageText.SetText($"<color=#{hero.CurData.uiTextColor.ToHexString()}>데미지 : {damage} ~ {Mathf.RoundToInt(damage * 1.3f)}</color>");
        _shootSpeedText.SetText($"<color=#{hero.CurData.uiTextColor.ToHexString()}>투사체 속도 : {bulletSpeed.ToString("N1")}</color>");
        _heroFaceImage.sprite = hero.CurData.uiSprite;
    }

    public void HeroDestroy()
    {
        if (_myHero == null)
            return;
        Destroy(_myHero.gameObject);
        UIGetter.Instance.PushUI(UIType.HeroInfoUI);
    }

    public void HeroUpgrade()
    {
        if (_myHero == null)
            return;
        _myHero.UpgradeHero();
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(1))
        {
            UIGetter.Instance.PushUI(UIType.HeroInfoUI);
        }
    }
}
