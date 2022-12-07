﻿using System;
using System.IO;

namespace TestTask
{
    public class ReadOnlyStream : IReadOnlyStream
    {
        private StreamReader _localStream;

        /// <summary>
        /// Конструктор класса. 
        /// Т.к. происходит прямая работа с файлом, необходимо 
        /// обеспечить ГАРАНТИРОВАННОЕ закрытие файла после окончания работы с таковым!
        /// </summary>
        /// <param name="fileFullPath">Полный путь до файла для чтения</param>
        public ReadOnlyStream(string fileFullPath)
        {
            _localStream = new StreamReader(fileFullPath);
        }

        /// <summary>
        /// Флаг окончания файла.
        /// </summary>
        public bool IsEof
        {
            get { return _localStream.EndOfStream; }
        }

        /// <summary>
        /// Ф-ция чтения следующего символа из потока.
        /// Если произведена попытка прочитать символ после достижения конца файла, метод 
        /// должен бросать соответствующее исключение
        /// </summary>
        /// <returns>Считанный символ.</returns>
        public char ReadNextChar()
        {
            if (IsEof)
            {
                throw new Exception("Достигнут конец файла.");
            }
            return (char)_localStream.Read();
        }

        /// <summary>
        /// Сбрасывает текущую позицию потока на начало.
        /// </summary>
        public void ResetPositionToStart()
        {
            if (_localStream == null)
            {
                return;
            }

            // Синхронизация внутреннего буфера и основного потока
            _localStream.DiscardBufferedData();
            // Смещение позиции в основном потоке
            _localStream.BaseStream.Seek(0, SeekOrigin.Begin);
        }

        /// <summary>
        /// Функция закрытия файла и освобождения всех ресурсов,
        /// используемых объектом.
        /// </summary>
        public void CloseFile()
        {
            _localStream.Close();
            _localStream.Dispose();
        }
    }
}
