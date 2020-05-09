using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class Cannon : MonoBehaviour
{
    public GameObject cannonBall;

    public TMP_Text remainingCannonBallCountText;
    
    public Transform shootTransform;

    public AudioSource shootSound;
    public Animator shootAnimator;
    
    private Rigidbody cannonballInstance;

    public int cannonBallCount;
    private int _cannonBallsLeft;
    private bool _reloaded;
    public float reloadDelay;
    private float _reloadTimer;

    private static Cannon _cannon;
    
    [SerializeField]
    [Range(10f, 80f)]
    private float angle = 45f;

    private bool _touched;
    
    private void Start()
    {
        _cannon = this;
        _touched = false;
        
        GetCannonBallCountFromPref();
        
        _cannonBallsLeft = cannonBallCount;
        _reloaded = true;
        _reloadTimer = 0;
    }

    private void Update()
    {
        remainingCannonBallCountText.text = _cannonBallsLeft.ToString();
        
        if (Input.GetMouseButtonDown(0) || Input.touchCount > 0)
        {
            if (!GameManager.CanPlay) return;
            
            Vector3 pointerPosition;
            if (Input.touchCount > 0 && _touched == false)
            {
                _touched = true;
                pointerPosition = Input.GetTouch(0).position;
            }
            else
            {
                pointerPosition = Input.mousePosition;
            }
            
            var ray = Camera.main.ScreenPointToRay(pointerPosition);

            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo))
            {
                // 8 -> GameBoard Layer
                if (hitInfo.transform.gameObject.layer != 8) return;
                
                //if user is at the menu start the game
                if (GameManager.GameState == GameState.AtMenu)
                {
                    _cannonBallsLeft = cannonBallCount;
                    GameManager.StartGame();
                }
                
                if (_cannonBallsLeft > 0 && _reloaded)
                {
                    FireCannonAtPoint(hitInfo.point);
                }
            }
        }
        else
        {
            _touched = false;
            if (_cannonBallsLeft > 0 && !_reloaded && _reloadTimer >= reloadDelay)
            {
                _reloadTimer = 0;
                _reloaded = true;
            }

            if (_reloadTimer < reloadDelay)
            {
                _reloadTimer += Time.deltaTime;
            }
        }
    }

    private void FireCannonAtPoint(Vector3 point)
    {
        shootSound.Play();
        shootAnimator.Play("CannonShootAnimation", -1, 0f);
        _cannonBallsLeft--;
        _reloaded = false;
        
        var rotation = transform.rotation;
        transform.LookAt(point);
        rotation.eulerAngles = new Vector3(rotation.x, transform.rotation.eulerAngles.y , rotation.z);
        transform.rotation = rotation;

        var cannonBallObject = GameObject.Instantiate(cannonBall);
        cannonballInstance = cannonBallObject.GetComponent<Rigidbody>();
        
        var velocity = BallisticVelocity(point, angle);
        
        cannonballInstance.transform.position = shootTransform.position;
        cannonballInstance.velocity = velocity;
        // Apply the rotation to the rigid body
    }

    private Vector3 BallisticVelocity(Vector3 destination, float angle)
    {
        Vector3 dir = destination - shootTransform.position; // get Target Direction
        float height = dir.y; // get height difference
        dir.y = 0; // retain only the horizontal difference
        float dist = dir.magnitude; // get horizontal direction
        float a = angle * Mathf.Deg2Rad; // Convert angle to radians
        dir.y = dist * Mathf.Tan(a); // set dir to the elevation angle.
        dist += height / Mathf.Tan(a); // Correction for small height differences

        // Calculate the velocity magnitude
        float velocity = Mathf.Sqrt(dist * Physics.gravity.magnitude / Mathf.Sin(2 * a));
        return velocity * dir.normalized; // Return a normalized vector.
    }

    public void AddExtraCannonBall(int count)
    {
        cannonBallCount += count;
        _cannonBallsLeft += 1;
        PlayerPrefs.SetInt("CannonBallCount", cannonBallCount);
    }

    public static void Reset()
    {
        var cannonBalls = GameObject.FindGameObjectsWithTag("CannonBallHolder");
        foreach (var ball in cannonBalls)
        {
            Destroy(ball);
        }
        GetCannonBallCountFromPref();
        _cannon._cannonBallsLeft = _cannon.cannonBallCount;

    }

    public static int CannonBallsRemaining()
    {
        return _cannon._cannonBallsLeft;
    }

    private static void GetCannonBallCountFromPref()
    {
        if (PlayerPrefs.HasKey("CannonBallCount"))
        {
            _cannon.cannonBallCount = PlayerPrefs.GetInt("CannonBallCount");
        }
        else
        {
            SetCannonBallCountToPref();
        }
    }

    private static void SetCannonBallCountToPref()
    {
        PlayerPrefs.SetInt("CannonBallCount", _cannon.cannonBallCount);
    }
    
    private bool IsPointerOverUIObject() {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        var results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }
}
