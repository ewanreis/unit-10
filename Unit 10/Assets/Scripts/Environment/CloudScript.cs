using UnityEngine;
using System.Collections;

public class CloudScript : MonoBehaviour 
{
    public float minSpeed;
    public float maxSpeed;
    public float minY;
    public float maxY;
    public float buffer;

    private float speed;
    private float camWidth;
    float camPos;

    void Start()
    {
        camWidth = Camera.main.orthographicSize * Camera.main.aspect;
        camPos = Camera.main.transform.position.x;
        speed = Random.Range(minSpeed, maxSpeed);
        transform.position = new Vector3(-camWidth - buffer + camPos, Random.Range(minY, maxY), transform.position.z);
    }

    void Update()
    {
        camPos = Camera.main.transform.position.x;
        transform.Translate(speed * Time.deltaTime, 0, 0);
        if(transform.position.x - (buffer + camPos) > camWidth)
            Destroy(gameObject);
    }
}