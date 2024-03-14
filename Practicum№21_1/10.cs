using System;
using System.IO;
using System.Linq;

// Определение класса Node для узла бинарного дерева
public class Node
{
    public int Value; // Значение узла
    public Node Left, Right; // Ссылки на левое и правое поддеревья

    public Node(int value)
    {
        Value = value; // Инициализация значения узла
        Left = Right = null; // Инициализация ссылок на поддеревья
    }
}


// Определение класса BinarySearchTree для бинарного дерева поиска
public class BinarySearchTree
{
    public Node root; // Корневой узел дерева

    public BinarySearchTree()
    {
        root = null;
    }

    // Метод вставки нового значения в дерево
    public void Insert(int value)
    {
        root = InsertRecursive(root, value);
    }

    // Рекурсивный метод вставки
    private Node InsertRecursive(Node root, int value)
    {
        // Если дерево пустое, создать новый узел и вернуть его
        if (root == null)
        {
            root = new Node(value);
            return root;
        }

        // Если значение меньше значения корневого узла, вставить в левое поддерево
        if (value < root.Value)
            root.Left = InsertRecursive(root.Left, value);
        // Если значение больше значения корневого узла, вставить в правое поддерево
        else if (value > root.Value)
            root.Right = InsertRecursive(root.Right, value);

        return root; // Вернуть корневой узел после вставки
    }

    // Метод поиска максимального значения в дереве
    public int FindMax()
    {
        return FindMaxRecursive(root);
    }

    // Рекурсивный метод поиска максимума
    private int FindMaxRecursive(Node root)
    {
        // Если дерево пустое, выбросить исключение
        if (root == null)
            throw new Exception("Дерево пусто");

        // Пока есть правый потомок, двигаться вправо
        while (root.Right != null)
            root = root.Right;

        return root.Value; // Вернуть значение самого правого узла
    }
}

class Program
{
    static void Main(string[] args)
    {
        string inputFile = "input.txt";

        try
        {
            string[] lines = File.ReadAllLines(inputFile);

            if (lines.Length == 0)
            {
                Console.WriteLine("Файл пуст.");
                return;
            }

            int[] numbers = lines.Select(int.Parse).ToArray();
            int maxInFile = numbers.Max();
            int countMaxInFile = numbers.Count(num => num == maxInFile);

            BinarySearchTree bst = new BinarySearchTree(); // Создание нового экземпляра бинарного дерева
            foreach (int num in numbers)
            {
                bst.Insert(num); // Вставка чисел в дерево
            }

            int maxInTree = bst.FindMax();
            Console.WriteLine($"Максимальное значение в дереве: {maxInTree}");
            Console.WriteLine($"Количество узлов с максимальным значением: {countMaxInFile}");
        }
        catch (FileNotFoundException)
        {
            Console.WriteLine($"Файл {inputFile} не найден.");
        }
        catch (FormatException)
        {
            Console.WriteLine("Файл содержит недопустимые данные.");
        }
    }
}
