using MoreMountains.Tools;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Combat/Move")]
public class MoveData : SerializedScriptableObject
{
    public string moveName;

    [Header("Combo Starter Requirement")]
    public EInputAction requiredInput;
    public PlayerStateMain.InputHoldDirection requiredDirection;

    [System.Serializable]
    public class ComboBranch
    {
        public MoveData nextMove;
        public EInputAction RequiredInput;
    }

    public enum EInputAction
    {
        LeftAttack,
        RightAttack,
        Shoot
    }

    public string AnimationStateName;
    [BoxGroup]
    public MoveStat Stat;

    [System.Serializable]
    public class MoveStat
    {
        [Header("Power")]
        [Range(0, 3)]
        public float PowerMultiplier = 1;
        [Header("Mana")]
        [Range(0, 10)]
        public int BulletGainOnHit = 2;

        [Header("Knockback")]
        [Range(0, 1)]
        public float HorizontalKnockback = 1;
        [Range(-1, 1)]
        public float VerticalKnockback = 0;
        public float KnockbackPower = 2;
        [Range(0, 1)]
        public float KnockbackDuration = .25f;

        public Vector2 ComputeKnockback(Vector3 DirFromAttacker)
        {
            float hkb = DirFromAttacker.x >= 0 ? HorizontalKnockback : HorizontalKnockback * -1;
            return new Vector2(hkb, VerticalKnockback) * KnockbackPower;
        }
    }
    

    [Header("Combo")]
    public Vector2 comboWindows;
    public List<ComboBranch> comboBranches;

    [Header("Enclose")]
    public bool encloseToTarget = true;
    public float encloseDuration = 0.1f;
    public float encloseRange = 1f;
    public float duration = 0.5f;

    [FoldoutGroup("Evocation")]
    public HorizontalEvocation EvocationPrefab;
    [FoldoutGroup("Evocation")]
    public float DelayBeforeSpawn = .3f;

    [FoldoutGroup("Sfx")]
    public AudioClip SoundWithoutDelay;
    [FoldoutGroup("Sfx")]
    public AudioClip Sound;
    [FoldoutGroup("Sfx")]
    public float DelayBeforeSoundPlayed = .3f;
    [FoldoutGroup("Sfx")]
    public Vector2 PitchRange = new Vector2(.95f, 1.1f);
    [FoldoutGroup("Sfx")]
    public MMSoundManagerPlayOptions SoundOption;

    public bool CanCancelByAttack = true;


    public IEnumerator AttackMoveRoutine(Animator animator, PlayerCombat attacker, Vector3 target, Vector3 attackDir)
    {
        PlayAnimation(animator, attacker, attackDir);

        float timer = 0f;
        bool spawned = false;
        bool soundPlayed = false;

        Vector3 startPos = attacker.transform.position;
        Vector3 targetPos = ComputeTargetStandingPosition(target, attackDir);
        SoundOption.AttachToTransform = attacker.transform;

        HandleInitialSound();

        while (timer < duration)
        {
            float dt = Time.deltaTime;
            timer += dt;

            HandleMovement(attacker, startPos, targetPos, timer);
            HandleEvocationSpawn(attacker, attackDir, timer, ref spawned);
            HandleSound(timer, ref soundPlayed);

            yield return null;
        }
    }

    private void HandleInitialSound()
    {
        if (SoundWithoutDelay)
        {
            SoundOption.Pitch = Random.Range(PitchRange.x, PitchRange.y);
            MMSoundManagerSoundPlayEvent.Trigger(SoundWithoutDelay, SoundOption);
        }
    }

    void PlayAnimation(Animator animator, PlayerCombat attacker, Vector3 attackDir)
    {
        animator.Play(AnimationStateName);
        attacker.SetFacing(attackDir);
    }

    Vector3 ComputeTargetStandingPosition(Vector3 target, Vector3 attackDir)
    {
        float dirX = attackDir.x >= 0 ? -1 : 1;
        return target + (Vector3.right * dirX * encloseRange);
    }

    void HandleMovement(PlayerCombat attacker, Vector3 startPos, Vector3 targetPos, float timer)
    {
        if (!encloseToTarget)
            return;

        if (timer > encloseDuration)
            return;

        float t = timer / encloseDuration;
        attacker.transform.position = Vector3.Lerp(startPos, targetPos, t);
    }

    void HandleEvocationSpawn(PlayerCombat attacker, Vector3 attackDir, float timer, ref bool spawned)
    {
        if (spawned || timer < DelayBeforeSpawn)
            return;

        spawned = true;
        SpawnEvocation(attacker, attackDir);
    }

    void SpawnEvocation(PlayerCombat attacker, Vector3 attackDir)
    {
        if (EvocationPrefab == null)
            return;

        HorizontalEvocation evo = Instantiate(
            EvocationPrefab,
            attacker.EvocationSpawnPoint.position,
            Quaternion.identity
        );

        evo.Init(attackDir, attacker);
    }

    private void HandleSound(float timer, ref bool soundPlayed)
    {
        if (soundPlayed || timer < DelayBeforeSoundPlayed)
            return;

        SoundOption.Pitch = Random.Range(PitchRange.x, PitchRange.y);
        MMSoundManagerSoundPlayEvent.Trigger(Sound, SoundOption);
        soundPlayed = true;
    }

    [Button]
    public void NameToAnimStateName()
    {
        AnimationStateName = name + "_Aclip";
    }

    // later:
    // animation, hitbox, etc.
}

public enum DirectionType
{
    Neutral,
    Up,
    Down,
    Forward,
    Back
}