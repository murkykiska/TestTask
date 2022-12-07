using System;
using System.Collections.Generic;
using System.Linq;

namespace TestTask
{
    public class Program
    {

        /// <summary>
        /// Программа принимает на входе 2 пути до файлов.
        /// Анализирует в первом файле кол-во вхождений каждой буквы (регистрозависимо). Например А, б, Б, Г и т.д.
        /// Анализирует во втором файле кол-во вхождений парных букв (не регистрозависимо). Например АА, Оо, еЕ, тт и т.д.
        /// По окончанию работы - выводит данную статистику на экран.
        /// </summary>
        /// <param name="args">Первый параметр - путь до первого файла.
        /// Второй параметр - путь до второго файла.</param>
        private static readonly char[] VowelLetters = { 'A', 'E', 'I', 'O', 'U', 'Y', 'А', 'Я', 'У', 'Ю', 'О', 'Е', 'Ё', 'Э', 'И', 'Ы',
                                                        'a', 'e', 'i', 'o', 'u', 'y', 'а', 'я', 'у', 'ю', 'о', 'е', 'ё', 'э', 'и', 'ы' };

        private static readonly char[] ConsonantLetters = { 'B', 'C', 'D', 'F', 'G', 'H', 'J', 'K', 'L', 'M', 'N', 'P', 'Q', 'R', 'S', 'T', 'V', 'W', 'X', 'Z',
                                                            'Б', 'В', 'Г', 'Д', 'Ж', 'З', 'Й', 'К', 'Л', 'М', 'Н', 'П', 'Р', 'С', 'Т', 'Ф', 'Х', 'Ц', 'Ч', 'Ш', 'Щ',
                                                            'b', 'c', 'd', 'f', 'g', 'h', 'j', 'k', 'l', 'm', 'n', 'p', 'q', 'r', 's', 't', 'v', 'w', 'x', 'z',
                                                            'б', 'в', 'г', 'д', 'ж', 'з', 'й', 'к', 'л', 'м', 'н', 'п', 'р', 'с', 'т', 'ф', 'х', 'ц', 'ч', 'ш', 'щ' };
        static void Main(string[] args)
        {
            IReadOnlyStream inputStream1 = GetInputStream(args[0]);
            IReadOnlyStream inputStream2 = GetInputStream(args[1]);

            IList<LetterStats> singleLetterStats = FillSingleLetterStats(inputStream1);
            IList<LetterStats> doubleLetterStats = FillDoubleLetterStats(inputStream2);

            RemoveCharStatsByType(singleLetterStats, CharType.Vowel);
            RemoveCharStatsByType(doubleLetterStats, CharType.Consonants);

            PrintStatistic(singleLetterStats);
            PrintStatistic(doubleLetterStats);

            Console.WriteLine("Нажмите любую клавишу для завершения программы");
            Console.ReadKey();
        }

        /// <summary>
        /// Ф-ция возвращает экземпляр потока с уже загруженным файлом для последующего посимвольного чтения.
        /// </summary>
        /// <param name="fileFullPath">Полный путь до файла для чтения</param>
        /// <returns>Поток для последующего чтения.</returns>
        private static IReadOnlyStream GetInputStream(string fileFullPath)
        {
            if (fileFullPath == null) 
            {
                throw new Exception("Указан неверный путь для файла.");
            }
            return new ReadOnlyStream(fileFullPath);
        }

        /// <summary>
        /// Ф-ция считывающая из входящего потока все буквы, и возвращающая коллекцию статистик вхождения каждой буквы.
        /// Статистика РЕГИСТРОЗАВИСИМАЯ!
        /// </summary>
        /// <param name="stream">Стрим для считывания символов для последующего анализа</param>
        /// <returns>Коллекция статистик по каждой букве, что была прочитана из стрима.</returns>
        private static IList<LetterStats> FillSingleLetterStats(IReadOnlyStream stream)
        {
            List<LetterStats> SingleLetterStats = new List<LetterStats>();
            stream.ResetPositionToStart();
            
            while (!stream.IsEof)
            {
                char c = stream.ReadNextChar();

                if (!char.IsLetter(c)) continue;

                LetterStats letterStats = SingleLetterStats.Find(x => x.Letter == c.ToString());

                if (letterStats.Letter == null) 
                {
                    letterStats = new LetterStats(c.ToString(), 1);
                }
                else
                {
                    SingleLetterStats.Remove(letterStats);
                    IncStatistic(ref letterStats);
                }

                SingleLetterStats.Add(letterStats);
            }

            return SingleLetterStats;
        }

        /// <summary>
        /// Ф-ция считывающая из входящего потока все буквы, и возвращающая коллекцию статистик вхождения парных букв.
        /// В статистику должны попадать только пары из одинаковых букв, например АА, СС, УУ, ЕЕ и т.д.
        /// Статистика - НЕ регистрозависимая!
        /// </summary>
        /// <param name="stream">Стрим для считывания символов для последующего анализа</param>
        /// <returns>Коллекция статистик по каждой букве, что была прочитана из стрима.</returns>
        private static IList<LetterStats> FillDoubleLetterStats(IReadOnlyStream stream)
        {
            List<LetterStats> DoubleLetterStats = new List<LetterStats>();
            char c1 = ' ';
            char c2 = ' ';
            stream.ResetPositionToStart();

            while (!stream.IsEof)
            {
                c1 = (c1 == ' ') ? stream.ReadNextChar() : c2;
                if (!char.IsLetter(c1)) continue;

                c2 = stream.ReadNextChar();
                if (!char.IsLetter(c2)) continue;

                if ((c1.ToString()).ToLower() != (c2.ToString()).ToLower()) continue;

                string s = $"{c1}{c2}";

                LetterStats letterStats = DoubleLetterStats.Find(x => x.Letter.ToLower() == s.ToLower());

                if (letterStats.Letter == null)
                {
                    letterStats = new LetterStats(s, 1);
                }
                else
                {
                    DoubleLetterStats.Remove(letterStats);
                    IncStatistic(ref letterStats);
                }

                DoubleLetterStats.Add(letterStats);
            }

            return DoubleLetterStats;
        }

        /// <summary>
        /// Ф-ция перебирает все найденные буквы/парные буквы, содержащие в себе только гласные или согласные буквы.
        /// (Тип букв для перебора определяется параметром charType)
        /// Все найденные буквы/пары соответствующие параметру поиска - удаляются из переданной коллекции статистик.
        /// </summary>
        /// <param name="letters">Коллекция со статистиками вхождения букв/пар</param>
        /// <param name="charType">Тип букв для анализа</param>
        /// <param name="registerIndependent">Зависит ли статистика от регистра</param>
        private static void RemoveCharStatsByType(IList<LetterStats> letters, CharType charType)
        {
            switch (charType)
            {
                case CharType.Consonants:
                    foreach (var letter in ConsonantLetters)
                    {
                        var firstOrDefault = letters.FirstOrDefault(x => x.Letter.Contains(letter.ToString()));

                        if (!firstOrDefault.Equals(default)) letters.Remove(firstOrDefault);
                    }
                    break;
                case CharType.Vowel:
                    foreach (var letter in VowelLetters)
                    {
                        var firstOrDefault = letters.FirstOrDefault(x => x.Letter.Contains(letter.ToString()));

                        if (!firstOrDefault.Equals(default)) letters.Remove(firstOrDefault);
                    }
                    break;
            }
            
        }

        /// <summary>
        /// Ф-ция выводит на экран полученную статистику в формате "{Буква} : {Кол-во}"
        /// Каждая буква - с новой строки.
        /// Выводить на экран необходимо предварительно отсортировав набор по алфавиту.
        /// В конце отдельная строчка с ИТОГО, содержащая в себе общее кол-во найденных букв/пар
        /// </summary>
        /// <param name="letters">Коллекция со статистикой</param>
        private static void PrintStatistic(IEnumerable<LetterStats> letters)
        {
            double sum = 0;

            letters = letters.OrderBy(x => x.Letter);

            Console.WriteLine("Letter\tCount");

            foreach (var letter in letters) 
            {
                Console.WriteLine(letter.ToString());
                sum += letter.Count;
            }

            Console.WriteLine($"ИТОГО\t{sum}\n");
        }

        /// <summary>
        /// Метод увеличивает счётчик вхождений по переданной структуре.
        /// </summary>
        /// <param name="letterStats"></param>
        private static void IncStatistic(ref LetterStats letterStats)
        {
            letterStats.Count++;
        }
    }
}
