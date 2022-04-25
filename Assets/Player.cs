using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] AudioClip[] _clips;
    private int clipIndex;
    public float moveSpeed = 10f;
    public float rotateSpeed = 150f;
    public float jumpVelocity = 5f;
    public float distanceToGround = 0.1f;
    public LayerMask groundLayer;
    public GameObject bullet;
    public float bulletSpeed = 100f;
    public float speedMultiplier;
    private float vInput;
    private float hInput;
    private Rigidbody _rb;
    private CapsuleCollider _col;
    private bool doJump = false;
    private bool doShoot = false;
    public GameBehavior _gameManager;
    public delegate void JumpingEvent();
    public event JumpingEvent playerJump;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _col = GetComponent<CapsuleCollider>();
        _gameManager = GameObject.Find("GameManager").GetComponent<GameBehavior>();
    }
    void Update()
    {
        vInput = Input.GetAxis("Vertical") * moveSpeed;
        hInput = Input.GetAxis("Horizontal") * rotateSpeed;
        this.transform.Translate(Vector3.forward * vInput * Time.deltaTime);
        this.transform.Rotate(Vector3.up * hInput * Time.deltaTime);
        
        if (IsGrounded() && Input.GetKeyDown(KeyCode.Space))
        {
            doJump = true;
        }
        if (Input.GetMouseButtonDown(0))
        {
            if(_gameManager.ammo > 0)
            {
                doShoot = true;
            }
        }
    }
    void FixedUpdate()
    {
        Vector3 rotation = Vector3.up * hInput;
        Quaternion angleRot = Quaternion.Euler(rotation * Time.fixedDeltaTime);
        _rb.MovePosition(this.transform.position + this.transform.forward * vInput * Time.fixedDeltaTime);
        _rb.MoveRotation(_rb.rotation * angleRot);
        if(doJump)
        {
            _rb.AddForce(Vector3.up * jumpVelocity, ForceMode.Impulse);
            playerJump();
            doJump = false;
            clipIndex = 0;
            AudioClip clip = _clips[clipIndex];
            GetComponent<AudioSource>().PlayOneShot(clip);
        }
        if(doShoot)
        {
            GameObject newBullet = Instantiate(bullet, this.transform.position + new Vector3(1, 0, 0), this.transform.rotation) as GameObject;
            Rigidbody bulletRB = newBullet.GetComponent<Rigidbody>(); 
            bulletRB.velocity = this.transform.forward * bulletSpeed;
            _gameManager.ammo -= 1;
            doShoot = false;
            clipIndex = 1;
            AudioClip clip = _clips[clipIndex];
            GetComponent<AudioSource>().PlayOneShot(clip);
        }
    }
    private bool IsGrounded()
    {
        Vector3 capsuleBottom = new Vector3(_col.bounds.center.x, _col.bounds.min.y, _col.bounds.center.z);
        bool grounded = Physics.CheckCapsule(_col.bounds.center, capsuleBottom, distanceToGround, groundLayer, QueryTriggerInteraction.Ignore);
        return grounded;
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "EnemyBasic" || collision.gameObject.name == "EnemySpeed")
        {
            _gameManager.HP -= 1;
            clipIndex = 2;
            AudioClip clip = _clips[clipIndex];
            GetComponent<AudioSource>().PlayOneShot(clip);
        }
        if (collision.gameObject.name == "Health_Pickup")
        {
            clipIndex = 7;
            AudioClip clip = _clips[clipIndex];
            GetComponent<AudioSource>().PlayOneShot(clip);
        }
        if (collision.gameObject.name == "Ammo_Pickup")
        {
            clipIndex = 8;
            AudioClip clip = _clips[clipIndex];
            GetComponent<AudioSource>().PlayOneShot(clip);
        }
    }
    public void SpeedBoost(float multiplier, float seconds)
    {
        speedMultiplier = multiplier;
        moveSpeed *= multiplier;
        _gameManager.boosted = "Yes";
        clipIndex = 3;
        AudioClip clip = _clips[clipIndex];
        GetComponent<AudioSource>().PlayOneShot(clip);
        Invoke("EndSpeedBoost", seconds);
    }
    private void EndSpeedBoost()
    {
        moveSpeed /= speedMultiplier;
        _gameManager.boosted = "No";
        clipIndex = 4;
        AudioClip clip = _clips[clipIndex];
        GetComponent<AudioSource>().PlayOneShot(clip);
        Debug.Log("Speed boost end");
    }
    public void Disguise(float seconds)
    {
        GetComponent<Renderer>().material.SetColor("_Color", Color.red);
        _gameManager.disguised = "Yes";
        name = "DisguisedPlayer";
        clipIndex = 5;
        AudioClip clip = _clips[clipIndex];
        GetComponent<AudioSource>().PlayOneShot(clip);
        Invoke("EndDisguise", seconds);
    }
    private void EndDisguise()
    {
        GetComponent<Renderer>().material.SetColor("_Color", Color.green);
        name = "Player";
        _gameManager.disguised = "No";
        clipIndex = 6;
        AudioClip clip = _clips[clipIndex];
        GetComponent<AudioSource>().PlayOneShot(clip);
        Debug.Log("Disguise end");
    }
}
