using UnityEngine;

public class State 
{
   protected StateMachine stateMachine;
   protected MonsterAI monsterAI;

   public State(MonsterAI monsterAI, StateMachine stateMachine)
   {
      this.monsterAI = monsterAI;
      this.stateMachine = stateMachine;
   }
   
   public virtual void Enter(){}
   public virtual void Update(){}
   public virtual void Exit(){}
}
