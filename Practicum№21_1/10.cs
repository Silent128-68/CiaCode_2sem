using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

// Определение класса узла бинарного дерева
public class Node
{
    public int Value; 
    public Node Left, Right; 

    // Конструктор класса Node, который принимает значение узла
    public Node(int value)
    {
        Value = value; 
        Left = Right = null; 
    }
}

// Определение класса бинарного дерева поиска
public class BinarySearchTree
{
    public Node root; 

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

        return root; 
    }

    // Метод поиска максимального значения в дереве
    public int FindMax()
    {
        if (root == null)
            throw new InvalidOperationException("Дерево пусто");

        Node current = root; 
        // Пока есть правый потомок, двигаться вправо
        while (current.Right != null)
            current = current.Right;

        return current.Value; // Вернуть значение самого правого узла
    }
}

class Program
{
    static void Main(string[] args)
    {
        string inputFile = "input.txt"; 

        try
        {
            if (!File.Exists(inputFile))
            {
                Console.WriteLine($"Файл {inputFile} не найден.");
                return;
            }

            List<int> numbers = new List<int>(); 
            int maxInFile = int.MinValue; 
            int countMaxInFile = 0; 

            using (StreamReader reader = new StreamReader(inputFile))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (int.TryParse(line, out int number)) // Попытка преобразования строки в число
                    {
                        numbers.Add(number); // Добавление числа в список
                        maxInFile = Math.Max(maxInFile, number); // Обновление максимального значения в файле
                    }
                    else
                    {
                        Console.WriteLine($"Невозможно преобразовать строку в число: {line}");
                    }
                }
            }

            countMaxInFile = numbers.Count(num => num == maxInFile); 

            BinarySearchTree bst = new BinarySearchTree(); // Создание нового экземпляра бинарного дерева
            foreach (int num in numbers)
            {
                bst.Insert(num); 
            }

            int maxInTree = bst.FindMax(); 
            Console.WriteLine($"Максимальное значение в дереве: {maxInTree}");
            Console.WriteLine($"Количество узлов с максимальным значением: {countMaxInFile}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Произошла ошибка: {ex.Message}"); 
        }
    }
}
