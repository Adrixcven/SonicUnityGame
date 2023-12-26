using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPoint : MonoBehaviour
{
    /// <summary>
    /// Retrieves the Transform of a waypoint at a specified index.
    /// </summary>
    /// <param name="waypointIndex">Index of the desired waypoint.</param>
    /// <returns>The Transform of the waypoint at the specified index.</returns>
    public Transform GetWaypoint(int waypointIndex)
    {
        return transform.GetChild(waypointIndex);
    }
    /// <summary>
    /// Calculates and returns the index of the next waypoint based on the current waypoint index.
    /// If the current waypoint is the last in the sequence, wraps around to the first waypoint.
    /// </summary>
    /// <param name="currentWaypointIndex">Index of the current waypoint in the sequence.</param>
    /// <returns>The index of the next waypoint in the sequence.</returns>
    public int GetNextWaypointIndex(int currentWaypointIndex)
    {
        int nextWaypointIndex = currentWaypointIndex + 1;
        if (nextWaypointIndex == transform.childCount)
        {
            nextWaypointIndex = 0;
        }
        return nextWaypointIndex;
    }
}
