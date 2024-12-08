using UnityEngine;
using UnityEngine.UI;

public class HidingObject : MonoBehaviour
{
    public bool isTrap = false; // Trap ����
    private Image buttonImage; // Button�� Image ����

    void OnEnable()
    {
        if (buttonImage == null)
        {
            var button = GetComponent<Button>();
            if (button != null)
            {
                buttonImage = button.targetGraphic as Image;

                if (buttonImage == null)
                {
                    Debug.LogError($"[HidingObject] TargetGraphic is not an Image on {gameObject.name}");
                    return;
                }
            }
            else
            {
                Debug.LogError($"[HidingObject] Missing Button component on {gameObject.name}");
                return;
            }
        }
        UpdateColor();
    }

    public void UpdateColor()
    {
        if (buttonImage != null)
        {
            buttonImage.color = isTrap ? Color.red : Color.white;
        }
    }

    public void OnClick()
    {
        // GameManager���� ó��
        FindObjectOfType<GameManager>().OnObjectClicked(gameObject);
    }
}
