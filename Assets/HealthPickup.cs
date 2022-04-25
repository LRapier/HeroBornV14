using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    [SerializeField] AudioClip[] _clips;
    private int clipIndex;
    public GameBehavior gameManager;
    public int heal = 2;
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameBehavior>();
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Player" || collision.gameObject.name == "DisguisedPlayer")
        {
            Destroy(this.transform.parent.gameObject);
            Debug.Log("Health Pickup collected!");
            GetComponent<AudioSource>().Play();
            gameManager.HP += heal;
        }
    }
}
