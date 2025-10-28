using System;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace IAM_Library.Utility
{
    public class IAM_Maintenance
    {
        public static string Decrypt(string cipherText, byte[] Key, byte[] IV)
        {
            using (var aes = Aes.Create())
            {
                aes.Key = Key;
                aes.IV = IV;

                using (var decryptor = aes.CreateDecryptor(aes.Key, aes.IV))
                using (var ms = new MemoryStream(Convert.FromBase64String(cipherText)))
                using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                using (var sr = new StreamReader(cs))
                {
                    return sr.ReadToEnd();
                }
            }
        }
        
            private const string EncryptionKey = "";

            public async Task SendExceptionEmailAsync(string encryptedRecipientEmail, string subject, string message, Exception exception)
            {
                var supportMail = "@gmail.com";
                var pass = "";
                try
                {
                    string recipientEmail = DecryptString(encryptedRecipientEmail);

                    var smtpClient = new SmtpClient("smtp.gmail.com")
                    {
                        Port = 587,
                        Credentials = new NetworkCredential(supportMail, pass),
                        EnableSsl = true,
                    };

                    var mailMessage = new MailMessage
                    {
                        From = new MailAddress(supportMail),
                        Subject = subject,
                        Body = message + "\n\nException Details:\n\n" + $"Exception Message: {exception.Message} \n\n " + exception.ToString(),
                        IsBodyHtml = false,
                        Priority = MailPriority.High,
                    };

                    mailMessage.To.Add(recipientEmail);

                    await smtpClient.SendMailAsync(mailMessage);
                }
                catch (Exception ex)
                {

                    Console.WriteLine("Failed to send email: " + ex.Message);
                }
            }

            private string DecryptString(string encryptedText)
            {
                byte[] encryptedBytes = Convert.FromBase64String(encryptedText);
                byte[] keyBytes = Encoding.UTF8.GetBytes(EncryptionKey);
                using (var aesAlg = Aes.Create())
                {
                    using (var decryptor = aesAlg.CreateDecryptor(keyBytes, keyBytes))
                    {
                        using (var msDecrypt = new MemoryStream(encryptedBytes))
                        {
                            using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                            {
                                using (var srDecrypt = new StreamReader(csDecrypt))
                                {
                                    return srDecrypt.ReadToEnd();
                                }
                            }
                        }
                    }
                }
            }

            private string EncryptString(string plainText)
            {
                byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);
                byte[] keyBytes = Encoding.UTF8.GetBytes(EncryptionKey);
                using (var aesAlg = Aes.Create())
                {
                    using (var encryptor = aesAlg.CreateEncryptor(keyBytes, keyBytes))
                    {
                        using (var msEncrypt = new MemoryStream())
                        {
                            using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                            {
                                csEncrypt.Write(plainBytes, 0, plainBytes.Length);
                                csEncrypt.FlushFinalBlock();
                                return Convert.ToBase64String(msEncrypt.ToArray());
                            }
                        }
                    }
                }
            }

            // Start work on IAM Maintenance EMAIL Sender to developers email

          
            public static string Obfuscate(string textToEncrypt, string password)
            {
                byte[] iv = new byte[16];
                byte[] array;

                using (Aes aes = Aes.Create())
                {
                    aes.Key = Encoding.UTF8.GetBytes(password.PadRight(32)); // Ensure key is 32 bytes
                    aes.IV = iv;

                    ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        using (CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                        {
                            using (StreamWriter streamWriter = new StreamWriter(cryptoStream))
                            {
                                streamWriter.Write(textToEncrypt);
                            }

                            array = memoryStream.ToArray();
                        }
                    }
                }

                return Convert.ToBase64String(array);
            }

           
            public static string Deobfuscate(string encryptedText, string password)
            {
                byte[] iv = new byte[16];
                byte[] buffer = Convert.FromBase64String(encryptedText);

                using (Aes aes = Aes.Create())
                {
                    aes.Key = Encoding.UTF8.GetBytes(password.PadRight(32)); // Ensure key is 32 bytes
                    aes.IV = iv;

                    ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                    using (MemoryStream memoryStream = new MemoryStream(buffer))
                    {
                        using (CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                        {
                            using (StreamReader streamReader = new StreamReader(cryptoStream))
                            {
                                return streamReader.ReadToEnd();
                            }
                        }
                    }
                }
            

            
        }

    }

}

