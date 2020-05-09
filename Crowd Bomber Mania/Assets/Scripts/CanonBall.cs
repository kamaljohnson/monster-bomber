using System;
using System.Collections;
using System.Linq;
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

    public Transform particleEffectTransform;
    
    public void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    
    public void Update()
    {
        _destroyCheckerTimer += Time.deltaTime;
        if (_destroyCheckerTimer >= 10 && isCannonFalling)
        {
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
            isCannonFalling = false;
            
            rb.isKinematic = true;
            cannonHitSound.Play();
            
            animator.Play("CannonTimerAnimation", -1, 0f);
            particleEffectTransform.eulerAngles = new Vector3(0, 1, 0);
            StartCoroutine(TriggerCannonDeactivation());
        }
    }

    private IEnumerator TriggerCannonDeactivation()
    {
        yield return new WaitForSeconds(6);
        
        DeactivateCannon();
    }
    
    private void DeactivateCannon()
    {
        transform.GetChild(0).tag = "UsedCannonBall";
        GameManager.ReportCannonBallUsed();
    }
    
    private void OnDestroy()
    {
        GameManager.ReportCannonBallUsed();
    }
}
