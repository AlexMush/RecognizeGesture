using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameConfigScript : MonoBehaviour
{
    #region Fields
    [Tooltip("Patterns of gestures for game")]
    [SerializeField]
    private Pattern[] patterns;

    [Tooltip("Initial time given to finish level")]
    [SerializeField]
    private float initialTime;

    [Tooltip("Time subtracted each new level")]
    [SerializeField]
    private float deltaTime;

    [Tooltip("Minimum time given to finish level")]
    [SerializeField]
    private float minTime;

    [Tooltip("Number of scores added for each level")]
    [SerializeField]
    private int scoresPerLevel;

    [Tooltip("Alert time")]
    [SerializeField]
    private float alertTime;

    [Tooltip("How accurate should player's gestures be")]
    [SerializeField]
    private float correctRate;

    [Space(10)]
    [Header("Advanced configuration")]
    [Tooltip("Width of line for player's pattern that should be correlated with width of line in pattern image")]
    [SerializeField]
    private int brushWidth;

    [Tooltip("Minimal quantity of waypoints that will be recognized")]
    [SerializeField]
    private int minWaypoints;

    [Tooltip("Minimal size of gesture that will be recognized")]
    [SerializeField]
    private float minGestureSize;

    [Tooltip("Size of pattern image")]
    [SerializeField]
    private int patternTextureSize;
    #endregion

    #region Properties
    public Pattern[] Patterns
    {
        get { return patterns; }
    }

    public float InitialTime
    {
        get { return initialTime; }
    }

    public float DeltaTime
    {
        get { return deltaTime; }
    }

    public float MinTime
    {
        get { return minTime; }
    }

    public int ScoresPerLevel
    {
        get { return scoresPerLevel; }
    }

    public float AlertTime
    {
        get { return alertTime; }
    }

    public float CorrectRate
    {
        get { return correctRate; }
    }

    public int BrushWidth
    {
        get { return brushWidth; }
    }

    public int MinWaypoints
    {
        get { return minWaypoints; }
    }

    public float MinGestureSize
    {
        get { return minGestureSize; }
    }

    public int PatternTextureSize
    {
        get { return patternTextureSize; }
    }
    #endregion
}
