using System;
using System.Collections.Generic;
using System.Linq;

// Клас, що представляє вузол в дереві Хаффмана
public class HuffmanNode : IComparable<HuffmanNode>
{
    public char Symbol { get; set; }
    public int Frequency { get; set; }
    public HuffmanNode Left { get; set; }
    public HuffmanNode Right { get; set; }

    // Конструктор для ініціалізації символу та його частоти
    public HuffmanNode(char symbol, int frequency)
    {
        Symbol = symbol;
        Frequency = frequency;
    }

    // Реалізація інтерфейсу для порівняння вузлів за частотою
    public int CompareTo(HuffmanNode other)
    {
        return Frequency - other.Frequency;
    }
}

// Клас, що представляє дерево Хаффмана
public class HuffmanTree
{
    public HuffmanNode Root { get; private set; }

    // Конструктор, який будує дерево Хаффмана на основі заданих частот
    public HuffmanTree(List<KeyValuePair<char, int>> frequencies)
    {
        BuildTree(frequencies);
    }

    // Приватний метод для побудови дерева Хаффмана
    private void BuildTree(List<KeyValuePair<char, int>> frequencies)
    {
        PriorityQueue<HuffmanNode> priorityQueue = new PriorityQueue<HuffmanNode>();

        // Створюємо пріорітетну чергу для сортування вузлів за частотою
        foreach (var kvp in frequencies)
        {
            priorityQueue.Enqueue(new HuffmanNode(kvp.Key, kvp.Value));
        }

        // Об'єднуємо вузли до тих пір, поки не залишиться один кореневий вузол
        while (priorityQueue.Count > 1)
        {
            HuffmanNode left = priorityQueue.Dequeue();
            HuffmanNode right = priorityQueue.Dequeue();

            HuffmanNode parent = new HuffmanNode('_', left.Frequency + right.Frequency)
            {
                Left = left,
                Right = right
            };

            priorityQueue.Enqueue(parent);
        }

        // Корінь дерева - останній вузол у черзі
        Root = priorityQueue.Dequeue();
    }

    // Метод для побудови таблиці кодів на основі дерева
    public Dictionary<char, string> BuildCodeTable()
    {
        Dictionary<char, string> codeTable = new Dictionary<char, string>();
        BuildCodeTable(Root, "", codeTable);
        return codeTable;
    }

    // Приватний рекурсивний метод для побудови таблиці кодів
    private void BuildCodeTable(HuffmanNode node, string code, Dictionary<char, string> codeTable)
    {
        if (node != null)
        {
            // Якщо вузол листовий, додаємо його символ та код в таблицю
            if (node.Left == null && node.Right == null)
            {
                codeTable.Add(node.Symbol, code);
            }

            // Рекурсивно викликаємо для лівого та правого піддерева
            BuildCodeTable(node.Left, code + "0", codeTable);
            BuildCodeTable(node.Right, code + "1", codeTable);
        }
    }
}

// Клас для пріорітетної черги
public class PriorityQueue<T> where T : IComparable<T>
{
    private List<T> heap;

    // Кількість елементів в черзі
    public int Count => heap.Count;

    // Конструктор для ініціалізації порожньої черги
    public PriorityQueue()
    {
        heap = new List<T>();
    }

    // Додає елемент до черги
    public void Enqueue(T item)
    {
        heap.Add(item);
        int currentIndex = heap.Count - 1;

        // Відновлюємо порядок у черзі після додавання нового елемента
        while (currentIndex > 0)
        {
            int parentIndex = (currentIndex - 1) / 2;

            if (heap[currentIndex].CompareTo(heap[parentIndex]) >= 0)
            {
                break;
            }

            Swap(currentIndex, parentIndex);
            currentIndex = parentIndex;
        }
    }

    // Видаляє та повертає елемент з найменшою пріоритетністю
    public T Dequeue()
    {
        if (heap.Count == 0)
        {
            throw new InvalidOperationException("PriorityQueue is empty");
        }

        T result = heap[0];
        heap[0] = heap[heap.Count - 1];
        heap.RemoveAt(heap.Count - 1);

        int currentIndex = 0;

        // Відновлюємо порядок у черзі після видалення елемента
        while (true)
        {
            int leftChildIndex = currentIndex * 2 + 1;
            int rightChildIndex = currentIndex * 2 + 2;

            if (leftChildIndex >= heap.Count)
            {
                break;
            }

            int minChildIndex = (rightChildIndex < heap.Count && heap[rightChildIndex].CompareTo(heap[leftChildIndex]) < 0) ? rightChildIndex : leftChildIndex;

            if (heap[currentIndex].CompareTo(heap[minChildIndex]) <= 0)
            {
                break;
            }

            Swap(currentIndex, minChildIndex);
            currentIndex = minChildIndex;
        }

        return result;
    }

    // Метод для обміну двох елементів у черзі
    private void Swap(int index1, int index2)
    {
        T temp = heap[index1];
        heap[index1] = heap[index2];
        heap[index2] = temp;
    }
}


class Program
{
    static void Main()
    {
        List<KeyValuePair<char, int>> frequencies = new List<KeyValuePair<char, int>>
        {
            new KeyValuePair<char, int>('1', 26),
            new KeyValuePair<char, int>('2', 14),
            new KeyValuePair<char, int>('3', 5),
            new KeyValuePair<char, int>('4', 10),
            new KeyValuePair<char, int>('5', 7),
            new KeyValuePair<char, int>('6', 11),
            new KeyValuePair<char, int>('7', 2),
            new KeyValuePair<char, int>('8', 20),
            new KeyValuePair<char, int>('9', 5),
        };

        HuffmanTree huffmanTree = new HuffmanTree(frequencies);
        Dictionary<char, string> codeTable = huffmanTree.BuildCodeTable();

        foreach (var kvp in codeTable)
        {
            Console.WriteLine($"Symbol: {kvp.Key}, Code: {kvp.Value}");
        }
    }
}
