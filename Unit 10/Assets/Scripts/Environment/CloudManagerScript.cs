using UnityEngine;
using System.Collections;

public class CloudManagerScript : MonoBehaviour 
{
    public GameObject cloudPrefab;
    public float delay;
    public static bool spawnClouds = true;

    void Start() => StartCoroutine(SpawnClouds());

    IEnumerator SpawnClouds()
    {
        while(spawnClouds)
        {
            Instantiate(cloudPrefab, transform);
            yield return new WaitForSeconds(delay);
        }
    }
}