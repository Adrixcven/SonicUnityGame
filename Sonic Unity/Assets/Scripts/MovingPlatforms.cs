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
    /// Inicializa el movimiento de la plataforma al apuntar al siguiente waypoint.
    /// </summary>
    void Start()
    {
        TargetNextWaypoint();
    }
    /// <summary>
    /// Mueve la plataforma entre waypoints de manera suave, actualizando la posición en cada fixed frame.
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
    /// Selecciona el siguiente waypoint para el movimiento de la plataforma.
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
    /// Activa el efecto de "padre" al entrar en el área de colisión.
    /// </summary>
    private void OnTriggerEnter(Collider other)
    {
        other.transform.parent.transform.SetParent(transform);
    }
     /// <summary>
    /// Desactiva el efecto de "padre" al salir del área de colisión.
    /// </summary>
        private void OnTriggerExit(Collider other)
    {
        other.transform.parent.transform.SetParent(null);
    }
}
