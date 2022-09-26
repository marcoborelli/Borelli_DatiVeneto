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
        public Form1()
        {
            InitializeComponent();
            textBox1.ReadOnly = textBox2.ReadOnly = textBox3.ReadOnly = textBox4.ReadOnly = textBox5.ReadOnly = textBox6.ReadOnly = textBox7.ReadOnly = textBox8.ReadOnly = textBox9.ReadOnly = textBox10.ReadOnly = textBox11.ReadOnly = textBox12.ReadOnly = textBox13.ReadOnly = true;
        }

        private void textBox14_TextChanged(object sender, EventArgs e)
        {
            string testo = textBox14.Text.ToUpper();
            var f = new FileStream(@"veneto_verona.csv", FileMode.Open, FileAccess.ReadWrite);
            BinaryReader reader = new BinaryReader(f);

            f.Seek(0, SeekOrigin.Begin);
            string linetot = "";
            bool helo = true;

            while (helo) //trovo quanto è lunga una riga
            {
                byte[] temp = reader.ReadBytes(1);
                linetot += Encoding.ASCII.GetString(temp);
                if (temp[0] == '\n')
                    helo = false;
            }

            int numm = linetot.Length;
            int m, i = 0, j = Convert.ToInt32(f.Length / numm), pos = -1;// m=inizio, j=fine

            do //trovo il primo record
            {
                m = (i + j) / 2;//così trovo solo la riga intermedia, non la posizione di byte intermedia
                f.Seek(m * numm, SeekOrigin.Begin);

                string temp = Encoding.ASCII.GetString(reader.ReadBytes(numm));

                if (myCompare(temp.Split(';')[0], testo) == 0)
                    pos = m*numm;
                else if (myCompare(temp.Split(';')[0], testo) == -1)
                    i = m + 1;
                else
                    j = m - 1;

            } while (i <= j && pos == -1);

            f.Seek(pos, SeekOrigin.Begin);
            string tempp = Encoding.ASCII.GetString(reader.ReadBytes(numm)); //lo rileggo

            string[] fields = tempp.Split(';');

            textBox1.Text = fields[0];
            textBox2.Text = fields[1];
            textBox3.Text = fields[3];
            textBox4.Text = fields[4];
            textBox5.Text = fields[6];
            textBox6.Text = fields[7];
            textBox7.Text = fields[8];
            textBox8.Text = fields[9];
            textBox9.Text = fields[10];
            textBox10.Text = fields[11];
            textBox11.Text = fields[12];
            textBox12.Text = fields[13];
            textBox13.Text = fields[14];

            f.Close();

        }
        static int myCompare(string stringa1, string stringa2)
        {
            if (stringa1 == stringa2)//0=sono uguali -1=stringa viene prima 1=stringa viene dopo
                return 0;

            char[] char1 = stringa1.ToCharArray();
            char[] char2 = stringa2.ToCharArray();
            int l = char1.Length;
            if (char2.Length < l)
                l = char2.Length;
            //in l c'è la lunghezza più piccola

            for (int i = 0; i < l; i++)
            {
                if (char1[i] < char2[i])
                    return -1;
                if (char1[i] > char2[i])
                    return 1;
            }

            return 0;//visto che qui non mi interessa lunghezza allora mi basta ch la prima parte si uguale
        }

    }
}
