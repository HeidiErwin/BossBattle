
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public float x;
    public float y;
    public int xIndex;
    public int yIndex;
    public List<Node> neighbors;

    public Node(int xIndex, int yIndex, float x, float y) {
        this.x = x;
        this.y = y;
        this.xIndex = xIndex;
        this.yIndex = yIndex;
        neighbors = new List<Node>();
    }

    public Vector2 getPosition() {
        return new Vector2(x, y);
    }

    public void AddNeighbor(Node neighbor) {
        if (!neighbors.Contains(neighbor)) {
            neighbors.Add(neighbor);
        }
    }

    public override string ToString()
    {
        return this.xIndex.ToString() + ", " + this.yIndex.ToString();
    }
}
