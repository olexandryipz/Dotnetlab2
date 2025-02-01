using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LabWork
{
    public static class Extensions
    {
        private static void ValidateNotNull(object obj, string paramName)
        {
            if (obj == null)
                throw new ArgumentNullException(paramName, $"{paramName} cannot be null.");
        }
        public static string ReverseString(this string input)
        {
            ValidateNotNull(input, nameof(input));
            return new string(input.Reverse().ToArray());
        }

        public static int CountOccurrences(this string input, char character)
        {
            ValidateNotNull(input, nameof(input));
            return input.Count(c => c == character);
        }

        public static int CountOccurrences<T>(this T[] array, T value) where T : IEquatable<T>
        {
            ValidateNotNull(array, nameof(array));
            return array.Count(item => item.Equals(value));
        }

        public static T[] GetUniqueElements<T>(this T[] array) where T : IEquatable<T>
        {
            ValidateNotNull(array, nameof(array));
            var uniqueElements = new HashSet<T>(array);
            return uniqueElements.ToArray();
        }
    }

    public class ExtendedDictionary<T, U, V> : IEnumerable<ExtendedDictionaryElement<T, U, V>>
    {
        private readonly Dictionary<T, ExtendedDictionaryElement<T, U, V>> _dictionary = new();

        public void Add(T key, U value1, V value2)
        {
            if (key == null) throw new ArgumentNullException(nameof(key), "Ключ не може бути null.");
            if (_dictionary.ContainsKey(key)) throw new ArgumentException("Ключ вже існує.");

            _dictionary[key] = new ExtendedDictionaryElement<T, U, V>(key, value1, value2);
        }

        public bool Remove(T key) => _dictionary.Remove(key);

        public bool ContainsKey(T key) => _dictionary.ContainsKey(key);

        public bool ContainsValue(U value1, V value2)
        {
            return _dictionary.Values.Any(e => EqualityComparer<U>.Default.Equals(e.Value1, value1) &&
                                                EqualityComparer<V>.Default.Equals(e.Value2, value2));
        }

        public ExtendedDictionaryElement<T, U, V> this[T key]
        {
            get
            {
                if (_dictionary.TryGetValue(key, out var element))
                {
                    return element;
                }
                throw new KeyNotFoundException($"Ключ '{key}' не знайдено.");
            }
        }

        public int Count => _dictionary.Count;

        public IEnumerator<ExtendedDictionaryElement<T, U, V>> GetEnumerator() => _dictionary.Values.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    public class ExtendedDictionaryElement<T, U, V>
    {
        public T Key { get; }
        public U Value1 { get; }
        public V Value2 { get; }

        public ExtendedDictionaryElement(T key, U value1, V value2)
        {
            Key = key;
            Value1 = value1;
            Value2 = value2;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.InputEncoding = Encoding.UTF8;
            string sample = "hello world";
            Console.WriteLine("Оригінальний рядок: " + sample);
            Console.WriteLine("Інвертований рядок: " + sample.ReverseString());
            Console.WriteLine("Кількість входжень 'l': " + sample.CountOccurrences('l'));

            int[] numbers = { 1, 2, 2, 3, 4, 4, 4, 5 };
            Console.WriteLine("Оригінальний масив: " + string.Join(", ", numbers));
            Console.WriteLine("Кількість входжень 4: " + numbers.CountOccurrences(4));
            Console.WriteLine("Унікальні елементи: " + string.Join(", ", numbers.GetUniqueElements()));

            var extendedDictionary = new ExtendedDictionary<int, string, string>();
            extendedDictionary.Add(1, "Перший", "Значення1");
            extendedDictionary.Add(2, "Другий", "Значення2");

            Console.WriteLine("Словник містить ключ 1: " + extendedDictionary.ContainsKey(1));
            Console.WriteLine("Словник містить значення (Перший, Значення1): " + extendedDictionary.ContainsValue("Перший", "Значення1"));

            var element = extendedDictionary[1];
            Console.WriteLine($"Ключ: {element.Key}, Значення1: {element.Value1}, Значення2: {element.Value2}");

            Console.WriteLine("Ітерація через словник:");
            foreach (var item in extendedDictionary)
            {
                Console.WriteLine($"Ключ: {item.Key}, Значення1: {item.Value1}, Значення2: {item.Value2}");
            }
        }
    }
}
