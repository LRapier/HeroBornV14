using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBasic : MonoBehaviour
{
    [SerializeField] AudioClip[] _clips;
    private int clipIndex;
    public Transform player;
    public Transform patrolRoute;
    public List<Transform> locations;
    private int locationIndex = 0;
    private NavMeshAgent agent;
    private bool detected;
    public bool obsvDetected;
    private int _lives = 3;
    public int EnemyLives
    {
        get { return _lives; }
        private set
        {
            _lives = value;
            if (_lives <= 0)
            {
                clipIndex = 1;
                AudioClip clip = _clips[clipIndex];
                GetComponent<AudioSource>().PlayOneShot(clip);
                Destroy(this.gameObject);
                Debug.Log("Enemy down.");
            }
        }
    }
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.Find("Player").transform;
        InitializePatrolRoute();
        MoveToNextPatrolLocation();
    }
    void Update()
    {
        if (detected)
        {
            agent.destination = player.position;
        }
        else if (obsvDetected)
        {
            agent.destination = player.position;
        }
        else
        {
            if (agent.remainingDistance < 0.2f && !agent.pathPending)
            {
                MoveToNextPatrolLocation();
            }
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
    void InitializePatrolRoute()
    {
        foreach (Transform child in patrolRoute)
        {
            locations.Add(child);
        }
    }
    void MoveToNextPatrolLocation()
    {
        if (locations.Count == 0)
            return;
        agent.destination = locations[locationIndex].position;
        locationIndex = (locationIndex + 1) % locations.Count;
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player")
        {
            agent.destination = player.position;
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
