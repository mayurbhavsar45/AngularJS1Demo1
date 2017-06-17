using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using System.Web;
using System.Web.Script.Serialization;
namespace Common
{
    public class Utilities
    {
        #region Encryption/Decryption Methods

        /// <summary>
        /// This Method Encrypt data using AES Alghorithm
        /// </summary>
        /// <param name="plainText">String to Encrypt</param>
        /// <returns>It returns encrypted string if succeed, else returns null</returns>
        public static string Encrypt(string plainText)
        {
            try
            {
                AESCFB8ModeEncryption AES = new AESCFB8ModeEncryption();
                return AES.Encrypt(plainText);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// This Method Decrypt data using AES Alghorithm
        /// </summary>
        /// <param name="plainText">Encrypted String to Decrypt</param>
        /// <returns>It returns decrypted string if succeed, else returns null</returns>
        public static string Decrypt(string cipherText)
        {
            try
            {
                AESCFB8ModeEncryption AES = new AESCFB8ModeEncryption();
                return AES.Decrypt(cipherText);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        #endregion Encryption/Decryption Methods

        #region Generate Random Account Number

        public static string GetAccountNo()
        {
            var RandomNumber = string.Join("", GetRandomNumbers(8, 10).ToArray());
            return DateTime.Now.Year + RandomNumber;
        }
        public static IEnumerable<int> GetRandomNumbers(int noOfRandomNumbers, int maxValue)
        {
            var mySet = new HashSet<int>();
            for (int i = 0; i < noOfRandomNumbers; i++)
            {
                int randomNo = new Random().Next(maxValue);

                while (!mySet.Add(randomNo))
                {
                    randomNo = new Random().Next(maxValue);
                }

                yield return randomNo;
            }
        }
        #endregion

        #region Json Serialize/Deserialize

        public static string JSONSerialize(object obj)
        {
            try
            {
                return new JavaScriptSerializer { MaxJsonLength = Int32.MaxValue }.Serialize(obj);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static object JSONDeSerialize(string Value, Type type)
        {
            try
            {
                return new JavaScriptSerializer().Deserialize(Value, type);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion
    }
}
