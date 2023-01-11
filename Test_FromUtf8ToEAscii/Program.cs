using System;
using System.Text;

namespace Test_FromUtf8ToEAscii
{
    class Program
    {
        static string FromUtf8ToEAscii(byte[] utf8Bytes)
        {
            /*
             * references : 
             * https://www.ime.usp.br/~pf/algoritmos/apend/unicode.html
             * https://www.utf8-chartable.de/unicode-utf8-table.pl
             * https://bjoern.hoehrmann.de/utf-8/decoder/dfa/
             */
            string eAsciiString = "";
            for (var i = 0; i < utf8Bytes.Length; i++)
            {
                byte byteScope = utf8Bytes[i];
                if (byteScope < 0x7F)
                {
                    eAsciiString += (char)byteScope;
                }
                else if (byteScope == 0xC3)
                {
                    i++;
                    byteScope = utf8Bytes[i];
                    if (byteScope > 0x7F)
                        eAsciiString += (char)(byteScope + 0x40);
                }
                else if (byteScope == 0xC2)
                {
                    i++;
                    byteScope = utf8Bytes[i];
                    if (byteScope > 0x7F)
                        eAsciiString += (char)byteScope;
                }
            }
            return eAsciiString;
        }

        static void Main()
        {
            string sampleUtf8 = "Á test Ô sofá crachá parabéns não jacarés açaí cipós ¢ªº ?Çç$%!@#";

            byte[] utf8Bytes = Encoding.UTF8.GetBytes(sampleUtf8);
            Console.WriteLine("input utf8: " + sampleUtf8 + "\n\nBytes:");

            foreach(byte b in utf8Bytes)
            {
                Console.Write(string.Format("0x{0:X2}, ", b));
            }

            Console.WriteLine("\n");

            string resultEAscii = FromUtf8ToEAscii(utf8Bytes);

            Console.WriteLine("output ascii:" + resultEAscii + "\n\nBytes:");
            foreach (char c in resultEAscii)
            {
                Console.Write(string.Format("0x{0:X2}, ", (ushort)c));
            }
            Console.ReadKey();
        }
    }
}
