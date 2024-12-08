using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.ARFoundation;

public class DualImageTrackingRevise : MonoBehaviour {
    public GameObject[] prefabMap; // ���� ���� Prefab�� �����ϴ� �迭
    public Dictionary<string, GameObject> imageToPrefabMap = new Dictionary<string, GameObject>(); // �̹��� �̸��� �����ϴ� Prefab ����

    public string[] targetImageNames; // Ʈ��ŷ�� �̹������� �̸�
    private ARTrackedImageManager trackedImageManager;
    // ������ GameObject ����
    private Dictionary<string, GameObject> instantiatedObjects = new Dictionary<string, GameObject>();

    // Variable to store the audio source component
    private AudioSource audioSource;
    // Audio clip that stores the click sound
    public AudioClip clickSound;

    void Start()
    {
        // Get the AudioSource component from the game object this script is attached to.
        audioSource = GetComponent<AudioSource>();
    }


    void Awake() {
        // ARTrackedImageManager ������Ʈ ��������
        trackedImageManager = GetComponent<ARTrackedImageManager>();

        // �̹��� �̸��� �����ϴ� Prefab ���� ����
        if (targetImageNames.Length == prefabMap.Length) {
            for (int i = 0; i < targetImageNames.Length; i++) {
                imageToPrefabMap[targetImageNames[i]] = prefabMap[i];
            }
        } else {
            Debug.LogError("Image names and prefab array size mismatch!");
        }
    }

    private void Update()
    {
        if (Input.touchCount > 0)
        {
            // About the first touch
            Touch touch = Input.GetTouch(0);

            // The state of the touch is Began (touch started state), and a raycast is fired from the location where the touch occurred.
            if (touch.phase == TouchPhase.Began)
            {
                // Fires a ray from the camera to the touched location.
                Ray ray = Camera.main.ScreenPointToRay(touch.position);
                RaycastHit hit;

                // Raycast checks for collided objects
                if (Physics.Raycast(ray, out hit))
                {
                    // If the collided object is a created prefab, the object is processed.
                    GameObject touchedObject = hit.collider.gameObject;

                    // When you touch the Chito prefabricated structure, a sound is output and then switches to "stage2".
                    if (touchedObject.CompareTag("Cat"))
                    {
                        if (clickSound != null)
                        {
                            audioSource.PlayOneShot(clickSound);
                            SceneManager.LoadScene("CatDropGame");
                        }

                    }
                    // When you touch the Flame prefabricated structure, a sound is output and then switches to "stage1".
                    if (touchedObject.CompareTag("Flame"))
                    {
                        if (clickSound != null)
                        {
                            audioSource.PlayOneShot(clickSound);
                            SceneManager.LoadScene("stage1");
                        }

                    }
                    // When you touch the Final prefabricated structure, a sound is output and then switches to "stage3".
                    if (touchedObject.CompareTag("Final"))
                    {
                        if (clickSound != null)
                        {
                            audioSource.PlayOneShot(clickSound);
                            SceneManager.LoadScene("stage3");
                        }

                    }

                }
            }
        }
    }

    void OnEnable() {
        // Ʈ��ŷ �̺�Ʈ ���
        trackedImageManager.trackedImagesChanged += OnTrackedImagesChanged;
    }

    void OnDisable() {
        // Ʈ��ŷ �̺�Ʈ ����
        trackedImageManager.trackedImagesChanged -= OnTrackedImagesChanged;
    }

    private void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs) {
        // ���� �νĵ� �̹��� ó��
        foreach (var trackedImage in eventArgs.added) {
            HandleTrackedImage(trackedImage);
        }

        // ������Ʈ�� �̹��� ó��
        foreach (var trackedImage in eventArgs.updated) {
            HandleTrackedImage(trackedImage);
        }
    }

    private void HandleTrackedImage(ARTrackedImage trackedImage) {
        // �̹��� �̸��� �´� Prefab�� ã��
        if (imageToPrefabMap.ContainsKey(trackedImage.referenceImage.name)) {
            GameObject prefab = imageToPrefabMap[trackedImage.referenceImage.name];
            GameObject instantiatedObject;

            // Ʈ��ŷ ���¿� ���� ��ü ����/������Ʈ
            if (trackedImage.trackingState == UnityEngine.XR.ARSubsystems.TrackingState.Tracking) {
                if (!instantiatedObjects.ContainsKey(trackedImage.referenceImage.name)) {
                    // ���ο� �̹����� Ʈ��ŷ�Ǿ��� �� ��ü ����
                    instantiatedObject = Instantiate(prefab, trackedImage.transform.position, trackedImage.transform.rotation);
                    instantiatedObject.transform.parent = trackedImage.transform; // �̹����� �Բ� �����̵��� ����
                    instantiatedObjects[trackedImage.referenceImage.name] = instantiatedObject;
                } else {
                    // ��ü�� �̹� �����Ѵٸ� ��ġ�� ȸ�� ������Ʈ
                    instantiatedObject = instantiatedObjects[trackedImage.referenceImage.name];
                    instantiatedObject.transform.position = trackedImage.transform.position;
                    instantiatedObject.transform.rotation = trackedImage.transform.rotation;
                }
            } else {
                // Ʈ��ŷ ���°� �սǵǾ��� ��� ��ü �����
                if (instantiatedObjects.ContainsKey(trackedImage.referenceImage.name)) {
                    instantiatedObjects[trackedImage.referenceImage.name].SetActive(false);
                }
            }
        }
    }
}