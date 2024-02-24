using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Paint
{
    public partial class Paint : Form
    {
        public Paint()
        {
            InitializeComponent();
            pictureBox.Invalidate();
            pictureBox.BorderStyle = BorderStyle.FixedSingle;
            toolStrip.ImageScalingSize = new Size(32, 32);
            pictureBox.Top = 100;
            pictureBox.Width = this.Width-40;
            pictureBox.Height = this.Height - 150;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }


        private void Paint_Resize(object sender, EventArgs e)
        {
            pictureBox.Width = this.Width - 40;
            pictureBox.Height = this.Height - 150;
        }

 

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Images|*.png;*.bmp;*.jpg";
            ImageFormat format = ImageFormat.Png;
            if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string ext = System.IO.Path.GetExtension(sfd.FileName);
                switch (ext)
                {
                    case ".jpg":
                        format = ImageFormat.Jpeg;
                        break;
                    case ".png":
                        format = ImageFormat.Png;
                        break;
                }
                if (pictureBox.Image != null)
                {
                    pictureBox.Image.Save(sfd.FileName, format);
                }
                else
                {
                    using (Bitmap emptyImage = new Bitmap(pictureBox.Width, pictureBox.Height))
                    {
                        // Save the empty image
                        emptyImage.Save(sfd.FileName);
                    }
                }
                
            }
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pictureBox.Image = null;
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var dialog = new OpenFileDialog();

            dialog.Title = "Open Image";
            dialog.Filter = "Image Files (*.jpg, *.jpeg, *.png, *.bmp)|*.jpg;*.jpeg;*.png;*.bmp|All files (*.*)|*.*";

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                pictureBox.Image = Image.FromFile(dialog.FileName);
            }

            dialog.Dispose();
        }

        private void printToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (PrintDialog printDialog = new PrintDialog())
            {
                printDialog.Document = printDocument;
                if (printDialog.ShowDialog() == DialogResult.OK)
                {
                    printDocument.Print();
                }
            }
        }

        private void PrintDocument_PrintPage(object sender, PrintPageEventArgs e)
        {
            // Get the image from PictureBox
            Image imageToPrint = pictureBox.Image;

            // Draw the image on the printer graphics
            if (imageToPrint != null)
            {
                e.Graphics.DrawImage(imageToPrint, e.PageBounds);
            }
            else
            {
               
                using (Bitmap emptyImage = new Bitmap(pictureBox.Width, pictureBox.Height))
                {
                // Save the empty image
                e.Graphics.DrawImage(emptyImage, e.PageBounds);
                }
            }
        }


        private void pictureBox_Click(object sender, EventArgs e)
        {
           
        }

        private void pictureBox_Paint(object sender, PaintEventArgs e)
        {
           
            //ControlPaint.DrawBorder(e.Graphics, pictureBox.ClientRectangle, Color.Black, ButtonBorderStyle.Solid);
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {

        }

        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }


        //pentool
        private void toolStripButton_Click(object sender, EventArgs e)
        {

        }

        //color picker
        private void toolStripButton5_Click(object sender, EventArgs e)
        {
        if (colorDialoge.ShowDialog() == DialogResult.OK)
            {
                // Set the ForeColor of label1 to the selected color
                label1.ForeColor = colorDialoge.Color;
            }
        }


        private void label1_Click(object sender, EventArgs e)
        {

        }

        
    }
}
