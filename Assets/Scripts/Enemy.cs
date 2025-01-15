using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float delayBeforeAttack = 1f;
    public static Enemy Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        GameManager.OnGameFinished += Kill;
    }
    private void OnDisable()
    {
        GameManager.OnGameFinished -= Kill;
    }

    private void Start()
    {
        //look at Player but only adjust the rotation on the Y axis
        Vector3 lookDirection = GameManager.Instance.PlayerHead.position - transform.position;
        lookDirection.y = 0;
        transform.rotation = Quaternion.LookRotation(lookDirection);
        StartCoroutine(Attack());
    }

    private IEnumerator Attack()
    {
        yield return new WaitForSeconds(delayBeforeAttack);
        while (GameManager.Instance.IsGameFinished == false)
        {
            if (GameManager.Instance.HasEyesClosed == false)
            {
               GameManager.Instance.PlayerKilled();
               Destroy(gameObject);
            }
            yield return null;
        }
    }

    public void DestroyEnemy(float delay)
    {
        Destroy(gameObject, delay);
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    private void Kill()
    {
        StopAllCoroutines();
        Destroy(gameObject);
    }
}
