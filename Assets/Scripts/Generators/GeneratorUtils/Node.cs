using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    Node parent;
    List<Node> children;
    int distance = 0;

    public Node ()
    {
        children = new List<Node>();
    }

    public Node (Node Parent, List<Node> Children)
    {
        parent = Parent;
        distance = parent.distance + 1;

        children = Children;
    }
}
