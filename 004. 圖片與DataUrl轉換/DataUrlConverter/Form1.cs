using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;
using DataUrlConverter.Properties;

namespace DataUrlConverter
{
    public partial class Form1 : Form
    {
        private MemoryStream MemoryStream { get; set; }

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.FileName = "";
            openFileDialog1.Multiselect = false;
            openFileDialog1.Filter = Resources.ImageFormat;
            var isSelected = openFileDialog1.ShowDialog() == DialogResult.OK;
            if (isSelected)
            {
                pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                pictureBox1.Load(openFileDialog1.FileName);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null)
            {
                using (var ms = new MemoryStream())
                {
                    string mimeType;
                    if (pictureBox1.Image.RawFormat.Equals(ImageFormat.Jpeg))
                    {
                        mimeType = "image/jpeg";
                    }
                    else if (pictureBox1.Image.RawFormat.Equals(ImageFormat.Png))
                    {
                        mimeType = "image/png";
                    }
                    else if (pictureBox1.Image.RawFormat.Equals(ImageFormat.Gif))
                    {
                        mimeType = "image/gif";
                    }
                    else
                    {
                        MessageBox.Show(Resources.NotImageFormat);
                        return;
                    }
                    ms.Position = 0;
                    pictureBox1.Image.Save(ms, pictureBox1.Image.RawFormat);
                    byte[] imageBytes = ms.ToArray();
                    string base64String = Convert.ToBase64String(imageBytes);
                    richTextBox1.Text = string.Format("data:{0};base64,{1}", mimeType, base64String);
                }
            }
            else
            {
                MessageBox.Show(Resources.NoImage);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = null;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                var format = richTextBox1.Text.Split(';');
                var base64 = format[1].Split(',')[1];
                var imageBytes = Convert.FromBase64String(base64);
                MemoryStream = new MemoryStream(imageBytes);
                pictureBox1.Image = Image.FromStream(MemoryStream);    
            }
            catch
            {
                MessageBox.Show(Resources.FormatError);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = "";
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null)
            {
                string saveType;
                ImageFormat imageFormat;
                if (pictureBox1.Image.RawFormat.Equals(ImageFormat.Jpeg))
                {
                    saveType = "圖檔|*.jpg";
                    imageFormat = ImageFormat.Jpeg;
                }
                else if (pictureBox1.Image.RawFormat.Equals(ImageFormat.Png))
                {
                    saveType = "圖檔|*.png";
                    imageFormat = ImageFormat.Png;
                }
                else if (pictureBox1.Image.RawFormat.Equals(ImageFormat.Gif))
                {
                    saveType = "圖檔|*.gif";
                    imageFormat = ImageFormat.Gif;
                }
                else
                {
                    MessageBox.Show(Resources.NotImageFormat);
                    return;
                }
                saveFileDialog1.Filter = saveType;
                var isSelected = saveFileDialog1.ShowDialog() == DialogResult.OK;
                if (isSelected)
                {
                    try
                    {
                        pictureBox1.Image.Save(saveFileDialog1.FileName, imageFormat);
                        MessageBox.Show(Resources.SaveCompleted);
                    }
                    catch
                    {
                        MessageBox.Show(Resources.SaveError);
                    }
                }
            }
            else
            {
                MessageBox.Show(Resources.NoImage);
            }
        }
    }
}
