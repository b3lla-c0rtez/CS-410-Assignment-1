using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public float speed = 0;
    public float jumpforce = 8;
    public int numJump = 0;

    public TextMeshProUGUI countText;
    public GameObject winTextObject;
    public Transform respawnPoint;

    private Rigidbody rb;

    private int count;

    private float movementX;
    private float movementY;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        count = 0;
        SetCountText();
        winTextObject.SetActive(false);
    }

    public void Update() 
    {
        if (transform.position.y < -15) {
            Respawn();
        }
    }

    void SetCountText() 
    {
        countText.text = "Count: " + count.ToString();

        if (count >= 26) {
            winTextObject.SetActive(true);
            Respawn();
        }
    }

    void OnMove(InputValue movementValue) 
    {
        Vector2 movementVector = movementValue.Get<Vector2>();
        movementX = movementVector.x;
        movementY = movementVector.y;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 movement = new Vector3(movementX, 0.0f, movementY);
        rb.AddForce(movement*speed);

        if (Input.GetKeyDown(KeyCode.Space) && numJump < 2) {
            Debug.Log("trying to jump");
            rb.AddForce(Vector3.up * jumpforce, ForceMode.Impulse);
            numJump++;
        }
    }

    private void OnTriggerEnter(Collider other) 
    {
        if (other.gameObject.CompareTag("PickUp")) {
            other.gameObject.SetActive(false);
            count = count + 1;
            SetCountText();
        }

        if (other.gameObject.CompareTag("ground")) {
            numJump = 0;
            Debug.Log("resetting jump");
        }
    }

    void Respawn ()
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.Sleep();
        transform.position = respawnPoint.position;
    }
}
