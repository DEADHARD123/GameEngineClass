using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public float speed = 0;
    public TextMeshProUGUI countText;
    public GameObject gameOverScreen;
    public GameObject winTextObject;
    public float scaleFactor = 0.3f;

    private Rigidbody rb;
    private int count;
    private float movementX;
    private float movementY;
    private Vector3 originalScale;
    private GameObject winText;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        count = 0;

        SetCountText();
        gameOverScreen.SetActive(false);
        winText.SetActive(false);

        // Save the original scale
        originalScale = transform.localScale;
    }

    void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();

        movementX = movementVector.x;
        movementY = movementVector.y;
    }

    void SetCountText()
    {
        countText.text = "Count: " + count.ToString();
        if(count >= 12)
        {
            winText.SetActive(true);
        }
    }

    void FixedUpdate()
    {
        Vector3 movement = new Vector3(movementX, 0.0f, movementY);

        rb.AddForce(movement * speed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PickUp"))
        {
            other.gameObject.SetActive(false);
            count = count + 1;

            SetCountText();

            // Apply the scale effect
            transform.localScale += Vector3.one * 0.3f;
        }
        else if (other.CompareTag("Dead"))
        {
            // Show the game over screen
            gameOverScreen.SetActive(true);
        }

        // Disable the win text
        winText.SetActive(false);
    }

    public void ResetScale()
    {
        // Revert to the original scale
        transform.localScale = originalScale;
    }

    public void RestartGame()
    {
        // Reset the count and scale
        count = 0;
        transform.localScale = originalScale;

        // Set the win text and game over screen to inactive
        winText.SetActive(false);
        gameOverScreen.SetActive(false);

        // Reactivate all pick ups
        GameObject[] pickUps = GameObject.FindGameObjectsWithTag("PickUp");
        foreach (GameObject pickUp in pickUps)
        {
            pickUp.SetActive(true);
        }

        // Reset the count text
        SetCountText();
    }
}
