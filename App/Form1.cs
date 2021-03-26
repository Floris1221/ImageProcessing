using ImageProcessing;
using System;
using System.Drawing;
using System.Windows.Forms;


namespace App
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            imgProces = new ImagePr();
            radioButton1.Checked = true;
        }

        public void button1_Click(object sender, EventArgs e)
        {
            this.openFileDialog1.CheckFileExists = false;
            this.openFileDialog1.ShowDialog();
            try
            {
                //Load image into picture box
                this.pictureBox1.Load(this.openFileDialog1.FileName);
                //Load image into ImagePr object
                imgProces.LoadImage(Image.FromFile(this.openFileDialog1.FileName));
            }
            catch (System.IO.FileNotFoundException ex2)
            {
                MessageBox.Show(ex2.Message);
            }
            catch (Exception ex1)
            {
                MessageBox.Show(ex1.Message);
            }

            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //Chceck check box if call method synchronous or asynchronous
            if (checkBox1.Checked)
            {
                synch = 1;
            }
            else
            {
                synch = 0;
            }
            // load new image into picturebox
            pictureBox2.Image = imgProces.ToMainColors(synch);
            //write down processing time 
            label1.Text ="Processing Time: " + imgProces.elapsedTime;

            button3.Enabled = true;
            



        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {

            if (radioButton1.Checked)
            {
                radioButton2.Checked = false;
                radioButton3.Checked = false;
            }
            if (radioButton2.Checked)
            {
                radioButton1.Checked = false;
                radioButton3.Checked = false;
            }
            if (radioButton3.Checked)
            {
                radioButton1.Checked = false;
                radioButton2.Checked = false;
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                imgProces.SaveImage("test", "BMP");
            }
            else if (radioButton2.Checked)
            {
                imgProces.SaveImage("test", "JPG");
            }
            else if (radioButton3.Checked)
            {
                imgProces.SaveImage("test", "PNG");
            }
            
        }
    }
}
