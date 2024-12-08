using UnityEngine;

public class FallingObject : MonoBehaviour
{
    public float fallSpeed = 300f; // ������Ʈ �ϰ� �ӵ�
    public RectTransform destroyArea; // �ı� ���� (Panel)

    private RectTransform rectTransform;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    private void Update()
    {
        transform.position += Vector3.down * fallSpeed * Time.deltaTime;

        Vector3 destroyAreaWorldPosition = destroyArea.position;
        float destroyAreaTopY = destroyAreaWorldPosition.y + (destroyArea.rect.height * 0.5f);

        if (transform.position.y < destroyAreaTopY)
        {
            GameManagerCat.Instance.OnObjectMissed();
            Destroy(gameObject);
        }
    }

    public void OnObjectClicked()
    {
        Debug.Log("Object clicked!");
        GameManagerCat.Instance.OnObjectHit(); // ���� ����
        Destroy(gameObject); // ������Ʈ ����
    }
}
