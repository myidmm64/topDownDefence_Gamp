using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyWaveManager : MonoBehaviour
{
    public static EnemyWaveManager Instance { get; private set; }

    private float nextWaveSpawnTimer;
    private float nextEnemySpawnTimer;
    private int remainingEnemySpawnAmount;
    private Vector3 spawnPosition;
    [SerializeField]
    private List<Transform> spawnPositionTransformList = new List<Transform>();
    [SerializeField]
    private Transform nextWaveSpawnPositionTransform;

    public event EventHandler OnWaveNumberChanged;

    [SerializeField]
    private List<GenerateData> _generateDatas = new List<GenerateData>();

    private State state;
    private int waveNumber = 0;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        nextWaveSpawnTimer = 8f;
        state = State.WaitingToSpawnNextWave;
        spawnPosition = spawnPositionTransformList[Random.Range(0, spawnPositionTransformList.Count)].position;
        nextWaveSpawnPositionTransform.position = spawnPosition;
        StartCoroutine(EnemyWaveCoroutine());
    }

    private IEnumerator EnemyWaveCoroutine()
    {
        while(true)
        {
            spawnPosition = spawnPositionTransformList[Random.Range(0, spawnPositionTransformList.Count)].position;
            nextWaveSpawnPositionTransform.position = spawnPosition;
            waveNumber++;
            OnWaveNumberChanged?.Invoke(this, EventArgs.Empty);
            int index = Mathf.Clamp(waveNumber, 0, _generateDatas.Count - 1);
            GenerateData curData = _generateDatas[index];
            for(int i = 0; i < curData.count; i++)
            {
                Enemy enemy = PoolManager.Pop(PoolType.Enemy).GetComponent<Enemy>();
                enemy.DataSet(curData.enemyDataSO, spawnPosition + UtilClass.GetRandomDir() * Random.Range(0f, 10f));
                yield return new WaitForSeconds(nextEnemySpawnTimer);
            }
            yield return new WaitForSeconds(nextWaveSpawnTimer);
        }
    }

    private void Update()
    {
        /*
        switch (state)
        {
            case State.WaitingToSpawnNextWave:
                nextWaveSpawnTimer -= Time.deltaTime;
                if (nextWaveSpawnTimer <= 0f)
                {
                    SpawnWave();
                }
                break;
            case State.SpawningWave:
                if (remainingEnemySpawnAmount > 0)
                {
                    nextEnemySpawnTimer -= Time.deltaTime;
                    if (nextEnemySpawnTimer <= 0f)
                    {
                        nextEnemySpawnTimer = Random.Range(0f, .2f);
                        Enemy.Create(spawnPosition + UtilClass.GetRandomDir() * Random.Range(0f, 10f));
                        remainingEnemySpawnAmount--;
                        if(remainingEnemySpawnAmount <= 0)
                        {
                            state = State.WaitingToSpawnNextWave;
                            spawnPosition = spawnPositionTransformList[Random.Range(0, spawnPositionTransformList.Count)].position;
                            nextWaveSpawnPositionTransform.position = spawnPosition;
                            nextWaveSpawnTimer = 10f;
                        }
                    }
                }
                break;
            default:
                break;
        }
        */
    }

    public Vector3 GetSpawnPosition()
    {
        return spawnPosition;
    }

    private void SpawnWave()
    {
        //spawnPosition = spawnPositionTransformList[Random.Range(0, spawnPositionTransformList.Count)].position;
        //nextWaveSpawnPositionTransform.position = spawnPosition;
        nextWaveSpawnTimer = 10f;
        remainingEnemySpawnAmount = 5 + 3 * waveNumber;
        state = State.SpawningWave;
        waveNumber++;
        OnWaveNumberChanged?.Invoke(this, EventArgs.Empty);
    }

    public int GetWaveNumber()
    {
        return waveNumber;
    }

    public float GetNextWaveSpawnTimer()
    {
        return nextWaveSpawnTimer;
    }

    private enum State
    {
        WaitingToSpawnNextWave,
        SpawningWave,
    }
}
