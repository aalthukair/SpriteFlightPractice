using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    private float elapsedTime = 0f;
    private float score = 0f;
    
    public float scoreMultiplier = 10f;
    public float thrustForce = 1f;
    
    Rigidbody2D rb;
    
    public UIDocument uiDocument;
    private Label scoreText;

    private bool boostersOn = false;
    public GameObject booster1;
    public GameObject booster2;
    
    private Button restartButton;

    public GameObject explosionEffect;

    public GameObject borderParent;

    public PhysicsMaterial2D bouncyMat;
        
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        scoreText = uiDocument.rootVisualElement.Q<Label>("ScoreLabel");
        restartButton = uiDocument.rootVisualElement.Q<Button>("RestartButton");
        restartButton.style.display = DisplayStyle.None;
        restartButton.clicked += ReloadScene;
    }

    void Update()
    {
        UpdatePlayer();
        UpdateScore();
    }

    void UpdateScore()
    {
        elapsedTime += Time.deltaTime;
        score = Mathf.FloorToInt(elapsedTime * scoreMultiplier);
        scoreText.text = "Score: " + score;
    }

    void UpdatePlayer()
    {
        booster1.SetActive(boostersOn);
        booster2.SetActive(boostersOn);
        
        if (Mouse.current.leftButton.isPressed) {
            
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.value);
            Vector2 direction = (mousePos - transform.position).normalized;

            Quaternion targetRotation = Quaternion.FromToRotation(Vector3.up, direction);

            transform.rotation = Quaternion.Lerp(
                transform.rotation,
                targetRotation,
                30 * Time.deltaTime
            );
            rb.AddForce(direction * thrustForce);
            boostersOn = true;
        }
        else
        {
            boostersOn = false;
        }

        float boosterSize = (float)((Math.Sin(Time.fixedTime * 100) / 2) - 1);
        booster1.transform.localScale = new Vector3(0.31f, boosterSize, 1);
        booster2.transform.localScale = new Vector3(0.31f, boosterSize, 1);
        
        bouncyMat.bounciness = (score / 200) + 1;

    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
        Instantiate(explosionEffect, transform.position, transform.rotation);
        borderParent.SetActive(false);
        restartButton.style.display = DisplayStyle.Flex;
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Obstacle")
        {
            Rigidbody2D rbObstacle = collision.gameObject.GetComponent<Rigidbody2D>();
            rbObstacle.AddForce(transform.up * -10);
        }
    }

    void ReloadScene() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}