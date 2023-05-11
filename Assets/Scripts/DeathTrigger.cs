using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathTrigger : MonoBehaviour
{
    public GameObject ghostPrefab;
    public Transform spawnTransform;

    private GameObject player;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void SpawnGhostDelay()
    {
        GameObject newGhost = Instantiate(ghostPrefab);
        newGhost.transform.position = spawnTransform.position;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            InputRecorder.SavePlayerInput();
            player.transform.position = spawnTransform.position;
            player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            Invoke("SpawnGhostDelay", 2f);
        }
        
        if (collision.gameObject.tag == "Ghost")
            Destroy(collision.gameObject);
    }
}
