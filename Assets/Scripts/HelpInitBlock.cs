using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpInitBlock : MonoBehaviour {

    public GameObject prefab;
    public float gridX = 5f;
    public float gridY = 3f;
    public float spacing = 20f;

    void Start()
    {
        for (int y = -2; y < gridY; y++)
        {
            for (int x = -4; x < gridX; x++)
            {
                Vector3 pos = new Vector3(x, y, 0) * spacing;
                Instantiate(prefab, pos, Quaternion.identity);
            }
        }
    }
}
