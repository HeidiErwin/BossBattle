using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public int numGridPointsX;
    public int numGridPointsY;
    public Node[,] grid;
    private List<Node> debugPath;

    // Start is called before the first frame update
    void Awake()
    {
        GenerateGrid();
        List<Node> path = Dijkstra(grid[0, 0], grid[7, 1]);
        debugPath = path;
    }

    // Update is called once per frame
    void Update()
    {
        //DrawGrid();
        //DrawPath(debugPath);

    }

    public void DrawPath(List<Node> path) {
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

        for (int y = 0; y < numGridPointsY; y++)
        {
            for (int x = 0; x < numGridPointsX; x++) {
                grid[x, y] = new Node(x, y, minX + x * gridDistance, minY + y * gridDistance);

                // Add connections between nodes in the graph
                if (x != 0 && IsConnection(grid[x, y], grid[x - 1, y])) {
                    CreateConnection(grid[x, y], grid[x - 1, y]);
                }
                if (y != 0 && IsConnection(grid[x, y], grid[x, y - 1])) {
                    CreateConnection(grid[x, y], grid[x, y - 1]);
                }
                if (y != 0 && x != 0 && IsConnection(grid[x, y], grid[x - 1, y - 1])) {
                    CreateConnection(grid[x, y], grid[x - 1, y - 1]);
                }
                if (y != 0 && x != numGridPointsX - 1 && IsConnection(grid[x, y], grid[x + 1, y - 1]))
                {
                    CreateConnection(grid[x, y], grid[x + 1, y - 1]);
                }
            }
        }
    }

    void CreateConnection(Node node1, Node node2) {
        node1.AddNeighbor(node2);
        node2.AddNeighbor(node1);
    }

    bool IsConnection(Node node1, Node node2) {
        float width = 1f;
        Vector2 pos1 = node1.getPosition();
        Vector2 pos2 = node2.getPosition();
        int mask = LayerMask.GetMask("pathobstacle");

        // Check if the direct path is empty
        bool directEmpty = !Physics2D.Linecast(pos1, pos2, mask);

        // Also maintain a width of emptiness between nodes
        Vector2 perp = Vector2.Perpendicular(pos1 - pos2).normalized * width / 2;
        bool leftEmpty = !Physics2D.Linecast(pos1 + perp, pos2 + perp, mask);
        bool rightEmpty = !Physics2D.Linecast(pos1 - perp, pos2 - perp, mask);
        bool diagEmpty = !Physics2D.Linecast(pos1 - perp, pos2 + perp, mask);
        return leftEmpty && rightEmpty && directEmpty && diagEmpty;
    }


    public List<Node> Dijkstra(Node start, Node end) {
        List<Node> frontier = new List<Node>();
        HashSet<Node> visited = new HashSet<Node>();

        float[,] costs = new float[numGridPointsX, numGridPointsY];
        Node[,] connections = new Node[numGridPointsX, numGridPointsY];

        frontier.Add(start);
        costs[start.xIndex, start.yIndex] = 0;
        while (true) {
            if (frontier.Count == 0) {
                return new List<Node>();
            }
            Node root = PopMin(frontier, costs);
            if (root == end) {
                return makePath(connections, start, end);
            }
            foreach (Node neighbor in root.neighbors) {
                if (!visited.Contains(neighbor)) {
                    float cost = costs[root.xIndex, root.yIndex] + (root.getPosition() - neighbor.getPosition()).magnitude;
                    if (frontier.Contains(neighbor)) {
                        if (costs[neighbor.xIndex, neighbor.yIndex] > cost) {
                            costs[neighbor.xIndex, neighbor.yIndex] = cost;
                            connections[neighbor.xIndex, neighbor.yIndex] = root;
                        }
                    } else {
                        frontier.Add(neighbor);
                        costs[neighbor.xIndex, neighbor.yIndex] = cost;
                        connections[neighbor.xIndex, neighbor.yIndex] = root;
                    }

                }
            }
            visited.Add(root);
        }
    }

    public Node PopMin(List<Node> nodes, float[,]costs) {
        float minDistance = 0;
        Node minNode = null;
        foreach (Node node in nodes) {
            if (minNode == null || costs[node.xIndex, node.yIndex] < minDistance) {
                minDistance = costs[node.xIndex, node.yIndex];
                minNode = node;
            }
        }
        nodes.Remove(minNode);
        return minNode;
    }

    public List<Node> FindPath(Vector2 start, Vector2 end) {
        return Dijkstra(GetClosestGrid(start), GetClosestGrid(end));
    }

    List<Node> makePath(Node[,] connections, Node start, Node end) {
        List<Node> path = new List<Node>();
        Node currNode = end;
        int i = 0;
        while (currNode != start) {
            i++;
            if (i > 40) {
                break;
            }
            path.Add(currNode);
            currNode = connections[currNode.xIndex, currNode.yIndex];
        }
        path.Add(start);
        path.Reverse();
        return path;
    }

    public Node GetClosestGrid(Vector2 position)
    {
        float bestDistance = 9999;
        Node bestNode = null;
        for (int xIndex = 0; xIndex < numGridPointsX; xIndex++)
        {
            for (int yIndex = 0; yIndex < numGridPointsY; yIndex++)
            {
                float distance = (grid[xIndex, yIndex].getPosition() - position).magnitude;
                if (distance < bestDistance)
                {
                    bestDistance = distance;
                    bestNode = grid[xIndex, yIndex];
                }
            }
        }
        return bestNode;
    }
}
