using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace BodolServer {
    class Program {
        const int PORTSERVER = 1234;
        const string IPSERVER = "127.0.0.1";

        static void Main(string[] args) {
            Console.Write("Port Server <" + PORTSERVER + ">: ");
            string PORTSERVERDiinput = Console.ReadLine();
            if (PORTSERVERDiinput == "") PORTSERVERDiinput = PORTSERVER.ToString();

            Console.WriteLine("\n-------------------------------------");
            Console.WriteLine("Jadwal kuliah Semester 1 LTI-2015 ITB");
            Console.WriteLine("-------------------------------------");
            Console.WriteLine("Rio Bastian/23215037\n\n");
            Hashtable JadwalKuliah = new Hashtable();
            JadwalKuliah.Add("EL5118", "Sistem dan Manajemen Teknologi Informasi");
            JadwalKuliah.Add("EL5117", "Service Oriented Architecture");
            JadwalKuliah.Add("EL5121", "Object Oriented Programming");
            JadwalKuliah.Add("EL5123", "Rekayasa Proses Bisnis");
            JadwalKuliah.Add("EL5000", "Matematika Lanjut");
            foreach (DictionaryEntry MataKuliah in JadwalKuliah) {
                Console.WriteLine(MataKuliah.Key + ": " + MataKuliah.Value);
            }
            Console.WriteLine("-------------------------------------");
            
            TcpListener listener = new TcpListener(IPAddress.Parse(IPSERVER), int.Parse(PORTSERVERDiinput));
            listener.Start();
            Console.WriteLine("\nSERVER berjalan di " + IPSERVER.ToString() + " port " + PORTSERVERDiinput +"\n");
            // Saat request dari klien muncul, TcpListener akan mengirimkan Socket
            Socket socket;
            while (true) {
                socket = listener.AcceptSocket();
                // Tampilkan client yang terhubung
                Console.WriteLine(socket.RemoteEndPoint + " terhubung.");
                try {
                    Stream stream = new NetworkStream(socket);
                    StreamReader streamReader = new StreamReader(stream);
                    StreamWriter streamWriter = new StreamWriter(stream);
                    streamWriter.AutoFlush = true;
                    StringBuilder line;
                    while (true) {
                        string KodeDicari = streamReader.ReadLine();
                        Console.WriteLine(socket.RemoteEndPoint + "> " + KodeDicari);
                        if (KodeDicari == "" || KodeDicari == null) break;
                        string MataKuliahDicari;
                        line = new StringBuilder();
                        if (JadwalKuliah.ContainsKey(KodeDicari.ToUpper())) {
                            MataKuliahDicari = KodeDicari.ToUpper() + " -> " + JadwalKuliah[KodeDicari.ToUpper()].ToString();
                            //streamWriter.WriteLine(MataKuliahDicari);
                            try {
                                line.AppendLine(MataKuliahDicari);

                                StreamReader file = new StreamReader(KodeDicari+".txt");
                                do {
                                    line.AppendLine(file.ReadLine());
                                } while (file.Peek() != -1);
                                streamWriter.Write(line);
                                file.Close();
                            }
                            catch (Exception e) {
                                streamWriter.WriteLine(e.Message);
                            }
                        }
                        else {
                            MataKuliahDicari = "Tidak ada mata kuliah dengan kode " + KodeDicari;
                            streamWriter.WriteLine(MataKuliahDicari);
                            
                        }
                        

                        
                        //// Suspend the screen.
                        //Console.ReadLine();
                    }
                    stream.Close();
                }
                catch (Exception e) {
                    //StreamWriter.WriteLine(e.Message);
                }
                Console.WriteLine("Koneksi " + socket.RemoteEndPoint + " terputus.");
            }
        }
    }
}
