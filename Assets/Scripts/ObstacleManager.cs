using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleManager : MonoBehaviour
{
    public GameObject Prefab;
    public int gridsize = 10;
    
    public void Start()
    {
       for (int x = 0; x < gridsize; x++)
        {
            for (int z = 0; z < gridsize; z++)
            {
                Vector3 position = new Vector3(x, 1, z); 
                Instantiate(Prefab, position, Quaternion.identity);
            }
        }
    }
}
