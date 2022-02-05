using Lab_2.Algorithm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab_2
{
    /// <summary>
    /// FileWorker class.
    /// </summary>
    public static class FileWorker
    {
        /// <summary>
        /// Crypts the file.
        /// </summary>
        /// <param name="inputPath">The input path.</param>
        /// <param name="outputPath">The output path.</param>
        /// <param name="key">The key.</param>
        /// <param name="algorithm">The algorithm.</param>
        /// <returns></returns>
        public static bool CryptFile(string inputPath, string outputPath, string key, Func<string, string, string> algorithm)
        {
            try
            {
                using var binaryReader = new BinaryReader(new FileStream(inputPath, FileMode.Open));
                using var binaryWriter = new BinaryWriter(new FileStream(outputPath, FileMode.Create));

                var size = binaryReader.BaseStream.Length;

                while (binaryReader.BaseStream.Position < size)
                {
                    var b = binaryReader.ReadByte();
                    var data = GetStringFromByte(b);
                    var a = algorithm(data, key);
                    var answer = Convert.ToByte(a, 2);
                    binaryWriter.Write(answer);
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Encrypts the file.
        /// </summary>
        /// <param name="inputPath">The input path.</param>
        /// <param name="outputPath">The output path.</param>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public static bool EncryptFile(string inputPath, string outputPath, string key)
            => CryptFile(inputPath, outputPath, key, SDesAlgorithm.Encrypt);

        /// <summary>
        /// Decrypts the file.
        /// </summary>
        /// <param name="inputPath">The input path.</param>
        /// <param name="outputPath">The output path.</param>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public static bool DecryptFile(string inputPath, string outputPath, string key)
            => CryptFile(inputPath, outputPath, key, SDesAlgorithm.Decrypt);

        /// <summary>
        /// Gets the string from byte.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        private static string GetStringFromByte(byte data)
        {
            var dataStr = Convert.ToString(data, 2).ToArray();
            var answer = new char[8];
            answer = answer.Select(x => '0').ToArray();

            dataStr.CopyTo(answer, answer.Length - dataStr.Length);

            return new string(answer);
        }
    }
}