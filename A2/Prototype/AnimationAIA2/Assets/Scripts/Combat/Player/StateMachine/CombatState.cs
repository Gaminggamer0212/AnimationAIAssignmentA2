using UnityEngine;

public abstract class CombatState
{
    public abstract void EnterState(CombatStateManager state);
    
    public abstract void UpdateState(CombatStateManager state);
    
    public abstract void ExitState(CombatStateManager state);
    
    //public abstract void OnCollisionEnter(Collider other);
    
}
