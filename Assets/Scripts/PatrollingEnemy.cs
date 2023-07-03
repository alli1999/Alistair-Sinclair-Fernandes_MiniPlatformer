using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrollingEnemy : MonoBehaviour
{
    private Rigidbody2D rigidBody;
    private Vector3 targetPos;
    private Vector2 moveDirection;

    [SerializeField] private float speed = 4f;

    [SerializeField] GameObject routes;
    [SerializeField] Transform[] points;
    private int pointIndex;
    private int pointCount;
    private int direction = 1;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();

        points = new Transform[routes.transform.childCount];
        for (int i = 0; i < points.Length; i++)
        {
            points[i] = routes.transform.GetChild(i).gameObject.transform;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        pointIndex = 1;
        pointCount = points.Length;
        targetPos = points[1].transform.position;
        DirectionCalculate();
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance(transform.position, targetPos) < 0.15f)
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
        if (pointIndex == pointCount - 1)
        {
            direction -= 1;
        }
        if (pointIndex == 0)
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
}
