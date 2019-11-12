using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Numerics;

namespace protect_inf_LR1
{
    public partial class Form1 : Form
    {
        char[] characters = new char[] { '#', 'А', 'Б', 'В', 'Г', 'Д', 'Е', 'Ё', 'Ж', 'З', 'И',
                                                        'Й', 'К', 'Л', 'М', 'Н', 'О', 'П', 'Р', 'С', 
                                                        'Т', 'У', 'Ф', 'Х', 'Ц', 'Ч', 'Ш', 'Щ', 'Ь', 'Ы', 'Ъ',
                                                        'Э', 'Ю', 'Я', ' ', '1', '2', '3', '4', '5', '6', '7',
                                                        '8', '9', '0' };


        public Form1()
        {
            InitializeComponent();
        }

        //зашифровать
        private void buttonEncrypt_Click(object sender, EventArgs e)
        {
            string lines = textBox1.Text;
            string writePath = "in.txt";


            using (StreamWriter sw = new StreamWriter(writePath, false, System.Text.Encoding.UTF8))
            {
                sw.WriteLine(lines);
            }
            if ((textBox_p.Text.Length > 0) && (textBox_q.Text.Length > 0))
            {
                long p = Convert.ToInt64(textBox_p.Text);
                long q = Convert.ToInt64(textBox_q.Text);
                long eo = Convert.ToInt64(textBox4.Text);
                
                if (IsTheNumberSimple(p) && IsTheNumberSimple(q) && IsTheNumberSimple(eo))
                {
                    string s = "";

                    StreamReader sr = new StreamReader("in.txt");

                    while (!sr.EndOfStream)
                    {
                        s += sr.ReadLine();
                    }

                    sr.Close();

                    s = s.ToUpper();

                    
                    long n = p * q;
                    long m = (p - 1) * (q - 1);
                    
                    long i = calculate_i(m, eo);
                    long d = (((m*i)+1)/eo);
                    
                    
                    List<string> result = RSA_Endoce(s, eo, n);

                    StreamWriter sw = new StreamWriter("out1.txt");
                    foreach (string item in result)
                        sw.WriteLine(item);
                    sw.Close();

                    textBox_d.Text = d.ToString();
                    textBox_n.Text = n.ToString();
                    textBox5.Text = i.ToString();
                    textBox6.Text = n.ToString();
                    textBox7.Text = m.ToString();
                    if (textBox1.Text.Length > 0)
                    {
                        textBox2.Text = File.ReadAllText("out1.txt", Encoding.UTF8);
                        if (eo < n)  {
                        if (textBox4.Text == "") { MessageBox.Show("Упс, ошибка!Введите число !"); }
                                
                                
                        }
                        else
                            MessageBox.Show("Упс, ошибка!Введите число в диапазоне!");
                    }
                    else
                        MessageBox.Show("Введите текст для шифрования!");
                }
                else
                    MessageBox.Show("p или q или е - не простые числа");
            }
            else
                MessageBox.Show("Введите p и q");
        }

        //расшифровать
        private void buttonDecipher_Click(object sender, EventArgs e)
        {
            if ((textBox_d.Text.Length > 0) && (textBox_n.Text.Length > 0))
            {
                long d = Convert.ToInt64(textBox_d.Text);
                long n = Convert.ToInt64(textBox_n.Text);

                List<string> input = new List<string>();

                StreamReader sr = new StreamReader("out1.txt");

                while (!sr.EndOfStream)
                {
                    input.Add(sr.ReadLine());
                }

                sr.Close();

                string result = RSA_Dedoce(input, d, n);

                StreamWriter sw = new StreamWriter("out2.txt");
                sw.WriteLine(result);
                sw.Close();

                MessageBox.Show(result, "Результат расшифровки!");
            }
            else
                MessageBox.Show("Введите секретный ключ!");
        }

        //проверка: простое ли число?
        private bool IsTheNumberSimple(long n)
        {
            if (n < 2)
                return false;

            if (n == 2)
                return true;

            for (long i = 2; i < n; i++)
                if (n % i == 0)
                    return false;

            return true;
        }

        //зашифровать
        private List<string> RSA_Endoce(string s, long eo, long n)
        {
            List<string> result = new List<string>();

            BigInteger bi;

            for (int i = 0; i < s.Length; i++)
            {
                int index = Array.IndexOf(characters, s[i]);

                bi = new BigInteger(index);
                bi = BigInteger.Pow(bi, (int)eo);

                BigInteger n_ = new BigInteger((int)n);

                bi = bi % n_;

                result.Add(bi.ToString());
            }

            return result;
        }

        //расшифровать
        private string RSA_Dedoce(List<string> input, long d, long n)
        {
            string result = "";

            BigInteger bi;

            foreach (string item in input)
            {
                bi = new BigInteger(Convert.ToDouble(item));
                bi = BigInteger.Pow(bi, (int)d);

                BigInteger n_ = new BigInteger((int)n);

                bi = bi % n_;

                int index = Convert.ToInt32(bi.ToString());

                result += characters[index].ToString();
            }

            return result;
        }

        
        private long calculate_i(long m, long eo)
        {
            long i=0;
            long c=(((m* i) +1) % eo);


            while (c != 0)
                {
                i++;
                c = (((m * i) + 1) % eo);
                
            }

           
            
            
           return i;
        }

        private void button1_Click(object sender, EventArgs e)
        {

            string lines = textBox1.Text;
            string writePath = "in.txt";


            using (StreamWriter sw = new StreamWriter(writePath, false, System.Text.Encoding.UTF8))
            {
                sw.WriteLine(lines);
            }

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

       
        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            {
                string tempstr = "";
                foreach (char sym in textBox1.Text)
                {
                    if (!TextBoxСheck(sym.ToString()))
                        tempstr += sym.ToString();
                }
                textBox1.Text = tempstr;
                textBox1.SelectionStart = textBox1.TextLength;
            }

            bool TextBoxСheck(string text)
            {
                return new Regex("[^А-Яа-яёЁ0-9 ]").IsMatch(text);
            }
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            {
                string tempstr = "";
                foreach (char sym in textBox4.Text)
                {
                    if (!TextBoxСheck(sym.ToString()))
                        tempstr += sym.ToString();
                }
                textBox4.Text = tempstr;
                textBox4.SelectionStart = textBox4.TextLength;
            }

            bool TextBoxСheck(string text)
            {
                return new Regex("[^0-9]").IsMatch(text);
            }
            int db1, db, db2, db3;
            try
            {
                db1 = int.Parse(textBox_p.Text);
                db3 = int.Parse(textBox_q.Text);
                db2 = int.Parse(textBox4.Text);
                db = db1 * db3;
                if (db2 > db - 1) { textBox4.Text = ""; }
            }
            catch (FormatException ex)
            { }


        }


        private void textBox_p_TextChanged(object sender, EventArgs e)
        {
            {
                string tempstr = "";
                foreach (char sym in textBox_p.Text)
                {
                    if (!TextBoxСheck(sym.ToString()))
                        tempstr += sym.ToString();
                }
                textBox_p.Text = tempstr;
                textBox_p.SelectionStart = textBox_p.TextLength;
            }
            int db1, db2, db3;
            try
            {
                db1 = int.Parse(textBox_p.Text);
                db3 = int.Parse(textBox_q.Text);

                db2 = db1 * db3;
                textBox3.Text = "0 < e < " + db2.ToString();
            }
            catch (FormatException ex)
            { }
            bool TextBoxСheck(string text)
            {
                return new Regex("[^0-9]").IsMatch(text);
            }
        }

        private void textBox_q_TextChanged(object sender, EventArgs e)
        {
            {
                string tempstr = "";
                foreach (char sym in textBox_q.Text)
                {
                    if (!TextBoxСheck(sym.ToString()))
                        tempstr += sym.ToString();
                }
                textBox_q.Text = tempstr;
                textBox_q.SelectionStart = textBox_q.TextLength;
            }
            int db1, db2, db3;
            try
            {
                db1 = int.Parse(textBox_p.Text);
                db3 = int.Parse(textBox_q.Text);
            
            db2 = db1 * db3;
            textBox3.Text = "0 < e < " + db2.ToString();
            }
            catch (FormatException ex)
            {  }

            bool TextBoxСheck(string text)
            {
                return new Regex("[^0-9]").IsMatch(text);
            }
            
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void label13_Click(object sender, EventArgs e)
        {

        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {

        }
    }
    }

