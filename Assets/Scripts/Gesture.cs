using System.Collections.Generic;
using UnityEngine;

public class Gesture
{
    #region Fields
    private List<Vector3> mouseData = new List<Vector3>();
    private int brushWidth;
    private int minWaypoints;
    private float minGestureSize;
    private int patternTextureSize;
    #endregion

    #region Properties
    public List<Vector3> MouseData
    {
        get { return mouseData; }
        set { mouseData = value; }
    }
    #endregion

    #region Constructors
    public Gesture (GameConfigScript config)
    {
        brushWidth = config.BrushWidth;
        minWaypoints = config.MinWaypoints;
        patternTextureSize = config.PatternTextureSize;
        minGestureSize = config.MinGestureSize;
    }
    #endregion

    #region Methods
    public Texture2D GetPattern()
    {
        if (mouseData.Count < minWaypoints)
            return null;

        Bounds bounds = new Bounds(mouseData[0], Vector3.zero);

        for (int i = 1; i < mouseData.Count; i++)
        {
            bounds.min = Vector3.Min(bounds.min, mouseData[i]);
            bounds.max = Vector3.Max((bounds).max, mouseData[i]);
        }

        Texture2D texture2D = new Texture2D(patternTextureSize, patternTextureSize);
        Color[] pixels = texture2D.GetPixels();

        for (int i = 0; i < pixels.Length; i++)
        {
            pixels[i] = Color.white;
        }

        Vector3 size = bounds.size;

        if (size.magnitude < minGestureSize)
            return null;

        for (int i = 0; i < mouseData.Count - 1; ++i)
        {
            int scaledStartX = (int)Mathf.Clamp(((mouseData[i].x - bounds.min.x) / bounds.size.x * patternTextureSize), 0, patternTextureSize - 1);
            int scaledStartY = (int)Mathf.Clamp(((mouseData[i].y - bounds.min.y) / bounds.size.y * patternTextureSize), 0, patternTextureSize - 1);
            int scaledEndX   = (int)Mathf.Clamp(((mouseData[i + 1].x - bounds.min.x) / bounds.size.x * patternTextureSize), 0, patternTextureSize - 1);
            int scaledEndY   = (int)Mathf.Clamp(((mouseData[i + 1].y - bounds.min.y) / bounds.size.y * patternTextureSize), 0, patternTextureSize - 1);

            float scaledLineLength = Mathf.Sqrt(Mathf.Pow((scaledEndX - scaledStartX), 2f) + Mathf.Pow((scaledEndY - scaledStartY), 2f));

            for (int j = 0; j <= 20; j++)
            {
                float index = j * 0.05f;

                int intermediateX = (int)(scaledStartX + (scaledEndX - scaledStartX) * index);
                int intermediateY = (int)(scaledStartY + (scaledEndY - scaledStartY) * index);

                pixels[intermediateX + intermediateY * patternTextureSize] = Color.black;

                for (int k = 1; k < brushWidth; k++)
                {
                    int widthX1 = (int)(intermediateX + ((scaledEndY - scaledStartY) * k) / scaledLineLength);
                    int widthY1 = (int)(intermediateY - ((scaledEndX - scaledStartX) * k) / scaledLineLength);
                    int widthX2 = (int)(intermediateX - ((scaledEndY - scaledStartY) * k) / scaledLineLength);
                    int widthY2 = (int)(intermediateY + ((scaledEndX - scaledStartX) * k) / scaledLineLength);

                    if (widthX1 >= 0 && widthX1 < patternTextureSize && (widthY1 >= 0 && widthY1 < patternTextureSize))
                    {
                        pixels[widthX1 + widthY1 * patternTextureSize] = Color.black;
                    }
                    if (widthX2 >= 0 && widthX2 < patternTextureSize && (widthY2 >= 0 && widthY2 < patternTextureSize))
                    {
                        pixels[widthX2 + widthY2 * patternTextureSize] = Color.black;
                    }
                }
            }
        }

        texture2D.SetPixels(pixels);
        texture2D.Apply();

        return texture2D;
    }

    public float TestPattern(Texture2D fromTexture, Texture2D toTexture)
    {
        if (fromTexture == null || toTexture == null)
        {
            Debug.LogError("Texture pattern for comparison is not set.");
            return 0f;
        }

        Color[] fromTexturePixels = fromTexture.GetPixels();
        Color[] toTexturePixels = toTexture.GetPixels();

        float toTextureBlackPixels = 0f;
        float commonBlackPixels = 0f;

        for (int i = 0; i < toTexturePixels.Length; ++i)
        {
            if (toTexturePixels[i] == Color.black)
            {
                toTextureBlackPixels++;
            }
        }

        for (int i = 0; i < fromTexturePixels.Length; ++i)
        {
            if (fromTexturePixels[i] == Color.black && toTexturePixels[i] == Color.black)
            {
                commonBlackPixels++;
            }
        }

        return commonBlackPixels / toTextureBlackPixels;
    }
    #endregion
}
