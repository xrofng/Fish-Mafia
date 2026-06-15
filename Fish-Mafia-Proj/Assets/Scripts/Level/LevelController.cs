using MoreMountains.Feedbacks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    public bool SkipCutscene = false;
    public List<LevelData> Levels;
    public Transform Player;
    public MMF_Player IntroFB;

    [Header("Transition")]
    public float MoveDuration = 0.5f;
    public PlayerStateMachine PlayerStateMachine;
    public AnimationCurve MoveCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    private int currentLevelIndex = 0;
    private bool isTransitioning = false;

    private void Start()
    {
        if (Levels.Count == 0) return;

        if (SkipCutscene)
        {
            Invoke(nameof(EnterFirsLevel), .5f);
        }
        else
        {
            IntroFB.PlayFeedbacks();
        }
    }

    public void EnterFirsLevel()
    {
        EnterLevel(0, instant: true);
    }

    private void Update()
    {
        if (isTransitioning) return;

        var level = GetCurrentLevel();
        if (level == null) return;

        if (level.IsCleared())
        {
            GoToNextLevel();
        }
    }

    LevelData GetCurrentLevel()
    {
        if (currentLevelIndex < 0 || currentLevelIndex >= Levels.Count)
            return null;

        return Levels[currentLevelIndex];
    }

    void GoToNextLevel()
    {
        int nextIndex = currentLevelIndex + 1;

        if (nextIndex >= Levels.Count)
        {
            EventBus.TriggerEvent(new EvsGameEnd("You Win", "Repeat Revenge"));
            return;
        }

        StartCoroutine(TransitionToLevel(nextIndex));
    }

    IEnumerator TransitionToLevel(int nextIndex)
    {
        isTransitioning = true;

        LevelData nextLevel = Levels[nextIndex];

        PlayerStateMachine.TransitionToState(PlayerStateMachine.EPlayerState.CantControlled);

        yield return MovePlayer(nextLevel.StartPoint.position);

        currentLevelIndex = nextIndex;

        isTransitioning = false;

        nextLevel.Hivemind.SetEnemyState(KokonutStateMachine.EEnemyState.Surround);
    }

    void EnterLevel(int index, bool instant = false)
    {
        currentLevelIndex = index;

        if (instant)
        {
            Player.position = Levels[index].StartPoint.position;
            Levels[index].Hivemind.SetEnemyState(KokonutStateMachine.EEnemyState.Surround);
        }
        else
        {
            StartCoroutine(MovePlayer(Levels[index].StartPoint.position));
        }
    }

    IEnumerator MovePlayer(Vector3 targetPos)
    {
        Vector3 start = Player.position;
        float time = 0f;

        while (time < MoveDuration)
        {
            time += Time.deltaTime;
            float t = time / MoveDuration;
            float eval = MoveCurve.Evaluate(t);

            Player.position = Vector3.Lerp(start, targetPos, eval);
            yield return null;
        }

        Player.position = targetPos;
    }

    public struct EvsGameEnd
    {
        public EvsGameEnd(string buttonText, string bigText)
        {
            ButtonText = buttonText;
            BigText = bigText;
        }

        public string ButtonText { get; internal set; }
        public string BigText { get; internal set; }
    }
}