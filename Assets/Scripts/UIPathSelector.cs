using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPathSelector : MonoBehaviour
{
    public Dropdown startDropdown;
    public Dropdown endDropdown;
    public SimplePathfinder pathfinder;

    [HideInInspector] public bool WaitingForCustomStart = false;

    private Vector3 customStartPoint;
    private List<Node> allNodes = new List<Node>();
    private Node chosenStart;
    private Node chosenEnd;
    private Node lastCustomNode;


    void Start()
    {
        Debug.Log("UIPathSelector Start()");
        // Pobierz wszystkie node'y
        allNodes.AddRange(FindObjectsOfType<Node>());

        // Odfiltruj "_o"
        allNodes = allNodes.FindAll(n => !n.name.EndsWith("_o"));

        // Posortuj alfabetycznie
        allNodes.Sort((a, b) => a.name.CompareTo(b.name));

        // Przygotuj nazwy do dropdownów
        List<string> nodeNames = new List<string>();

        // CUSTOM tylko dla startu
        nodeNames.Insert(0, "CUSTOM");

        foreach (var node in allNodes)
            nodeNames.Add(node.name);

        startDropdown.ClearOptions();
        startDropdown.AddOptions(nodeNames);

        endDropdown.ClearOptions();
        List<string> endNames = new List<string>();
        foreach (var node in allNodes)
            endNames.Add(node.name);
        endDropdown.AddOptions(endNames);
        // Ustawienie domyślnych wartości z dropdownów
        ChooseStart(startDropdown.value);
        ChooseEnd(endDropdown.value);

    }

    // Wywoływane przez Start Dropdown (OnValueChanged)
    public void ChooseStart(int index)
    {
        if (index == 0)
        {
            WaitingForCustomStart = true;
            Debug.Log("CUSTOM AKTYWNY – czekam na klik...");
        }
        else
        {
            WaitingForCustomStart = false;
            chosenStart = allNodes[index - 1];
            Debug.Log("Wybrano start: " + chosenStart.name);
        }
    }


    // Wywoływane przez End Dropdown (OnValueChanged)
    public void ChooseEnd(int index)
    {
        chosenEnd = allNodes[index];
    }

    // WYWOŁYWANE PRZEZ MapClickHandler (ETAP 4)
    public void SetCustomStart(Vector3 pos)
    {
        customStartPoint = pos;
        WaitingForCustomStart = false;

        Node nearest = FindNearestNode(pos);

        if (nearest == null)
        {
            Debug.LogWarning("Nie znaleziono najbliższego node’a!");
            return;
        }

        chosenStart = nearest;

        Debug.Log("CUSTOM klik: " + pos + " | Start od najbliższego node’a: " + nearest.name);

        // Informujemy pathfindera, że start jest customowy
        pathfinder.SetCustomStartPoint(pos, nearest);
    }

    // ETAP 5 — SZUKANIE NAJBLIŻSZEGO NODE’A
    private Node FindNearestNode(Vector3 point)
    {
        Node best = null;
        float bestDist = Mathf.Infinity;

        foreach (var node in allNodes)
        {
            float d = Vector3.Distance(point, node.transform.position);
            if (d < bestDist)
            {
                bestDist = d;
                best = node;
            }
        }
        return best;
    }

    // Wywoływane przez przycisk "Pokaż trasę"
    [System.Obsolete]
    public void OnShowPathClicked()
    {
        if (chosenStart == null || chosenEnd == null)
        {
            Debug.LogWarning("Nie wybrano startu lub celu!");
            return;
        }

        if (chosenStart == chosenEnd)
        {
            Debug.LogWarning("Start i cel są tym samym punktem!");
            // opcjonalnie: wyczyść starą ścieżkę
            pathfinder.ClearPath();
            return;
        }

        pathfinder.startNode = chosenStart;
        pathfinder.targetNode = chosenEnd;
        pathfinder.DrawPath();
    }


}
