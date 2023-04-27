using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/EnemyData")]
public class EnemyDataSO : ScriptableObject
{
    public float speed = 5f;
    public int maxHp = 10;
    public int damage = 10;
    public Sprite sprite = null;
    public float size = 1f;
    public int dieCost = 10;
}
