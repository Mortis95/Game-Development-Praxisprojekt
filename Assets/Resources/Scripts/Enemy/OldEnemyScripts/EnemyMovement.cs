using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public AudioClip enemySound;
    public Sprite moveTop;
    public Sprite moveBot;
    public Sprite moveRight;
    public Sprite moveLeft;
    public Vector2 direction;
    public int directionUnits;
    public int speed;
    public int playerDistance;

    AudioSource audioSource;
    Rigidbody2D rigidbody;
    GameObject player;
    PolygonCollider2D viewArea;
    float savedX;
    float savedY;
    SpriteRenderer actualSprite;
    float[][] watchUp = { new float[] { -2, 2.5f }, new float[] { 2, 2.5f } };
    float[][] watchDown = { new float[] { -2, -2 }, new float[] { 2, -2 } };
    float[][] watchLeft = { new float[] { -2.5f, 2 }, new float[] { -2.5f, -2 } };
    float[][] watchRight = { new float[] { 2.5f, 2 }, new float[] { 2.5f, -2 } };
    bool goingBack;
    bool playerFound;
    int directionCounter;

    void Start()
    {
        actualSprite = GetComponentInChildren<SpriteRenderer>();
        viewArea = GetComponentInChildren<PolygonCollider2D>();
        audioSource = GetComponent<AudioSource>();
        goingBack = false;
        playerFound = false;
        directionCounter = 0;
        rigidbody = GetComponent<Rigidbody2D>();
        savedX = 0;
        savedY = 0;
    }

    void FixedUpdate()
    {
        // change viewarea and moving direction
        if (direction.x > 0) // moves right
        {
            viewArea.SetPath(0, new Vector2[] { new Vector2(0, 0.5f), new Vector2(watchRight[0][0], watchRight[0][1]), new Vector2(watchRight[1][0], watchRight[1][1]) });
            actualSprite.sprite = moveRight;

        }
        else if (direction.x < 0) // moves left
        {
            viewArea.SetPath(0, new Vector2[] { new Vector2(0, 0.5f), new Vector2(watchLeft[0][0], watchLeft[0][1]), new Vector2(watchLeft[1][0], watchLeft[1][1]) });
            actualSprite.sprite = moveLeft;
        }
        else if (direction.y > 0) // moves up
        {
            viewArea.SetPath(0, new Vector2[] { new Vector2(0, 0.5f), new Vector2(watchUp[0][0], watchUp[0][1]), new Vector2(watchUp[1][0], watchUp[1][1]) });
            actualSprite.sprite = moveTop;
        }
        else if (direction.y < 0) // moves down
        {
            viewArea.SetPath(0, new Vector2[] { new Vector2(0, 0.5f), new Vector2(watchDown[0][0], watchDown[0][1]), new Vector2(watchDown[1][0], watchDown[1][1]) });
            actualSprite.sprite = moveBot;
        }


        if (playerFound)
        {
            audioSource.volume = 0.05f;
            audioSource.clip = enemySound;
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }

            player = GameObject.FindWithTag("Player");
            Vector2 playerCoord = new Vector2((player.transform.position - transform.position).normalized.x, (player.transform.position - transform.position).normalized.y);
            rigidbody.velocity = playerCoord * speed;
            if (Vector3.Distance(player.transform.position, rigidbody.transform.position) >= playerDistance)
            {
                playerFound = false;
                goingBack = true;
            }
        }
        else if (goingBack)
        {
            Vector2 goingBackDirection = new Vector2(savedX - rigidbody.position.x, savedY - rigidbody.position.y).normalized * speed;

            rigidbody.velocity = goingBackDirection;
            if (goingBackDirection.x < 1 && goingBackDirection.y < 1)
            {
                goingBack = false;
            }
        }
        else
        {
            if (directionCounter <= directionUnits && !goingBack && !playerFound)
            {
                rigidbody.velocity = direction;
                directionCounter++;
            }
            else
            {
                directionCounter = 0;
                direction.x = -direction.x;
                direction.y = -direction.y;
            }
            savedX = rigidbody.position.x;
            savedY = rigidbody.position.y;
        }
    }

    public void SetPlayerFound(bool playerFound)
    {
        this.playerFound = playerFound;
    }
}
