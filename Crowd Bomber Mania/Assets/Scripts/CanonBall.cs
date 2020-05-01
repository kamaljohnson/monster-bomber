using System;
using UnityEngine;

public class CanonBall : MonoBehaviour
{
    public float destructionDuration;

    public AudioSource cannonHitSound;
    
    public void OnCollisionEnter(Collision other)
    {
        if (other.collider.CompareTag("Ground"))
        {
            cannonHitSound.Play();
            Destroy(gameObject, destructionDuration);
        }
    }

    private void OnDestroy()
    {
        GameManager.ReportCannonBallUsed();
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Person.GetTag(PersonTags.Healthy)))
        {
            other.gameObject.GetComponent<Person>().TriggerInfection();
        }
    }
}
