using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float minSpeed = 5f;
    public float maxSpeed = 7f;
    public float maxTorque = 10f;

    private float speed = 5;

    private Vector3 newPlayerScale = new Vector3(1, 1, 1);

    private bool readyForScaleChange = false;

    private bool isGameFinished = false;

    private Rigidbody playerRb;

    private bool startGame = false;

    public GameObject playButton;

    public ParticleSystem win;
    public ParticleSystem death;
    public ParticleSystem scaleChangeFX;

    private Transform platformTransform;  // Reference to the rotating platform

    private Vector2 startTouchPosition;
    private Vector2 currentPosition;
    private bool stopTouch = false;
    public float swipeRange = 50f;
    public float tapRange = 10f;

    public float jumpForce = 5f;

    public string nextScene = "Level1";
    public int currentLevelNumber = 1;

    public float speedIncreasePerLevel = 0.2f; // Adjust this value as per game design

    private AdManager adManager;

    public GameObject continuePanel;

    public GameObject tapInstructionText;
    public GameObject tapInstructionButton;

    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        adManager = GameObject.Find("Ad Manager").GetComponent<AdManager>();
        platformTransform = GameObject.FindWithTag(ConstStrings.PLATFORM_TAG).transform;  // Adjust the tag as per your platform tag

        CalculateSpeedForLevel();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isGameFinished && startGame)
        {
            // Move with the platform's rotation
            Vector3 newPosition = new Vector3(platformTransform.position.x, transform.position.y, transform.position.z);
            transform.position = newPosition;

            // Rotate the cube with the platform's rotation
            transform.rotation = Quaternion.Euler(transform.rotation.x, platformTransform.rotation.eulerAngles.y, transform.rotation.z);

            // Move the cube forward
            transform.Translate(Vector3.forward * Time.deltaTime * speed);

            HandleInput();

            CheckForFalling();
        }
    }

    void CalculateSpeedForLevel()
    {
        speed = minSpeed + (currentLevelNumber - 1) * speedIncreasePerLevel;

        // Clamp the speed to not exceed maxSpeed
        speed = Mathf.Min(speed, maxSpeed);
    }

    private void HandleInput()
    {
        // PC Controls
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Input.mousePosition;
            ChangeScale();
        }
        if(Input.GetMouseButtonDown(1))
        {
            Jump();
        }

        // Mobile Controls
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                startTouchPosition = touch.position;
                currentPosition = touch.position;
                stopTouch = false;
            }
            else if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
            {
                currentPosition = touch.position;
                Vector2 distance = currentPosition - startTouchPosition;

                if (!stopTouch)
                {
                    if (distance.y > swipeRange)
                    {
                        stopTouch = true;
                        Jump();
                    }
                    else if (distance.magnitude < tapRange)
                    {
                        stopTouch = true;
                        ChangeScale();
                    }
                }
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                stopTouch = true;
            }
        }
    }

    private void Jump()
    {
        // Add jump logic here
        if (playerRb != null)
        {
            playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    //Game Ending Point.
    private void CheckForFalling()
    {
        if(transform.position.y < -5f)
        {
            adManager.ShowInterstitialAd();
            Invoke("RestartGame", 3);
        }
    }

    private void ChangeScale()
    {
        if (readyForScaleChange)
        {
            transform.localScale = newPlayerScale;
            readyForScaleChange = false;
            InstantiateParticle(scaleChangeFX);
        }
    }

    public void GetNewScale(Vector3 newScale)
    {
        newPlayerScale = newScale;
        readyForScaleChange = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(ConstStrings.WALL_TAG))
        {
            //Fix the setactive not working correctly. not all children of OBs are deactivating.
            if (collision.gameObject.transform.parent != null)
            {
                collision.gameObject.transform.parent.gameObject.SetActive(false);
            }
            InstantiateParticle(death);
            startGame = false;
            continuePanel.gameObject.SetActive(true);
            //adManager.ShowRewardedAd();
        }


        // Extra.
        if (collision.gameObject.CompareTag("Finish"))
        {
            InstantiateParticle(win);
            isGameFinished = true;
            collision.gameObject.SetActive(false);

            playerRb.useGravity = false;
            playerRb.mass = 1;
            transform.localScale = new Vector3(1, 1, 1);
            transform.position = new Vector3(transform.position.x, 3f, transform.position.z);
            playerRb.AddTorque(RandomTorque(), RandomTorque(), RandomTorque(), ForceMode.Impulse);
            //Add condition if it is last level.
            PlayerPrefs.SetString(ConstStrings.CURRENT_SCENE_STRING, nextScene);
            adManager.ShowInterstitialAd();
            Invoke("NextScene", 5f);

            
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag(ConstStrings.INSTRUCTION_TARRIGER_TAG))
        {
            startGame = false;
            print("Inside");
            tapInstructionText.SetActive(true);
            tapInstructionButton.SetActive(true);
            other.gameObject.SetActive(false);
        }
    }

    public void ContinueGameAfterTap()
    {
        ChangeScale();
        tapInstructionText.SetActive(false);
        tapInstructionButton.SetActive(false);
        startGame = true;
    }

    public void ContinueGameAfterSwipeUp()
    {
        Jump();
        tapInstructionText.SetActive(false);
        tapInstructionButton.SetActive(false);
        startGame = true;
    }

    public void SkipRewardedAdAndEndLevel()
    {
        isGameFinished = true;
        continuePanel.gameObject.SetActive(false);
        Invoke("RestartGame", 3f);
    }

    float RandomTorque()
    {
        return UnityEngine.Random.Range(-maxTorque, maxTorque);
    }

    private void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void StartGame()
    {
        continuePanel.gameObject.SetActive(false);
        startGame = true;
        playButton.gameObject.SetActive(false);
    }

    void InstantiateParticle(ParticleSystem PS)
    {
        ParticleSystem newPS = Instantiate(PS, transform.position, PS.transform.rotation);
        newPS.transform.SetParent(transform);
        newPS.Play();
    }

    void NextScene()
    {
        SceneManager.LoadScene(nextScene);
    }
}
