using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections;
using SuperfastBlur;
using System.Diagnostics;

namespace photoEditor
{
    public partial class Form1 : Form
    {
        public bool isImageLoaded = false;
        public double redVal = 0.3, greenVal = 0.6, blueVal = 0.4;
        public Image image = null;

        public Point currentPoint = new Point();
        public Point oldPoint = new Point();
        public Point tempPoint = new Point();
        public Boolean drawModeOn = false;
        public Boolean drawRectangleModeOn = false;
        public Boolean drawCircleModeOn = false;

        public Graphics draw;
        public Color penColour = Color.Black;
        public Pen pen;
        public Bitmap inputImage;
        public Bitmap outputImage;
        public int mouseX, mouseY;

        public Color activeButtonColor = Color.FromArgb(25, 25, 25);
        public Color selectedButtonColor = Color.FromArgb(15, 15, 15);

        public Form1()
        {
            InitializeComponent();
            pen = new Pen(penColour, 5);
            if (!isImageLoaded)
            {
                menuStripItemsEnabled(false);
                buttonsEnabled(false);
            }

            // Set property null and it makes the property box empty.
            propertiesVisibilityOperations("null");
        }

        private void menuStripItemsEnabled(Boolean value)
        {
            kaydetToolStripMenuItem.Enabled = value;
            blackAndWhite.Enabled = value;
            kontrastToolStripMenuItem.Enabled = value;
            bulanıklıkToolStripMenuItem.Enabled = value;
            gaussianBlur.Enabled = value;
            medianBlur.Enabled = value;
            negatifToolStripMenuItem.Enabled = value;
            parlaklıkToolStripMenuItem.Enabled = value;
            Thresholding.Enabled = value;
            histogramToolStripMenuItem.Enabled = value;
            flipHorizontallyMenu.Enabled = value;
            flipVerticallyMenu.Enabled = value;
            flipMenuItem.Enabled = value;
            rotateMenuStripItem.Enabled = value;
            kenarBulmaToolStripMenuItem.Enabled = value;
            threeColorChannel.Enabled = value;
            netleştirmeToolStripMenuItem.Enabled = value;
            resizeMenuItem.Enabled = value;
            sumTwoImageMenuItem.Enabled = value;
            extractionMenuItem.Enabled = value;
            andGateMenuItem.Enabled = value;
        }

        private void buttonsEnabled(Boolean value)
        {
            // Butonların arka plan rengini resim yüklendikten sonra değiştirir
            if (value)
            {
                drawPencilButton.BackColor = activeButtonColor;
                drawRectangleButton.BackColor = activeButtonColor;
                drawCircleButton.BackColor = activeButtonColor;
                flipVertically.BackColor = activeButtonColor;
                flipHorizontally.BackColor = activeButtonColor;
                rotateToLeft.BackColor = activeButtonColor;
            }
            drawPencilButton.Enabled = value;
            drawRectangleButton.Enabled = value;
            drawCircleButton.Enabled = value;
            flipVertically.Enabled = value;
            flipHorizontally.Enabled = value;
            rotateToLeft.Enabled = value;
        }

        private void visibilityFalse()
        {
            brightnessGroupBox.Visible = false;
            thresholdingGroupBox.Visible = false;
            histogramGroupBox.Visible = false;
            kontrastGroupBox.Visible = false;
            gaussianBlurGroupBox.Visible = false;
            medianBlurGroupBox.Visible = false;
            rectangleGroupBox.Visible = false;
            circleGroupBox.Visible = false;
            konvolusyonNetlestirmeGroupBox.Visible = false;
            robertCrossGroupBox.Visible = false;
            prewittGroupBox.Visible = false;
            sobelGroupBox.Visible = false;
            threeColorChannelGroupBox.Visible = false;
            sumPixelGroupBox.Visible = false;
            extractionGroupBox.Visible = false;
            andGateGroupBox.Visible = false;
            resizeImageGroupBox.Visible = false;
        }

        private void propertiesVisibilityOperations(String property)
        {
            switch (property)
            {
                case "threeColor":
                    visibilityFalse();
                    threeColorChannelGroupBox.Visible = true;
                    break;

                case "brightness":
                    visibilityFalse();
                    brightnessGroupBox.Visible = true;
                    break;

                case "kontrast":
                    visibilityFalse();
                    kontrastGroupBox.Visible = true;
                    break;

                case "medianBlur":
                    visibilityFalse();
                    medianBlurGroupBox.Visible = true;
                    break;

                case "gaussianBlur":
                    visibilityFalse();
                    gaussianBlurGroupBox.Visible = true;
                    break;

                case "thresholding":
                    visibilityFalse();
                    thresholdingGroupBox.Visible = true;
                    break;

                case "histogram":
                    visibilityFalse();
                    histogramGroupBox.Visible = true;
                    break;

                case "circle":
                    visibilityFalse();
                    circleGroupBox.Visible = true;
                    break;

                case "rectangle":
                    visibilityFalse();
                    rectangleGroupBox.Visible = true;
                    break;

                case "konvolusyonNetlestirme":
                    visibilityFalse();
                    konvolusyonNetlestirmeGroupBox.Visible = true;
                    break;

                case "robertCross":
                    visibilityFalse();
                    robertCrossGroupBox.Visible = true;
                    break;

                case "prewitt":
                    visibilityFalse();
                    prewittGroupBox.Visible = true;
                    break;

                case "sobel":
                    visibilityFalse();
                    sobelGroupBox.Visible = true;
                    break;

                case "sumPixels":
                    visibilityFalse();
                    sumPixelGroupBox.Visible = true;
                    break;

                case "extractPixels":
                    visibilityFalse();
                    extractionGroupBox.Visible = true;
                    break;

                case "andGate":
                    visibilityFalse();
                    andGateGroupBox.Visible = true;
                    break;

                case "resize":
                    visibilityFalse();
                    resizeImageGroupBox.Visible = true;
                    break;

                default:
                    visibilityFalse();
                    break;
            }
        }

        private void İçeAktarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pictureBox.SizeMode = PictureBoxSizeMode.Zoom;
            openFileDialog.ShowDialog();
            pictureBox.ImageLocation = openFileDialog.FileName;
            isImageLoaded = true;
            buttonsEnabled(isImageLoaded);
            menuStripItemsEnabled(isImageLoaded);
            Image im = pictureBox.Image;
            image = im;
        }

        private void kaydetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (isImageLoaded)
            {
                FolderBrowserDialog dialog = new FolderBrowserDialog();
                if (dialog.ShowDialog() != DialogResult.OK)
                {
                    return;
                }
                string dir = dialog.SelectedPath;
                pictureBox.Image.Save(dir+"/image.jpeg", System.Drawing.Imaging.ImageFormat.Jpeg);
                MessageBox.Show("Dışa aktarma başarılı!");
            } else
            {
                MessageBox.Show("Dışa aktarılacak resim bulunamadı!");
            }   
        }

        private void resizeMenuItem_Click(object sender, EventArgs e)
        {
            propertiesVisibilityOperations("resize");
            resizeWidth.Text = pictureBox.Image.Width.ToString();
            resizeHeight.Text = pictureBox.Image.Height.ToString();
        }

        private void resizeSaveButton_Click(object sender, EventArgs e)
        {
            Bitmap tempImage = new Bitmap(pictureBox.Image, new Size(Convert.ToInt32(resizeNewWidth.Text), Convert.ToInt32(resizeNewHeight.Text)));
            pictureBox.Image = tempImage;
        }

        private void negatifToolStripMenuItem_Click(object sender, EventArgs e)
        {
            propertiesVisibilityOperations("negative");

            Bitmap inputImage = new Bitmap(pictureBox.Image);
            pictureBox.Image = negativeImage(inputImage);
        }

        public Bitmap negativeImage(Bitmap image)
        {
            Bitmap tempImage = new Bitmap(image);
            
            Color currentColour, transformedColor;
            int R, G, B, x, y;
            int w = tempImage.Width;
            int h = tempImage.Height;

            Bitmap outputImage = new Bitmap(w, h);

            for (x = 0; x < w; x++)
            {
                for (y = 0; y < h; y++)
                {
                    currentColour = tempImage.GetPixel(x, y);
                    R = 255 - currentColour.R;
                    G = 255 - currentColour.G;
                    B = 255 - currentColour.B;
                    transformedColor = Color.FromArgb(R, G, B);
                    outputImage.SetPixel(x, y, transformedColor);
                }
            }

            return outputImage;
        }

        private void blackAndWhite_Click(object sender, EventArgs e)
        {
            Color currentColour, transformedColor;

            Bitmap inputImage = new Bitmap(pictureBox.Image);

            int width = inputImage.Width;
            int height = inputImage.Height;
            Bitmap outputImage = new Bitmap(width, height);
            int grey = 0;
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    currentColour = inputImage.GetPixel(x, y);
                    double R = currentColour.R;
                    double G = currentColour.G;
                    double B = currentColour.B;

                    grey = Convert.ToInt16(R * 0.3 + G * 0.6 + B * 0.1);
                    transformedColor = Color.FromArgb(grey, grey, grey);
                    outputImage.SetPixel(x, y, transformedColor);
                }
            }
            pictureBox.Image = outputImage;
        }

        private void threeColorChannel_Click(object sender, EventArgs e)
        {
            propertiesVisibilityOperations("threeColor");

            Bitmap bmp = new Bitmap(pictureBox.Image);
            Bitmap rbmp = new Bitmap(bmp);
            Bitmap gbmp = new Bitmap(bmp);
            Bitmap bbmp = new Bitmap(bmp);

            var width = bmp.Width;
            var height = bmp.Height;

            //Convert Red Green and Blue
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    Color pixel = bmp.GetPixel(x, y);
                    int a = pixel.A;
                    int r = pixel.R;
                    int g = pixel.G;
                    int b = pixel.B;

                    rbmp.SetPixel(x, y, Color.FromArgb(a, r, 0, 0));
                    gbmp.SetPixel(x, y, Color.FromArgb(a, 0, g, 0));
                    bbmp.SetPixel(x, y, Color.FromArgb(a, 0, 0, b));
                }
            }

            redColorPictureBox.Image = rbmp;
            greenColorPictureBox.Image = gbmp;
            blueColorPictureBox.Image = bbmp;
        }

        #region AND ve NAND Gate Operatörleri
        private void andGateMenuItem_Click(object sender, EventArgs e)
        {
            propertiesVisibilityOperations("andGate");
            previewAndButton.Enabled = false;
            applyAndButton.Enabled = false;
        }

        private void selectImageANDButton_Click(object sender, EventArgs e)
        {
            selectedANDPictureBox.SizeMode = PictureBoxSizeMode.Zoom;
            openFileDialog.ShowDialog();
            selectedANDPictureBox.ImageLocation = openFileDialog.FileName;
            previewAndButton.Enabled = true;
        }

        private void previewAndButton_Click(object sender, EventArgs e)
        {
            applyAndButton.Enabled = true;
            andGateOperation();
        }

        private void applyAndButton_Click(object sender, EventArgs e)
        {
            pictureBox.Image = andOutputPicture.Image;
            andOutputPicture.Image = null;
            selectedANDPictureBox.Image = null;
        }

        private void andGateOperation()
        {
            Bitmap Resim1, Resim2, CikisResmi;
            Resim1 = new Bitmap(selectedANDPictureBox.Image);
            Resim2 = new Bitmap(pictureBox.Image);
           
            int ResimGenisligi = Resim1.Width;
            int ResimYuksekligi = Resim1.Height;
            Resim2 = new Bitmap(Resim2, new Size(ResimGenisligi, ResimYuksekligi));
            CikisResmi = new Bitmap(ResimGenisligi, ResimYuksekligi);
            Color Renk1, Renk2;
            int x, y;
            int R = 0, G = 0, B = 0;
            for (x = 0; x < ResimGenisligi; x++) //Resmi taramaya şablonun yarısı kadar dış kenarlardan içeride başlayacak ve bitirecek.
            {
                for (y = 0; y < ResimYuksekligi; y++)
                {
                    Renk1 = Resim1.GetPixel(x, y);
                    Renk2 = Resim2.GetPixel(x, y);
                    string binarySayi1R = Convert.ToString(Renk1.R, 2).PadLeft(8, '0'); //Gri renk olduğundan tek kanal üzerinden yapılıyor.
                string binarySayi2R = Convert.ToString(Renk2.R, 2).PadLeft(8, '0');
                    string binarySayi1G = Convert.ToString(Renk1.G, 2).PadLeft(8, '0'); //Gri renk olduğundan tek kanal üzerinden yapılıyor.
                string binarySayi2G = Convert.ToString(Renk2.G, 2).PadLeft(8, '0');
                    string binarySayi1B = Convert.ToString(Renk1.B, 2).PadLeft(8, '0'); //Gri renk olduğundan tek kanal üzerinden yapılıyor.
                string binarySayi2B = Convert.ToString(Renk2.B, 2).PadLeft(8, '0');
                    string Bit1R = null, Bit1G = null, Bit1B = null, Bit2R = null, Bit2G = null, Bit2B =
                   null;
                    string StringIkiliSayiR = null, StringIkiliSayiG = null, StringIkiliSayiB = null;
                    for (int i = 0; i < 8; i++)
                    {
                        Bit1R = binarySayi1R.Substring(i, 1);
                        Bit2R = binarySayi2R.Substring(i, 1);
                        //AND İŞLEMİ
                        if (Bit1R == "0" && Bit2R == "0") StringIkiliSayiR = StringIkiliSayiR + "0";
                        else if (Bit1R == "1" && Bit2R == "1") StringIkiliSayiR = StringIkiliSayiR + "1";
                        else StringIkiliSayiR = StringIkiliSayiR + "0";
                        Bit1G = binarySayi1G.Substring(i, 1);
                        Bit2G = binarySayi2G.Substring(i, 1);
                        //AND İŞLEMİ
                        if (Bit1G == "0" && Bit2G == "0") StringIkiliSayiG = StringIkiliSayiG + "0";
                        else if (Bit1G == "1" && Bit2G == "1") StringIkiliSayiG = StringIkiliSayiG + "1";
                        else StringIkiliSayiG = StringIkiliSayiG + "0";
                        Bit1B = binarySayi1B.Substring(i, 1);
                        Bit2B = binarySayi2B.Substring(i, 1);
                        //AND İŞLEMİ
                        if (Bit1B == "0" && Bit2B == "0") StringIkiliSayiB = StringIkiliSayiB + "0";
                        else if (Bit1B == "1" && Bit2B == "1") StringIkiliSayiB = StringIkiliSayiB + "1";
                        else StringIkiliSayiB = StringIkiliSayiB + "0";
                    }
                    R = Convert.ToInt32(StringIkiliSayiR, 2); //İkili sayıyı tam sayıya dönüştürüyor.
                    G = Convert.ToInt32(StringIkiliSayiG, 2); //İkili sayıyı tam sayıya dönüştürüyor.
                    B = Convert.ToInt32(StringIkiliSayiB, 2); //İkili sayıyı tam sayıya dönüştürüyor.
                    CikisResmi.SetPixel(x, y, Color.FromArgb(R, G, B));
                }
            }
            andOutputPicture.Image = CikisResmi;
        }
        #endregion


        #region Pixel Çıkarma İşlemi
        private void extractionMenuItem_Click(object sender, EventArgs e)
        {
            propertiesVisibilityOperations("extractPixels");
            applyExtractionButton.Enabled = false;
        }

        private void applyExtractionButton_Click(object sender, EventArgs e)
        {
            pictureBox.Image = resultExtractionPictureBox.Image;
            resultExtractionPictureBox.Image = null;
            beforeExtractPictureBox.Image = null;
        }

        private void extractionPixelTrackBar_MouseUp(object sender, MouseEventArgs e)
        {
            beforeExtractPictureBox.Image = ResmiEsiklemeYap(extractionPixelTrackBar.Value.ToString());
            applyExtractionButton.Enabled = true;
            extractPixels();
        }

        private void extractionPixelTrackBar_Scroll(object sender, EventArgs e)
        {
            thresholdValueExtractionLabel.Text = extractionPixelTrackBar.Value.ToString();
            
        }

        private void  extractPixels()
        {
            Bitmap image1 = new Bitmap(pictureBox.Image);
            Bitmap image2 = new Bitmap(beforeExtractPictureBox.Image);
            Bitmap outputImage;

            Color color1, color2;

            int x, y, R, G, B;


            int w = image1.Width;
            int h = image1.Height;

            outputImage = new Bitmap(w, h);

            for (x = 0; x < w; x++)
            {
                for (y = 0; y < h; y++)
                {
                    color1 = image1.GetPixel(x, y);
                    color2 = image2.GetPixel(x, y);

                    R = Math.Abs(color1.R - color2.R);
                    G = Math.Abs(color1.G - color2.G);
                    B = Math.Abs(color1.B - color2.B);
                    outputImage.SetPixel(x, y, Color.FromArgb(R, G, B));
                }
            }
            resultExtractionPictureBox.Image = negativeImage(outputImage);
        }
        #endregion

        #region Pixel Toplama İşlemi
        private void selectAnotherPhoto_Click(object sender, EventArgs e)
        {
            selectedPictureBox.SizeMode = PictureBoxSizeMode.Zoom;
            openFileDialog.ShowDialog();
            selectedPictureBox.ImageLocation = openFileDialog.FileName;
            applySum.Enabled = true;
        }

        private void sumTwoImageMenuItem_Click(object sender, EventArgs e)
        {
            propertiesVisibilityOperations("sumPixels");
            applySum.Enabled = false;
            applySummationButton.Enabled = false;
        }

        private void applySum_Click(object sender, EventArgs e)
        {
            summationPixels();
            applySummationButton.Enabled = true;
        }
        private void applySummationButton_Click(object sender, EventArgs e)
        {
            pictureBox.Image = sumPreviewPictureBox.Image;
            sumPreviewPictureBox.Image = null;
            selectedPictureBox.Image = null;
        }

        private void summationPixels()
        {
            Bitmap Resim1, Resim2, CikisResmi;
            Resim1 = new Bitmap(pictureBox.Image);
            Resim2 = new Bitmap(selectedPictureBox.Image);
            int ResimGenisligi = Resim1.Width;
            int ResimYuksekligi = Resim1.Height;
            CikisResmi = new Bitmap(ResimGenisligi, ResimYuksekligi);
            Color Renk1, Renk2;
            int x, y, R, G, B;

            int EnBuyukDeger = 0, EnKucukDeger = 0;
            for (x = 0; x < ResimGenisligi; x++) //Resmi taramaya şablonun yarısı kadar dış kenarlardan içeride başlayacak ve bitirecek.
            {
                for (y = 0; y < ResimYuksekligi; y++)
                {
                    Renk1 = Resim1.GetPixel(x, y);
                    Renk2 = Resim2.GetPixel(x, y);
                    ////İki resmi direk toplama
                    R = Renk1.R + Renk2.R;
                    G = Renk1.G + Renk2.G;
                    B = Renk1.B + Renk2.B;
                    int Gri = (R + G + B) / 3;
                    //Sınırı aşan değerleri 255 ayarlama. Gri resim üzerinde işlem yapıldığı için Sadece R ye bakıldı.
                    if (Gri > EnBuyukDeger)
                        EnBuyukDeger = Gri;
                    if (Gri < EnKucukDeger)
                        EnKucukDeger = Gri;
                    if (R > 255) R = 255;
                    if (G > 255) G = 255;
                    if (B > 255) B = 255;
                    CikisResmi.SetPixel(x, y, Color.FromArgb(R, G, B));
                }
            }
            sumPreviewPictureBox.Image = Normalizasyon(Resim1, Resim2, EnBuyukDeger, EnKucukDeger);
        }

        public Bitmap Normalizasyon(Bitmap Resim1, Bitmap Resim2, int EnBuyukDeger, int EnKucukDeger)
        {
            Bitmap CikisResmi;
            int ResimGenisligi = Resim1.Width;
            int ResimYuksekligi = Resim1.Height;
            CikisResmi = new Bitmap(ResimGenisligi, ResimYuksekligi);
            Color Renk1, Renk2;
            int x, y;
            int R = 0, G = 0, B = 0;
            int UstSinir = 0, AltSinir = 0;
            if (EnBuyukDeger > 255)
                UstSinir = 255;
            else
                UstSinir = EnBuyukDeger;
            if (EnKucukDeger < 0)
                AltSinir = 0;
            else
                AltSinir = EnKucukDeger;
            for (x = 0; x < ResimGenisligi; x++) //Resmi taramaya şablonun yarısı kadar dış kenarlardan içeride başlayacak ve bitirecek.
 {
                for (y = 0; y < ResimYuksekligi; y++)
                {
                    Renk1 = Resim1.GetPixel(x, y);
                    Renk2 = Resim2.GetPixel(x, y);
                    ////İki resmi direk toplama
                    R = Renk1.R + Renk2.R;
                    G = Renk1.G + Renk2.G;
                    B = Renk1.B + Renk2.B;
                    int NormalDegerR = (((UstSinir - AltSinir) * (R - EnKucukDeger)) / (EnBuyukDeger -
                   EnKucukDeger)) + AltSinir;
                    int NormalDegerG = (((UstSinir - AltSinir) * (G - EnKucukDeger)) / (EnBuyukDeger -
                   EnKucukDeger)) + AltSinir;
                    int NormalDegerB = (((UstSinir - AltSinir) * (B - EnKucukDeger)) / (EnBuyukDeger -
                   EnKucukDeger)) + AltSinir;
                    //EnBuyuk ve EnKucuk değerler Gri renge göre ayarlandığından Yinede bu renkler sınırı geçebilir.
                if (NormalDegerR > 255) NormalDegerR = 255;
                    if (NormalDegerG > 255) NormalDegerG = 255;
                    if (NormalDegerB > 255) NormalDegerB = 255;
                    if (NormalDegerR < 0) NormalDegerR = 0;
                    if (NormalDegerG < 0) NormalDegerG = 0;
                    if (NormalDegerB < 0) NormalDegerB = 0;
                    CikisResmi.SetPixel(x, y, Color.FromArgb(NormalDegerR, NormalDegerG, NormalDegerB));
                }
            }
            return CikisResmi;
        }
        #endregion

        #region Parlaklık - Brightness
        private void parlaklıkToolStripMenuItem_Click(object sender, EventArgs e)
        {
            propertiesVisibilityOperations("brightness");
        }

        private void brightnessTrackBar_Scroll(object sender, EventArgs e)
        {
            parlaklikLabel.Text = brightnessTrackBar.Value.ToString();
            brightnessPictureBox.Refresh();
            brightnessPictureBox.Image = null;
            brightnessPictureBox.Image = changeBrightness(brightnessTrackBar.Value);
        }

        private void applyBrightnessButton_Click(object sender, EventArgs e)
        {
            pictureBox.Image = changeBrightness(brightnessTrackBar.Value);
            brightnessPictureBox.Refresh();
            brightnessPictureBox.Image = null;
            brightnessTrackBar.Value = 0;
            parlaklikLabel.Text = "0";
        }
        public Bitmap changeBrightness(int value)
        {
            int R, G, B;
            Color OkunanRenk, DonusenRenk;
            Bitmap GirisResmi, CikisResmi;
            GirisResmi = new Bitmap(pictureBox.Image);
            int ResimGenisligi = GirisResmi.Width; //GirisResmi global tanımlandı.
            int ResimYuksekligi = GirisResmi.Height;
            CikisResmi = new Bitmap(ResimGenisligi, ResimYuksekligi); //Cikis resmini oluşturuyor. Boyutları giriş resmi ile aynı olur.
            int i = 0, j = 0; //Çıkış resminin x ve y si olacak.
            for (int x = 0; x < ResimGenisligi; x++)
            {
                j = 0;
                for (int y = 0; y < ResimYuksekligi; y++)
                {
                    OkunanRenk = GirisResmi.GetPixel(x, y);
                    //Rengini 50 değeri ile açacak.
                    R = OkunanRenk.R + value;
                    G = OkunanRenk.G + value;
                    B = OkunanRenk.B + value;
                    //Renkler 255 geçtiyse son sınır olan 255 alınacak.
                    if (R >= 255) R = 255;
                    if (G >= 255) G = 255;
                    if (B >= 255) B = 255;
                    if (R <= 0) R = 0;
                    if (G <= 0) G = 0;
                    if (B <= 0) B = 0;
                    DonusenRenk = Color.FromArgb(R, G, B);
                    CikisResmi.SetPixel(i, j, DonusenRenk);
                    j++;
                }
                i++;
            }
            return CikisResmi;
        }
        #endregion

        #region Eşikleme - Thresholding
        private void Thresholding_Click(object sender, EventArgs e)
        {
            propertiesVisibilityOperations("thresholding");
        }

        private void thresholdingButton_Click(object sender, EventArgs e)
        {
            ResmiEsiklemeYap(thresholdingTextBox.Text);

        }
        private void esiklemeApplyButton_Click(object sender, EventArgs e)
        {
            pictureBox.Image = esiklemePictureBox.Image;
            esiklemePictureBox.Image = null;
            thresholdingTextBox.Text = null;
        }
        public Bitmap ResmiEsiklemeYap(String value)
        {
            int R = 0, G = 0, B = 0;
            Color OkunanRenk, DonusenRenk;
            Bitmap GirisResmi, CikisResmi;
            GirisResmi = new Bitmap(pictureBox.Image);
            int ResimGenisligi = GirisResmi.Width; //GirisResmi global tanımlandı.
            int ResimYuksekligi = GirisResmi.Height;
            CikisResmi = new Bitmap(ResimGenisligi, ResimYuksekligi); //Cikis resmini oluşturuyor. Boyutları giriş resmi ile aynı olur.
            int EsiklemeDegeri = Convert.ToInt32(value);

            for (int x = 0; x < ResimGenisligi; x++)
            {
                for (int y = 0; y < ResimYuksekligi; y++)
                {
                    OkunanRenk = GirisResmi.GetPixel(x, y);
                    if (OkunanRenk.R >= EsiklemeDegeri)
                        R = 255;
                    else
                        R = 0;
                    if (OkunanRenk.G >= EsiklemeDegeri)
                        G = 255;
                    else
                        G = 0;
                    if (OkunanRenk.B >= EsiklemeDegeri)
                        B = 255;
                    else
                        B = 0;
                    DonusenRenk = Color.FromArgb(R, G, B);
                    CikisResmi.SetPixel(x, y, DonusenRenk);
                }
            }
            esiklemePictureBox.Image = CikisResmi;
            return CikisResmi;
        }
        #endregion

        #region Kontrast ve Histogram
        private void histogramToolStripMenuItem_Click(object sender, EventArgs e)
        {
            propertiesVisibilityOperations("histogram");
        }
        private void histogramButton_Click(object sender, EventArgs e)
        {
            ResminHistograminiCiz("sadeceHistogram");
        }
        public void ResminHistograminiCiz(String value)
        {
            if( value == "sadeceHistogram")
            {
                ArrayList DiziPiksel = new ArrayList();
                int OrtalamaRenk;
                Color OkunanRenk;
                Bitmap GirisResmi;
                GirisResmi = new Bitmap(pictureBox.Image);
                int ResimGenisligi = GirisResmi.Width;
                int ResimYuksekligi = GirisResmi.Height;

                for (int x = 0; x < GirisResmi.Width; x++)
                {
                    for (int y = 0; y < GirisResmi.Height; y++)
                    {
                        OkunanRenk = GirisResmi.GetPixel(x, y);
                        OrtalamaRenk = (int)(OkunanRenk.R + OkunanRenk.G + OkunanRenk.B) / 3;
                        DiziPiksel.Add(OrtalamaRenk);
                    }
                }
                int[] DiziPikselSayilari = new int[256];
                for (int r = 0; r <= 255; r++)
                {
                    int PikselSayisi = 0;
                    for (int s = 0; s < DiziPiksel.Count; s++)
                    {
                        if (r == Convert.ToInt16(DiziPiksel[s]))
                            PikselSayisi++;
                    }
                    DiziPikselSayilari[r] = PikselSayisi;
                }

                int RenkMaksPikselSayisi = 0;
                for (int k = 0; k <= 255; k++)
                {
                    histogramListBox.Items.Add("Renk:" + k + "=" + DiziPikselSayilari[k]);
                    if (DiziPikselSayilari[k] > RenkMaksPikselSayisi)
                    {
                        RenkMaksPikselSayisi = DiziPikselSayilari[k];
                    }
                }
                
                Graphics CizimAlani;
                Pen Kalem1 = new Pen(System.Drawing.Color.Yellow, 1);
                Pen Kalem2 = new Pen(System.Drawing.Color.Red, 1);
                CizimAlani = histogramPictureBox.CreateGraphics();
                histogramPictureBox.Refresh();
                int GrafikYuksekligi = histogramPictureBox.Height;
                double OlcekY = RenkMaksPikselSayisi / GrafikYuksekligi;
                double OlcekX = 1.5;
                int X_kaydirma = 10;
                for (int x = 0; x <= 255; x++)
                {
                    if (x % 50 == 0)
                        CizimAlani.DrawLine(Kalem2, (int)(X_kaydirma + x * OlcekX),
                       GrafikYuksekligi, (int)(X_kaydirma + x * OlcekX), 0);
                    CizimAlani.DrawLine(Kalem1, (int)(X_kaydirma + x * OlcekX), GrafikYuksekligi,
                   (int)(X_kaydirma + x * OlcekX), (GrafikYuksekligi - (int)(DiziPikselSayilari[x] / OlcekY)));
                }
                histogramInformationLabel.Text = "En çok tekrarlayan pixel sayısı: " + RenkMaksPikselSayisi.ToString();
            }

            if (value == "kontrastHistogram")
            {
                ArrayList DiziPiksel = new ArrayList();
                int OrtalamaRenk;
                Color OkunanRenk;
                Bitmap GirisResmi;
                GirisResmi = new Bitmap(kontrastOnizlemePictureBox.Image);
                int ResimGenisligi = GirisResmi.Width;
                int ResimYuksekligi = GirisResmi.Height;
                for (int x = 0; x < GirisResmi.Width; x++)
                {
                    for (int y = 0; y < GirisResmi.Height; y++)
                    {
                        OkunanRenk = GirisResmi.GetPixel(x, y);
                        OrtalamaRenk = (int)(OkunanRenk.R + OkunanRenk.G + OkunanRenk.B) / 3;
                        DiziPiksel.Add(OrtalamaRenk);
                    }
                }
                int[] DiziPikselSayilari = new int[256];
                for (int r = 0; r <= 255; r++)
                {
                    int PikselSayisi = 0;
                    for (int s = 0; s < DiziPiksel.Count; s++)
                    {
                        if (r == Convert.ToInt16(DiziPiksel[s]))
                            PikselSayisi++;
                    }
                    DiziPikselSayilari[r] = PikselSayisi;
                }
                
                int RenkMaksPikselSayisi = 0;
                for (int k = 0; k <= 255; k++)
                {
                    //Maksimum piksel sayısını bulmaya çalışıyor.
                    if (DiziPikselSayilari[k] > RenkMaksPikselSayisi)
                    {
                        RenkMaksPikselSayisi = DiziPikselSayilari[k];
                    }
                }
                //Grafiği çiziyor.
                Graphics CizimAlani;
                Pen Kalem1 = new Pen(System.Drawing.Color.Yellow, 1);
                Pen Kalem2 = new Pen(System.Drawing.Color.Red, 1);
                CizimAlani = kontrastHistogram.CreateGraphics();
                kontrastHistogram.Refresh();
                int GrafikYuksekligi = kontrastHistogram.Height;
                double OlcekY = RenkMaksPikselSayisi / GrafikYuksekligi;
                double OlcekX = 1.5;
                int X_kaydirma = 10;
                for (int x = 0; x <= 255; x++)
                {
                    if (x % 50 == 0)
                        CizimAlani.DrawLine(Kalem2, (int)(X_kaydirma + x * OlcekX),
                       GrafikYuksekligi, (int)(X_kaydirma + x * OlcekX), 0);
                    CizimAlani.DrawLine(Kalem1, (int)(X_kaydirma + x * OlcekX), GrafikYuksekligi,
                   (int)(X_kaydirma + x * OlcekX), (GrafikYuksekligi - (int)(DiziPikselSayilari[x] / OlcekY)));
                }
            } 
        }

        private void kontrastToolStripMenuItem_Click(object sender, EventArgs e)
        {
            propertiesVisibilityOperations("kontrast");
        }

        private void kontrastUygulaButton_Click(object sender, EventArgs e)
        {
            kontrast(false);
            ResminHistograminiCiz("kontrastHistogram");
        }

        private void saveKontrastButton_Click(object sender, EventArgs e)
        {
            kontrast(true);
        }


        private void kontrast(Boolean output)
        {
            int R = 0, G = 0, B = 0;
            Color OkunanRenk, DonusenRenk;
            Bitmap GirisResmi, CikisResmi;
            GirisResmi = new Bitmap(pictureBox.Image);
            int ResimGenisligi = GirisResmi.Width; //GirisResmi global tanımlandı.
            int ResimYuksekligi = GirisResmi.Height;
            CikisResmi = new Bitmap(ResimGenisligi, ResimYuksekligi); //Cikis resmini oluşturuyor. Boyutları giriş resmi ile aynı olur.
            double C_KontrastSeviyesi = Convert.ToInt32(kontrastDuzeyi.Text);
            double F_KontrastFaktoru = (259 * (C_KontrastSeviyesi + 255)) / (255 * (259 -
            C_KontrastSeviyesi));
            for (int x = 0; x < ResimGenisligi; x++)
            {
                for (int y = 0; y < ResimYuksekligi; y++)
                {
                    OkunanRenk = GirisResmi.GetPixel(x, y);
                    R = OkunanRenk.R;
                    G = OkunanRenk.G;
                    B = OkunanRenk.B;
                    R = (int)((F_KontrastFaktoru * (R - 128)) + 128);
                    G = (int)((F_KontrastFaktoru * (G - 128)) + 128);
                    B = (int)((F_KontrastFaktoru * (B - 128)) + 128);
                    //Renkler sınırların dışına çıktıysa, sınır değer alınacak.
                    if (R > 255) R = 255;
                    if (G > 255) G = 255;
                    if (B > 255) B = 255;
                    if (R < 0) R = 0;
                    if (G < 0) G = 0;
                    if (B < 0) B = 0;
                    DonusenRenk = Color.FromArgb(R, G, B);
                    CikisResmi.SetPixel(x, y, DonusenRenk);
                }
            }



            if (output == true)
            {
                pictureBox.Refresh();
                kontrastOnizlemePictureBox.Refresh();
                pictureBox.Image = null;
                kontrastOnizlemePictureBox.Image = null;
                pictureBox.Image = CikisResmi;
            }else
            {
                kontrastOnizlemePictureBox.Refresh();
                kontrastOnizlemePictureBox.Image = null;
                kontrastOnizlemePictureBox.Image = CikisResmi;
            }
        }
        #endregion

        #region Gaussian Blur
        private void gaussianBlur_Click(object sender, EventArgs e)
        {
            propertiesVisibilityOperations("gaussianBlur");
        }
        private void gaussianBlurApplyToPictureButton_Click(object sender, EventArgs e)
        {
            pictureBox.Refresh();
            pictureBox.Image = null;
            pictureBox.Image = gaussianBlurPictureBoxPreview.Image;
            gaussianBlurPictureBoxPreview.Refresh();
            gaussianBlurPictureBoxPreview.Image = null;
            gaussianBlurTrackBar.Value = 0;
        }

        private void gaussianBlurTrackBar_Scroll(object sender, EventArgs e)
        {
            var blur = new GaussianBlur(pictureBox.Image as Bitmap);
            var result = blur.Process(gaussianBlurTrackBar.Value);
            gaussianBlurPictureBoxPreview.Refresh();
            gaussianBlurPictureBoxPreview.Image = null;
            gaussianBlurPictureBoxPreview.Image = result;
        }
        #endregion

        #region Median Bulanıklaştırma
        private void medianBlur_Click(object sender, EventArgs e)
        {
            propertiesVisibilityOperations("medianBlur");
        }
        private void medianBlurSaveButton_Click(object sender, EventArgs e)
        {
            pictureBox.Image = medianBlurPictureBox.Image;
            medianBlurPictureBox.Image = null;
            medianBlurTrackBar.Value = 3;
        }

        private void medianBlurTrackBar_Scroll(object sender, EventArgs e)
        {
            medianFiltresi();
        }


        public void medianFiltresi()
        {
            Color OkunanRenk;
            Bitmap GirisResmi, CikisResmi;
            GirisResmi = new Bitmap(pictureBox.Image);
            int ResimGenisligi = GirisResmi.Width;
            int ResimYuksekligi = GirisResmi.Height;
            CikisResmi = new Bitmap(ResimGenisligi, ResimYuksekligi);
            int SablonBoyutu = Convert.ToInt32(medianBlurTrackBar.Value); //şablon boyutu 3 den büyük tek rakam olmalıdır(3, 5, 7 gibi).
            int ElemanSayisi = SablonBoyutu * SablonBoyutu;
            int[] R = new int[ElemanSayisi];
            int[] G = new int[ElemanSayisi];
            int[] B = new int[ElemanSayisi];
            int[] Gri = new int[ElemanSayisi];
            int x, y, i, j;
            for (x = (SablonBoyutu - 1) / 2; x < ResimGenisligi - (SablonBoyutu - 1) / 2; x++)
            {
                for (y = (SablonBoyutu - 1) / 2; y < ResimYuksekligi - (SablonBoyutu - 1) / 2; y++)
                {
                    //Şablon bölgesi (çekirdek matris) içindeki pikselleri tarıyor.
                    int k = 0;
                    for (i = -((SablonBoyutu - 1) / 2); i <= (SablonBoyutu - 1) / 2; i++)
                    {
                        for (j = -((SablonBoyutu - 1) / 2); j <= (SablonBoyutu - 1) / 2; j++)
                        {
                            OkunanRenk = GirisResmi.GetPixel(x + i, y + j);
                            R[k] = OkunanRenk.R;
                            G[k] = OkunanRenk.G;
                            B[k] = OkunanRenk.B;
                            Gri[k] = Convert.ToInt16(R[k] * 0.299 + G[k] * 0.587 + B[k] * 0.114); //Gri ton formülü

                            k++;
                        }
                    }
                    //Gri tona göre sıralama yapıyor. Aynı anda üç rengide değiştiriyor.
                    int GeciciSayi = 0;
                    for (i = 0; i < ElemanSayisi; i++)
                    {
                        for (j = i + 1; j < ElemanSayisi; j++)
                        {
                            if (Gri[j] < Gri[i])
                            {
                                GeciciSayi = Gri[i];
                                Gri[i] = Gri[j];
                                Gri[j] = GeciciSayi;
                                GeciciSayi = R[i];
                                R[i] = R[j];
                                R[j] = GeciciSayi;
                                GeciciSayi = G[i];
                                G[i] = G[j];
                                G[j] = GeciciSayi;
                                GeciciSayi = B[i];
                                B[i] = B[j];
                                B[j] = GeciciSayi;
                            }
                        }
                    }
                    //Sıralama sonrası ortadaki değeri çıkış resminin piksel değeri olarak atıyor.
                    CikisResmi.SetPixel(x, y, Color.FromArgb(R[(ElemanSayisi - 1) / 2], G[(ElemanSayisi - 1) /
                   2], B[(ElemanSayisi - 1) / 2]));
                }
            }
            medianBlurPictureBox.Image = CikisResmi;
        }
        #endregion

        #region Resim üzerine çizim yapma algoritması
        private void pictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            if (isImageLoaded && drawModeOn)
            {
                remapLocation(e.Location.X, e.Location.Y);
                tempPoint = e.Location;
                tempPoint.X = mouseX;
                oldPoint = tempPoint;
            }
        }

        private void pictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (isImageLoaded && drawModeOn)
            {
                if (e.Button == MouseButtons.Left)
                {
                    currentPoint = e.Location;
                    remapLocation(currentPoint.X, currentPoint.Y);

                    currentPoint.X = mouseX;

                    draw.DrawLine(pen, oldPoint, currentPoint);
                    oldPoint = currentPoint;
                    pictureBox1.Image = outputImage;
                    pictureBox.Image = pictureBox1.Image;
                }
            }
        }

        private void remapLocation(int x, int y)
        {
            var imageX = pictureBox.Image.Size.Width;
            var pictureBoxX = pictureBox.Width;
            var newX = pictureBoxX - imageX;


            mouseX = x - (newX / 2);
            
        }

        private void drawPencilButton_Click(object sender, EventArgs e)
        {
            if (isImageLoaded)
            {
                drawModeOn = !drawModeOn;

                if (drawModeOn)
                {
                    drawPencilButton.BackColor = selectedButtonColor;
                }
                else
                {
                    drawPencilButton.BackColor = activeButtonColor;
                }

                inputImage = new Bitmap(pictureBox.Image);
                outputImage = new Bitmap(inputImage);
                inputImage = outputImage;
                draw = Graphics.FromImage(inputImage);
            }
        }
        #endregion

        private void drawRectangleButton_Click(object sender, EventArgs e)
        {
            drawRectangleModeOn = !drawRectangleModeOn;
            if (drawRectangleModeOn)
            {
                propertiesVisibilityOperations("rectangle");
                drawRectangleButton.BackColor = selectedButtonColor;
            }else
            {
                propertiesVisibilityOperations(null);
                drawRectangleButton.BackColor = activeButtonColor;
            }
        }

        private void addRectangle_Click(object sender, EventArgs e)
        {
            var rectWidth = 100;
            var rectHeight = 100;

            if (rectangleWidth.Text != "")
            {
                rectWidth = Convert.ToInt32(rectangleWidth.Text);
            }
            if (rectangleHeight.Text != "")
            {
                rectHeight = Convert.ToInt32(rectangleHeight.Text);
            }

            Random random = new Random();
            var xPosition = random.Next(pictureBox.Image.Size.Width);
            var yPosition = random.Next(pictureBox.Image.Size.Height);

            Bitmap GirisResmi = new Bitmap(pictureBox.Image);
            Bitmap CikisResmi = new Bitmap(GirisResmi);
            CikisResmi = GirisResmi;
            Graphics Grafik = Graphics.FromImage(CikisResmi);

            Color Renk = Color.Red;// Çizim kaleminin rengini beyaz atıyor
            Grafik.DrawRectangle(pen, xPosition, yPosition, rectWidth, rectHeight);

            pictureBox.Image = CikisResmi;
        }

        private void drawCircleButton_Click(object sender, EventArgs e)
        {
            drawCircleModeOn = !drawCircleModeOn;
            if (drawCircleModeOn)
            {
                propertiesVisibilityOperations("circle");
                drawCircleButton.BackColor = selectedButtonColor;
            }
            else
            {
                propertiesVisibilityOperations(null);
                drawCircleButton.BackColor = activeButtonColor;
            }
            
        }

        private void addCircleButton_Click(object sender, EventArgs e)
        {
            var circleWidth = 100;
            var circleHeight = 100;

            if (circleWidthTB.Text != "")
            {
                circleWidth = Convert.ToInt32(circleWidthTB.Text);
            }
            if (circleHeightTB.Text != "")
            {
                circleHeight = Convert.ToInt32(circleHeightTB.Text);
            }

            Random random = new Random();
            var xPosition = random.Next(pictureBox.Image.Size.Width);
            var yPosition = random.Next(pictureBox.Image.Size.Height);

            Bitmap GirisResmi = new Bitmap(pictureBox.Image);
            Bitmap CikisResmi = new Bitmap(GirisResmi);
            CikisResmi = GirisResmi;
            Graphics Grafik = Graphics.FromImage(CikisResmi);

            Color Renk = Color.Red;// Çizim kaleminin rengini beyaz atıyor
            Grafik.DrawEllipse(pen, xPosition, yPosition, circleWidth, circleHeight);

            pictureBox.Image = CikisResmi;
        }

        private void flipVertically_Click(object sender, EventArgs e)
        {
            Bitmap bmp = new Bitmap(pictureBox.Image);

            bmp.RotateFlip(RotateFlipType.RotateNoneFlipX);

            pictureBox.Image = bmp;
        }

        private void flipHorizontally_Click(object sender, EventArgs e)
        {
            Bitmap bmp = new Bitmap(pictureBox.Image);

            bmp.RotateFlip(RotateFlipType.RotateNoneFlipY);

            pictureBox.Image = bmp;
        }

        private void rotateToLeft_Click(object sender, EventArgs e)
        {
            Bitmap bmp = new Bitmap(pictureBox.Image);
            bmp.RotateFlip(RotateFlipType.Rotate90FlipNone);
            pictureBox.Image = bmp;
        }

        #region Konvülsiyon Netleştirme Algoritması
        private void netlestirmeKonvolusyon_Click(object sender, EventArgs e)
        {
            propertiesVisibilityOperations("konvolusyonNetlestirme");
            konvolusyonApplyButton.Enabled = false;
        }

        private void konvolusyonPreviewButton_Click(object sender, EventArgs e)
        {
            netlestirmeKonvolusyonAlgoritmasi();
            konvolusyonApplyButton.Enabled = true;
        }

        private void konvolusyonApplyButton_Click(object sender, EventArgs e)
        {
            pictureBox.Image = konvolusyonPictureBox.Image;
            konvolusyonPictureBox.Image = null;
        }

        private void netlestirmeKonvolusyonAlgoritmasi()
        {
            Color OkunanRenk;
            Bitmap GirisResmi, CikisResmi;
            GirisResmi = new Bitmap(pictureBox.Image);
            int ResimGenisligi = GirisResmi.Width;
            int ResimYuksekligi = GirisResmi.Height;
            CikisResmi = new Bitmap(ResimGenisligi, ResimYuksekligi);
            int SablonBoyutu = 3;
            int ElemanSayisi = SablonBoyutu * SablonBoyutu;
            int x, y, i, j, toplamR, toplamG, toplamB;
            int R, G, B;
            int[] Matris = { 0, -2, 0, -2, 11, -2, 0, -2, 0 };
            int MatrisToplami = 0 + -2 + 0 + -2 + 11 + -2 + 0 + -2 + 0;
            for (x = (SablonBoyutu - 1) / 2; x < ResimGenisligi - (SablonBoyutu - 1) / 2; x++) //Resmi taramaya şablonun yarısı kadar dış kenarlardan içeride başlayacak ve bitirecek.
 {
                for (y = (SablonBoyutu - 1) / 2; y < ResimYuksekligi - (SablonBoyutu - 1) / 2; y++)
                {
                    toplamR = 0;
                    toplamG = 0;
                    toplamB = 0;
                    //Şablon bölgesi (çekirdek matris) içindeki pikselleri tarıyor.
                    int k = 0; //matris içindeki elemanları sırayla okurken kullanılacak.
                    for (i = -((SablonBoyutu - 1) / 2); i <= (SablonBoyutu - 1) / 2; i++)
                    {
                        for (j = -((SablonBoyutu - 1) / 2); j <= (SablonBoyutu - 1) / 2; j++)
                        {
                            OkunanRenk = GirisResmi.GetPixel(x + i, y + j);
                            toplamR = toplamR + OkunanRenk.R * Matris[k];
                            toplamG = toplamG + OkunanRenk.G * Matris[k];
                            toplamB = toplamB + OkunanRenk.B * Matris[k];
                            k++;
                        }
                    }
                    R = toplamR / MatrisToplami;
                    G = toplamG / MatrisToplami;
                    B = toplamB / MatrisToplami;
                    //===========================================================
                    //Renkler sınırların dışına çıktıysa, sınır değer alınacak.
                    if (R > 255) R = 255;
                    if (G > 255) G = 255;
                    if (B > 255) B = 255;
                    if (R < 0) R = 0;
                    if (G < 0) G = 0;
                    if (B < 0) B = 0;
                    //===========================================================
                    CikisResmi.SetPixel(x, y, Color.FromArgb(R, G, B));
                }
            }
            konvolusyonPictureBox.Image = CikisResmi;
        }
        #endregion

        #region RobertCross Kenar Bulma Algoritması
        private void robetCross_Click(object sender, EventArgs e)
        {
            propertiesVisibilityOperations("robertCross");
            robertCrossApplyButton.Enabled = false;
        }

        private void robertBrossMethod()
        {
            Bitmap GirisResmi, CikisResmi;
            GirisResmi = new Bitmap(pictureBox.Image);
            int ResimGenisligi = GirisResmi.Width;
            int ResimYuksekligi = GirisResmi.Height;
            CikisResmi = new Bitmap(ResimGenisligi, ResimYuksekligi);
            int x, y;
            Color Renk;
            int P1, P2, P3, P4;
            for (x = 0; x < ResimGenisligi - 1; x++) //Resmi taramaya şablonun yarısı kadar dış kenarlardan içeride başlayacak ve bitirecek.
 {
                for (y = 0; y < ResimYuksekligi - 1; y++)
                {
                    Renk = GirisResmi.GetPixel(x, y);
                    P1 = (Renk.R + Renk.G + Renk.B) / 3;
                    Renk = GirisResmi.GetPixel(x + 1, y);
                    P2 = (Renk.R + Renk.G + Renk.B) / 3;
                    Renk = GirisResmi.GetPixel(x, y + 1);
                    P3 = (Renk.R + Renk.G + Renk.B) / 3;
                    Renk = GirisResmi.GetPixel(x + 1, y + 1);
                    P4 = (Renk.R + Renk.G + Renk.B) / 3;
                    int Gx = Math.Abs(P1 - P4); //45 derece açı ile duran çizgileri bulur.
                    int Gy = Math.Abs(P2 - P3); //135 derece açı ile duran çizgileri bulur.
                    int RobertCrossDegeri = 0;
                    RobertCrossDegeri = Gx;
                    RobertCrossDegeri = Gy;
                    RobertCrossDegeri = Gx + Gy; //1. Formül
                                                 //RobertCrossDegeri = Convert.ToInt16(Math.Sqrt(Gx * Gx + Gy * Gy)); //2.Formül
                                                 //Renkler sınırların dışına çıktıysa, sınır değer alınacak.
                    if (RobertCrossDegeri > 255) RobertCrossDegeri = 255; //Mutlak değer kullanıldığı için negatif değerler oluşmaz.
                     //Eşikleme
                     //if (RobertCrossDegeri > 50)
                     // RobertCrossDegeri = 255;
                     //else
                     // RobertCrossDegeri = 0;
                     CikisResmi.SetPixel(x, y, Color.FromArgb(RobertCrossDegeri, RobertCrossDegeri,
                    RobertCrossDegeri));
                }
            }
            robertCrossPictureBox.Image = CikisResmi;
        }

        private void robertCrossPreviewButton_Click(object sender, EventArgs e)
        {
            robertBrossMethod();
            robertCrossApplyButton.Enabled = true;
        }
        private void robertCrossApplyButton_Click(object sender, EventArgs e)
        {
            pictureBox.Image = robertCrossPictureBox.Image;
            robertCrossPictureBox.Image = null;
        }
        #endregion

        #region Prewitt Kenar Bulma Metodu
        private void PrewittMethod()
        {
            Bitmap GirisResmi, CikisResmi;
            GirisResmi = new Bitmap(pictureBox.Image);
            int ResimGenisligi = GirisResmi.Width;
            int ResimYuksekligi = GirisResmi.Height;
            CikisResmi = new Bitmap(ResimGenisligi, ResimYuksekligi);
            int SablonBoyutu = 3;
            int ElemanSayisi = SablonBoyutu * SablonBoyutu;
            int x, y;
            Color Renk;
            int P1, P2, P3, P4, P5, P6, P7, P8, P9;
            for (x = (SablonBoyutu - 1) / 2; x < ResimGenisligi - (SablonBoyutu - 1) / 2; x++) //Resmi taramaya şablonun yarısı kadar dış kenarlardan içeride başlayacak ve bitirecek.
 {
                for (y = (SablonBoyutu - 1) / 2; y < ResimYuksekligi - (SablonBoyutu - 1) / 2; y++)
                {
                    Renk = GirisResmi.GetPixel(x - 1, y - 1);
                    P1 = (Renk.R + Renk.G + Renk.B) / 3;
                    Renk = GirisResmi.GetPixel(x, y - 1);
                    P2 = (Renk.R + Renk.G + Renk.B) / 3;
                    Renk = GirisResmi.GetPixel(x + 1, y - 1);
                    P3 = (Renk.R + Renk.G + Renk.B) / 3;
                    Renk = GirisResmi.GetPixel(x - 1, y);
                    P4 = (Renk.R + Renk.G + Renk.B) / 3;
                    Renk = GirisResmi.GetPixel(x, y);
                    P5 = (Renk.R + Renk.G + Renk.B) / 3;
                    Renk = GirisResmi.GetPixel(x + 1, y);
                    P6 = (Renk.R + Renk.G + Renk.B) / 3;
                    Renk = GirisResmi.GetPixel(x - 1, y + 1);
                    P7 = (Renk.R + Renk.G + Renk.B) / 3;
                    Renk = GirisResmi.GetPixel(x, y + 1);
                    P8 = (Renk.R + Renk.G + Renk.B) / 3;
                    Renk = GirisResmi.GetPixel(x + 1, y + 1);
                    P9 = (Renk.R + Renk.G + Renk.B) / 3;
                    int Gx = Math.Abs(-P1 + P3 - P4 + P6 - P7 + P9); //Dikey çizgileri Bulur
                    int Gy = Math.Abs(P1 + P2 + P3 - P7 - P8 - P9); //Yatay Çizgileri Bulur.
                    int PrewittDegeri = 0;
                    PrewittDegeri = Gx;
                    PrewittDegeri = Gy;
                    PrewittDegeri = Gx + Gy; //1. Formül
                                             //PrewittDegeri = Convert.ToInt16(Math.Sqrt(Gx * Gx + Gy * Gy)); //2.Formül
                                             //Renkler sınırların dışına çıktıysa, sınır değer alınacak.
                    if (PrewittDegeri > 255) PrewittDegeri = 255;
                    //Eşikleme: Örnek olarak 100 değeri kullanıldı.
                    //if (PrewittDegeri > 100)
                    //PrewittDegeri = 255;
                    //else
                    //PrewittDegeri = 0;
                    CikisResmi.SetPixel(x, y, Color.FromArgb(PrewittDegeri, PrewittDegeri, PrewittDegeri));
                }
            }
            prewittPictureBox.Image = CikisResmi;
        }

        private void prewitt_Click(object sender, EventArgs e)
        {
            propertiesVisibilityOperations("prewitt");
            prewittApplyButton.Enabled = false;
        }

        private void prewittPreviewButton_Click(object sender, EventArgs e)
        {
            PrewittMethod();
            prewittApplyButton.Enabled = true;
        }

        private void prewittApplyButton_Click(object sender, EventArgs e)
        {
            pictureBox.Image = prewittPictureBox.Image;
            prewittPictureBox.Image = null;
        }
        #endregion
        
        #region Sobel Genişletme ve Kenar Bulma Algoritması
        private void sobelEdgeDetection_Click(object sender, EventArgs e)
        {
            propertiesVisibilityOperations("sobel");
        }

        private void sobelPreviewButton_Click(object sender, EventArgs e)
        {
            
            genisletmeSinirBulma();
        }

        private void genisletmeSinirBulma()
        {
            Color OkunanRenk;
            Bitmap GirisResmi, CikisResmi;
            Bitmap GenislemisResim;
            GirisResmi = new Bitmap(pictureBox.Image);

            int ResimGenisligi = GirisResmi.Width;
            int ResimYuksekligi = GirisResmi.Height;
            CikisResmi = new Bitmap(ResimGenisligi, ResimYuksekligi);

            Color currentColour, transformedColor;

            Bitmap inputImage = new Bitmap(GirisResmi);

            int width = inputImage.Width;
            int height = inputImage.Height;
            Bitmap outputImage = new Bitmap(width, height);
            int grey = 0;
            for (int o = 0; o < width; o++)
            {
                for (int u = 0; u < height; u++)
                {
                    currentColour = inputImage.GetPixel(o, u);
                    double R = currentColour.R;
                    double G = currentColour.G;
                    double B = currentColour.B;

                    grey = Convert.ToInt16(R * 0.3 + G * 0.6 + B * 0.1);
                    transformedColor = Color.FromArgb(grey, grey, grey);
                    outputImage.SetPixel(o, u, transformedColor);
                }
            }
            
            Bitmap SiyahBeyazResim = outputImage;
            pictureBox2.Image = SiyahBeyazResim;
            

            GirisResmi = SiyahBeyazResim;
            int x, y, i, j;
            int SablonBoyutu = 3;
            int ElemanSayisi = SablonBoyutu * SablonBoyutu;
            for (x = (SablonBoyutu - 1) / 2; x < ResimGenisligi - (SablonBoyutu - 1) / 2; x++) //Resmi taramaya şablonun yarısı kadar dış kenarlardan içeride başlayacak ve bitirecek.
            {
                for (y = (SablonBoyutu - 1) / 2; y < ResimYuksekligi - (SablonBoyutu - 1) / 2; y++)
                {
                    bool RenkSiyah = false;
                    for (i = -((SablonBoyutu - 1) / 2); i <= (SablonBoyutu - 1) / 2; i++)
                    {
                        for (j = -((SablonBoyutu - 1) / 2); j <= (SablonBoyutu - 1) / 2; j++)
                        {
                            OkunanRenk = GirisResmi.GetPixel(x + i, y + j);
                            if (OkunanRenk.R < 128) //Siyah ise
                                RenkSiyah = true;
                        }
                    }
                    if (RenkSiyah == true) //Komşularda siyah varsa
                    {
                        Color KendiRengi = GirisResmi.GetPixel(x, y);
                        if (KendiRengi.R > 128) //kendi rengin beyaz ise onu da siyah yap.
                            CikisResmi.SetPixel(x, y, Color.FromArgb(255, 0, 0)); //siyah yerine kırmızı kullandık.Genişleyen bölgeyi görmek için
                    }
                    else //komşularda siyah yok ise kendi rengi yine aynı beyaz kalmalı.
                        CikisResmi.SetPixel(x, y, Color.FromArgb(255, 255, 255));
                }
            }
            sobelPictureBox.Image = CikisResmi;
            siyahBolgeyiGenislet();
            SiyahBeyazResim = new Bitmap(pictureBox2.Image);
            GenislemisResim = new Bitmap(pictureBox3.Image);
            //Bitmap KenarGoruntuResmi = orijinalResimdenGenislemisResmiCikar(SiyahBeyazResim, GenislemisResim);
            pictureBox2.Image = orijinalResimdenGenislemisResmiCikar(SiyahBeyazResim, GenislemisResim);
        }

        public Bitmap orijinalResimdenGenislemisResmiCikar(Bitmap SiyahBeyazResim, Bitmap GenislemisResim)
        {

            Bitmap CikisResmi;
            int ResimGenisligi = SiyahBeyazResim.Width;
            int ResimYuksekligi = SiyahBeyazResim.Height;
            CikisResmi = new Bitmap(ResimGenisligi, ResimYuksekligi);
            int x, y;
            int Fark;
            for (x = 0; x < ResimGenisligi; x++) //Resmi taramaya şablonun yarısı kadar dış kenarlardan içeride başlayacak ve bitirecek.
            {
                for (y = 0; y < ResimYuksekligi; y++)
                {
                    Color OrjinalRenk = SiyahBeyazResim.GetPixel(x, y);
                    Color GenislemisResimRenk = GenislemisResim.GetPixel(x, y);
                    int OrjinalGri = (OrjinalRenk.R + OrjinalRenk.G + OrjinalRenk.B) / 3;
                    int GenislemisGri = (GenislemisResimRenk.R + GenislemisResimRenk.G +
                   GenislemisResimRenk.B) / 3;
                    Fark = Math.Abs(OrjinalGri - GenislemisGri);
                    CikisResmi.SetPixel(x, y, Color.FromArgb(Fark, Fark, Fark));
                }
            }
            return CikisResmi;
        }

        private void siyahBolgeyiGenislet()
        {
            Color OkunanRenk;
            Bitmap GirisResmi, CikisResmi;
            //Not: PictureBox2 de eşiklenmiş Siyah Beyaz resim olmalı.
            Bitmap SiyahBeyazResim = new Bitmap(pictureBox2.Image);
            int ResimGenisligi = SiyahBeyazResim.Width;
            int ResimYuksekligi = SiyahBeyazResim.Height;
            CikisResmi = new Bitmap(ResimGenisligi, ResimYuksekligi);
            GirisResmi = SiyahBeyazResim;
            int x, y, i, j;
            int SablonBoyutu = 3;
            int ElemanSayisi = SablonBoyutu * SablonBoyutu;
            int Esikleme = 128;
            
            for (x = (SablonBoyutu - 1) / 2; x < ResimGenisligi - (SablonBoyutu - 1) / 2; x++) //Resmi taramaya şablonun yarısı kadar dış kenarlardan içeride başlayacak ve bitirecek.
            {
                for (y = (SablonBoyutu - 1) / 2; y < ResimYuksekligi - (SablonBoyutu - 1) / 2; y++)
                {
                    OkunanRenk = GirisResmi.GetPixel(x, y);
                    if (OkunanRenk.R > Esikleme) //Rengin Beyaz, Komşularında da siyaha varsa kendi rengini siyah yapacaksın.
                {
                        bool KomsulardaSiyahVarMi = false;
                        //Komşuların içerisinde siyah var mı yok mu, onu bulalım.
                        for (i = -((SablonBoyutu - 1) / 2); i <= (SablonBoyutu - 1) / 2; i++)
                        {
                            for (j = -((SablonBoyutu - 1) / 2); j <= (SablonBoyutu - 1) / 2; j++)
                            {
                                OkunanRenk = GirisResmi.GetPixel(x + i, y + j);
                                if (OkunanRenk.R < Esikleme) //Komşularda Siyah Var ise
                                    KomsulardaSiyahVarMi = true;
                            }
                        }
                        //Komşularda Beyaz var ise, Kendi rengimizi Beyaz yapalım.
                        if (KomsulardaSiyahVarMi == true) //Komşularda Beyaz varsa
                        {
                            Color KendiRengi = GirisResmi.GetPixel(x, y);
                            CikisResmi.SetPixel(x, y, Color.FromArgb(0, 0, 0));
                        }
                        else //komşularda siyah yok ise kendi rengin yine beyaz kalmalı.
                            CikisResmi.SetPixel(x, y, Color.FromArgb(255, 255, 255));
                    }
                }
            }
            pictureBox3.Image = CikisResmi;
        }

        private void sobelApplyButton_Click(object sender, EventArgs e)
        {
            pictureBox.Image = sobelPictureBox.Image;
            sobelPictureBox.Image = null;
            pictureBox2.Image = null;
            pictureBox3.Image = null;
        }
        #endregion

        #region Kalem ve Renk Ayarları
        private void updatePenColour(Color value)
        {
            penColour = value;
            pen = new Pen(penColour, 5);
        }
        private void redColorButton_Click(object sender, EventArgs e)
        {
            updatePenColour(Color.Red);
        }
        private void orangeColorButton_Click(object sender, EventArgs e)
        {
            updatePenColour(Color.Orange);
        }
        private void yellowButtonColor_Click(object sender, EventArgs e)
        {
            updatePenColour(Color.Yellow);
        }
        private void limeColorButton_Click(object sender, EventArgs e)
        {
            updatePenColour(Color.Lime);
        }
        private void aquaColorButton_Click(object sender, EventArgs e)
        {
            updatePenColour(Color.Aqua);
        }
        private void blueColorButton_Click(object sender, EventArgs e)
        {
            updatePenColour(Color.Blue);
        }
        private void fuchsiaColorButton_Click(object sender, EventArgs e)
        {
            updatePenColour(Color.Fuchsia);
        }
        private void whiteColorButton_Click(object sender, EventArgs e)
        {
            updatePenColour(Color.White);
        }
        private void blackColorButton_Click(object sender, EventArgs e)
        {
            updatePenColour(Color.Black);
        }
        #endregion
    }
}