using System;
using System.IO;
using System.Net;
namespace CSharp_LogErros
{
    class GravaLog
    {
        public void grava(String texto)
        {
            //using (StreamWriter outputFile = new StreamWriter("c:\\temp\\MyTest.txt", true))
            using (StreamWriter outputFile = new StreamWriter("Telegram.log", true))
            {
                String data = DateTime.Now.ToShortDateString();
                String hora = DateTime.Now.ToShortTimeString();
                String computador = Dns.GetHostName();
                outputFile.WriteLine(data + " " + hora + " (" + computador + ")" + texto);
            }
        }
    }
}