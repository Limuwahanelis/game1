using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrollingEnemy : Enemy
{
    public enum State
    {
        ALWAYS_IDLE,
        PATROLLING,
        IDLE_AT_PATROL_POINT
    }
    protected State currentState;

    public int idleCycles;
    public List<Transform> patrolPoints = new List<Transform>();
    private List<Vector3> _patrolpositions = new List<Vector3>();
    private int _patrolPointIndex = 0;

    public void MoveToPatrolPoint()
    {
        transform.position = Vector3.MoveTowards(transform.position, _patrolpositions[_patrolPointIndex], speed * Time.deltaTime);

        if (Mathf.Abs(transform.position.x - _patrolpositions[_patrolPointIndex].x) < 0.1)
        {
            if (_patrolPointIndex + 1 > _patrolpositions.Count - 1) _patrolPointIndex = 0;
            else _patrolPointIndex++;
            currentState = State.IDLE_AT_PATROL_POINT;
        }
    }

    protected void RotateEnemyTowardsNextPatrolPoint()
    {
        float direction;

        if (_patrolpositions[_patrolPointIndex].x < transform.position.x) direction = -1;
        else direction = 1;

        transform.localScale = new Vector3(direction, transform.localScale.y, transform.localScale.z);
    }

    protected void SetUpBehaviour()
    {

        for (int i = 0; i < patrolPoints.Count; i++)
        {
            _patrolpositions.Add(patrolPoints[i].position);
        }
        if (patrolPoints.Count < 2)
        {
            currentState = State.ALWAYS_IDLE;
        }
        else
        {
            currentState = State.PATROLLING;
            RotateEnemyTowardsNextPatrolPoint();
        }
    }
}
