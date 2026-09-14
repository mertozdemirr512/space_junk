using UnityEngine;

[CreateAssetMenu(fileName = "LevelData_00", menuName = "SpaceJunk/Level Data")]
public class LevelData : ScriptableObject
{
    [Header("Level Rules")]
    public int levelNumber = 1;
    public float timeLimit = 60f;
    public float requiredScoreGoal = 2.5f;

    [Header("Gate States (0:North, 1:South, 2:East, 3:West)")]
    public GoalGate.GateState[] gateStates = new GoalGate.GateState[4]
    {
        GoalGate.GateState.Open,
        GoalGate.GateState.Open,
        GoalGate.GateState.Open,
        GoalGate.GateState.Open
    };
}