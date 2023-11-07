using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class HuffmanNode
{
    public string Text { get; set; }
    public int Frequency { get; set; }
    public HuffmanNode Left { get; set; }
    public HuffmanNode Right { get; set; }
}

public class HuffmanTree
{
    public HuffmanNode Root { get; private set; }

    public HuffmanTree(Dictionary<string, int> frequencies)
    {
        BuildHuffmanTree(frequencies);
    }

    private void BuildHuffmanTree(Dictionary<string, int> frequencies)
    {
        // Створюємо список вузлів для кожного рядка та його частоти входження
        var nodes = frequencies.Select(kv => new HuffmanNode { Text = kv.Key, Frequency = kv.Value }).ToList();

        while (nodes.Count() > 1)
        {
            // Сортуємо вузли за частотою входження
            nodes = nodes.OrderBy(node => node.Frequency).ToList();

            // Беремо два вузли з найменшою частотою входження
            var left = nodes[0];
            var right = nodes[1];

            // Створюємо новий внутрішній вузол з цими двома вузлами як дітьми
            var newNode = new HuffmanNode
            {
                Frequency = left.Frequency + right.Frequency,
                Left = left,
                Right = right
            };

            // Видаляємо оброблені вузли зі списку та додаємо новий внутрішній вузол
            nodes.Remove(left);
            nodes.Remove(right);
            nodes.Add(newNode);
        }

        // Залишений вузол - корінь дерева Хаффмана
        Root = nodes.Single();
    }
}
class Program
{
    public static Dictionary<string, string> BuildHuffmanCodes(HuffmanNode node)
    {
        var codes = new Dictionary<string, string>();
        GenerateHuffmanCodes(node, codes, "");
        return codes;
    }

    private static void GenerateHuffmanCodes(HuffmanNode node, Dictionary<string, string> codes, string currentCode)
    {
        if (node.Text != null)
        {
            codes[node.Text] = currentCode;
            return;  // Зупиняємо рекурсію, коли досягли листового вузла
        }

        if (node.Left != null)
        {
            GenerateHuffmanCodes(node.Left, codes, currentCode + "0");
        }

        if (node.Right != null)
        {
            GenerateHuffmanCodes(node.Right, codes, currentCode + "1");
        }
    }

    public static void Main(string[] args)
    {
        string[] input = Console.ReadLine().Split();
        HashSet<string> specificStrs = new HashSet<string>();
        Dictionary<string, int> characterFrequencies = new Dictionary<string, int>();

        foreach (string s in input)
        {
            specificStrs.Add(s);
        }
        foreach (string s in specificStrs)
        {
            characterFrequencies.Add(s, input.Where(x => x == s).Count());
        }        

        var huffmanTree = new HuffmanTree(characterFrequencies);
        var huffmanCodes = BuildHuffmanCodes(huffmanTree.Root);

        Console.WriteLine("Huffman Codes:");
        foreach (var kv in huffmanCodes)
        {
            Console.WriteLine($"Character: {kv.Key}, Code: {kv.Value}");
        }
    }
}
