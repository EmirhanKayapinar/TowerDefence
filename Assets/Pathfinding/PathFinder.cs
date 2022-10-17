using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinder : MonoBehaviour
{
    [SerializeField] Vector2Int startCoordinates;
    [SerializeField] Vector2Int destinateCoordinates;

    Node startNode;
    Node destinationNode;
    Node currentSearchNode;

    Queue<Node> frontier = new Queue<Node>();
    Dictionary<Vector2Int, Node> reached = new Dictionary<Vector2Int, Node>();

    Vector2Int[] directions = { Vector2Int.right, Vector2Int.left, Vector2Int.up, Vector2Int.down };
    GridManager gridManager;
    Dictionary<Vector2Int, Node> grid;

    private void Awake()
    {
        gridManager = FindObjectOfType<GridManager>();
        if (gridManager!= null)
        {
            grid = gridManager.Grid;
        }

        
    }
    void Start()
    {
        startNode = gridManager.Grid[startCoordinates];
        destinationNode = gridManager.Grid[destinateCoordinates];

        BreadthFirstSearch();
        BuildPath();
    }

    private void ExploreNeighbors()
    {
        List<Node> neighbours = new List<Node>();
        foreach (Vector2Int item in directions)
        {
            Vector2Int neighborCoords = currentSearchNode.coordinates + item;
            if (grid.ContainsKey(neighborCoords))
            {
                neighbours.Add(grid[neighborCoords]);

            }
        }

        foreach (Node item in neighbours)
        {
            if (!reached.ContainsKey(item.coordinates) && item.isWalkable)
            {
                item.connectedTo = currentSearchNode;
                reached.Add(item.coordinates, item);
                frontier.Enqueue(item);
            }
        }
    }

    void BreadthFirstSearch()
    {
        bool isRunning = true;
        frontier.Enqueue(startNode);
        reached.Add(startCoordinates, startNode);

        while (frontier.Count > 0 && isRunning)
        {
            currentSearchNode = frontier.Dequeue();
            currentSearchNode.isExplored = true;
            ExploreNeighbors();

            if (currentSearchNode.coordinates == destinateCoordinates)
            {
                isRunning = false;
            }
        }
    }

    List<Node> BuildPath() 
    {
        List<Node> path = new List<Node>();
        Node currentNode = destinationNode;

        path.Add(currentNode);
        currentNode.isPath = true;

        while (currentNode.connectedTo != null)
        {
            currentNode = currentNode.connectedTo;
            path.Add(currentNode);
            currentNode.isPath = true;
        }

        path.Reverse();
        return path;

    }



}
