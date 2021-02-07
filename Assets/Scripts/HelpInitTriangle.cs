using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpInitTriangle : MonoBehaviour {

    public GameObject prefab;
    public float gridX = 11f;
    public float gridY = 6f;
    public float spacing = 20;

    void Start()
    {
        int N = 11;
        int center = N / 2;
        for (int i = 0; i < gridY; i++)
        {
            for (int j = 0; j < gridX; j++)
            {
                if (i <= center)
                {
                    if (j >= center - i && j <= center + i)
                    {
                        Vector3 pos = new Vector3(j, i, 0) * spacing;
                        Instantiate(prefab, pos, Quaternion.identity);
                    }
                }
                else
                {
                    if (j >= center + i - N + 1 && j <= center - i + N - 1)
                    {
                        Vector3 pos = new Vector3(j, i, 0) * spacing;
                        Instantiate(prefab, pos, Quaternion.identity);
                    }
                }
            }
        }
    }
}
