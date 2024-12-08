using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    public GameObject objectPrefab; // ������ ������Ʈ ������
    public RectTransform spawnArea; // ���� ���� (Panel)
    public RectTransform destroyArea; // �ı� ���� (Panel) �߰�
    public float spawnInterval = 1.5f; // ���� ����

    private void Start()
    {
        InvokeRepeating(nameof(SpawnObject), 0f, spawnInterval); // ���� �������� ������Ʈ ����
    }

    void SpawnObject()
    {
        if (objectPrefab == null || spawnArea == null || destroyArea == null) return;

        // SpawnArea ���ο��� ������ ��ġ ���
        float randomX = Random.Range(spawnArea.rect.xMin, spawnArea.rect.xMax);
        float randomY = spawnArea.rect.yMax;
        Vector3 spawnPosition = spawnArea.TransformPoint(new Vector3(randomX, randomY, 0));

        // ������Ʈ ����
        GameObject newObject = Instantiate(objectPrefab, spawnPosition, Quaternion.identity, spawnArea.transform);


        // FallingObject�� DestroyArea ����
        FallingObject fallingObject = newObject.GetComponent<FallingObject>();
        if (fallingObject != null)
        {
            fallingObject.destroyArea = destroyArea; // DestroyArea�� �ڵ����� �Ҵ�
        }
    }
}
