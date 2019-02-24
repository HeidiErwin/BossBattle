using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManage : MonoBehaviour
{
    private int numGridPointsX;
    private int numGridPointsY;
    private Node[,] grid;
    private List<Node> debugPath;

    // Start is called before the first frame update
    void Awake()
    {
        GenerateGrid();
        List<Node> path = BreadthFirstSearch(grid[0, 0], grid[7, 1]);
        Debug.Log("FINAL PATH");
        for (int i = 0; i < path.Count; i++) {
            Debug.Log(path[i]);
        }
        debugPath = path;
    }

    // Update is called once per frame
    void Update()
    {
        DrawGrid();
        DrawPath(debugPath);
    }

    void DrawPath(List<Node> path) {
        for (int i = 0; i < path.Count - 1; i++) {
            Debug.DrawLine(path[i].getPosition(), path[i + 1].getPosition(), Color.blue);
        }
    }

    void DrawGrid()
    {
        for (int i = 0; i < numGridPointsX; i++)
        {
            for (int j = 0; j < numGridPointsY; j++)
            {
                Debug.DrawLine(grid[i, j].getPosition(), grid[i, j].getPosition() + new Vector2(0.1f, 0.1f), Color.white);
                for (int k = 0; k < grid[i, j].neighbors.Count; k++)
                {
                    //Debug.DrawLine(grid[i, j].getPosition(), grid[i, j].neighbors[k].getPosition(), Color.green);
                    Debug.DrawRay(grid[i, j].getPosition(), 
                                  (grid[i, j].neighbors[k].getPosition() - grid[i, j].getPosition()) / 4, 
                                  Color.green);
                }
            }
        }
    }

    void GenerateGrid() {
        float minX = -8;
        float maxX = 8;
        float minY = -5;
        float maxY = 5;
        float xRange = maxX - minX;
        float yRange = maxY - minY;

        float gridDistance = 1.3f;
        numGridPointsX = (int) (xRange / gridDistance);
        numGridPointsY = (int) (yRange / gridDistance);
        grid = new Node[numGridPointsX, numGridPointsY];

        for (int yIndex = 0; yIndex < numGridPointsY; yIndex++)
        {
            for (int xIndex = 0; xIndex < numGridPointsX; xIndex++) {
                grid[xIndex, yIndex] = new Node(xIndex, yIndex, minX + xIndex * gridDistance, minY + yIndex * gridDistance);

                // Add connections between nodes in the graph
                if (xIndex != 0 && IsConnection(grid[xIndex, yIndex], grid[xIndex - 1, yIndex])) {
                    CreateConnection(grid[xIndex, yIndex], grid[xIndex - 1, yIndex]);
                }
                if (yIndex != 0 && IsConnection(grid[xIndex, yIndex], grid[xIndex, yIndex - 1])) {
                    CreateConnection(grid[xIndex, yIndex], grid[xIndex, yIndex - 1]);
                }
                if (yIndex != 0 && xIndex != 0 && IsConnection(grid[xIndex, yIndex], grid[xIndex - 1, yIndex - 1])) {
                    CreateConnection(grid[xIndex, yIndex], grid[xIndex - 1, yIndex - 1]);
                }
                if (yIndex != 0 && xIndex != numGridPointsX - 1 && IsConnection(grid[xIndex, yIndex], grid[xIndex + 1, yIndex - 1]))
                {
                    CreateConnection(grid[xIndex, yIndex], grid[xIndex + 1, yIndex - 1]);
                }
            }
        }
    }

    void CreateConnection(Node node1, Node node2) {
        node1.AddNeighbor(node2);
        node2.AddNeighbor(node1);
    }

    bool IsConnection(Node node1, Node node2) {
        return !Physics2D.Linecast(node1.getPosition(), node2.getPosition(), LayerMask.GetMask("pathobstacle"));
    }


    List<Node> BreadthFirstSearch(Node start, Node end) {
        Queue<Node> frontier = new Queue<Node>();
        HashSet<Node> visited = new HashSet<Node>();
        Node[,] connections = new Node[numGridPointsX, numGridPointsY];

        Debug.Log(start.neighbors);
        frontier.Enqueue(start);
        int i = 0;

        while (true) {
            if (frontier.Count == 0) {
                return new List<Node>();
            }
            Node root = frontier.Dequeue();
            if (root == end) {
                return makePath(connections, start, end);
            }
            Debug.Log("looping through neighbors" + i);
            foreach (Node neighbor in root.neighbors) {
                if (!visited.Contains(neighbor) && !frontier.Contains(neighbor)) {
                    connections[neighbor.xIndex, neighbor.yIndex] = root;
                    Debug.Log("enqueing..." + neighbor + " " + i);
                    frontier.Enqueue(neighbor);
                }
            }
            visited.Add(root);
            i += 1;
        }
    }

    List<Node> makePath(Node[,] connections, Node start, Node end) {
        List<Node> path = new List<Node>();
        Node currNode = end;
        int i = 0;
        Debug.Log(start);
        while (currNode != start) {
            i++;
            if (i > 40) {
                break;
            }
            Debug.Log(currNode);
            path.Add(currNode);
            currNode = connections[currNode.xIndex, currNode.yIndex];
        }
        path.Add(start);
        path.Reverse();
        return path;
    }
}
