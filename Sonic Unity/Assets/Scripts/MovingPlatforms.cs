using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatforms : MonoBehaviour
{
    [SerializeField]
    private WayPoint waypointPath;

    [SerializeField]
    private float speed;
    private int targetWaypointIndex;
    private Transform previousWaypoint;
    private Transform targetWaypoint;

    private float timeToWaypoint;
    private float elapsedTime;

    /// <summary>
    /// Targets the next waypoint for movement.
    /// </summary>
    void Start()
    {
        TargetNextWaypoint();
    }

    /// <summary>
    /// Called at a fixed rate, updates the object's position along the waypoint path.
    /// </summary>
    void FixedUpdate()
    {
        elapsedTime += Time.deltaTime;

        float elapsedPercentage = elapsedTime / timeToWaypoint;
        elapsedPercentage = Mathf.SmoothStep(0, 1, elapsedPercentage);
        transform.position = Vector3.Lerp(previousWaypoint.position, targetWaypoint.position, elapsedPercentage);

        if (elapsedPercentage >= 1)
        {
            TargetNextWaypoint();
        }
    }
    /// <summary>
    /// Sets the next waypoint as the target for movement.
    /// </summary>
    private void TargetNextWaypoint()
    {
        previousWaypoint = waypointPath.GetWaypoint(targetWaypointIndex);
        targetWaypointIndex = waypointPath.GetNextWaypointIndex(targetWaypointIndex);
        targetWaypoint = waypointPath.GetWaypoint(targetWaypointIndex);

        elapsedTime = 0;

        float distanceToWaypoint = Vector3.Distance(previousWaypoint.position, targetWaypoint.position);
        timeToWaypoint = distanceToWaypoint / speed;
    }
    /// <summary>
    /// Attaches the parent of the colliding object to the current object.
    /// </summary>
    /// <param name="other">The Collider entering the trigger zone.</param>
    private void OnTriggerEnter(Collider other)
    {
        other.transform.parent.transform.SetParent(transform);
    }
    /// <summary>
    /// Triggered when a Collider exits the trigger zone.
    /// Detaches the parent of the colliding object from the current object.
    /// </summary>
    /// <param name="other">The Collider exiting the trigger zone.</param>
    private void OnTriggerExit(Collider other)
    {
        other.transform.parent.transform.SetParent(null);
    }
}
