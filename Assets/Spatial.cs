using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Circle
{
    public Vector2 m_pos;
    public float m_radius;
}

public class Grid
{
    public List<Circle> circles = new List<Circle>();
}

public class Spatial : MonoBehaviour {
    Texture2D tex;
    List<Circle> circles = new List<Circle>();
    int count = 1000;
    int gridSize;
    int cellAmount;

    void Start () {
        cellAmount = 16;
        gridSize = 1024 / cellAmount;
        Grid[,] gridArray = new Grid[cellAmount, cellAmount];
        Random.InitState(0);
        tex = GetComponent<SpriteRenderer>().sprite.texture;
        for (int x = 0; x < cellAmount; x++)
        {
            for (int y = 0; y < cellAmount; y++)
            {
                gridArray[x,y] = new Grid();
            }
        }
        for(int i = 0; i < count; ++i)
        {
            Circle c = new Circle();
            c.m_pos = new Vector2(Random.Range(0, 1023), Random.Range(0, 1023));
            c.m_radius = Random.Range(1, 49);
            int leftSide = (int)(c.m_pos.x - c.m_radius) / gridSize;
            int rightSide = (int)(c.m_pos.x + c.m_radius) / gridSize;
            int bottomSide = (int)(c.m_pos.y - c.m_radius) / gridSize;
            int topSide = (int)(c.m_pos.y - c.m_radius) / gridSize;

            if (leftSide < 0)
            {
                leftSide = 0;
            }
            if (rightSide > cellAmount - 1)
            {
                rightSide = cellAmount - 1;
            }
            if (bottomSide < 0)
            {
                bottomSide = 0;
            }
            if (topSide > cellAmount - 1)
            {
                topSide = cellAmount - 1;
            }

            for (int x = leftSide; x <= rightSide; x++)
            {
                for (int y = bottomSide; y <= topSide; y++)
                {
                    gridArray[x, y].circles.Add(c);
                }
            }
            circles.Add(c);
        }
        Color32[] colours = new Color32[1024 * 1024];
        float t = Time.realtimeSinceStartup;
        for (int y = 0; y < 1024; ++y)
        {
            for (int x = 0; x < 1024; ++x)
            {
                float value = 0;
                int x_ = (int)x / 64;
                int y_ = (int)y / 64;
                for (int i = 0; i < gridArray[x_, y_].circles.Count; ++i)
                {
                    float d = (new Vector2((float)x, (float)y) - gridArray[x_, y_].circles[i].m_pos).magnitude;
                    if (d < gridArray[x_, y_].circles[i].m_radius)
                    {
                        value = value + (1.0f - d / gridArray[x_, y_].circles[i].m_radius);
                       value =  Mathf.Clamp(value, 0.0f, 1.0f);
                    }

                }
                colours[x + y * 1024].r = (byte)(value * 255);
                colours[x + y * 1024].g = (byte)(value * 255);
                colours[x + y * 1024].b = (byte)(value * 255);
                colours[x + y * 1024].a = 255;
            }
        }
        Debug.Log("Time taken = " + (Time.realtimeSinceStartup - t));
        tex.SetPixels32(colours);
        tex.Apply();
    }

    void Update () {
    }
}
