namespace TestTask
{
    /// <summary>
    /// Статистика вхождения буквы/пары букв
    /// </summary>
    public struct LetterStats
    {
        /// <summary>
        /// Буква/Пара букв для учёта статистики.
        /// </summary>
        public string Letter;

        /// <summary>
        /// Кол-во вхождений буквы/пары.
        /// </summary>
        public int Count;
        /// <summary>
        /// Конструктор для структуры
        /// </summary>
        /// <param name="letter"></param>
        /// <param name="count"></param>
        public LetterStats(string letter, int count)
        {
            Letter = letter;
            Count = count;
        }
        /// <summary>
        /// Функция преобразования структуры в строку
        /// </summary>
        public string ToString()
        {
            return $"{Letter}\t{Count}";
        }
    }
}
