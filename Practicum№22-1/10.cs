using System;
using System.IO;

class Program
{
    static void Main(string[] args)
    {
        string inputPath = "input.txt";
        string outputPath = "output.txt";

        // Чтение входных данных из файла
        string[] lines = File.ReadAllLines(inputPath);
        int n = int.Parse(lines[0]);  // количество вершин
        int[,] adjacencyMatrix = new int[n, n];

        // Заполнение матрицы смежности
        for (int i = 1; i <= n; i++)
        {
            string[] parts = lines[i].Split(' ');
            for (int j = 0; j < n; j++)
            {
                adjacencyMatrix[i - 1, j] = int.Parse(parts[j]);
            }
        }

        // Чтение вершин, которые нужно удалить
        string[] vertices = lines[n + 1].Split(' ');
        int a = int.Parse(vertices[0]) - 1;  // вершина a
        int b = int.Parse(vertices[1]) - 1;  // вершина b

        // Удаление дуги (a, b)
        adjacencyMatrix[a, b] = 0;

        // Запись измененной матрицы смежности в выходной файл
        using (StreamWriter writer = new StreamWriter(outputPath))
        {
            writer.WriteLine(n);
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    writer.Write(adjacencyMatrix[i, j] + " ");
                }
                writer.WriteLine();
            }
        }

        Console.WriteLine("Дуга удалена и матрица смежности записана в output.txt");
    }
}
