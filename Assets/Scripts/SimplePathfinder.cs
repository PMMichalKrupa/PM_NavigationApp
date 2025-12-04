using System.Collections.Generic;
using UnityEngine;

public class SimplePathfinder : MonoBehaviour
{
    public Node startNode;
    public Node targetNode;
    public LineRenderer lineRenderer;

    // CUSTOM START
    private bool useCustomStart = false;
    private Vector3 customStartPoint;
    private Node customNearestNode;

    void Start()
    {
        if (lineRenderer == null)
        {
            lineRenderer = gameObject.AddComponent<LineRenderer>();
            lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
            lineRenderer.startColor = Color.red;
            lineRenderer.endColor = Color.red;
            lineRenderer.startWidth = 0.1f;
            lineRenderer.endWidth = 0.1f;
            lineRenderer.positionCount = 0;
        }
    }

    public void ClearPath()
    {
        lineRenderer.positionCount = 0;
    }

    // Wywoływane z UI
    public void DrawPath()
    {
        if (targetNode == null)
        {
            Debug.LogWarning("Brak celu!");
            return;
        }

        List<Node> path;

        if (useCustomStart)
        {
            path = FindPath(customNearestNode, targetNode);
        }
        else
        {
            if (startNode == null)
            {
                Debug.LogWarning("Brak startu!");
                return;
            }

            path = FindPath(startNode, targetNode);
        }

        if (path == null || path.Count == 0)
        {
            Debug.LogWarning("Brak ścieżki!");
            lineRenderer.positionCount = 0;
            return;
        }

        // ILOŚĆ PUNKTÓW LINII
        int extra = useCustomStart ? 1 : 0;
        lineRenderer.positionCount = path.Count + extra;

        int index = 0;

        // 🔴 PIERWSZY ODCINEK: custom → najbliższy node
        if (useCustomStart)
        {
            lineRenderer.SetPosition(0, customStartPoint + Vector3.up * 0.2f);
            index = 1;
        }

        // 🔴 RESZTA TRASY PO NODE'ACH
        for (int i = 0; i < path.Count; i++)
        {
            lineRenderer.SetPosition(index + i,
                path[i].transform.position + Vector3.up * 0.2f);
        }

        // Kulka / marker
        PathMarker marker = FindObjectOfType<PathMarker>();
        if (marker != null)
        {
            marker.StartMoving(lineRenderer);
        }

        // RESET CUSTOMA PO NARYSOWANIU
        useCustomStart = false;
    }

    // Ustawiane z UIPathSelector
    public void SetCustomStartPoint(Vector3 point, Node nearest)
    {
        useCustomStart = true;
        customStartPoint = point;
        customNearestNode = nearest;
    }

    // BFS
    List<Node> FindPath(Node start, Node goal)
    {
        Queue<Node> frontier = new Queue<Node>();
        frontier.Enqueue(start);

        Dictionary<Node, Node> cameFrom = new Dictionary<Node, Node>();
        cameFrom[start] = null;

        while (frontier.Count > 0)
        {
            Node current = frontier.Dequeue();

            if (current == goal)
                break;

            foreach (Node next in current.neighbors)
            {
                if (!cameFrom.ContainsKey(next))
                {
                    frontier.Enqueue(next);
                    cameFrom[next] = current;
                }
            }
        }

        if (!cameFrom.ContainsKey(goal))
            return null;

        // Odtwarzanie ścieżki
        List<Node> path = new List<Node>();
        Node currentNode = goal;

        while (currentNode != null)
        {
            path.Insert(0, currentNode);
            cameFrom.TryGetValue(currentNode, out currentNode);
        }

        return path;
    }
}
