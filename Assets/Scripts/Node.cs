using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    [Tooltip("S¹siednie wêz³y, z którymi ten node jest po³¹czony")]
    public List<Node> neighbors = new List<Node>();

    private void OnDrawGizmos()
    {
        // Rysuj sam punkt
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position + Vector3.up * 0.1f, 0.1f);

        // Rysuj linie do s¹siadów
        Gizmos.color = Color.green;
        foreach (var neighbor in neighbors)
        {
            if (neighbor != null)
            {
                Gizmos.DrawLine(transform.position + Vector3.up * 0.1f, neighbor.transform.position + Vector3.up * 0.1f);
            }
        }
    }
}
