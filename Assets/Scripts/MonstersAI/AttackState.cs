using UnityEngine;

public class AttackState : State
{
    public AttackState(MonsterAI monsterAI, StateMachine stateMachine) : base(monsterAI, stateMachine){}

    public override void Enter()
    {
        Debug.Log("AttackState Enter");
    }

    public override void Update()
    {
        if (monsterAI.HasTarget())
        {
            monsterAI.AlertNearbyAllies();
            monsterAI.ChaseTarget();
        }
    }
    
    public override void Exit()
    {
        Debug.Log("Exiting AttackState");
    }
}
