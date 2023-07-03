using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private float speed;
    private Vector3 targetPos;

    private Rigidbody2D rigidBody;
    private Vector2 moveDirection;

    [SerializeField] GameObject routes;
    [SerializeField] Transform[] points;
    private int pointIndex;
    private int pointCount;
    private int direction = 1;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();

        points = new Transform[routes.transform.childCount];
        for(int i = 0; i < points.Length; i++)
        {
            points[i] = routes.transform.GetChild(i).gameObject.transform;
        }
    }

    private void Start()
    {
        pointIndex = 1;
        pointCount = points.Length;
        targetPos = points[1].transform.position;
        DirectionCalculate();
    }

    private void Update()
    {
        if(Vector2.Distance(transform.position, targetPos) < 0.05f)
        {
            NextPoint();
        }
    }

    private void FixedUpdate()
    {
        rigidBody.velocity = moveDirection * speed;
    }

    private void NextPoint()
    {
        transform.position = targetPos;

        if(pointIndex == pointCount - 1)
        {
            direction -= 1;
        }
        if(pointIndex == 0)
        {
            direction += 1;
        }

        pointIndex += direction;
        targetPos = points[pointIndex].transform.position;
        DirectionCalculate();
    }

    private void DirectionCalculate()
    {
        moveDirection = (targetPos - transform.position).normalized;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerController2D>().isOnPlatform = true;
            collision.GetComponent<PlayerController2D>().platformRigidBody = rigidBody;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerController2D>().isOnPlatform = false;
        }
    }
}
