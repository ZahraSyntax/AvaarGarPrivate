using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Ball : MonoBehaviour
{
    public Rigidbody rb;
    public float startSpeed = 40f;

    private Transform _arrow;

    private bool _ballMoving;

    private Transform _startPosition;

    private List<GameObject> _pins = new();

    private readonly Dictionary<GameObject, Transform> _pinsDefaultTransform = new();

    public int Point { get; set; }

    [SerializeField] private Animator cameraAnim;

    private TextMeshProUGUI feedBack;

    private Vector2 _touchStartPos;
    private Vector2 _touchEndPos;

    private void Start()
    {
        Application.targetFrameRate = 60;

        _arrow = GameObject.FindGameObjectWithTag("Arrow").transform;

        rb = GetComponent<Rigidbody>();

        _startPosition = transform;

        _pins = new List<GameObject>();
        _pins.AddRange(GameObject.FindGameObjectsWithTag("Pin1"));
        _pins.AddRange(GameObject.FindGameObjectsWithTag("Pin2"));
        _pins.AddRange(GameObject.FindGameObjectsWithTag("Pin3"));
        _pins.AddRange(GameObject.FindGameObjectsWithTag("Pin4"));
        _pins.AddRange(GameObject.FindGameObjectsWithTag("Pin5"));

        foreach (var pin in _pins)
        {
            _pinsDefaultTransform.Add(pin, pin.transform);
        }

        feedBack = GameObject.FindGameObjectWithTag("FeedBack").GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        if (_ballMoving)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(Shoot());
        }

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                _touchStartPos = touch.position;
            }

            if (touch.phase == TouchPhase.Ended)
            {
                _touchEndPos = touch.position;

                Vector2 direction = _touchEndPos - _touchStartPos;
                if (direction.magnitude > 50f)
                {
                    StartCoroutine(Shoot());
                }
            }
        }
    }

    private IEnumerator Shoot()
    {
        cameraAnim.SetTrigger("Go");
        cameraAnim.SetFloat("CameraSpeed", _arrow.transform.localScale.z);
        _ballMoving = true;
        _arrow.gameObject.SetActive(false);
        rb.isKinematic = false;

        Vector3 forceVector = _arrow.forward * (startSpeed * _arrow.transform.localScale.z);

        Vector3 forcePosition = transform.position + (transform.right * 0.5f);

        rb.AddForceAtPosition(forceVector, forcePosition, ForceMode.Impulse);

        yield return new WaitForSecondsRealtime(7);

        _ballMoving = false;

        GenerateFeedBack();

        yield return new WaitForSecondsRealtime(2);

        ResetGame();
    }

private static void ResetGame()
{
    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    
    int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
    PlayerPrefs.SetInt("Level" + currentSceneIndex + "Score", 0);
}


    public void GenerateFeedBack()
    {
        int countPin1 = _pins.Count(pin => pin.CompareTag("Pin1") && IsPinFallen(pin));
        int countPin2 = _pins.Count(pin => pin.CompareTag("Pin2") && IsPinFallen(pin));
        int countPin3 = _pins.Count(pin => pin.CompareTag("Pin3") && IsPinFallen(pin));
        int countPin4 = _pins.Count(pin => pin.CompareTag("Pin4") && IsPinFallen(pin));
        int countPin5 = _pins.Count(pin => pin.CompareTag("Pin5") && IsPinFallen(pin));

        int totalFallenPins = countPin1 + countPin2 + countPin3 + countPin4 + countPin5;

        feedBack.text = totalFallenPins switch
        {
            0 => "Nothing!",
            > 0 and < 3 => "You are learning Now!",
            >= 3 and < 6 => "It was close!",
            >= 6 and < 10 => "You are a master!",
            _ => "Perfect! Level Complited"
        };

        feedBack.GetComponent<Animator>().SetTrigger("Show");
    }

    private bool IsPinFallen(GameObject pin)
    {
        return Mathf.Abs(pin.transform.rotation.eulerAngles.x) > 45f || Mathf.Abs(pin.transform.rotation.eulerAngles.z) > 45f;
    }
}
