using UnityEngine;

public class GuardState : State
{
    public GuardState(MonsterAI monsterAI, StateMachine stateMachine) : base(monsterAI, stateMachine){}

    public override void Enter()
    {
        monsterAI.StartPatrol();
        monsterAI.questionMark.SetActive(false);
        monsterAI.exclamationMark.SetActive(false);
        Debug.Log("GuardState Enter");
    }

    public override void Update()
    {
        monsterAI.PatrolUpdate();

        if (monsterAI.EnemyDetected())
        {
            stateMachine.ChangeState(new IdentifyingState(monsterAI, stateMachine));
        }
    }

    public override void Exit()
    {
        Debug.Log("Exiting GuardState");
        monsterAI.StopMoving();
    }
}
