using System;
using System.Collections;
using System.Collections.Generic;
using Meta.XR.MRUtilityKit;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    [SerializeField] FindSpawnPositions spawnPositions;
    [SerializeField] Vector2 startDelayRange = new Vector2(20, 30);
    [SerializeField] Vector2 attackCoolDownRange = new Vector2(5, 10);
    [SerializeField] Vector2 attackDurationRange = new Vector2(3, 5);
    [SerializeField] [Range(0,1)] float attackChance = 0.4f;
    [SerializeField] float tvOnIncreaseRate = 1.3f;
    [SerializeField] float lightOnIncreaseRate = 1.1f;
    [SerializeField] private AudioClip[] eventNoises;
    [SerializeField] private AudioClip hardBreathing;

    public static event Action OnEnemyAttack;
    private float currentAttackChance;
    private AudioClip currentEventNoise;
    private bool hasStarted = false;

    private void OnEnable()
    {
        GameManager.OnGameFinished += () =>
        {
            StopAllCoroutines();
            audioSource.Stop();
        };
    }

    private void Start()
    {
        StartCoroutine(AttackRoutine());
    }

    private void Update()
    {
        if (GameManager.Instance.IsGameFinished) return;
        float increaseRate = 1;
        if (GameManager.Instance.IsTVOn)
        {
            increaseRate *= tvOnIncreaseRate;
        }
        if (GameManager.Instance.IsLightOn)
        {
            increaseRate *= lightOnIncreaseRate;
        }
        currentAttackChance = attackChance * increaseRate;
    }

    private IEnumerator AttackRoutine()
    {
        if (hasStarted == false)
        {
            yield return new WaitForSeconds(Random.Range(startDelayRange.x, startDelayRange.y));
            hasStarted = true;
        }
        while (GameManager.Instance.IsGameFinished == false)
        {
            yield return new WaitForSeconds(Random.Range(attackCoolDownRange.x, attackCoolDownRange.y));
            if (GameManager.Instance.IsGameFinished) yield break;
            if (Random.value < currentAttackChance)
            {
                StartCoroutine(Attack());
                yield break;
            }
        }
    }

    private IEnumerator Attack()
    {
        currentEventNoise = eventNoises[Random.Range(0, eventNoises.Length)];
        audioSource.PlayOneShot(currentEventNoise);
        yield return new WaitForSeconds(currentEventNoise.length + Random.Range(attackDurationRange.x, attackDurationRange.y));
        if (GameManager.Instance.IsGameFinished) yield break;
        spawnPositions.StartSpawn(MRUK.Instance.GetCurrentRoom());
        OnEnemyAttack?.Invoke();
        audioSource.clip = hardBreathing;
        audioSource.loop = true;
        audioSource.Play();
        float randomAttackDuration = Random.Range(attackDurationRange.x, attackDurationRange.y);
        Enemy.Instance.DestroyEnemy(randomAttackDuration);
        yield return new WaitForSeconds(randomAttackDuration);
        if (GameManager.Instance.IsGameFinished) yield break;
        audioSource.Stop();
        StartCoroutine(AttackRoutine());
    }
}
