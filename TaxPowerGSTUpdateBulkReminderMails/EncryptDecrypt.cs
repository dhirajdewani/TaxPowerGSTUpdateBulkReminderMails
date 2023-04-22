using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace TaxPowerGSTUpdateBulkReminderMails
{
    public class EncryptDecrypt
    {
        public static string Encrypt(string EncryptText, string Key)
        {
            if (!string.IsNullOrEmpty(EncryptText) && Key == "MAGTPED$998#540@1")
            {
                string EncryptionKey = "MAGTPED$998#540@1";
                byte[] clearBytes = Encoding.Unicode.GetBytes(EncryptText);
                using (Aes encryptor = Aes.Create())
                {
                    Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                    encryptor.Key = pdb.GetBytes(32);
                    encryptor.IV = pdb.GetBytes(16);
                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                        {
                            cs.Write(clearBytes, 0, clearBytes.Length);
                            cs.Close();
                        }
                        EncryptText = Convert.ToBase64String(ms.ToArray());
                    }
                }
            }

            return EncryptText;
        }

        public static string Decrypt(string DecryptText, string Key)
        {
            try
            {
                if (!string.IsNullOrEmpty(DecryptText) && Key == "MAGTPED$998#540@1")
                {
                    string EncryptionKey = "MAGTPED$998#540@1";
                    byte[] cipherBytes = Convert.FromBase64String(DecryptText);
                    using (Aes encryptor = Aes.Create())
                    {
                        Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                        encryptor.Key = pdb.GetBytes(32);
                        encryptor.IV = pdb.GetBytes(16);
                        using (MemoryStream ms = new MemoryStream())
                        {
                            using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                            {
                                cs.Write(cipherBytes, 0, cipherBytes.Length);
                                cs.Close();
                            }
                            DecryptText = Encoding.Unicode.GetString(ms.ToArray());
                        }
                    }
                }
            }
            catch (Exception)
            {
                return string.Empty;
            }

            return DecryptText;
        }

        public static string GetMD5Hash(string input, string Key)
        {
            if (Key == "MAGTPED$998#540@1")
            {
                // Use input string to calculate MD5 hash
                MD5 md5 = System.Security.Cryptography.MD5.Create();
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Convert the byte array to hexadecimal string
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString().Substring(0, 25).Insert(5, "-").Insert(11, "-").Insert(17, "-").Insert(23, "-");
            }
            else
            {
                return string.Empty;
            }
        }

        public static string EncryptLicense(string Txt, string Key)
        {
            string RawHash = "";
            string TheKey = "";

            if (Key == "MAGTPED$998#540@1")
            {
                string AlphaNum = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789" + "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789" + "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789" + "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789" + "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789" + "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789" + "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789" + "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
                SHA256Managed sha = new SHA256Managed();
                ASCIIEncoding ae = new ASCIIEncoding();
                byte[] Hash = sha.ComputeHash(ae.GetBytes(Txt));
                int ndx = 0;
                int charsProcessed = 0;
                for (ndx = 0; ndx <= Hash.Length - 1; ndx++)
                {
                    RawHash += Hash[ndx].ToString("X2");
                }
                for (ndx = 0; ndx <= RawHash.Length - 1; ndx += 2)
                {
                    TheKey += AlphaNum.Substring(int.Parse(RawHash.Substring(ndx, 2), System.Globalization.NumberStyles.AllowHexSpecifier), 1);
                    charsProcessed += 1;
                    if (charsProcessed % 5 == 0 & ndx < RawHash.Length - 2)
                    {
                        TheKey += "-";
                    }
                }
                TheKey = TheKey.Remove(29, (TheKey.Length - 29));
            }

            return TheKey;
        }
    }
}
