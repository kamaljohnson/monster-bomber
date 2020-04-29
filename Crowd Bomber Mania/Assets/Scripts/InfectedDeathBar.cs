using UnityEngine;
using UnityEngine.UI;

public class InfectedDeathBar : MonoBehaviour
{
    public Slider slider;
    public Gradient gradient;
    public Image fill;
    
    public float barLifeTime;
    
    private float _deathTimer;

    public void Start()
    {
        gameObject.transform.parent.gameObject.SetActive(true);
        _deathTimer = barLifeTime;
    }

    public void Update()
    {
        slider.value = _deathTimer / barLifeTime;
        _deathTimer -= Time.deltaTime;
        fill.color = gradient.Evaluate(slider.normalizedValue);
        if (_deathTimer <= 0)
        {
            Destroy(gameObject);
        }
    }
}
