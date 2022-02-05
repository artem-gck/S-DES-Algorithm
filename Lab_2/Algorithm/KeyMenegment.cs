using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Lab_2.Algorithm
{
    /// <summary>
    /// KeyMenegment class.
    /// </summary>
    public class KeyMenegment
    {
        /// <summary>
        /// The binary.
        /// </summary>
        private readonly Regex binary = new Regex("^[01]{1,32}$", RegexOptions.Compiled);

        /// <summary>
        /// The key.
        /// </summary>
        private char[] _key;

        /// <summary>
        /// The P10.
        /// </summary>
        private readonly byte[] P10 = new byte[10] { 3, 5, 2, 7, 4, 10, 1, 9, 8, 6 };

        /// <summary>
        /// The p8.
        /// </summary>
        private readonly byte[] P8 = new byte[8] { 6, 3, 7, 4, 8, 5, 10, 9 };

        /// <summary>
        /// Initializes a new instance of the <see cref="KeyMenegment"/> class.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <exception cref="System.ArgumentException"></exception>
        public KeyMenegment(string key)
            => _key = binary.IsMatch(key) ? key.ToArray() : throw new ArgumentException($"{nameof(key)} is not valid.");

        /// <summary>
        /// Gets the keys.
        /// </summary>
        /// <returns></returns>
        public (byte key1, byte key2) GetKeys()
        {
            var permutationKey = GetPermutationKey(_key, P10);
            var permutationKeys = SeparateKey(permutationKey);

            permutationKeys.permutationKey1 = ShiftLeft(permutationKeys.permutationKey1, 1);
            permutationKeys.permutationKey2 = ShiftLeft(permutationKeys.permutationKey2, 1);
            var permutationKey1 = ConnectKeys(permutationKeys.permutationKey1, permutationKeys.permutationKey2);
            var key1 = GetPermutationKey(permutationKey1, P8);

            permutationKeys.permutationKey1 = ShiftLeft(permutationKeys.permutationKey1, 2);
            permutationKeys.permutationKey2 = ShiftLeft(permutationKeys.permutationKey2, 2);
            var permutationKey2 = ConnectKeys(permutationKeys.permutationKey1, permutationKeys.permutationKey2);
            var key2 = GetPermutationKey(permutationKey2, P8);

            return (Convert.ToByte(new string(key1), 2), Convert.ToByte(new string(key2), 2));
        }

        /// <summary>
        /// Gets the permutation key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="tample">The tample.</param>
        /// <returns></returns>
        private char[] GetPermutationKey(char[] key, byte[] tample)
        {
            var permutatedKey = new char[tample.Length];

            for (int i = 0; i < permutatedKey.Length; i++)
                permutatedKey[i] = key[tample[i] - 1];

            return permutatedKey;
        }

        /// <summary>
        /// Separates the key.
        /// </summary>
        /// <param name="permutationKey">The permutation key.</param>
        /// <returns></returns>
        private (char[] permutationKey1, char[] permutationKey2) SeparateKey(char[] permutationKey)
            => (permutationKey[0..5], permutationKey[5..10]);

        /// <summary>
        /// Connects the keys.
        /// </summary>
        /// <param name="permutationKey1">The permutation key1.</param>
        /// <param name="permutationKey2">The permutation key2.</param>
        /// <returns></returns>
        private char[] ConnectKeys(char[] permutationKey1, char[] permutationKey2)
            => permutationKey1.Concat(permutationKey2).ToArray();

        /// <summary>
        /// Shifts the left.
        /// </summary>
        /// <param name="permutationKeyPart">The permutation key part.</param>
        /// <param name="numberOfBits">The number of bits.</param>
        /// <returns></returns>
        private char[] ShiftLeft(char[] permutationKeyPart, int numberOfBits)
        {
            var twoKeyParts = permutationKeyPart.Concat(permutationKeyPart).ToArray();
            return twoKeyParts[numberOfBits..(permutationKeyPart.Length + numberOfBits)];
        }
    }
}
