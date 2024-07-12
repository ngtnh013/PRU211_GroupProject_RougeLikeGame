using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    protected EnemyStats enemy;
    protected Transform player;

    protected Vector2 knockbackVelocity;
    protected float knockbackDuration;

    public enum OutOfFrameAction { none, respawnAtEdge, despawn }
    public OutOfFrameAction outOfFrameAction = OutOfFrameAction.respawnAtEdge;

    protected bool spawnOutOfFrame = false;

    protected virtual void Start()
    {
        spawnOutOfFrame = !SpawnManager.IsWithinBoundaries(transform);
        enemy = GetComponent<EnemyStats>();

        PlayerMovement[] allPlayers = FindObjectsOfType<PlayerMovement>();
        player = allPlayers[Random.Range(0, allPlayers.Length)].transform;
    }

    protected virtual void Update()
    {
        if (knockbackDuration > 0)
        {
            transform.position += (Vector3)knockbackVelocity * Time.deltaTime;
            knockbackDuration -= Time.deltaTime;
        }
        else
        {
            Move();
            HandleOutOfFrameAction();
        }
    }

    protected virtual void HandleOutOfFrameAction()
    {
        if (!SpawnManager.IsWithinBoundaries(transform))
        {
            switch (outOfFrameAction)
            {
                case OutOfFrameAction.none:
                default:
                    break;
                case OutOfFrameAction.respawnAtEdge:
                    transform.position = SpawnManager.GeneratePosition();
                    break;
                case OutOfFrameAction.despawn:
                    if (!spawnOutOfFrame)
                    {
                        Destroy(gameObject);
                    }
                    break;
            }
        }
        else spawnOutOfFrame = false;
    }

    public virtual void Knockback(Vector2 velocity, float duration)
    {
        if (knockbackDuration > 0) return;

        knockbackVelocity = velocity;
        knockbackDuration = duration;
    }

    public virtual void Move()
    {
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, enemy.currentMoveSpeed * Time.deltaTime);
    }
}
