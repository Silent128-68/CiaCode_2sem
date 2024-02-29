using System;
using System.IO;

class Node
{
    public int Data;
    public Node Left, Right;

    public Node(int item)
    {
        Data = item;
        Left = Right = null;
    }
}

class BinarySearchTree
{
    private Node root;

    public BinarySearchTree()
    {
        root = null;
    }

    public void Insert(int data)
    {
        root = InsertRecursive(root, data);
    }

    private Node InsertRecursive(Node root, int data)
    {
        if (root == null)
        {
            root = new Node(data);
            return root;
        }

        if (data < root.Data)
            root.Left = InsertRecursive(root.Left, data);
        else if (data > root.Data)
            root.Right = InsertRecursive(root.Right, data);

        return root;
    }

    public int FindLevel(int key)
    {
        return FindLevelRecursive(root, key, 1);
    }

    public int FindLevelRecursive(Node root, int key, int level)
    {
        if (root == null)
            return 0;

        if (root.Data == key)
            return level;

        int leftLevel = FindLevelRecursive(root.Left, key, level + 1);
        if (leftLevel != 0)
            return leftLevel;

        return FindLevelRecursive(root.Right, key, level + 1);
    }

}

class Program
{
    static void Main(string[] args)
    {
        string[] lines = File.ReadAllLines("input.txt");
        int[] numbers = Array.ConvertAll(lines[0].Split(' '), int.Parse);

        BinarySearchTree tree = new BinarySearchTree();

        foreach (int num in numbers)
        {
            tree.Insert(num);
        }

        Console.Write("Введите число для поиска уровня: ");
        int searchKey = int.Parse(Console.ReadLine());

        int level = tree.FindLevel(searchKey);

        if (level != 0)
            Console.WriteLine($"Уровень узла {searchKey}: {level}");
        else
            Console.WriteLine($"Узел {searchKey} не найден в дереве.");
    }
}

//5 3 8 2 4 7 9

//        5
//       / \
//      3   8
//     / \ / \
//    2  4 7  9

//100 50 150 25 75 125 175 10 30 60 80 110 130 160 200 105

  //              100
  //         /          \
  //       50           150
  //     /   \         /    \
  //   25    75      125    175
  //  / \    / \     /       \
  //10  30  60 80  110       200
  //                /
  //              105
