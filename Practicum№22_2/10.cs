using System;
using System.IO;
using System.Linq;

class Program
{
    // Метод для чтения матрицы смежности из файла
    static int[,] ReadAdjacencyMatrix(string filename)
    {
        string[] lines = File.ReadAllLines(filename); 
        int size = int.Parse(lines[0]); // Получаем количество вершин из первой строки
        int[,] adjacencyMatrix = new int[size, size]; // Создаем матрицу смежности заданного размера

        for (int i = 1; i <= size; i++)
        {
            string[] rowValues = lines[i].Split(' '); 
            for (int j = 0; j < size; j++)
            {
                adjacencyMatrix[i - 1, j] = int.Parse(rowValues[j]); 
            }
        }
        return adjacencyMatrix; 
    }

    // Метод для поиска гамильтонова цикла
    static void FindHamiltonianCycle(int[,] graph, int startVertex)
    {
        int numVertices = graph.GetLength(0); // Получаем количество вершин графа
        bool[] visited = new bool[numVertices]; // Создаем массив для отслеживания посещенных вершин
        int[] cycle = new int[numVertices]; // Создаем массив для хранения гамильтонова цикла
        cycle[0] = startVertex; // Задаем начальную вершину цикла
        visited[startVertex] = true; // Помечаем начальную вершину как посещенную

        // Вызываем вспомогательный метод для поиска гамильтонова цикла
        if (FindHamiltonianCycleUtil(graph, visited, cycle, 1, startVertex))
        {
            // Если цикл найден, выводим его
            Console.WriteLine("Hamiltonian cycle found:");
            foreach (int vertex in cycle)
            {
                Console.Write($"{vertex} ");
            }
            Console.WriteLine(cycle[0]); // Добавляем начальную вершину для завершения цикла
        }
        else
        {
            Console.WriteLine("Hamiltonian cycle not found."); 
        }
    }

    // Вспомогательный метод для рекурсивного поиска гамильтонова цикла
    static bool FindHamiltonianCycleUtil(int[,] graph, bool[] visited, int[] cycle, int pos, int start)
    {
        if (pos == cycle.Length)
        {
            // Проверяем, есть ли ребро между последней и первой вершинами для завершения цикла
            return graph[cycle[pos - 1], start] == 1;
        }

        // Перебираем все вершины, с которыми связана текущая вершина
        for (int v = 0; v < visited.Length; v++)
        {
            // Если есть ребро и вершина не посещена, пробуем добавить ее в цикл
            if (graph[cycle[pos - 1], v] == 1 && !visited[v])
            {
                visited[v] = true; // Помечаем вершину как посещенную
                cycle[pos] = v; // Добавляем вершину в цикл

                if (FindHamiltonianCycleUtil(graph, visited, cycle, pos + 1, start))
                    return true;

                visited[v] = false; // Отмечаем вершину как непосещенную, если не удалось найти цикл
            }
        }
        return false; 
    }

    static void Main(string[] args)
    {
        Console.WriteLine("Введите имя файла с матрицей смежности:");
        string filename = Console.ReadLine(); 

        Console.WriteLine("Введите номер вершины А:");
        int startVertex = int.Parse(Console.ReadLine()); 

        int[,] adjacencyMatrix = ReadAdjacencyMatrix(filename); 
        FindHamiltonianCycle(adjacencyMatrix, startVertex); 
    }
}
