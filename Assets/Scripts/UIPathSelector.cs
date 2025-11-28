using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // lub TMPro jeúli uøywasz TMP_Dropdown

public class UIPathSelector : MonoBehaviour
{
    public Dropdown startDropdown;
    public Dropdown endDropdown;
    public SimplePathfinder pathfinder;

    private List<Node> allNodes = new List<Node>();

    [System.Obsolete]
    void Start()
    {
        // Znajdü wszystkie node'y w scenie
        // Pobierz wszystkie node'y w scenie
        allNodes.AddRange(FindObjectsOfType<Node>());

        // Odfiltruj te, ktÛre koÒczπ siÍ na "_o"
        allNodes = allNodes.FindAll(node => !node.name.EndsWith("_o"));

        // Posortuj alfabetycznie, øeby lista by≥a czytelna
        allNodes.Sort((a, b) => a.name.CompareTo(b.name));

        // Przygotuj nazwy do wyúwietlenia
        List<string> nodeNames = new List<string>();
        foreach (var node in allNodes)
        {
            nodeNames.Add(node.name);
        }

        startDropdown.ClearOptions();
        endDropdown.ClearOptions();
        startDropdown.AddOptions(nodeNames);
        endDropdown.AddOptions(nodeNames);


        startDropdown.ClearOptions();
        endDropdown.ClearOptions();
        startDropdown.AddOptions(nodeNames);
        endDropdown.AddOptions(nodeNames);
    }

    public void OnShowPathClicked()
    {
        int startIndex = startDropdown.value;
        int endIndex = endDropdown.value;

        if (startIndex < 0 || endIndex < 0 || startIndex == endIndex)
        {
            Debug.LogWarning("Niepoprawny wybÛr punktÛw!");
            return;
        }

        Node startNode = allNodes[startIndex];
        Node endNode = allNodes[endIndex];

        pathfinder.startNode = startNode;
        pathfinder.targetNode = endNode;

        pathfinder.SendMessage("DrawPath"); // wywo≥aj rysowanie trasy
    }
}
