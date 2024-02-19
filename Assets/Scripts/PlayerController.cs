using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

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

    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isGameFinished && startGame)
        {
            transform.Translate(Vector3.forward * Time.deltaTime * speed);
            transform.position = new Vector3(0, transform.position.y, transform.position.z);

            if (Input.GetMouseButtonDown(0))
            {
                ChangeScale();
            }
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
        if(collision.gameObject.CompareTag("Wall"))
        {
            InstantiateParticle(death);
            isGameFinished = true;
            Invoke("RestartGame", 3f);
        }

        //Extra.
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
            Invoke("RestartGame", 5f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(Tags.TURN_LEFT_TAG))
        {
            transform.rotation = Quaternion.Euler(transform.rotation.x, ConstantValues.PLAYER_TURN_LEFT_Y_ASIX_VALUE, transform.rotation.z);
        }
    }

    //Vector3 RandomForce()
    //{
    //    return Vector3.up * Random.Range(minSpeed, maxSpeed);
    //}

    float RandomTorque()
    {
        return Random.Range(-maxTorque, maxTorque);
    }

    private void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void StartGame()
    {
        startGame = true;
        playButton.gameObject.SetActive(false);
    }

    void InstantiateParticle(ParticleSystem PS)
    {
        ParticleSystem newPS = Instantiate(PS, transform.position, PS.transform.rotation);
        newPS.transform.SetParent(transform);
        newPS.Play();
    }

    
}
