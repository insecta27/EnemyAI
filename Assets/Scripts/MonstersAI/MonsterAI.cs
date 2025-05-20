using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class MonsterAI : MonoBehaviour
{
    public State CurrentState => _stateMachine.currentState;
    public List<Transform> waypoints;
    public GameObject questionMark;
    public GameObject exclamationMark;
    public MonsterSettings settings;
    public MonsterFaction faction;
    
    private NavMeshAgent _agent;
    private Transform _currentTarget;
    private StateMachine _stateMachine;
    private int _currentWaypointIndex;

    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _stateMachine = new StateMachine();
        _currentWaypointIndex = Random.Range(0, waypoints.Count);
        _stateMachine.Initialize(new GuardState(this, _stateMachine));
    }

    private void Update()
    {
        _stateMachine.Update();
    }

    public void StartPatrol()
    {
        if (waypoints == null || waypoints.Count == 0) return;
        
        _agent.isStopped = false;
        _agent.SetDestination(waypoints[_currentWaypointIndex].position);
    }
    public void StopMoving() => _agent.isStopped = true;

    public void PatrolUpdate()
    {
        if (_agent.remainingDistance <= _agent.stoppingDistance && !_agent.pathPending)
        {
            int _randomIndex = Random.Range(0, waypoints.Count);
            _agent.SetDestination(waypoints[_randomIndex].position);
        }
    }

    public void AlertNearbyAllies()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, settings.alertRadius);
        foreach (var hit in hits)
        {
            MonsterAI ally = hit.GetComponent<MonsterAI>();
            if (ally != null && ally != this && ally.faction == this.faction)
            {
                if (ally.CurrentState is GuardState)
                {
                    ally.ReceiveAlert(_currentTarget);
                }
            }
        }
    }
    public bool EnemyDetected()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, settings.visionRange);

        foreach (var hit in hits)
        {
            MonsterAI other = hit.GetComponent<MonsterAI>();

            if (other != null && other != this && other.faction != this.faction)
            {
                Vector3 dirToOther = (other.transform.position - transform.position).normalized;
                float angle = Vector3.Angle(transform.forward, dirToOther);

                if (angle <= settings.visionAngle / 2f)
                {
                    if (!Physics.Linecast(transform.position, other.transform.position))
                    {
                        _currentTarget = other.transform;
                        return true;
                    }
                }
            }
        }

        return false;
    }
    public bool HasTarget() => _currentTarget != null;

    public void ChaseTarget()
    {
        _agent.isStopped = false;
        _agent.SetDestination(_currentTarget.position);
    }
    
    public void ReceiveAlert(Transform enemy)
    {
        _currentTarget = enemy;
        ShowExclamationMark();
        _stateMachine.ChangeState(new AttackState(this, _stateMachine));
        Debug.Log("Allies allerted");
    }

    public void ShowQuestionMark()
    {
        questionMark.SetActive(true);
        exclamationMark.SetActive(false);
    }

    public void ShowExclamationMark()
    {
        exclamationMark.SetActive(true);
        questionMark.SetActive(false);
    }

    public void HideMarks()
    {
        exclamationMark.SetActive(false);
        questionMark.SetActive(false);
    }
    
    
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, settings.visionRange);

        Vector3 leftBoundary = Quaternion.Euler(0, -settings.visionAngle / 2f, 0) * transform.forward;
        Vector3 rightBoundary = Quaternion.Euler(0, settings.visionAngle / 2f, 0) * transform.forward;

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + leftBoundary * settings.visionRange);
        Gizmos.DrawLine(transform.position, transform.position + rightBoundary * settings.visionRange);
    }
}
