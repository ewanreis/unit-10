using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingScript : MonoBehaviour
{
    public AudioClip pickupSound;
    public AudioSource source;
    private PlayerScore playerScore;
    public void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(other.gameObject.tag);
        if(other.gameObject.tag == "Player")
            PickUpRing(other);
    }
    private void PickUpRing(Collider2D playerCollider)
    {
        playerScore = playerCollider.gameObject.GetComponent<PlayerScore>();
        playerScore.rings++;
        source.PlayOneShot(pickupSound);
        Destroy(this.gameObject);
    }
}
