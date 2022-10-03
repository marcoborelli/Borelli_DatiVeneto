using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.IO;

namespace Borelli_DatiVeneto
{
    public partial class Form1 : Form
    {
        string filename = @"veneto_verona.csv";
        public Form1()
        {
            InitializeComponent();
            textBox1.ReadOnly = textBox2.ReadOnly = textBox3.ReadOnly = textBox4.ReadOnly = textBox5.ReadOnly = textBox6.ReadOnly = textBox7.ReadOnly = textBox8.ReadOnly = textBox9.ReadOnly = textBox10.ReadOnly = textBox11.ReadOnly = textBox12.ReadOnly = textBox13.ReadOnly = true;
        }

        private void textBox14_TextChanged(object sender, EventArgs e)
        {
            int pos = -1;

            if (textBox14.Text.ToUpper() != "")
            {
                int numm = TrovaNUMM(filename, '\n');
                pos = Posizione(textBox14.Text.ToUpper(), @"veneto_verona.csv");
                var f = new FileStream(filename, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                BinaryReader reader = new BinaryReader(f);
                if (pos != -1)//se non ha trovato nessun risultato
                {
                    f.Seek(pos, SeekOrigin.Begin);

                    string tempp = Encoding.ASCII.GetString(reader.ReadBytes(numm)); //lo rileggo
                    string[] fields = tempp.Split(';');

                    TextBox[] provaPerOttimizzare = new TextBox[] { textBox1, textBox2, textBox3, textBox4, textBox5, textBox6, textBox7, textBox8, textBox9, textBox10, textBox11, textBox12, textBox13 };
                    int[] indiciValidi = new int[] { 0, 1, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14 };//sono solo gli indici della split con dei campi utili da stampare

                    for (int t = 0; t < provaPerOttimizzare.Length; t++)
                        provaPerOttimizzare[t].Text = fields[indiciValidi[t]];
                }
                f.Close();
            }
            else//sennò cancello
                textBox1.Text = textBox2.Text = textBox3.Text = textBox4.Text = textBox5.Text = textBox6.Text = textBox7.Text = textBox8.Text = textBox9.Text = textBox10.Text = textBox11.Text = textBox12.Text = textBox13.Text = null;
        }
        public static int TrovaNUMM(string filename,char ultimo)
        {
            string linetot = "";
            var f = new FileStream(filename, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            BinaryReader reader = new BinaryReader(f);

            while (true) //trovo quanto è lunga una riga
            {
                byte[] temp = reader.ReadBytes(1);
                linetot += Encoding.ASCII.GetString(temp);
                if (temp[0] == '\n')//perchè ultimo carattere è andare a capo
                    break;
            }

            f.Close();

            return linetot.Length;
        }
        public static int Posizione(string testo,string filename)
        {
            int numm = TrovaNUMM(filename, '\n');

            var f = new FileStream(filename, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            BinaryReader reader = new BinaryReader(f);

            f.Seek(0, SeekOrigin.Begin);

            int m, i = 0, j = Convert.ToInt32(f.Length / numm), pos = -1;// m=inizio, j=fine

            do //trovo il primo record
            {
                m = (i + j) / 2;//così trovo solo la riga intermedia, non la posizione di byte intermedia perchè sennò potrei finire in mezzo a una riga a caso
                f.Seek(m * numm, SeekOrigin.Begin);

                string temp = Encoding.ASCII.GetString(reader.ReadBytes(numm));

                if (myCompare(temp.Split(';')[0], testo) == 0)
                    pos = m * numm;
                else if (myCompare(temp.Split(';')[0], testo) == -1)
                    i = m + 1;
                else
                    j = m - 1;

            } while (i <= j && pos == -1);
            f.Close();
            return pos;
        }
        static int myCompare(string stringa1, string stringa2)
        {
            if (stringa1 == stringa2)//0=sono uguali 1=stringa viene prima -1=stringa viene dopo
                return 0;

            char[] char1 = stringa1.ToCharArray();
            char[] char2 = stringa2.ToCharArray();
            int l = char1.Length;
            if (char2.Length < l)//in l c'è la lunghezza più piccola
                l = char2.Length;

            for (int i = 0; i < l; i++)
            {
                if (char1[i] < char2[i])
                    return -1;
                if (char1[i] > char2[i])
                    return 1;
            }

            return 0;//visto che qui non mi interessa lunghezza allora mi basta che la prima parte si uguale
        }

    }
}
