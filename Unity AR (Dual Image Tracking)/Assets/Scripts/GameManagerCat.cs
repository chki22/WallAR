using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManagerCat : MonoBehaviour
{
    public static GameManagerCat Instance;

    public int maxMissCount = 3; // �ִ� ���� Ƚ��
    public TextMeshProUGUI scoreText; // ���� ǥ��
    public TextMeshProUGUI livesText; // ���� ���� Ƚ�� ǥ��
    public TextMeshProUGUI timerText; // Ÿ�̸� ǥ��
    public float timeLimit = 30f; // ���� �ð� (��)
    public Button retryButton; // ����� ��ư

    private int score = 0;
    private int missCount = 0;
    private float remainingTime;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        remainingTime = timeLimit; // �ʱ� �ð� ����

        // Score Text: ���� �Ʒ� ��ġ
        scoreText.rectTransform.anchorMin = new Vector2(0, 0); // ���� �Ʒ�
        scoreText.rectTransform.anchorMax = new Vector2(0, 0); // ���� �Ʒ�
        scoreText.rectTransform.pivot = new Vector2(0, 0);     // ���� �Ʒ� ����
        scoreText.rectTransform.anchoredPosition = new Vector2(10, 10); // ȭ�� ��迡�� 10px ������

        // Lives Text: ������ �Ʒ� ��ġ
        livesText.rectTransform.anchorMin = new Vector2(1, 0); // ������ �Ʒ�
        livesText.rectTransform.anchorMax = new Vector2(1, 0); // ������ �Ʒ�
        livesText.rectTransform.pivot = new Vector2(1, 0);     // ������ �Ʒ� ����
        livesText.rectTransform.anchoredPosition = new Vector2(-10, 10); // ȭ�� ��迡�� 10px ������

        // Timer Text: ȭ�� �߾� ��ġ
        timerText.rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        timerText.rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        timerText.rectTransform.pivot = new Vector2(0.5f, 0.5f); // �߽� ����
        timerText.rectTransform.anchoredPosition = new Vector2(0, 0); // �߾ӿ� ��ġ

        // Retry Button �ʱ� ���� (��Ȱ��ȭ �� ȭ�� �߾� ��ġ)
        retryButton.gameObject.SetActive(false); // �ʱ� ���¿��� ��Ȱ��ȭ
        retryButton.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 0.5f);
        retryButton.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 0.5f);
        retryButton.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
        retryButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -50); // Ÿ�̸� �Ʒ��� ��ġ

        retryButton.onClick.AddListener(RestartGame); // ��ư Ŭ�� �̺�Ʈ �߰�

        UpdateUI();
        StartTimer();
    }

    private void Update()
    {
        UpdateTimer();
    }

    public void OnObjectHit()
    {
        score++;
        UpdateUI();
    }

    public void OnObjectMissed()
    {
        missCount++;
        if (missCount >= maxMissCount)
        {
            timerText.text = "Game Over!";
            GameOver();
        }
        UpdateUI();
    }

    private void UpdateUI()
    {
        scoreText.text = $"Score: {score}";
        livesText.text = $"Lives: {maxMissCount - missCount}";
    }

    private void UpdateTimer()
    {
        if (remainingTime > 0)
        {
            remainingTime -= Time.deltaTime;
            timerText.text = $"Time: {Mathf.CeilToInt(remainingTime)}";

            if (remainingTime <= 0)
            {
                timerText.text = "Clear!";
                Clear();
            }
        }
    }

    private void GameOver()
    {
        Debug.Log("Game Over!");
        Time.timeScale = 0f; // ���� �Ͻ�����

        // Retry Button Ȱ��ȭ
        retryButton.gameObject.SetActive(true);
    }

    private void Clear()
    {
        Debug.Log("Clear!");
        Time.timeScale = 0f;
    }

    private void StartTimer()
    {
        Time.timeScale = 1f; // Ÿ�̸� ����
    }

    private void RestartGame()
    {
        SceneManager.LoadScene("CatDropGame");
    }
}
