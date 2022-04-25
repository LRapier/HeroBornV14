using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyObserver : MonoBehaviour
{
    [SerializeField] AudioClip[] _clips;
    private int clipIndex;
    public Transform player;
    public EnemyBasic enemy1;
    public EnemySpeed enemy2;
    private NavMeshAgent agent;
    private bool detected;
    private int _lives = 1;
    public int EnemyLives
    {
        get { return _lives; }
        private set
        {
            _lives = value;
            if (_lives <= 0)
            {
                Destroy(this.gameObject);
                Debug.Log("Enemy down.");
                clipIndex = 1;
                AudioClip clip = _clips[clipIndex];
                GetComponent<AudioSource>().PlayOneShot(clip);
            }
        }
    }
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.Find("Player").transform;
        enemy1 = GameObject.Find("EnemyBasic").GetComponent<EnemyBasic>();
        enemy2 = GameObject.Find("EnemySpeed").GetComponent<EnemySpeed>();
    }
    void Update()
    {
        if (detected)
        {
            enemy1.obsvDetected = true;
            enemy2.obsvDetected = true;
        }
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Bullet(Clone)")
        {
            EnemyLives -= 1;
            Debug.Log("Hit!");
            clipIndex = 0;
            AudioClip clip = _clips[clipIndex];
            GetComponent<AudioSource>().PlayOneShot(clip);
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player")
        {
            detected = true;
            Debug.Log("Player detected - attack!");
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.name == "Player")
        {
            detected = false;
            Debug.Log("Player out of range, resume patrol");
        }
    }
}
