using UnityEngine;

public class GroundAttack1 : CombatState
{
    public float damage = 1f;
    float stateDuration = 2f;
    float bufferDuration = 0.5f;
    public float shadowCharge = 20f;

    public override void EnterState(CombatStateManager stateManager)
    {
        stateManager.currentDamage = damage;
        stateManager.shadowCharge = shadowCharge;

        Debug.Log("Melee1");
        stateManager.KatanaEnableAnimator.SetTrigger("Attack1");
        stateManager.AttackAnimator.SetTrigger("Attack1");
    }

    public override void UpdateState(CombatStateManager stateManager)
    {
        if (stateManager.stateTime >= (stateDuration + bufferDuration))
        {
            stateManager.ContinueCombo(stateManager.Melee1);
        }
    }

    public override void ExitState(CombatStateManager stateManager)
    {
        stateManager.movementController.EnableMovement();
    }
}
