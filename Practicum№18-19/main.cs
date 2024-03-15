using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace ConsoleApp1
{
    class Program
    {

        static void Main(string[] args)
        {
            // Создаем список клиентов и добавляем в него данные
            List<Client> clients = new List<Client>();


            using (FileStream f = new FileStream("data.bin", FileMode.OpenOrCreate))
            {
                if (f.Length == 0)
                {
                    clients.Add(new Organization("HELP", new DateTime(2023, 01, 10), "0", 0));
                    clients.Add(new Depositor("Doe", new DateTime(2023, 01, 15), 5000, 2.5));
                    clients.Add(new Creditor("Smith", new DateTime(2023, 01, 05), 0, 0, 0));
                }
                else
                {
                    clients = DeserializeClients("data.bin");
                }
            }

            // Выводим информацию о клиентах до сериализации
            Console.WriteLine("Информация о клиентах");
            PrintClients(clients);
            Console.WriteLine("----------------------------------------------");

            // Сохраняем данные в бинарный файл и выводим количество клиентов до сериализации
            SerializeClients(clients, "data.bin");
            Console.WriteLine("----------------------------------------------");
        }

        // Метод для сериализации списка клиентов и сохранения их в бинарный файл
        static void SerializeClients(List<Client> clients, string fileName)
        {
            try
            {
                using (FileStream fileStream = new FileStream(fileName, FileMode.Create))
                {
                    BinaryFormatter binaryFormatter = new BinaryFormatter();
                    binaryFormatter.Serialize(fileStream, clients);
                }
                Console.WriteLine($"Данные сохранены в файл {fileName}.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при сохранении данных в файл {fileName}: {ex.Message}");
            }
        }

        // Метод для десериализации списка клиентов из бинарного файла
        static List<Client> DeserializeClients(string fileName)
        {
            List<Client> deserializedClients = new List<Client>();

            try
            {
                using (FileStream fileStream = new FileStream(fileName, FileMode.Open))
                {
                    BinaryFormatter binaryFormatter = new BinaryFormatter();
                    deserializedClients = (List<Client>)binaryFormatter.Deserialize(fileStream);
                }
                Console.WriteLine($"Данные загружены из файла {fileName}.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при загрузке данных из файла {fileName}: {ex.Message}");
            }

            return deserializedClients;
        }

        // Метод для вывода информации о клиентах на консоль
        static void PrintClients(List<Client> clients)
        {
            if (clients.Count == 0)
            {
                Console.WriteLine("Список пуст.");
            }
            else
            {
                Console.WriteLine("Полная информация о клиентах:");
                foreach (Client client in clients)
                {
                    Console.WriteLine(client.ToString());
                }

                DateTime targetDate = new DateTime(2023, 2, 1);
                Console.WriteLine($"Клиенты с {targetDate}:");
                foreach (Client client in clients)
                {
                    if (client.IsMatch(targetDate))
                    {
                        Console.WriteLine(client.ToString());
                    }
                }

                clients.Sort();
                Console.WriteLine("Отсортированная информация:");
                foreach (Client client in clients)
                {
                    Console.WriteLine(client.ToString());
                }
            }
        }
    }
}
