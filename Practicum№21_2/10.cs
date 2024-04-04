using System;
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

public class BinaryTree
{
    public Node root;

    public BinaryTree()
    {
        root = null; 
    }

    // Метод для вставки нового узла в дерево
    public void Insert(int data)
    {
        root = InsertRec(root, data); 
    }

    // Рекурсивный метод вставки нового узла
    private Node InsertRec(Node root, int data)
    {
        if (root == null)
        {
            root = new Node(data); // Создание нового узла
            return root;
        }

        // Вставка узла в соответствующее место дерева
        if (data < root.Data)
            root.Left = InsertRec(root.Left, data); 
        else if (data > root.Data)
            root.Right = InsertRec(root.Right, data); 

        return root;
    }

    // Метод для поиска уровня узла в дереве
    public int FindLevel(int data)
    {
        return FindLevel(root, data, 1); 
    }

    // Рекурсивный метод поиска уровня узла
    private int FindLevel(Node root, int data, int level)
    {
        if (root == null)
            return 0; 

        if (root.Data == data)
            return level; // Если узел найден, возвращаем его уровень

        // Рекурсивный поиск в левом и правом поддеревьях
        int downlevel = FindLevel(root.Left, data, level + 1); 
        if (downlevel != 0)
            return downlevel;

        downlevel = FindLevel(root.Right, data, level + 1); 
        return downlevel; 
    }
}

class Program
{
    static void Main(string[] args)
    {
        string inputFilePath = "input.txt"; 

        // Проверка наличия файла
        if (!File.Exists(inputFilePath))
        {
            Console.WriteLine("Файл не найден!");
            return; 
        }

        BinaryTree tree = new BinaryTree(); 

        // Считывание данных из файла и вставка их в дерево
        string[] lines = File.ReadAllLines(inputFilePath);
        foreach (string line in lines)
        {
            int number;
            if (int.TryParse(line, out number))
            {
                tree.Insert(number); 
            }
        }

        Console.Write("Введите узел для поиска его уровня: ");
        int nodeToFind = int.Parse(Console.ReadLine());

        // Поиск уровня узла
        int level = tree.FindLevel(nodeToFind);

        if (level != 0)
            Console.WriteLine($"Узел {nodeToFind} находится на уровне {level}.");
        else
            Console.WriteLine($"Узел {nodeToFind} не найден в дереве.");
    }
}
