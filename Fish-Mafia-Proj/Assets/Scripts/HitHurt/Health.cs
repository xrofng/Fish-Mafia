using UnityEngine;
using static LevelController;

public class Health : MonoBehaviour, IDamageable
{
    [Header("HP")]
    public int maxHP = 12;

    int currentHP;
    public bool IsEnemy = true;

    public float DestroyDelay;

    void Awake()
    {
        if (IsEnemy && CoreGameManager.Instance.MechanicSettingSO.OneHitKill)
        {
            maxHP = 1;
        }
        currentHP = maxHP;
    }

    public void TakeDamage(int damage)
    {
        currentHP -= damage;

        //Debug.Log($"{gameObject.name} took {damage} damage. HP: {currentHP}");


        if (IsEnemy == false)
        {
            EventBus.TriggerEvent(new EvsPlayerHPChanged(currentHP, maxHP));
        }
        if (currentHP <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        //Debug.Log($"{gameObject.name} died");
        if (IsEnemy)
        {
            KokonutStateMachine s = GetComponent<KokonutStateMachine>();
            s.Kokonut.Animator.Play("Enemy_Death_Aclip");
            EventBus.TriggerEvent(new EvsEnemyDied(s));
        }
        else
        {
            EventBus.TriggerEvent(new EvsGameEnd("You Fail", "Retry"));
        }
        Destroy(gameObject, DestroyDelay);
    }

    public struct EvsEnemyDied
    {
        public KokonutStateMachine KokonutStateMachine;

        public EvsEnemyDied(KokonutStateMachine kokonutStateMachine)
        {
            this.KokonutStateMachine = kokonutStateMachine;
        }
    }

    public struct EvsPlayerHPChanged
    {
        public int CurrentHP;
        public int MaxHP;

        public EvsPlayerHPChanged(int currentHP, int maxHP)
        {
            this.CurrentHP = currentHP;
            this.MaxHP = maxHP;
        }
    }
}