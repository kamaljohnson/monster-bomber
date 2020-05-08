using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CanonBall : MonoBehaviour
{
    public float destructionDuration;

    public AudioSource cannonHitSound;

    private float _destroyCheckerTimer;

    private Rigidbody rb;

    private bool isCannonFalling = true;

    public Animator animator;

    public Collider triggerCollider;
    
    public void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    
    public void Update()
    {
        _destroyCheckerTimer += Time.deltaTime;
        if (_destroyCheckerTimer >= 10)
        {
            cannonHitSound.Play();
            Destroy(gameObject);
        }
        
        if (isCannonFalling)
        {
            transform.LookAt(transform.position + rb.velocity);
        }
    }

    public void OnCollisionEnter(Collision other)
    {
        if (other.collider.CompareTag("Ground"))
        {
            cannonHitSound.Play();
            Destroy(gameObject.GetComponent<Rigidbody>());
            isCannonFalling = false;
            
            animator.Play("CannonTimerAnimation", -1, 0f);
            StartCoroutine(TriggerCannonActivation());
        }
    }
    
    IEnumerator TriggerCannonActivation()
    {
        yield return new WaitForSeconds(2.5f);
 
        // Code to execute after the delay
        ActivateCannon();
    }

    private void ActivateCannon()
    {
        triggerCollider.enabled = true;
        Destroy(triggerCollider, 1f);
    }
    
    private void OnDestroy()
    {
        GameManager.ReportCannonBallUsed();
    }
}
