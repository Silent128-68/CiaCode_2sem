using System;
using System.IO;
using System.Collections.Generic;

public class Node
{
    public int data;
    public Node left, right;

    public Node(int item)
    {
        data = item;
        left = right = null;
    }
}

public class BinaryTree
{
    public Node root;

    public BinaryTree()
    {
        root = null;
    }

    public void Insert(int data)
    {
        root = InsertRec(root, data);
    }

    Node InsertRec(Node root, int data)
    {
        if (root == null)
        {
            root = new Node(data);
            return root;
        }

        if (data < root.data)
            root.left = InsertRec(root.left, data);
        else if (data > root.data)
            root.right = InsertRec(root.right, data);

        return root;
    }

    public void InOrder()
    {
        InOrderRec(root);
    }

    void InOrderRec(Node root)
    {
        if (root != null)
        {
            InOrderRec(root.left);
            Console.Write(root.data + " ");
            InOrderRec(root.right);
        }
    }

    public int CountMax()
    {
        int max = FindMax(); // Находим максимальное значение
        return CountMaxRec(root, max);
    }

    public int FindMax()
    {
        return FindMaxRec(root);
    }

    int FindMaxRec(Node root)
    {
        if (root == null)
            return int.MinValue;

        int leftMax = FindMaxRec(root.left);
        int rightMax = FindMaxRec(root.right);

        int max = Math.Max(root.data, Math.Max(leftMax, rightMax));

        return max;
    }

    int CountMaxRec(Node root, int max)
    {
        if (root == null)
            return 0;

        int count = 0;
        if (root.data == max)
            count++;

        count += CountMaxRec(root.left, max);
        count += CountMaxRec(root.right, max);

        return count;
    }
}

class Program
{
    static void Main()
    {
        // Чтение данных из файла
        int[] numbers;
        using (StreamReader sr = new StreamReader("input.txt"))
        {
            string line = sr.ReadLine();
            string[] tokens = line.Split(' ');
            numbers = Array.ConvertAll(tokens, int.Parse);
        }

        BinaryTree tree = new BinaryTree();

        // Строим дерево
        foreach (int num in numbers)
        {
            tree.Insert(num);
        }

        // Поиск наибольшего значения и их количества
        int max = tree.FindMax();
        Console.WriteLine("Максимальное значение в дереве: " + max);
        int count = tree.CountMax();

        Console.WriteLine("Наибольшее значение узлов в дереве: " + string.Join(", ", max));
        Console.WriteLine("Количество узлов с наибольшим значением: " + count);
    }
}
