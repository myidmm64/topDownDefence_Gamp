
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
    public int newEvolutionCount; // ���� ��ȭ �ʿ� ��ȭ
    public int originUpgradeCost; // ��ȭ �ڽ�Ʈ
    public PopupDataSO idleTextData;
    public float idleTextRepeatTime = 2.5f;
    public List<string> idleTexts = new List<string>();
    public Sprite sprite; // ���� ������ ��������Ʈ
    public Sprite uiSprite; // �� ���� !!
    public Color uiTextColor;
    public Color uiBackgroundColor;
    public float shootingDelay; // ���� ������ ������
    public int damage; // ���� ������ ������
    public float bulletSpeed;

    public int specialAttackCount = 0; // �� �� ġ�� �����?
    public int feverAttackCount = 0;
    public Color feverColor = Color.white;
    public string feverDialog = "���� ���� �����ž�!!";
    [Range(0, 100f)]
    public float criticalPercent = 0f; // ũ��Ƽ��

    public PoolType normalArrow;
    public PoolType specialArrow;
}