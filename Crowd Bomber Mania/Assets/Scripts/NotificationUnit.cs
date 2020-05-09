using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NotificationUnit : MonoBehaviour
{
    public Image image;
    public TMP_Text text;
    
    public void Start()
    {
        // syncs with the animation
        Destroy(gameObject, 2f);
    }

    public void SetSprite(Sprite sprite)
    {
        image.sprite = sprite;
    }

    public void SetText(string text)
    {
        this.text.text = text;
    }
}
