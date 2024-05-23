using System;             // Пространство имен, содержащее базовые классы и типы .NET
using System.IO;          // Пространство имен для работы с файлами и потоками
using System.Linq;        // Пространство имен для работы с LINQ (Language Integrated Query)
using System.Collections.Generic;  // Пространство имен для работы с коллекциями

// Класс, представляющий узел бинарного дерева
class Node
{
    public int Data;     // Значение узла
    public Node Left;    // Левый потомок
    public Node Right;   // Правый потомок
    public int Count;    // Счетчик узлов в поддереве с корнем в данном узле

    // Конструктор класса Node
    public Node(int data)
    {
        Data = data;
        Left = Right = null;
        Count = 1;  // Инициализируем счетчик как 1 при создании узла
    }
}

// Класс, представляющий бинарное дерево поиска
class BinarySearchTree
{
    public Node Root;   // Корень дерева

    // Метод для вставки нового узла в дерево
    public void Insert(int data)
    {
        Root = InsertRec(Root, data);  // Вызов вспомогательного рекурсивного метода вставки
    }

    // Вспомогательный рекурсивный метод для вставки узла в дерево
    private Node InsertRec(Node root, int data)
    {
        // Если дерево пустое, создаем новый узел и возвращаем его
        if (root == null)
        {
            return new Node(data);
        }

        // Если значение для вставки меньше значения текущего узла, рекурсивно вставляем в левое поддерево
        if (data < root.Data)
        {
            root.Left = InsertRec(root.Left, data);
        }
        // Если значение для вставки больше значения текущего узла, рекурсивно вставляем в правое поддерево
        else if (data > root.Data)
        {
            root.Right = InsertRec(root.Right, data);
        }

        // После вставки обновляем счетчик узлов в поддереве с корнем в данном узле
        root.Count = 1 + GetCount(root.Left) + GetCount(root.Right);
        return root;
    }

    // Метод для удаления узла из дерева
    public Node Delete(Node root, int key)
    {
        // Если дерево пустое, возвращаем его
        if (root == null)
            return root;

        // Рекурсивно ищем узел с заданным ключом для удаления
        if (key < root.Data)
        {
            root.Left = Delete(root.Left, key);
        }
        else if (key > root.Data)
        {
            root.Right = Delete(root.Right, key);
        }
        else
        {
            // Если у узла нет потомков или только один потомок, просто удаляем его
            if (root.Left == null)
                return root.Right;
            else if (root.Right == null)
                return root.Left;

            // Если у узла два потомка, находим минимальное значение в правом поддереве
            // и заменяем значение текущего узла на найденное минимальное значение
            root.Data = MinValue(root.Right);
            root.Right = Delete(root.Right, root.Data);
        }

        // После удаления узла обновляем счетчик узлов в поддереве с корнем в данном узле
        root.Count = 1 + GetCount(root.Left) + GetCount(root.Right);
        return root;
    }

    // Метод для нахождения минимального значения в дереве
    private int MinValue(Node node)
    {
        int minValue = node.Data;
        while (node.Left != null)
        {
            minValue = node.Left.Data;
            node = node.Left;
        }
        return minValue;
    }

    // Метод для получения количества узлов в поддереве с корнем в данном узле
    public int GetCount(Node node)
    {
        return node == null ? 0 : node.Count;
    }

    // Метод для проверки идеальной сбалансированности дерева
    public bool IsPerfectlyBalanced(Node root)
    {
        // Если дерево пустое, оно считается идеально сбалансированным
        if (root == null)
            return true;

        int leftCount = GetCount(root.Left);   
        int rightCount = GetCount(root.Right); 

        // Проверяем, что разница в количестве узлов между поддеревьями не превышает 1
        // И рекурсивно проверяем идеальную сбалансированность левого и правого поддеревьев
        if (Math.Abs(leftCount - rightCount) <= 1 && IsPerfectlyBalanced(root.Left) && IsPerfectlyBalanced(root.Right))
            return true;

        return false;
    }

    // Метод для нахождения узла, который можно удалить для сбалансирования дерева
    public Node FindNodeToRemove(Node root)
    {
        Queue<Node> queue = new Queue<Node>();
        queue.Enqueue(root);

        while (queue.Count > 0)
        {
            Node currentNode = queue.Dequeue();

            // Проверяем, сбалансировано ли дерево после удаления данного узла
            if (IsBalancedAfterRemoval(root, currentNode))
                return currentNode;

            if (currentNode.Left != null)
                queue.Enqueue(currentNode.Left);
            if (currentNode.Right != null)
                queue.Enqueue(currentNode.Right);
        }

        return null;
    }

    // Метод для проверки сбалансированности дерева после удаления узла
    public bool IsBalancedAfterRemoval(Node root, Node nodeToRemove)
    {
        if (root == null || nodeToRemove == null)
            return false;

        // Удаляем узел и проверяем, остается ли дерево идеально сбалансированным
        Node newRoot = RemoveNode(root, nodeToRemove.Data);
        return IsPerfectlyBalanced(newRoot);
    }

    // Вспомогательный метод для удаления узла из дерева
    private Node RemoveNode(Node root, int key)
    {
        return Delete(root, key);
    }
}

// Основной класс программы
class Program
{
    static void Main(string[] args)
    {
        // Считываем данные из файла
        string[] lines = File.ReadAllLines("input.txt");
        int[] numbers = lines.Select(int.Parse).ToArray();

        BinarySearchTree bst = new BinarySearchTree();

        // Вставляем все числа из файла в бинарное дерево поиска
        foreach (int number in numbers)
        {
            bst.Insert(number);
        }

        // Находим узел, который можно удалить для сбалансирования дерева
        Node nodeToRemove = bst.FindNodeToRemove(bst.Root);

        // Выводим результат
        if (nodeToRemove != null)
        {
            Console.WriteLine($"Remove node with value: {nodeToRemove.Data}");
        }
        else
        {
            Console.WriteLine("No single node can be removed to balance the tree.");
        }
    }
}
