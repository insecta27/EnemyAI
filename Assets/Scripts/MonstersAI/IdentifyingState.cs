using Unity.VisualScripting;
using UnityEngine;

public class IdentifyingState : State
{
    private float _timer;

    public IdentifyingState(MonsterAI monsterAI, StateMachine stateMachine) : base(monsterAI, stateMachine){}

    public override void Enter()
    {
        _timer = 0f;
        monsterAI.StopMoving();
        monsterAI.ShowQuestionMark();
        Debug.Log("IdentifyingState Enter");
    }

    public override void Update()
    {
        _timer += Time.deltaTime;

        if (_timer >= monsterAI.settings.identificationDelay)
        {
            if (!monsterAI.EnemyDetected())
            {
                monsterAI.HideMarks();
                stateMachine.ChangeState(new GuardState(monsterAI, stateMachine));
                return;
            }

            monsterAI.ShowExclamationMark();
            monsterAI.AlertNearbyAllies();
            stateMachine.ChangeState(new AttackState(monsterAI, stateMachine));
        }

        if (!monsterAI.EnemyDetected())
        {
            monsterAI.HideMarks();
            stateMachine.ChangeState(new GuardState(monsterAI, stateMachine));
        }
    }

    public override void Exit()
    {
        Debug.Log("Exiting IdentifyingState");
        monsterAI.questionMark.SetActive(false);
    }
}
