using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace ConsoleApp2
{
    class Program   
    {

        static void Main(string[] args)
        {
           // Sukuria nauja AES sifravimo algoritmo egzemplioriu
            
            Aes aes = Aes.Create();
            aes.KeySize = 128;
            aes.GenerateIV();
            
            //Iveda teksta
            Console.WriteLine("Iveskite teksta");
            string Text = Console.ReadLine();
            Console.WriteLine("Iveskite rakta");
            byte[] key = Convert.FromBase64String(Console.ReadLine());
            aes.Key = key;

            Console.WriteLine("Tekstas: {0}", Text);

            Console.WriteLine("Pasirinkite:");
            Console.WriteLine("1. Uzsifruoti");
            Console.WriteLine("2. Desifruoti");
            int a = Convert.ToInt32(Console.ReadLine());

            if(a == 1)
            {
                // Uzsifruoja teksta ir saugoja faile
                byte[] encryptedBytes = Encrypt(Encoding.UTF8.GetBytes(Text), aes.Key, aes.IV);
                File.WriteAllBytes("encrypted.txt", encryptedBytes);
                Console.WriteLine("Uzsifruotas tekstas: {0}", Convert.ToBase64String(encryptedBytes));
            }
            else if (a == 2)
            {
                // Paima teksta is failo ir desifruoja
                byte[] decryptedBytes = Decrypt(File.ReadAllBytes("encrypted.txt"), aes.Key, aes.IV);
                string decryptedText = Encoding.UTF8.GetString(decryptedBytes);
                Console.WriteLine("Desifruotas tekstas: {0}", decryptedText);
            }
           
        }

        static byte[] Encrypt(byte[] data, byte[] key, byte[] iv)
        {
           
            using (Aes aes = Aes.Create())
            {
            Console.WriteLine("Pasirinkite sifravimo moda (1. CBC, 2.ECB)");
            int c = Convert.ToInt32(Console.ReadLine());
            if (c == 1)
            {
                    aes.Mode = CipherMode.CBC;
            }
            else if (c == 2)
            {
                    aes.Mode = CipherMode.ECB;
            }
            
                aes.Padding = PaddingMode.PKCS7;
                aes.KeySize = 128;
                aes.Key = key;
                aes.IV = iv;

                using (MemoryStream ms = new MemoryStream())
                using (ICryptoTransform encryptor = aes.CreateEncryptor())
                using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                {
                    cs.Write(data, 0, data.Length);
                    cs.FlushFinalBlock();
                    return ms.ToArray();
                }
            }
        }

        static byte[] Decrypt(byte[] data, byte[] key, byte[] iv)
        {
           
            using (Aes aes = Aes.Create())
            {
                Console.WriteLine("Pasirinkite sifravimo moda (1.CBC, 2.ECB)");
                int c = Convert.ToInt32(Console.ReadLine());
                if (c == 1)
                {
                    aes.Mode = CipherMode.CBC;
                 }
                    else if (c == 2)
                     {
                    aes.Mode = CipherMode.ECB;
                    }
                
                aes.Padding = PaddingMode.PKCS7;
                aes.KeySize = 128;
                aes.Key = key;
                aes.IV = iv;

                using (MemoryStream ms = new MemoryStream())
                using (ICryptoTransform decryptor = aes.CreateDecryptor())
                using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Write))
                {
                    cs.Write(data, 0, data.Length);
                    cs.FlushFinalBlock();
                    return ms.ToArray();
                }
            }
        }
    }
}
