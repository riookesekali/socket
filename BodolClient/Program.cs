using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace BodolClient {
    class Program {
        const int PORTSERVER = 1234;
        const string IPSERVER = "127.0.0.1";
        static TcpClient client = new TcpClient();

        static void Main(string[] args) {
                Console.WriteLine("Aplikasi Client Jadwal Kuliah");
                Console.WriteLine("-----------------------------");
                Console.WriteLine("Rio Bastian/23215037\n");
            
                string IPSERVERDiinput, PORTSERVERDiinput;
                do {
                    Console.Write("\nIP Server <" + IPSERVER + ">: ");
                    IPSERVERDiinput = Console.ReadLine();
                    if (IPSERVERDiinput == "") IPSERVERDiinput = IPSERVER;
                    Console.Write("Port Server <" + PORTSERVER + ">: ");
                    PORTSERVERDiinput = Console.ReadLine();
                    if (PORTSERVERDiinput == "") PORTSERVERDiinput = PORTSERVER.ToString();
                    
                    try {
                        client = new TcpClient(IPSERVERDiinput, int.Parse(PORTSERVERDiinput));
                    }
                    catch (Exception e) {
                        Console.WriteLine(e.Message);
                    }
                } while (client.Connected == false);
                Console.WriteLine("Terhubung dengan server di " + IPSERVERDiinput + " port " + PORTSERVERDiinput);
                Console.WriteLine();
                Stream stream = client.GetStream();
                StreamReader streamReader = new StreamReader(stream);
                StreamWriter streamWriter = new StreamWriter(stream);
                streamWriter.AutoFlush = true;
                string line;
                //Console.WriteLine(streamReader.ReadLine());
                while (true) {
                    try {
                        Console.Write("\nKode kuliah: ");
                        string KodeDicari = Console.ReadLine();
                        if (KodeDicari == "") break;
                        streamWriter.WriteLine(KodeDicari);
                        do {
                            line = streamReader.ReadLine();
                            Console.WriteLine(line);
                        } while ((streamReader.Peek() != -1));
                        
                    }
                    catch (Exception e) {
                        Console.WriteLine(e.Message);
                    }

                    //while (!streamReader.EndOfStream) {
                    //    Console.WriteLine(streamReader.ReadLine());
                    //}
                    //Console.ReadLine();
                }
                stream.Close();
            
            
            //finally {
            //    //client.Close();
            //    Console.ReadLine();
            //}
        }
    }
}
