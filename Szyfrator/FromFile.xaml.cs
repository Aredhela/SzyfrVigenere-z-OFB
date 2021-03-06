﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Szyfrator
{
    /// <summary>
    /// Interaction logic for FromFile.xaml
    /// </summary>
    public partial class FromFile : Window
    {
        public const int AlphabetLength = LastAlphabetCode - FirstAlphabetCode + 1;
        public const int LastAlphabetCode = '~';
        public const int FirstAlphabetCode = ' ';


        public FromFile()
        {
            InitializeComponent();
        }

        private void Button_encrypt_File_Click(object sender, RoutedEventArgs e)
        {
            int rozmiar_bloku = 16;
            string wektor_inicjujacy = "qwertyuiopasdfgh";
            TextBox_encrypted_File.Text = ofb(TextBox_decrypted_File.Text, TextBox_Password.Text, wektor_inicjujacy, rozmiar_bloku);
        }

        public static string Encryption(string encrypted, string key)
        {
            var encryptedBytes = new byte[encrypted.Length];
            Encryption(Encoding.ASCII.GetBytes(encrypted), encryptedBytes, new RingKey(key));
            return Encoding.ASCII.GetString(encryptedBytes);
        }

        public static void Encryption(byte[] input, byte[] output, RingKey key)
        {
            for (var i = 0; i < input.Length; i++)
            {
                if (input[i] == '\n' || input[i] == '\r' || input[i] == ' ')
                {
                    output[i] = input[i];
                    continue;
                }
                var encryptedChar = (byte)(input[i] - FirstAlphabetCode) + key.Current;
                var decryptedChar = (byte)(encryptedChar < AlphabetLength ? encryptedChar : (encryptedChar - AlphabetLength));
                output[i] = (byte)(decryptedChar + FirstAlphabetCode);
                key.MoveNext();
            }
        }

        static string ofb(string tekst, string klucz, string wektor_inicjujacy, int rozmiar_bloku)
        {
            int liczba_blokow = (int)Math.Ceiling(((double)tekst.Length) / rozmiar_bloku);
            string blok_wejsciowy = "";
            string szyfrogram = "";
            blok_wejsciowy = wektor_inicjujacy;

            for (int nr_bloku = 0; nr_bloku < liczba_blokow; ++nr_bloku)
            {
                string blok_wyjsciowy = Encryption(blok_wejsciowy, klucz);
                int koniec_bloku = Math.Min(tekst.Length, (nr_bloku + 1) * rozmiar_bloku);
                string blok_szyfrogramu = "";
                int i = 0;

                for (int nr_znaku = nr_bloku * rozmiar_bloku; nr_znaku < koniec_bloku; ++nr_znaku)
                {
                    blok_szyfrogramu += (char)(((int)(tekst[nr_znaku]) ^ (int)(blok_wyjsciowy[i])));
                    i++;
                }
                blok_wejsciowy = blok_wyjsciowy;
                szyfrogram += blok_szyfrogramu;
            }
            return szyfrogram;
        }

        private void Button_BackToMain_File_Click(object sender, RoutedEventArgs e)
        {
            var Wmainwindow = new MainWindow();
            Wmainwindow.Show();
            this.Close();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Button_Read(object sender, RoutedEventArgs e)
        {
            try
            {
                using (StreamReader sr = new StreamReader("G:\\Users\\Szyfrator - POD\\Test1.txt"))
                {
                    String line = sr.ReadToEnd();
                    TextBox_decrypted_File.Text = line;
                }
            }
            catch (Exception ex)
            {
                TextBox_decrypted_File.Text = "NIE MOZNA ODCZYTAC PLIKU !";
            }
        }

        private void Button_Read2(object sender, RoutedEventArgs e)
        {
            try
            {
                using (StreamReader sr = new StreamReader("G:\\Users\\Szyfrator - POD\\Test1.txt"))
                {
                    String line = sr.ReadToEnd();
                    TextBox_encrypted_File.Text = line;
                }
            }
            catch (Exception ex)
            {
                TextBox_encrypted_File.Text = "NIE MOZNA ODCZYTAC PLIKU !";
            }
        }

        private void Button_decrypt_File_Click(object sender, RoutedEventArgs e)
        {
            int rozmiar_bloku = 16;
            string wektor_inicjujacy = "qwertyuiopasdfgh";
            TextBox_decrypted_File.Text = ofb(TextBox_encrypted_File.Text, TextBox_Password.Text, wektor_inicjujacy, rozmiar_bloku);
        }

        private void TextBox_Password_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
