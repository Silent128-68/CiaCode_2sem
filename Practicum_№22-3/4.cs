using System;
using System.Collections.Generic;
using System.IO;

// Определение структуры для представления города
struct City
{
    public string Name; // Название города
    public int X;       // Координата X города
    public int Y;       // Координата Y города
}

class Program
{
    // Функция для вычисления евклидова расстояния между двумя точками
    static double EuclideanDistance(City city1, City city2)
    {
        return Math.Sqrt(Math.Pow(city1.X - city2.X, 2) + Math.Pow(city1.Y - city2.Y, 2));
    }

    // Функция для вывода взвешенного графа
    static void PrintWeightedGraph(double[,] adjacencyMatrix, City[] cities)
    {
        int n = adjacencyMatrix.GetLength(0);
        Console.WriteLine("Взвешенный граф (расстояния между городами):");
        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < n; j++)
            {
                if (adjacencyMatrix[i, j] == double.MaxValue)
                {
                    Console.Write("INF\t");
                }
                else
                {
                    Console.Write($"{adjacencyMatrix[i, j]:F2}\t");
                }
            }
            Console.WriteLine($" - {cities[i].Name}");
        }
    }

    // Алгоритм Дейкстры для поиска кратчайшего пути с использованием евклидова расстояния
    static (double, List<int>) DijkstraAlgorithm(double[,] adjacencyMatrix, int start, int end, HashSet<int> avoid)
    {
        int n = adjacencyMatrix.GetLength(0); // Количество городов
        double[] distance = new double[n]; // Массив для хранения расстояний
        bool[] visited = new bool[n]; // Массив для отслеживания посещенных городов
        int[] previous = new int[n]; // Массив для хранения предыдущих вершин на кратчайшем пути

        // Инициализация массивов
        for (int i = 0; i < n; i++)
        {
            distance[i] = double.MaxValue; // Изначально все расстояния бесконечны
            visited[i] = false; // Изначально все города не посещены
            previous[i] = -1; // Изначально нет предыдущих вершин
        }

        distance[start] = 0; // Расстояние от начального города до него самого равно 0

        // Алгоритм Дейкстры
        while (true)
        {
            // Находим непосещенный город с наименьшим текущим расстоянием
            double minDistance = double.MaxValue;
            int minIndex = -1;
            for (int v = 0; v < n; v++)
            {
                if (!visited[v] && distance[v] < minDistance)
                {
                    minDistance = distance[v];
                    minIndex = v;
                }
            }

            // Если не найдено непосещенных городов, выходим из цикла
            if (minIndex == -1)
                break;

            visited[minIndex] = true; // Помечаем город как посещенный

            // Обновляем расстояния до соседних городов
            for (int v = 0; v < n; v++)
            {
                if (!visited[v] && !avoid.Contains(v) && adjacencyMatrix[minIndex, v] != double.MaxValue &&
                    distance[minIndex] != double.MaxValue && distance[minIndex] + adjacencyMatrix[minIndex, v] < distance[v])
                {
                    distance[v] = distance[minIndex] + adjacencyMatrix[minIndex, v]; // Обновляем расстояние
                    previous[v] = minIndex; // Запоминаем предыдущий город
                }
            }
        }

        // Формируем путь
        List<int> path = new List<int>();
        for (int at = end; at != -1; at = previous[at])
        {
            path.Add(at);
        }
        path.Reverse();

        return (distance[end], path); // Возвращаем кратчайшее расстояние и путь до конечного города
    }

    static void Main()
    {
        // Считывание данных из файла
        string[] lines = File.ReadAllLines("input.txt"); // Чтение всех строк из файла "input.txt"
        int n = int.Parse(lines[0]); // Первая строка файла содержит количество городов
        City[] cities = new City[n]; // Массив для хранения данных о городах
        Dictionary<string, int> cityIndices = new Dictionary<string, int>(); // Словарь для хранения пар "имя города - индекс"

        for (int i = 0; i < n; i++)
        {
            string[] parts = lines[i + 1].Split(' '); // Разделение строки на части
            cities[i].Name = parts[0]; // Название города
            cities[i].X = int.Parse(parts[1]); // Координата X
            cities[i].Y = int.Parse(parts[2]); // Координата Y
            cityIndices[cities[i].Name] = i; // Заполнение словаря
        }

        double[,] adjacencyMatrix = new double[n, n]; // Матрица смежности для хранения информации о связях между городами
        for (int i = 0; i < n; i++)
        {
            string[] row = lines[i + n + 1].Split(' '); // Строка содержит информацию о связях текущего города с другими
            for (int j = 0; j < n; j++)
            {
                if (row[j] == "1")
                {
                    adjacencyMatrix[i, j] = EuclideanDistance(cities[i], cities[j]); // Вычисляем евклидово расстояние между городами
                }
                else
                {
                    adjacencyMatrix[i, j] = double.MaxValue; // Если связи нет, устанавливаем бесконечное расстояние
                }
            }
        }

        // Вывод взвешенного графа
        PrintWeightedGraph(adjacencyMatrix, cities);

        // Ввод данных с клавиатуры
        Console.WriteLine("Введите название начального города:");
        string startCityName = Console.ReadLine();
        Console.WriteLine("Введите название конечного города:");
        string endCityName = Console.ReadLine();

        // Ввод городов, которые нужно обойти
        Console.WriteLine("Введите названия городов, которые необходимо обойти (через пробел):");
        string[] avoidCityNames = Console.ReadLine().Split(' ');

        // Нахождение индексов городов в массиве
        int startIndex = -1;
        int endIndex = -1;

        try
        {
            startIndex = cityIndices[startCityName];
            endIndex = cityIndices[endCityName];
        }
        catch (KeyNotFoundException)
        {
            Console.WriteLine("Один из введенных городов не найден.");
            return;
        }

        // Множество для хранения информации о городах, которые нужно обойти
        HashSet<int> avoid = new HashSet<int>();
        foreach (string avoidCityName in avoidCityNames)
        {
            try
            {
                avoid.Add(cityIndices[avoidCityName]);
            }
            catch (KeyNotFoundException)
            {
                Console.WriteLine($"Город {avoidCityName} не найден.");
            }
        }

        // Нахождение кратчайшего пути
        var (shortestDistance, path) = DijkstraAlgorithm(adjacencyMatrix, startIndex, endIndex, avoid);

        if (shortestDistance == double.MaxValue)
        {
            Console.WriteLine("Невозможно найти путь.");
        }
        else
        {
            // Вывод результата
            Console.WriteLine($"Кратчайшее расстояние между {startCityName} и {endCityName}: {shortestDistance}");
            Console.Write("Путь: ");
            for (int i = 0; i < path.Count; i++)
            {
                Console.Write(cities[path[i]].Name);
                if (i < path.Count - 1)
                {
                    Console.Write(" -- ");
                }
            }
            Console.WriteLine();
        }
    }
}
