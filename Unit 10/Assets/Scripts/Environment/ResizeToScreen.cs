using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResizeToScreen : MonoBehaviour
{
    private Vector2 resolution;

    private void Start() 
    {
        resolution = new Vector2(Screen.width, Screen.height);
        Resize();
        InvokeRepeating("SlowUpdate", 0.0f, 1f);
    }

    private void Resize()
    {
        SpriteRenderer sr=GetComponent<SpriteRenderer>();
        if(sr==null) return;

        transform.localScale=new Vector3(1,1,1);

        float width=sr.sprite.bounds.size.x;
        float height=sr.sprite.bounds.size.y;

        Vector3 desiredSize = transform.localScale;
        float worldScreenHeight = Camera.main.orthographicSize*2f;
        float worldScreenWidth = worldScreenHeight/Screen.height*Screen.width;
        desiredSize.x = worldScreenWidth / width;
        desiredSize.y = worldScreenHeight / height;
        transform.localScale = desiredSize;
    }

    private void SlowUpdate()
    {
        CheckResolutionChange();
    }

    void CheckResolutionChange()
    {
        if (resolution.x != Screen.width || resolution.y != Screen.height)
        {
            Resize();
            resolution.x = Screen.width;
            resolution.y = Screen.height;
        }
    }
}
