using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab_2.Algorithm
{
    /// <summary>
    /// SDesAlgorithm class.
    /// </summary>
    public static class SDesAlgorithm
    {
        /// <summary>
        /// The ip.
        /// </summary>
        private static readonly byte[] IP = new byte[8] { 2, 6, 3, 1, 4, 8, 5, 7 };

        /// <summary>
        /// The ip minus.
        /// </summary>
        private static readonly byte[] IPMinus = new byte[8] { 4, 1, 3, 5, 7, 2, 8, 6 };

        /// <summary>
        /// The ep.
        /// </summary>
        private static readonly byte[] EP = new byte[8] { 4, 1, 2, 3, 2, 3, 4, 1 };

        /// <summary>
        /// The p4.
        /// </summary>
        private static readonly byte[] P4 = new byte[4] { 2, 4, 3, 1 };

        /// <summary>
        /// The s0.
        /// </summary>
        private static string[,] S0 =
        {
            { "01" , "00" , "11" , "10" },
            { "11" , "10" , "01" , "00" },
            { "00" , "10" , "01" , "11" },
            { "11" , "01" , "11" , "10" }
        };

        /// <summary>
        /// The s1.
        /// </summary>
        private static string[,] S1 =
        {
            { "00" , "01" , "10" , "11" },
            { "10" , "00" , "01" , "11" },
            { "11" , "00" , "01" , "00" },
            { "10" , "01" , "00" , "11" }
        };

        /// <summary>
        /// Encrypts the specified data.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="key">The key.</param>
        /// <returns>Encrypted string.</returns>
        public static string Encrypt(string data, string key)
        {
            var dataArray = data.ToArray();

            var keyMenegment = new KeyMenegment(key);
            var keys = keyMenegment.GetKeys();

            var step1 = GetPermutationData(dataArray, IP);
            var raund1 = GetRaund(step1, keys.key1);
            var step3 = SwapPlaces(raund1);
            var raund2 = GetRaund(step3, keys.key2);
            var step5 = GetPermutationData(raund2, IPMinus);

            return new string(step5);
        }

        /// <summary>
        /// Decrypts the specified data.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="key">The key.</param>
        /// <returns>Decrypted string.</returns>
        public static string Decrypt(string data, string key)
        {
            var dataArray = data.ToArray();

            var keyMenegment = new KeyMenegment(key);
            var keys = keyMenegment.GetKeys();

            var step1 = GetPermutationData(dataArray, IP);
            var raund1 = GetRaund(step1, keys.key2);
            var step3 = SwapPlaces(raund1);
            var raund2 = GetRaund(step3, keys.key1);
            var step5 = GetPermutationData(raund2, IPMinus);

            return new string(step5);
        }

        /// <summary>
        /// Gets the raund.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        private static char[] GetRaund(char[] data, byte key)
        {
            var dataParts = SeparateData(data);
            var part2 = GetPermutationData(dataParts.permutationData2, EP);
            var xorPart2 = GetXor(part2, key);
            var xorParts = SeparateData(xorPart2);
            var resultOfBlockS0 = SubstitutionBoxes(xorParts.permutationData1, S0);
            var resultOfBlockS1 = SubstitutionBoxes(xorParts.permutationData2, S1);
            var resultOfBlocks = ConnectData(resultOfBlockS0, resultOfBlockS1);
            var permutationP4 = GetPermutationData(resultOfBlocks, P4);
            var xorResult = GetXor(permutationP4, dataParts.permutationData1);

            return ConnectData(xorResult, dataParts.permutationData2);
        }

        /// <summary>
        /// Gets the permutation data.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="tample">The tample.</param>
        /// <returns></returns>
        private static char[] GetPermutationData(char[] data, byte[] tample)
        {
            var permutatedKey = new char[tample.Length];

            for (int i = 0; i < permutatedKey.Length; i++)
                permutatedKey[i] = data[tample[i] - 1];

            return permutatedKey;
        }

        /// <summary>
        /// Swaps the places.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        private static char[] SwapPlaces(char[] data)
            => data[(data.Length / 2)..data.Length].Concat(data[0..(data.Length / 2)]).ToArray();

        /// <summary>
        /// Separates the data.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        private static (char[] permutationData1, char[] permutationData2) SeparateData(char[] data)
            => (data[0..(data.Length / 2)], data[(data.Length / 2)..data.Length]);

        /// <summary>
        /// Connects the data.
        /// </summary>
        /// <param name="permutationData1">The permutation data1.</param>
        /// <param name="permutationData2">The permutation data2.</param>
        /// <returns></returns>
        private static char[] ConnectData(char[] permutationData1, char[] permutationData2)
            => permutationData1.Concat(permutationData2).ToArray();

        /// <summary>
        /// Gets the xor.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        private static char[] GetXor(char[] data, byte key)
        { 
            var xor = Convert.ToString((byte)(Convert.ToInt16(new string(data), 2) ^ key), 2).ToArray();
            var answer = new char[data.Length];
            answer = answer.Select(x => '0').ToArray();

            xor.CopyTo(answer, data.Length - xor.Length);

            return answer;
        }

        /// <summary>
        /// Gets the xor.
        /// </summary>
        /// <param name="data1">The data1.</param>
        /// <param name="data2">The data2.</param>
        /// <returns></returns>
        private static char[] GetXor(char[] data1, char[] data2)
        {
            var xor = Convert.ToString((byte)(Convert.ToByte(new string(data1), 2) ^ Convert.ToByte(new string(data2), 2)), 2).ToArray();
            var answer = new char[data1.Length];
            answer = answer.Select(x => '0').ToArray();

            xor.CopyTo(answer, data1.Length - xor.Length);

            return answer;
        }

        /// <summary>
        /// Substitutions the boxes.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="block">The block.</param>
        /// <returns></returns>
        private static char[] SubstitutionBoxes(char[] data, string[,] block)
        {
            var row = Convert.ToByte(data.First().ToString() + data.Last().ToString(), 2);
            var col = Convert.ToByte(data[1].ToString() + data[2].ToString(), 2);

            return block[row, col].ToArray();
        }
    }
}
