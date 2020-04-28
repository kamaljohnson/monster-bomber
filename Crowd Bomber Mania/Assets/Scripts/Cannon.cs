using System.Globalization;
using TMPro;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    public GameObject cannonBall;

    public TMP_Text remainingCannonBallCountText;
    
    public Transform shootTransform;
    
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

    private void Start()
    {
        _cannon = this;
        
        if (PlayerPrefs.HasKey("CannonBallCount"))
        {
            cannonBallCount = PlayerPrefs.GetInt("CannonBallCount");
        }
        else
        {
            PlayerPrefs.SetInt("CannonBallCount", cannonBallCount);
        }
        
        _cannonBallsLeft = cannonBallCount;
        _reloaded = true;
        _reloadTimer = 0;
    }

    private void Update()
    {
        if (!(GameManager.GameState == GameState.AtMenu || GameManager.GameState == GameState.Playing)) return;

        remainingCannonBallCountText.text = _cannonBallsLeft.ToString();
        
        if (Input.GetMouseButtonDown(0) || Input.touchCount > 0)
        {

            Vector3 pointerPosition;
            if (Input.touchCount > 0)
            {
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
        _cannonBallsLeft--;
        _reloaded = false;
        
        var rotation = transform.rotation;
        transform.LookAt(point);
        rotation.eulerAngles = new Vector3(rotation.x,transform.rotation.eulerAngles.y , rotation.z);
        transform.rotation = rotation;

        var cannonBallObject = GameObject.Instantiate(cannonBall);
        cannonballInstance = cannonBallObject.GetComponent<Rigidbody>();
        
        var velocity = BallisticVelocity(point, angle);
        
        cannonballInstance.transform.position = shootTransform.position;
        cannonballInstance.velocity = velocity;
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
        if (PlayerPrefs.HasKey("CannonBallCount"))
        {
            _cannon.cannonBallCount = PlayerPrefs.GetInt("CannonBallCount");
        }
        else
        {
            PlayerPrefs.SetInt("CannonBallCount", _cannon.cannonBallCount);
        }

        _cannon._cannonBallsLeft = _cannon.cannonBallCount;

    }

    public static int CannonBallsRemaining()
    {
        return _cannon._cannonBallsLeft;
    }
}
