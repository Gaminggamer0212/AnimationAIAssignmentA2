using UnityEngine;

public class Idle : CombatState
{
    
    public override void EnterState(CombatStateManager stateManager)
    {
        Debug.Log("Idle");
    }

    public override void UpdateState(CombatStateManager stateManager)
    {
        if (stateManager.attacking)
        {
            stateManager.StartCombo();
        }
    }
    
    public override void ExitState(CombatStateManager stateManager)
    {
        return;
    }

    /*public override void OnCollisionEnter(Collider other)
    {
        
    }*/
}
