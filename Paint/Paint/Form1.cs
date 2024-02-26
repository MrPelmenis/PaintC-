using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Media;

namespace Paint
{

       public partial class Paint : Form
    {
        private bool isMouseDown = false;
        private DrawingMode mode = DrawingMode.Pen;
        private Coordinates mouseCords;
        private Coordinates prevMouseCoords;
        private Bitmap drawingBitmap;
        private Graphics bitmapGraphics;
        private Coordinates previewCoords;
        private Bitmap previewBitmap;
        private Graphics previewGraphics;
        private System.Media.SoundPlayer player;


        public Paint()
        {
            InitializeComponent();
            pictureBox.Invalidate();
            pictureBox.BorderStyle = BorderStyle.FixedSingle;
            toolStrip.ImageScalingSize = new Size(32, 32);
            pictureBox.Top = 100;
            pictureBox.Width = this.Width-40;
            pictureBox.Height = this.Height - 150;
            this.previewCoords = new Coordinates(0, 0);
            this.drawingBitmap = new Bitmap(pictureBox.Width, pictureBox.Height);


            previewBitmap = new Bitmap(drawingBitmap.Width, drawingBitmap.Height);
            previewGraphics = Graphics.FromImage(previewBitmap);
            player = new System.Media.SoundPlayer(Properties.Resources.sound);

            using (Graphics g = Graphics.FromImage(drawingBitmap))
            {
               g.Clear(Color.White);
            }

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.mouseCords = new Coordinates(0, 0);
            this.prevMouseCoords = new Coordinates(0, 0);
        }

 

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            player.Play();
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
                    drawingBitmap.Save(sfd.FileName, format);
                }
                else
                {
                    using (Bitmap emptyImage = new Bitmap(pictureBox.Width, pictureBox.Height))
                    {
                        emptyImage.Save(sfd.FileName);
                    }
                }
                
            }
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            player.Play();
            pictureBox.Image = null;
            this.drawingBitmap = new Bitmap(pictureBox.Width, pictureBox.Height);
            using (Graphics g = Graphics.FromImage(drawingBitmap))
            {
                g.Clear(Color.White);
            }
            
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            player.Play();
            var dialog = new OpenFileDialog();
          
            dialog.Title = "Open Image";
            dialog.Filter = "Image Files (*.jpg, *.jpeg, *.png, *.bmp)|*.jpg;*.jpeg;*.png;*.bmp|All files (*.*)|*.*";

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    drawingBitmap = (Bitmap)Image.FromFile(dialog.FileName);
                    pictureBox.Image = drawingBitmap;
                    pictureBox.Refresh();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading image: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            dialog.Dispose();
        }

        private void printToolStripMenuItem_Click(object sender, EventArgs e)
        {
            player.Play();
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
            Image imageToPrint = pictureBox.Image;
            if (imageToPrint != null)
            {
                e.Graphics.DrawImage(imageToPrint, e.PageBounds);
            }
            else
            {
               
                using (Bitmap emptyImage = new Bitmap(pictureBox.Width, pictureBox.Height))
                {
                 e.Graphics.DrawImage(emptyImage, e.PageBounds);
                }
            }
        }


        private void pictureBox_Click(object sender, EventArgs e)
        {
        
        }

        private void pictureBox_Paint(object sender, PaintEventArgs e)
        {
                e.Graphics.DrawImage(drawingBitmap, 0, 0);

                e.Graphics.DrawImage(previewBitmap, 0, 0);
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


        private void toolStripButton_Click(object sender, EventArgs e)
        {
            this.mode = DrawingMode.Pen;
        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
        if (colorDialoge.ShowDialog() == DialogResult.OK)
            {
                
            }
        }


        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void Paint_MouseDown(object sender, MouseEventArgs e)
        {
            
        }

        private void pictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            isMouseDown = true;
            this.previewCoords.x = e.X;
            this.previewCoords.y = e.Y;
            this.mouseCords.x = e.X;
            this.mouseCords.y = e.Y;
        }

        private void penTimer_Tick(object sender, EventArgs e)
        {
            
        }

        private void pictureBox_MouseUp(object sender, MouseEventArgs e)
        {
            isMouseDown = false;

            if(mode == DrawingMode.Line)
            {
                using (Graphics bitmapGraphics = Graphics.FromImage(drawingBitmap))
                {
                    bitmapGraphics.DrawLine(new Pen(colorDialoge.Color), previewCoords.x, previewCoords.y, e.X, e.Y);
                }
                previewGraphics.Clear(Color.Transparent);
                pictureBox.Invalidate();
            }

            if (mode == DrawingMode.Rectangle)
            {
                using (Graphics bitmapGraphics = Graphics.FromImage(drawingBitmap))
                {
                    int width = Math.Abs(this.mouseCords.x - previewCoords.x);
                    int height = Math.Abs(this.mouseCords.y - previewCoords.y);
                    int startX = Math.Min(previewCoords.x, this.mouseCords.x);
                    int startY = Math.Min(previewCoords.y, this.mouseCords.y);
                    if (width < 0)
                    {
                        startX += width;
                    }
                    if (height < 0)
                    {
                        startX += height;
                    }
                    bitmapGraphics.DrawRectangle(new Pen(colorDialoge.Color), startX, startY, width, height);
                }
                previewGraphics.Clear(Color.Transparent);
                pictureBox.Invalidate();
            }

            if (mode == DrawingMode.Ellipse)
            {
                using (Graphics bitmapGraphics = Graphics.FromImage(drawingBitmap))
                {
                    bitmapGraphics.DrawEllipse(new Pen(colorDialoge.Color), previewCoords.x, previewCoords.y, (this.mouseCords.x - previewCoords.x), (this.mouseCords.y - previewCoords.y));
                }
                previewGraphics.Clear(Color.Transparent);
                pictureBox.Invalidate();
            }

        }


        private void pictureBox_MouseMove(object sender, MouseEventArgs e)
        {   
            this.prevMouseCoords.x = mouseCords.x;
            this.prevMouseCoords.y = mouseCords.y;
            this.mouseCords.x = e.X;
            this.mouseCords.y = e.Y;
            if (isMouseDown)
            {
                if (this.mode == DrawingMode.Pen)
                {
                    using (Graphics g = Graphics.FromImage(drawingBitmap))
                    {
                        g.DrawLine(new Pen(colorDialoge.Color), prevMouseCoords.x, prevMouseCoords.y, mouseCords.x, mouseCords.y);
                    }
                    pictureBox.Image = drawingBitmap;
                }

                if(this.mode == DrawingMode.Line)
                {
                    previewGraphics.Clear(Color.Transparent);
                    previewGraphics.DrawLine(new Pen(colorDialoge.Color), previewCoords.x, previewCoords.y, this.mouseCords.x, this.mouseCords.y);
                    pictureBox.Invalidate();
                }


                if (this.mode == DrawingMode.Rectangle)
                {
                    previewGraphics.Clear(Color.Transparent);
                    int width = Math.Abs(this.mouseCords.x - previewCoords.x);
                    int height = Math.Abs(this.mouseCords.y - previewCoords.y);
                    int startX = Math.Min(previewCoords.x, this.mouseCords.x);
                    int startY = Math.Min(previewCoords.y, this.mouseCords.y);
                    if (width < 0)
                    {
                        startX += width;
                    }
                    if (height < 0)
                    {
                        startX += height;
                    }
                    previewGraphics.DrawRectangle(new Pen(colorDialoge.Color), startX, startY, width, height);
                    //label1.Text = (this.mouseCords.x - previewCoords.x).ToString() + ", " + (this.mouseCords.y - previewCoords.y).ToString();
                    pictureBox.Invalidate();
                }


                if (this.mode == DrawingMode.Ellipse)
                {
                    previewGraphics.Clear(Color.Transparent);
                    previewGraphics.DrawEllipse(new Pen(colorDialoge.Color), previewCoords.x, previewCoords.y, (this.mouseCords.x - previewCoords.x), (this.mouseCords.y - previewCoords.y));
                    //label1.Text = (this.mouseCords.x - previewCoords.x).ToString() + ", " + (this.mouseCords.y - previewCoords.y).ToString();
                    pictureBox.Invalidate();
                }



                if (this.mode == DrawingMode.Eraser)
                {
                    using (Graphics g = Graphics.FromImage(drawingBitmap))
                    {
                        g.DrawLine(new Pen(Color.White, 15), prevMouseCoords.x, prevMouseCoords.y, mouseCords.x, mouseCords.y);
                    }
                    pictureBox.Image = drawingBitmap;
                }
            }
        }

        private void Paint_Resize(object sender, EventArgs e)
        {
            Bitmap resizedBitmap = new Bitmap(pictureBox.Width+1, pictureBox.Height+1);
            using (Graphics g = Graphics.FromImage(resizedBitmap))
            {
                g.Clear(Color.White);
                g.DrawImage(drawingBitmap, 0, 0, drawingBitmap.Width, drawingBitmap.Height);
            }
            this.drawingBitmap = resizedBitmap;

            previewBitmap = new Bitmap(pictureBox.Width + 1, pictureBox.Height + 1);
            previewGraphics = Graphics.FromImage(previewBitmap);
        }

        //line
        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            this.mode = DrawingMode.Line;
        }

        private void pictureBox_Paint_1(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawImage(drawingBitmap, 0, 0);

            e.Graphics.DrawImage(previewBitmap, 0, 0);            
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            this.mode = DrawingMode.Rectangle;
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            this.mode = DrawingMode.Ellipse;
        }

        private void toolStripButton1_Click_1(object sender, EventArgs e)
        {
            this.mode = DrawingMode.Eraser;
        }
    }

}
