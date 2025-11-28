using System.Collections.Generic;
using UnityEngine;

public class SimplePathfinder : MonoBehaviour
{
    public Node startNode;
    public Node targetNode;
    public LineRenderer lineRenderer;

    [System.Obsolete]
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
        }

        if (startNode != null && targetNode != null)
        {
            DrawPath();
        }
    }

    [System.Obsolete]
    void DrawPath()
    {
        List<Node> path = FindPath(startNode, targetNode);

        if (path == null)
        {
            Debug.LogWarning("Brak œcie¿ki miêdzy wêz³ami!");
            lineRenderer.positionCount = 0;
            return;
        }

        lineRenderer.positionCount = path.Count;
        for (int i = 0; i < path.Count; i++)
        {
            lineRenderer.SetPosition(i, path[i].transform.position + Vector3.up * 0.2f);
        }
        // Jeœli masz marker na scenie, znajdŸ go i ruszaj
        PathMarker marker = FindObjectOfType<PathMarker>();
        if (marker != null)
        {
            marker.StartMoving(lineRenderer);
        }

    }

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

        // Odtwarzamy trasê
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
