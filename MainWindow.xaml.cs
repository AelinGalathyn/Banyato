using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Banyato
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int[,] melysegekTomb;
        List<List<int>> melysegekLista = new List<List<int>>();
        int sorszam;
        int oszlopszam;

        private void kirajzol()
        {
            gridTo.Children.Clear();
            int meret = 15;
            int bord = 2;
            int l, r, t, b;
            for (int i = 0; i < sorszam; i++)
            {
                for (int j = 0; j < oszlopszam; j++)
                {
                    l = 0;
                    r = 0;
                    t = 0;
                    b = 0;
                    int mely = melysegekLista[i][j];
                    Button btn = new Button();
                    btn.Width = meret;
                    btn.Height = meret;
                    btn.Margin = new Thickness(j * meret, i * meret, 0, 0);
                    btn.VerticalAlignment = VerticalAlignment.Top;
                    btn.HorizontalAlignment = HorizontalAlignment.Left;
                    btn.Tag = melysegekLista[i][j];
                    btn.MouseEnter += kiir;
                    btn.MouseLeave += torol;
                    if (mely == 0)
                    {
                        btn.Background = Brushes.Green;
                    }
                    else
                    {
                        if (melysegekLista[i][j - 1] == 0)
                        {
                            l = bord;
                        }
                        if (melysegekLista[i][j + 1] == 0)
                        {
                            r = bord;
                        }
                        if (melysegekLista[i - 1][j] == 0)
                        {
                            t = bord;
                        }
                        if (melysegekLista[i + 1][j] == 0)
                        {
                            b = bord;
                        }
                        btn.BorderBrush = Brushes.Black;
                        btn.BorderThickness = new Thickness(l, t, r, b);
                        btn.Background = new SolidColorBrush(Color.FromArgb(byte.Parse((melysegekLista[i][j] * 2).ToString()), 0, 0, 255));
                    }
                    gridTo.Children.Add(btn);
                }
            }
        }

        private void torol(object sender, MouseEventArgs e)
        {
            (sender as Button).Content = "";
        }

        private void kiir(object sender, MouseEventArgs e)
        {
            (sender as Button).Content = (sender as Button).Tag;
        }

        private void minSzinez()
        {
            int mely = minMely();
            foreach (Button item in gridTo.Children)
            {
                if (Convert.ToInt32(item.Tag) == mely)
                {
                    item.Background = Brushes.Red;
                }
            }
        }

        private int minMely()
        {
            int min = 0;
            for (int i = 0; i < sorszam; i++)
            {
                for (int j = 0; j < oszlopszam; j++)
                {
                    if (melysegekLista[i][j] > min)
                    {
                        min = melysegekLista[i][j];
                    }
                }
            }
            return min;
        }

        private void atlagol()
        {
            int sum = 0;
            int meret = 0;
            for (int i = 0; i < sorszam; i++)
            {
                for (int j = 0; j < oszlopszam; j++)
                {
                    if (melysegekLista[i][j] > 0)
                    {
                        meret++;
                        sum += melysegekLista[i][j];
                    }
                }
            }
            double avg = Math.Round((double)sum / meret);
            labelAtlag.Content = $"Az átlagos tó mélység: {avg}";
            labelMeret.Content = $"A tó területe: {meret}";
        }

        private void beolvas(string f)
        {
            StreamReader sr = new StreamReader(f);
            sorszam = Convert.ToInt32(sr.ReadLine());
            oszlopszam = Convert.ToInt32(sr.ReadLine());
            melysegekTomb = new int[sorszam, oszlopszam];
            int sor = 0;
            while (!sr.EndOfStream)
            {
                string[] temp = sr.ReadLine().Split(' ');
                for (int i = 0; i < temp.Length; i++)
                {
                    melysegekTomb[sor, i] = int.Parse(temp[i]);
                }
                sor++;
            }
            sr.Close();
        }

        private void lbeolvas(string f)
        {
            StreamReader sr = new StreamReader(f);
            sorszam = Convert.ToInt32(sr.ReadLine());
            oszlopszam = Convert.ToInt32(sr.ReadLine());

            int sor = 0;
            while (!sr.EndOfStream)
            {
                List<int> list = new List<int>();
                string[] temp = sr.ReadLine().Split(' ');
                for (int i = 0; i < temp.Length; i++)
                {
                    list.Add(int.Parse(temp[i]));
                }
                melysegekLista.Add(list);
                sor++;
            }
            sr.Close();
            kirajzol();
            atlagol();
            minSzinez();
        }
        public MainWindow()
        {
            InitializeComponent();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == true)
            {
                beolvas(ofd.FileName);
                lbeolvas(ofd.FileName);
            }
        }
    }
}
