using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using TMPro;
public class GameManager : MonoBehaviour
{
    public GameObject[] hidingObjects; // ���� ������Ʈ �迭
    public float appearDuration = 1.0f; // ������Ʈ�� ��Ÿ���� �ð�
    public float gameTime = 30.0f; // ���� �ð�
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI trapClickText; // ���� �޽����� ǥ���� Text (�ɼ�)
    public Button retryButton; // ����� ��ư

    private int score = 0;
    private float remainingTime;
    private bool gameActive = true;
    private int trapClickCount = 3; // ������ Ŭ�� ������ Ƚ��

    bool isWin = false;

    void Start()
    {
        // Score Text: ���� �Ʒ� ��ġ
        scoreText.rectTransform.anchorMin = new Vector2(0, 0); // ���� �Ʒ�
        scoreText.rectTransform.anchorMax = new Vector2(0, 0); // ���� �Ʒ�
        scoreText.rectTransform.pivot = new Vector2(0, 0);     // ���� �Ʒ� ����
        scoreText.rectTransform.anchoredPosition = new Vector2(10, 10); // ȭ�� ��迡�� 10px ������

        // Lives Text: ������ �Ʒ� ��ġ
        trapClickText.rectTransform.anchorMin = new Vector2(1, 0); // ������ �Ʒ�
        trapClickText.rectTransform.anchorMax = new Vector2(1, 0); // ������ �Ʒ�
        trapClickText.rectTransform.pivot = new Vector2(1, 0);     // ������ �Ʒ� ����
        trapClickText.rectTransform.anchoredPosition = new Vector2(-10, 10); // ȭ�� ��迡�� 10px ������

        // ȭ�� �߾� ��ܿ� ��ġ
        timerText.rectTransform.anchorMin = new Vector2(0.5f, 1.0f); // ȭ���� �߾� ���
        timerText.rectTransform.anchorMax = new Vector2(0.5f, 1.0f); // ȭ���� �߾� ���
        timerText.rectTransform.pivot = new Vector2(0.5f, 1.0f); // �߽� ������ ������� ����
        timerText.rectTransform.anchoredPosition = new Vector2(0, -50); // ��ܿ��� �Ʒ��� 50px �̵�

        // Retry Button �ʱ� ���� (��Ȱ��ȭ �� ȭ�� �߾� ��ġ)
        retryButton.gameObject.SetActive(false); // �ʱ� ���¿��� ��Ȱ��ȭ
        retryButton.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 0.5f);
        retryButton.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 0.5f);
        retryButton.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
        retryButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0); // Ÿ�̸� �Ʒ��� ��ġ

        retryButton.onClick.AddListener(RestartGame); // ��ư Ŭ�� �̺�Ʈ �߰�

        remainingTime = gameTime;
        UpdateUI();
        StartCoroutine(SpawnHidingObjects());
    }

    void Update()
    {
        if (gameActive)
        {
            remainingTime -= Time.deltaTime;
            if (remainingTime <= 0)
            {
                EndGame();
            }
            UpdateUI();
        }
    }
    IEnumerator SpawnHidingObjects()
    {
        while (gameActive)
        {
            int randomIndex = Random.Range(0, hidingObjects.Length);
            GameObject obj = hidingObjects[randomIndex];

            // �������� ���θ� ����
            var hidingObj = obj.GetComponent<HidingObject>();
            hidingObj.isTrap = Random.value < 0.2f; // 20% Ȯ���� ���� ����
            hidingObj.UpdateColor(); // ���� ������Ʈ

            // ������Ʈ ��Ÿ����
            obj.SetActive(true);
            yield return new WaitForSeconds(appearDuration);

            // ������Ʈ �����
            obj.SetActive(false);
            yield return new WaitForSeconds(0.5f); // ���� ������Ʈ ���� �� ���
        }
    }


    public void OnObjectClicked(GameObject obj)
    {
        if (!gameActive) return;

        var hidingObj = obj.GetComponent<HidingObject>();

        if (hidingObj.isTrap)
        {
            // ������ Ŭ���� ���
            if (score >= 2)
            {
                score -= 2;
            }
            trapClickCount--;
            if (trapClickCount <= 0)
            {
                isWin = false;
                EndGame();
                return;
            }
        }
        else
        {
            // �Ϲ� ������Ʈ�� Ŭ���� ���
            score++;
        }

        obj.SetActive(false); // Ŭ���� ������Ʈ ����
        UpdateUI();

        if (score >= 10)
        {
            isWin = true;
        }
    }

    void UpdateUI()
    {
        scoreText.text = "Score: " + score;
        timerText.text = "Time: " + Mathf.CeilToInt(remainingTime);
        trapClickText.text = "Lives: " + trapClickCount;
    }

    void EndGame()
    {
        gameActive = false;

        if (isWin)
        {
            Debug.Log("You Win!");
        }
        else
        {
            // Retry Button Ȱ��ȭ
            retryButton.gameObject.SetActive(true);
            Debug.Log("Game Over!");
        }
    }
    private void RestartGame()
    {
        SceneManager.LoadScene("HolePlantGame");
    }
}



