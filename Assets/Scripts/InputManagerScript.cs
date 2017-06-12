using System.Collections.Generic;
using UnityEngine;

public class InputManagerScript : MonoBehaviour
{
    #region Fields
    [Tooltip("Script with game configuration")]
    [SerializeField]
    private GameConfigScript config;

    [Tooltip("Custom line renderer")]
    [SerializeField]
    private LineRenderer lineRenderer;

    [Tooltip("Custom trail renderer")]
    [SerializeField]
    private TrailRenderer trailRenderer;

    private Texture2D objectivePattern;
    private float correctRate;

    private Gesture gesture;
    private GameManagerScript manager;
    #endregion

    #region Methods
    private void Start()
    {
        manager = GameManagerScript.Instance;
        gesture = new Gesture(config);
    }

    private void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            lineRenderer.positionCount = 0;
            Texture2D gesturePattern = gesture.GetPattern();
            objectivePattern = config.Patterns[manager.Index].pattern;
            correctRate = config.CorrectRate;

            if (gesturePattern != null)
            {
                if (gesture.ComparePattern(gesturePattern, objectivePattern, correctRate))
                {
                    manager.Alert("Correct!");
                    manager.NextLevel();
                }
                else
                {
                    manager.Alert("Wrong!");
                }
            }

            gesture.MouseData = new List<Vector3>();
        }
    }

    private void FixedUpdate()
    {
        if (Input.GetMouseButton(0) && manager.GameInProcess)
        {
            gesture.MouseData.Add(Input.mousePosition);

            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z - Camera.main.transform.position.z));
            trailRenderer.transform.position = mousePosition;
            lineRenderer.positionCount++;
            lineRenderer.SetPosition(lineRenderer.positionCount - 1, new Vector3(mousePosition.x, mousePosition.y, 0));
        }
    }
    #endregion
}
