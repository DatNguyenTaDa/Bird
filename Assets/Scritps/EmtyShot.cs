using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Windows;

public class EmtyShot : MonoBehaviour
{

    public LineRenderer[] lineRenderers;
    public Transform[] stripPositions;
    public Transform center;
    public Transform idlePosition;

    public Vector3 currentPosition;
    public float maxLeght;
    public float bottomBoudary;

    bool isMouseDown;
    public float birdPositionOffset;
    public float force;

    public GameObject birdPrefab;

    Rigidbody2D bird;
    Collider2D birdCollider;

    public GameObject PointPrefab;
    public GameObject[] Points;
    public int numPoint;


    void Start()
    {
        lineRenderers[0].positionCount = 2;
        lineRenderers[1].positionCount = 2;
        lineRenderers[0].SetPosition(0, stripPositions[0].position);
        lineRenderers[1].SetPosition(0, stripPositions[1].position);

        CreateBird();
        CreatePoints();
    }

    void CreatePoints()
    {
        Points = new GameObject[numPoint];
        for(int i = 0; i<numPoint;i++)
        {
            Points[i] = Instantiate(PointPrefab, transform.position, Quaternion.identity);
        }
    }
    void CreateBird()
    {
        bird = Instantiate(birdPrefab).GetComponent<Rigidbody2D>();
        birdCollider = bird.GetComponent<Collider2D>();
        birdCollider.enabled = false;

        bird.isKinematic = true;
        ResetStrip();
    }

    // Update is called once per frame
    void Update()
    {
        if (isMouseDown)
        {
            Vector3 mousePosition = UnityEngine.Input.mousePosition;
            mousePosition.z = 10;

            currentPosition = Camera.main.ScreenToWorldPoint(mousePosition);
            currentPosition = center.position + Vector3.ClampMagnitude(currentPosition - center.position, maxLeght);

            currentPosition = ClamBoundary(currentPosition);

            SetsStrips(currentPosition);

            if(birdCollider)
            {
                birdCollider.enabled = true;
            }
            for(int i = 0; i<Points.Length; i++)
            {
                Points[i].transform.position = PoinPosition(i*0.1f);
            }
        }
        else
        {
            ResetStrip();

        }
    }

    private void OnMouseDown()
    {
        isMouseDown = true;
    }
    private void OnMouseUp()
    {
        isMouseDown = false;
        Shoot();
        DestroyPoints();
    }
    void DestroyPoints()
    {
        for(int i=0; i<Points.Length;i++)
        {
            Destroy(Points[i], 0.1f);
        }
        CreatePoints();
    }
    void Shoot()
    {
        bird.isKinematic = false;
        Vector3 birdForce = (currentPosition - center.position) * force * -1;
        bird.velocity = birdForce;

        bird = null;
        birdCollider = null;
        Invoke("CreateBird", 2);
    }
    void ResetStrip()
    {
        currentPosition = idlePosition.position;
        SetsStrips(currentPosition);
    }

    void SetsStrips(Vector3 position)
    {
        lineRenderers[0].SetPosition(1, position);
        lineRenderers[1].SetPosition(1, position);

        if (bird)
        {
            Vector3 dir = position - center.position;
            bird.transform.position = position + dir.normalized * birdPositionOffset;
            bird.transform.right = -dir.normalized;
        }

    }

    Vector3 ClamBoundary(Vector3 vector)
    {
        vector.y = Mathf.Clamp(vector.y, bottomBoudary, 1000);
        return vector;
    }
    Vector2 PoinPosition(float t)
    {
        Vector2 currentPP = (Vector2)transform.position + ((Vector2)(currentPosition - center.position) * force * -1 * t) + 0.5f * Physics2D.gravity * (t * t);
        return currentPP;
    }
}
