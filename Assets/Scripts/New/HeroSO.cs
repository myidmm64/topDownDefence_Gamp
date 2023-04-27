
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Hero/Data")]
public class HeroSO : ScriptableObject
{
    public HeroQualityType heroQuality = HeroQualityType.None;
    public List<HeroData> heroDatas = new List<HeroData>();
}

[System.Serializable]
public class HeroData
{
    public string heroName;
    public string heroSubName;
    public int newEvolutionCount; // 다음 진화 필요 강화
    public int originUpgradeCost; // 강화 코스트
    public PopupDataSO idleTextData;
    public float idleTextRepeatTime = 2.5f;
    public List<string> idleTexts = new List<string>();
    public Sprite sprite; // 현재 업글의 스프라이트
    public Sprite uiSprite; // 얼굴 사진 !!
    public Color uiTextColor;
    public Color uiBackgroundColor;
    public float shootingDelay; // 현재 업글의 딜레이
    public int damage; // 현재 업글의 데미지
    public float bulletSpeed;

    public int specialAttackCount = 0; // 몇 번 치면 스페셜?
    public int feverAttackCount = 0;
    public Color feverColor = Color.white;
    public string feverDialog = "가만 두지 않을거야!!";
    [Range(0, 100f)]
    public float criticalPercent = 0f; // 크리티컬

    public PoolType normalArrow;
    public PoolType specialArrow;
}