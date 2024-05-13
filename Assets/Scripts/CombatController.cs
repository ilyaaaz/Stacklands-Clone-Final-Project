using UnityEngine;
using System.Collections;
using System;  // Include System for the Action type

public class CombatController : MonoBehaviour
{
    public static CombatController instance;
    public float attackDelay = 1f;  // Delay between turns

    private void Awake() 
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    public IEnumerator HandleCombat(GameObject attacker, GameObject defender, Action onCombatEnd) 
    {
        if (attacker == null || defender == null) {
            onCombatEnd?.Invoke();
            yield break;
        }

        // Stop regular movement and actions
        StopEntityActions(attacker);
        StopEntityActions(defender);

        Health attackerHealth = attacker.GetComponent<Health>();
        Health defenderHealth = defender.GetComponent<Health>();

        Vector3 attackerStartPos = attacker.transform.position;
        Vector3 defenderStartPos = defender.transform.position;

        // Define fixed combat positions
        Vector3 attackerCombatPosition = defenderStartPos + (attackerStartPos - defenderStartPos) * 0.9f;
        Vector3 defenderCombatPosition = attackerStartPos + (defenderStartPos - attackerStartPos) * 0.9f;

        while (attackerHealth.currentHealth > 0 && defenderHealth.currentHealth > 0) {
            // Attacker's turn
            yield return ExecuteAttackSequence(attacker, defender, attackerCombatPosition, attackerStartPos);
            if (defenderHealth.currentHealth <= 0) break;

            // Defender's turn
            yield return ExecuteAttackSequence(defender, attacker, defenderCombatPosition, defenderStartPos);
            if (attackerHealth.currentHealth <= 0) break;

            yield return new WaitForSeconds(attackDelay);
        }

        // Allow regular movement and actions
        ResumeEntityActions(attacker);
        ResumeEntityActions(defender);

        onCombatEnd?.Invoke();
    }

    private IEnumerator ExecuteAttackSequence(GameObject combatant, GameObject target, Vector3 combatPos, Vector3 startPos) 
    {
        // Move to combat position
        yield return Move(combatant, combatPos, 0.5f);
        // Attack
        target.GetComponent<Health>().TakeDamage(1);
        // Return to start position
        yield return new WaitForSeconds(0.5f);  // Wait for the attack to register visually
        yield return Move(combatant, startPos, 0.5f);
    }

    private IEnumerator Move(GameObject entity, Vector3 targetPos, float duration) 
    {
        float time = 0;
        Vector3 startPos = entity.transform.position;

        while (time < duration) 
        {
            entity.transform.position = Vector3.Lerp(startPos, targetPos, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        entity.transform.position = targetPos;
    }

    private void StopEntityActions(GameObject entity)
    {
        IMobActions mobActions = entity.GetComponent<IMobActions>();
        if (mobActions != null)
        {
            mobActions.StopActions();
        }
    }

    private void ResumeEntityActions(GameObject entity)
    {
        IMobActions mobActions = entity.GetComponent<IMobActions>();
        if (mobActions != null)
        {
            mobActions.ResumeActions();
        }
    }
}

public interface IMobActions
{
    void StopActions();
    void ResumeActions();
}
