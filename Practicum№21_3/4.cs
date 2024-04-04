using System;
using System.Collections.Generic;
using System.IO;

public class Node
{
    public int Data;
    public Node Left, Right;

    public Node(int item)
    {
        Data = item;
        Left = Right = null;
    }
}

public class BinarySearchTree
{
    private Node root;

    public BinarySearchTree()
    {
        root = null;
    }

    // Вставка нового узла в дерево
    private Node Insert(Node node, int data)
    {
        if (node == null)
        {
            node = new Node(data);
            return node;
        }

        if (data < node.Data)
            node.Left = Insert(node.Left, data);
        else if (data > node.Data)
            node.Right = Insert(node.Right, data);

        return node;
    }

    public void Insert(int data)
    {
        root = Insert(root, data);
    }

    // Удаление узла из дерева
    private Node DeleteNode(Node root, int key)
    {
        if (root == null)
            return root;

        if (key < root.Data)
            root.Left = DeleteNode(root.Left, key);
        else if (key > root.Data)
            root.Right = DeleteNode(root.Right, key);
        else
        {
            if (root.Left == null)
                return root.Right;
            else if (root.Right == null)
                return root.Left;

            root.Data = MinValue(root.Right);
            root.Right = DeleteNode(root.Right, root.Data);
        }
        return root;
    }

    private int MinValue(Node root)
    {
        int minv = root.Data;
        while (root.Left != null)
        {
            minv = root.Left.Data;
            root = root.Left;
        }
        return minv;
    }

    public void Delete(int key)
    {
        root = DeleteNode(root, key);
    }

    // Проверка, является ли дерево бинарным поиском
    private bool IsBSTUtil(Node node, int min, int max)
    {
        if (node == null)
            return true;

        if (node.Data < min || node.Data > max)
            return false;

        return IsBSTUtil(node.Left, min, node.Data - 1) && IsBSTUtil(node.Right, node.Data + 1, max);
    }

    public bool IsBST()
    {
        return IsBSTUtil(root, int.MinValue, int.MaxValue);
    }

    // Высота дерева
    private int Height(Node node)
    {
        if (node == null)
            return 0;
        else
        {
            int leftHeight = Height(node.Left);
            int rightHeight = Height(node.Right);

            return Math.Max(leftHeight, rightHeight) + 1;
        }
    }

    // Проверка, является ли дерево сбалансированным
    public bool IsBalanced()
    {
        int lh;
        int rh;

        if (root == null)
            return true;

        lh = Height(root.Left);
        rh = Height(root.Right);

        if (Math.Abs(lh - rh) <= 1 && IsBalanced(root.Left) && IsBalanced(root.Right))
            return true;

        return false;
    }

    private bool IsBalanced(Node root)
    {
        int lh;
        int rh;

        if (root == null)
            return true;

        lh = Height(root.Left);
        rh = Height(root.Right);

        if (Math.Abs(lh - rh) <= 1 && IsBalanced(root.Left) && IsBalanced(root.Right))
            return true;

        return false;
    }

    // Проверка возможности удаления одного узла и сбалансированности дерева после удаления
    public void CheckRemovalAndBalance()
    {
        List<int> keys = new List<int>();

        // Считывание последовательности целых чисел из файла
        try
        {
            using (StreamReader sr = new StreamReader("input.txt"))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    int key;
                    if (int.TryParse(line, out key))
                        keys.Add(key);
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("Ошибка при чтении файла: " + e.Message);
        }

        // Проверка возможности удаления узла и сбалансированности дерева после удаления
        foreach (int key in keys)
        {
            BinarySearchTree tempTree = new BinarySearchTree();

            // Построение дерева без одного узла
            foreach (int k in keys)
            {
                if (k != key)
                    tempTree.Insert(k);
            }

            // Проверка на то, что дерево без узла остается деревом бинарного поиска
            if (tempTree.IsBST())
            {
                // Проверка на то, что дерево становится идеально сбалансированным после удаления узла
                if (tempTree.IsBalanced())
                {
                    Console.WriteLine("Узел " + key + " может быть удален, чтобы дерево стало идеально сбалансированным.");
                    return;
                }
            }
        }

        Console.WriteLine("Невозможно найти узел для удаления, чтобы дерево стало идеально сбалансированным.");
    }
}

class Program
{
    static void Main(string[] args)
    {
        BinarySearchTree bst = new BinarySearchTree();
        bst.CheckRemovalAndBalance();
    }
}
