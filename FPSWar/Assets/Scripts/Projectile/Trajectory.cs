using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trajectory : MonoBehaviour
{
    public int resolution = 10; // Number of points to calculate the trajectory
    public float maxTime = 2f; // Maximum time for the trajectory prediction
    public float timeInterval = 0.1f; // Time interval between each point on the trajectory
    public Vector3 velocity; // Initial velocity of the grenade

    private LineRenderer lineRenderer;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = resolution + 1;
    }

    void Update()
    {
        DrawTrajectory();
    }

    void DrawTrajectory()
    {
        Vector3[] points = new Vector3[resolution + 1];
        float timeStep = maxTime / resolution;

        for (int i = 0; i <= resolution; i++)
        {
            float time = i * timeStep;
            points[i] = CalculatePositionAtTime(time);
        }

        lineRenderer.SetPositions(points);
    }

    Vector3 CalculatePositionAtTime(float time)
    {
        // Calculate position using kinematic equations
        Vector3 gravity = Physics.gravity;
        Vector3 position = transform.position + velocity * time + 0.5f * gravity * time * time;
        return position;
    }
}
