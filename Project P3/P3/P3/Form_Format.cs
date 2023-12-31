﻿using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Net;
using System.Drawing.Text;
using System.Runtime.InteropServices;
using System.Reflection.Emit;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Drawing.Drawing2D;

namespace P3
{
	public partial class Form_Format : Form
	{
        private Font OldFont;
        Font myFont;
        PrivateFontCollection private_fonts = new PrivateFontCollection();
        private ImageCodecInfo GetCodecInfo(string mimeType)
		{
			ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();
			foreach (ImageCodecInfo codec in codecs) 
			{
				if (codec.MimeType == mimeType)
				{
					return codec;
				}
			}
			return null;
		}
		public Form_Format()
		{
			InitializeComponent();
            btnSelectFile.Image = P3.Properties.Resources.SETI1;
            btnSelectFile.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            btnConvert.Image = P3.Properties.Resources.SETI1;
            btnConvert.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            pictureBox2.Image = P3.Properties.Resources.back1;
            pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            pictureBox2.BackColor = System.Drawing.Color.Transparent;
            LoadFont();
            label1.Font = new Font(private_fonts.Families[0], 10);
            label1.UseCompatibleTextRendering = true;
            txtFilePath.Font = new Font(private_fonts.Families[0], 10);
            txtFilePath.UseCompatibleTextRendering = true;
            btnSelectFile.Font = new Font(private_fonts.Families[0], 10);
            btnSelectFile.UseCompatibleTextRendering = true;
            btnConvert.Font = new Font(private_fonts.Families[0], 10);
            btnConvert.UseCompatibleTextRendering = true;
            cmbOutputFormat.Font = new Font(private_fonts.Families[0], 10);
            this.Icon = Properties.Resources.icon7;
            pictureBox1.Image = P3.Properties.Resources.close_inactive;
            pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            pictureBox1.BackColor = System.Drawing.Color.Transparent;
            pictureBox4.Image = P3.Properties.Resources.maximize_inactive;
            pictureBox4.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            pictureBox4.BackColor = System.Drawing.Color.Transparent;
            pictureBox5.Image = P3.Properties.Resources.minimize_inactive;
            pictureBox5.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            pictureBox5.BackColor = System.Drawing.Color.Transparent;
            pictureBox3.Image = P3.Properties.Resources.icon7_transformed;
            pictureBox3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            pictureBox3.BackColor = System.Drawing.Color.Transparent;
            this.FormBorderStyle = FormBorderStyle.None;
            this.MaximumSize = Screen.PrimaryScreen.WorkingArea.Size;
        }
        private void Form_Format_Paint(object sender, PaintEventArgs e) 
        {
            DrawBorder(e.Graphics, ClientRectangle);
            DrawNeonInserts(e.Graphics, ClientRectangle);
        }
        private void DrawBorder(Graphics graphics, Rectangle rectangle) 
        {
            int borderWidth = 2;
            Pen borderPen = new Pen(Color.Black, borderWidth);
            graphics.DrawRectangle(borderPen, rectangle);
            rectangle.Inflate(-borderWidth / 2, -borderWidth / 2);
            borderPen.Color = Color.Red;
            graphics.DrawRectangle(borderPen, rectangle);
        }
        private void DrawNeonInserts(Graphics graphics, Rectangle rectangle) 
        {
            int insetsWidth = 30;
            int insetsHeight = 15;
            Pen insetsPen = new Pen(Color.Red, 2);
            Rectangle topInsetValue = new Rectangle(rectangle.Left + 30, rectangle.Top + 10, insetsWidth, insetsHeight);
            graphics.DrawArc(insetsPen, topInsetValue, 180, 180);
            graphics.DrawLine(insetsPen, topInsetValue.Left + insetsWidth / 2, topInsetValue.Bottom, topInsetValue.Right - 10, topInsetValue.Bottom);
            graphics.DrawLine(insetsPen, topInsetValue.Left + 10, topInsetValue.Bottom, topInsetValue.Left + insetsWidth / 2, topInsetValue.Bottom + insetsHeight);
            Rectangle bottomInsetValue = new Rectangle(rectangle.Left + 30, rectangle.Bottom - 25, insetsWidth, insetsHeight);
            graphics.DrawArc(insetsPen, bottomInsetValue, 0, 180);
            graphics.DrawLine(insetsPen, bottomInsetValue.Left + insetsWidth / 2, bottomInsetValue.Top, bottomInsetValue.Right - 10, bottomInsetValue.Top);
            graphics.DrawLine(insetsPen, bottomInsetValue.Left + 10, bottomInsetValue.Top, bottomInsetValue.Left + insetsWidth / 2, bottomInsetValue.Top - insetsHeight);
        }
        protected override void WndProc(ref Message message)
        {
            if (message.Msg == 0x201)
            {
                base.Capture = false;
                message = Message.Create(base.Handle, 0xa1, new IntPtr(2), IntPtr.Zero);
            }
            if (message.Msg == 0x84)
            {
                const int resizeArea = 10;
                Point cursorPosition = PointToClient(new Point(message.LParam.ToInt32() & 0xffff, message.LParam.ToInt32() >> 16));
                if (cursorPosition.X >= ClientSize.Width - resizeArea && cursorPosition.Y >= ClientSize.Height - resizeArea)
                {
                    message.Result = (IntPtr)17;
                    return;
                }
                else if (cursorPosition.X <= resizeArea && cursorPosition.Y >= ClientSize.Height - resizeArea)
                {
                    message.Result = (IntPtr)16;
                    return;
                }
                else if (cursorPosition.X <= resizeArea && cursorPosition.Y <= resizeArea)
                {
                    message.Result = (IntPtr)13;
                    return;
                }
                else if (cursorPosition.X >= ClientSize.Width - resizeArea && cursorPosition.Y <= resizeArea)
                {
                    message.Result = (IntPtr)14;
                    return;
                }
                else if (cursorPosition.X <= resizeArea)
                {
                    message.Result = (IntPtr)10;
                    return;
                }
                else if (cursorPosition.X >= ClientSize.Width - resizeArea)
                {
                    message.Result = (IntPtr)11;
                    return;
                }
                else if (cursorPosition.Y >= ClientSize.Height - resizeArea)
                {
                    message.Result = (IntPtr)15;
                    return;
                }
                else if (cursorPosition.Y <= resizeArea)
                {
                    message.Result = (IntPtr)12;
                    return;
                }
            }
            base.WndProc(ref message);
        }
        private void LoadFont()
        {
            using (MemoryStream fontStream = new MemoryStream(Properties.Resources.SeldomScene_B6lG))
            {
                System.IntPtr data = Marshal.AllocCoTaskMem((int)fontStream.Length);
                byte[] fontdata = new byte[fontStream.Length];
                fontStream.Read(fontdata, 0, (int)fontStream.Length);
                Marshal.Copy(fontdata, 0, data, (int)fontStream.Length);
                private_fonts.AddMemoryFont(data, (int)fontStream.Length);
                fontStream.Close();
                Marshal.FreeCoTaskMem(data);
            }
        }
        private void OnFrameChanged(object sender, EventArgs e)
        {
            if (InvokeRequired)
            {
                BeginInvoke((Action)(() => OnFrameChanged(sender, e)));
                return;
            }
            ImageAnimator.UpdateFrames();
            Invalidate(false);
        }
        private void MainForm_Load(object sender, EventArgs e)
        {
            openFileDialog.Filter = "JPEG files (*.jpg)|*.jpg|PNG files (*.png)|*.png|BMP files (*.bmp)";
            saveFileDialog.Filter = "JPEG files (*.jpg)|*.jpg|PNG files (*.png)|*.png|BMP files (*.bmp)";
        }
		void BtnSelectFileClick(object sender, EventArgs e)
		{
			try
			{
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Multiselect = true;
                openFileDialog.Filter = "Image Files (*.jpg, *.jpeg, *.png, *.bmp)|*.jpg;*.jpeg;*.png;*.bmp";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
            	{
                    string fileName = openFileDialog.SafeFileName;
                    if (fileName.Length > 20)
                    {
                        string extension = Path.GetExtension(fileName);
                        fileName = fileName.Substring(0, Math.Min(fileName.Length, 16)) + "...";
                        fileName += extension;
                    }
                    txtFilePath.Text = fileName;
                    //txtFilePath.Text = openFileDialog.SafeFileName;
                    pictureBox.Image = Image.FromFile(openFileDialog.FileName);
            	}	
			}
			catch
			{
				MessageBox.Show("Невозможно выбрать файл", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}		
		}
		int i;
        void BtnConvertClick(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtFilePath.Text))
            {
                if (cmbOutputFormat.SelectedIndex == -1)
                {
                    MessageBox.Show("Выберите формат вывода", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                int i = 0;
                ImageFormat outputFormat = ImageFormat.Jpeg;
                switch (cmbOutputFormat.SelectedIndex)
                {
                    case 0:
                        i = 1;
                        outputFormat = ImageFormat.Jpeg;
                        break;
                    case 1:
                        i = 2;
                        outputFormat = ImageFormat.Png;
                        break;
                    case 2:
                        i = 3;
                        outputFormat = ImageFormat.Bmp;
                        break;
                }
                if (pictureBox.Image != null)
                {
                    SaveFileDialog savedialog = new SaveFileDialog();
                    savedialog.Title = "Сохранить картинку как...";
                    savedialog.OverwritePrompt = true;
                    savedialog.CheckPathExists = true;
                    if (i == 1)
                    {
                        savedialog.Filter = "Image Files(*.JPG)|*.JPEG";
                    }
                    else if (i == 2)
                    {
                        savedialog.Filter = "Image Files(*.PNG)|*.PNG";
                    }
                    else if (i == 3)
                    {
                        savedialog.Filter = "Image Files(*.BMP)|*.BMP";
                    }
                    savedialog.ShowHelp = true;
                    EncoderParameters encoderParams = new EncoderParameters(1);
                    encoderParams.Param[0] = new EncoderParameter(Encoder.Compression, (long)EncoderValue.CompressionNone);
                    if (savedialog.ShowDialog() == DialogResult.OK)
                    {
                        try
                        {
                            if (i == 1)
                            {
                                pictureBox.Image.Save(savedialog.FileName, System.Drawing.Imaging.ImageFormat.Jpeg);
                            }
                            else if (i == 2)
                            {
                                pictureBox.Image.Save(savedialog.FileName, System.Drawing.Imaging.ImageFormat.Png);
                            }
                            else if (i == 3)
                            {
                                pictureBox.Image.Save(savedialog.FileName, System.Drawing.Imaging.ImageFormat.Bmp);
                            }
                        }
                        catch
                        {
                            MessageBox.Show("Невозможно сохранить изображение", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
        }
        private void pictureBox2_Click(object sender, EventArgs e)
        {
            if (this.WindowState != FormWindowState.Maximized)
            {
                this.Hide();
                MainForm f1 = new MainForm();
                if (this.WindowState == FormWindowState.Maximized)
                {
                    f1.WindowState = FormWindowState.Maximized;
                }
                f1.ShowDialog();
            }
            else
            {
                this.Hide();
                MainForm f1 = new MainForm();
                f1.WindowState = FormWindowState.Maximized;
                f1.ShowDialog();
            }
        }
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        private void pictureBox1_MouseEnter(object sender, EventArgs e)
        {
            pictureBox1.Image = P3.Properties.Resources.close_active;
            pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            pictureBox1.BackColor = System.Drawing.Color.Transparent;
        }
        private void pictureBox1_MouseLeave(object sender, EventArgs e)
        {
            pictureBox1.Image = P3.Properties.Resources.close_inactive;
            pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            pictureBox1.BackColor = System.Drawing.Color.Transparent;
        }
        private void pictureBox4_MouseEnter(object sender, EventArgs e)
        {
            pictureBox4.Image = P3.Properties.Resources.maximize_active;
            pictureBox4.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            pictureBox4.BackColor = System.Drawing.Color.Transparent;
        }
        private void pictureBox4_MouseLeave(object sender, EventArgs e)
        {
            pictureBox4.Image = P3.Properties.Resources.maximize_inactive;
            pictureBox4.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            pictureBox4.BackColor = System.Drawing.Color.Transparent;
        }
        private void pictureBox5_MouseEnter(object sender, EventArgs e)
        {
            pictureBox5.Image = P3.Properties.Resources.minimize_active;
            pictureBox5.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            pictureBox5.BackColor = System.Drawing.Color.Transparent;
        }
        private void pictureBox5_MouseLeave(object sender, EventArgs e)
        {
            pictureBox5.Image = P3.Properties.Resources.minimize_inactive;
            pictureBox5.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            pictureBox5.BackColor = System.Drawing.Color.Transparent;
        }
        private void pictureBox4_Click(object sender, EventArgs e)
        {
            if (this.WindowState != FormWindowState.Maximized)
                this.WindowState = FormWindowState.Maximized;
            else
                this.WindowState = FormWindowState.Normal;
        }
        private void pictureBox5_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
    }
}
