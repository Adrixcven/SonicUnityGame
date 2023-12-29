using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPoint : MonoBehaviour
{
    /// <summary>
    /// Obtiene el Transform de un punto de ruta en un índice especificado.
    /// </summary>
    /// <param name="waypointIndex">Índice del punto de ruta deseado.</param>
    /// <returns>El Transform del punto de ruta en el índice especificado.</returns>
    public Transform GetWaypoint(int waypointIndex)
    {
        return transform.GetChild(waypointIndex);
    }
    /// <summary>
    /// Calcula y devuelve el índice del próximo punto de ruta basado en el índice actual.
    /// Si el punto de ruta actual es el último en la secuencia, retrocede al primer punto de ruta.
    /// </summary>
    /// <param name="currentWaypointIndex">Índice del punto de ruta actual en la secuencia.</param>
    /// <returns>El índice del próximo punto de ruta en la secuencia.</returns>
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
