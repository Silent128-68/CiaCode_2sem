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

    // Конструктор класса Node
    public Node(int data)
    {
        Data = data;
        Left = Right = null;
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

        return root;
    }

    // Метод для получения высоты дерева
    private int GetHeight(Node node)
    {
        if (node == null) return 0;
        return 1 + Math.Max(GetHeight(node.Left), GetHeight(node.Right));
    }

    // Метод для проверки идеальной сбалансированности дерева
    public bool IsPerfectlyBalanced(Node root)
    {
        if (root == null) return true;

        int leftHeight = GetHeight(root.Left);
        int rightHeight = GetHeight(root.Right);

        if (Math.Abs(leftHeight - rightHeight) <= 1 &&
            IsPerfectlyBalanced(root.Left) &&
            IsPerfectlyBalanced(root.Right))
        {
            return true;
        }

        return false;
    }

    // Метод для проверки сбалансированности дерева после удаления узла
    public bool IsBalancedAfterRemoval(Node root, Node nodeToRemove)
    {
        if (root == null || nodeToRemove == null)
            return false;

        // Удаляем узел и проверяем, остается ли дерево идеально сбалансированным
        Node newRoot = RemoveNode(CloneTree(root), nodeToRemove.Data);
        return IsPerfectlyBalanced(newRoot);
    }

    // Метод для клонирования дерева
    private Node CloneTree(Node root)
    {
        if (root == null) return null;

        Node newNode = new Node(root.Data);
        newNode.Left = CloneTree(root.Left);
        newNode.Right = CloneTree(root.Right);
        return newNode;
    }

    // Вспомогательный метод для удаления узла из дерева
    private Node RemoveNode(Node root, int key)
    {
        return Delete(root, key);
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

    // Метод для нахождения узла, который можно удалить для сбалансирования дерева
    public Node FindNodeToRemove(Node root)
    {
        if (root == null) return null;

        Queue<Node> queue = new Queue<Node>();
        queue.Enqueue(root);

        while (queue.Count > 0)
        {
            Node currentNode = queue.Dequeue();

            if (IsBalancedAfterRemoval(root, currentNode))
                return currentNode;

            if (currentNode.Left != null)
                queue.Enqueue(currentNode.Left);
            if (currentNode.Right != null)
                queue.Enqueue(currentNode.Right);
        }

        return null;
    }
}

// Основной класс программы
class Program
{
    static void Main(string[] args)
    {
        // Считываем данные из файла
        string input = File.ReadAllText("input.txt");

        // Разбиваем строку ввода на отдельные числа
        string[] numbersAsString = input.Split(new char[] { ' ', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

        // Преобразуем ввод в массив целых чисел
        int[] numbers = Array.ConvertAll(numbersAsString, int.Parse);

        BinarySearchTree bst = new BinarySearchTree();

        // Вставляем все числа из файла в бинарное дерево поиска
        foreach (int number in numbers)
        {
            bst.Insert(number);
        }

        // Проверяем, является ли дерево идеально сбалансированным
        if (bst.IsPerfectlyBalanced(bst.Root))
        {
            Console.WriteLine("The tree is already perfectly balanced. No need to remove any nodes.");
        }
        else
        {
            // Проверяем, можно ли сбалансировать дерево удалением одного узла
            Node nodeToRemove = bst.FindNodeToRemove(bst.Root);
            if (nodeToRemove != null)
            {
                Console.WriteLine($"Remove node with value: {nodeToRemove.Data}");
            }
            else
            {
                Console.WriteLine("No single node can be removed to balance the tree. It requires removing more than one node.");
            }
        }
    }
}
