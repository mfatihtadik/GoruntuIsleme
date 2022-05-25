using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections; 

namespace Goruntu_Isleme_Odevi
{
    public partial class Form1 : Form
    {


        public Form1()
        {
            InitializeComponent();

        }


        //================== Perspektif düzeltme işlemi =================
        public void PerspektifDuzelt(double a00, double a01, double a02, double a10, double
        a11, double a12, double a20, double a21, double a22)
        {
            Bitmap GirisResmi, CikisResmi;
            Color OkunanRenk;
            GirisResmi = new Bitmap(pictureBox1.Image);
            int ResimGenisligi = GirisResmi.Width;
            int ResimYuksekligi = GirisResmi.Height;

            CikisResmi = new Bitmap(ResimGenisligi, ResimYuksekligi);
            double X, Y, z;
            for (int x = 0; x < (ResimGenisligi); x++)
            {
                for (int y = 0; y < (ResimYuksekligi); y++)
                {
                    OkunanRenk = GirisResmi.GetPixel(x, y);
                    z = a20 * x + a21 * y + 1;
                    X = (a00 * x + a01 * y + a02) / z;
                    Y = (a10 * x + a11 * y + a12) / z;
                    if (X > 0 && X < ResimGenisligi && Y > 0 && Y < ResimYuksekligi)
                        //Picturebox ın dışına çıkan kısımlar oluşturulmayacak.
                        CikisResmi.SetPixel((int)X, (int)Y, OkunanRenk);
                }
            }
            pictureBox2.Image = CikisResmi;
        }



        // MATRİS TERSİNİ ALMA---------------------
        public double[,] MatrisTersiniAl(double[,] GirisMatrisi)
        {
            int MatrisBoyutu = Convert.ToInt16(Math.Sqrt(GirisMatrisi.Length)); //matris boyutu içindeki eleman sayısı olduğu için kare matrisde karekökü matris boyutu olur.
        double[,] CikisMatrisi = new double[MatrisBoyutu, MatrisBoyutu]; //A nın tersi alındığında bu  matris içinde tutulacak.
        //--I Birim matrisin içeriğini dolduruyor 
        for (int i = 0; i < MatrisBoyutu; i++)
            {
                for (int j = 0; j < MatrisBoyutu; j++)
                {
                    if (i == j)
                        CikisMatrisi[i, j] = 1;
                    else
                        CikisMatrisi[i, j] = 0;
                }
            }
            //--Matris Tersini alma işlemi---------
            double d, k;
            for (int i = 0; i < MatrisBoyutu; i++)
            {
                d = GirisMatrisi[i, i];
                for (int j = 0; j < MatrisBoyutu; j++)
                {
                    if (d == 0)
                    {
                        d = 0.0001; //0 bölme hata veriyordu. 
                    }
                    GirisMatrisi[i, j] = GirisMatrisi[i, j] / d;
                    CikisMatrisi[i, j] = CikisMatrisi[i, j] / d;
                }
                for (int x = 0; x < MatrisBoyutu; x++)
                {
                    if (x != i)
                    {
                        k = GirisMatrisi[x, i];
                        for (int j = 0; j < MatrisBoyutu; j++)
                        {
                            GirisMatrisi[x, j] = GirisMatrisi[x, j] - GirisMatrisi[i, j] * k;
                            CikisMatrisi[x, j] = CikisMatrisi[x, j] - CikisMatrisi[i, j] * k;
                        }
                    }
                }
            }
            return CikisMatrisi;
        }





        private void acToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DosyaAc();
        }

        public void DosyaAc()
        {
            try
            {
                
                openFileDialog1.DefaultExt = ".jpg";
                openFileDialog1.Filter = "Image Files(*.BMP;*.JPG;*.GIF)|*.BMP;*.JPG;*.GIF|All files (*.*)|*.*";
                openFileDialog1.ShowDialog();
                String ResminYolu = openFileDialog1.FileName;
                pictureBox1.Image = Image.FromFile(ResminYolu);
            }
            catch { }
        }

        private void kaydetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ResmiKaydet();
        }

        public void ResmiKaydet()
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "Jpeg Resmi|*.jpg|Bitmap Resmi|*.bmp|Gif Resmi|*.gif";
            saveFileDialog1.Title = "Resmi Kaydet";
            saveFileDialog1.ShowDialog();
            if (saveFileDialog1.FileName != "") //Dosya adı boş değilse kaydedecek.
            {
                // FileStream nesnesi ile kayıtı gerçekleştirecek. 
                FileStream DosyaAkisi = (FileStream)saveFileDialog1.OpenFile();
                switch (saveFileDialog1.FilterIndex)
                {
                    case 1:
                        pictureBox2.Image.Save(DosyaAkisi, System.Drawing.Imaging.ImageFormat.Jpeg);
                        break;
                    case 2:
                        pictureBox2.Image.Save(DosyaAkisi, System.Drawing.Imaging.ImageFormat.Bmp);
                        break;
                    case 3:
                        pictureBox2.Image.Save(DosyaAkisi, System.Drawing.Imaging.ImageFormat.Gif);
                        break;
                }
                DosyaAkisi.Close();
            }
        }

        private void negatifToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image==null)
            {
                MessageBox.Show("Resim ekleyiniz.");
            }
            else
            {
                Color OkunanRenk, DonusenRenk;
                int R = 0, G = 0, B = 0;
                Bitmap GirisResmi, CikisResmi;
                GirisResmi = new Bitmap(pictureBox1.Image);
                int ResimGenisligi = GirisResmi.Width; //GirisResmi global tanımlandı. İçerisine görüntü yüklendi.
                int ResimYuksekligi = GirisResmi.Height;
                CikisResmi = new Bitmap(ResimGenisligi, ResimYuksekligi); //Cikis resmini oluşturuyor.  Boyutları giriş resmi ile aynı olur. Tanımlaması globalde yapıldı.
                int i = 0, j = 0; //Çıkış resminin x ve y si olacak.
                for (int x = 0; x < ResimGenisligi; x++)
                {
                    for (int y = 0; y < ResimYuksekligi; y++)
                    {
                        OkunanRenk = GirisResmi.GetPixel(x, y);
                        R = 255 - OkunanRenk.R;
                        G = 255 - OkunanRenk.G;
                        B = 255 - OkunanRenk.B;
                        DonusenRenk = Color.FromArgb(R, G, B);
                        CikisResmi.SetPixel(x, y, DonusenRenk);
                    }
                }
                pictureBox2.Image = CikisResmi;

            }
            
        }

        private void griToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (pictureBox1.Image == null)
            {
                MessageBox.Show("Resim ekleyiniz.");
            }
            else
            {
                Color OkunanRenk, DonusenRenk;

                Bitmap GirisResmi = new Bitmap(pictureBox1.Image);
                int ResimGenisligi = GirisResmi.Width; //GirisResmi global tanımlandı. Fonksiyonla gelmedi.
                int ResimYuksekligi = GirisResmi.Height;
                Bitmap CikisResmi = new Bitmap(ResimGenisligi, ResimYuksekligi); //Cikis resmini  oluşturuyor.Boyutları giriş resmi ile aynı olur.
                int GriDeger = 0;
                for (int x = 0; x < ResimGenisligi; x++)
                {
                    for (int y = 0; y < ResimYuksekligi; y++)
                    {
                        OkunanRenk = GirisResmi.GetPixel(x, y);
                        double R = OkunanRenk.R;
                        double G = OkunanRenk.G;
                        double B = OkunanRenk.B;
                        //GriDeger = Convert.ToInt16((R + G + B) / 3); 

                        GriDeger = Convert.ToInt16(R * 0.3 + G * 0.6 + B * 0.1);
                        DonusenRenk = Color.FromArgb(GriDeger, GriDeger, GriDeger);
                        CikisResmi.SetPixel(x, y, DonusenRenk);
                    }
                }
                pictureBox2.Image = CikisResmi;
            }
           
        }

        private void esiklemeThresholdingToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (pictureBox1.Image == null)
            {
                MessageBox.Show("Resim ekleyiniz.");
            }

            else
            {


                if (textBox1.Text == "")
                {
                    MessageBox.Show("Eşikleme değeri giriniz.");
                    textBox1.BackColor = Color.Red;
                    textBox1.Focus();
                }
                else
                {
                    textBox1.BackColor = Color.White;
                    int R = 0, G = 0, B = 0;
                    Color OkunanRenk, DonusenRenk;
                    Bitmap GirisResmi, CikisResmi;
                    GirisResmi = new Bitmap(pictureBox1.Image);
                    int ResimGenisligi = GirisResmi.Width; //GirisResmi global tanımlandı.
                    int ResimYuksekligi = GirisResmi.Height;
                    CikisResmi = new Bitmap(ResimGenisligi, ResimYuksekligi); //Cikis resmini oluşturuyor. Boyutları  giriş resmi ile aynı olur.
                    int EsiklemeDegeri = Convert.ToInt32(textBox1.Text);
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
                    pictureBox2.Image = CikisResmi;
                }
            }

            
        }

        private void histogramToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image == null)
            {
                MessageBox.Show("Resim ekleyiniz.");
            }
            else
            {
                ResminHistograminiCiz();
                label2.Visible = true;
                textBox2.Visible = true;
                listBox1.Visible = true;
            }
         
        }

        public void ResminHistograminiCiz()
        {
            ArrayList DiziPiksel = new ArrayList();
            int OrtalamaRenk = 0;
            Color OkunanRenk;
            int R = 0, G = 0, B = 0;
            Bitmap GirisResmi; //Histogram için giriş resmi gri-ton olmalıdır.
            GirisResmi = new Bitmap(pictureBox1.Image);
            int ResimGenisligi = GirisResmi.Width; //GirisResmi global tanımlandı.
            int ResimYuksekligi = GirisResmi.Height;
            int i = 0; //piksel sayısı tutulacak.
            for (int x = 0; x < GirisResmi.Width; x++)
            {
                for (int y = 0; y < GirisResmi.Height; y++)
                {
                    OkunanRenk = GirisResmi.GetPixel(x, y);
                    OrtalamaRenk = (int)(OkunanRenk.R + OkunanRenk.G + OkunanRenk.B) / 3; //Griton resimde üç kanal rengi aynı değere sahiptir.
                DiziPiksel.Add(OrtalamaRenk); //Resimdeki tüm noktaları diziye atıyor. 
                }
            }
            int[] DiziPikselSayilari = new int[256];
            for (int r = 0; r <= 255; r++) //256 tane renk tonu için dönecek.
            {
                int PikselSayisi = 0;
                for (int s = 0; s < DiziPiksel.Count; s++) //resimdeki piksel sayısınca dönecek. 
                {
                    if (r == Convert.ToInt16(DiziPiksel[s]))
                        PikselSayisi++;
                }
                DiziPikselSayilari[r] = PikselSayisi;
            }
            //Değerleri listbox'a ekliyor. 
            int RenkMaksPikselSayisi = 0; //Grafikte y eksenini ölçeklerken kullanılacak. 
            for (int k = 0; k <= 255; k++)
            {
                listBox1.Items.Add("Renk:" + k + "=" + DiziPikselSayilari[k]);
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
            CizimAlani = pictureBox2.CreateGraphics();
            pictureBox2.Refresh();
            int GrafikYuksekligi = 300;
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
                //Dikey kırmızı çizgiler.

            }
            textBox2.Text =RenkMaksPikselSayisi.ToString();
        }

        private void karsıtlıkContrastToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image == null)
            {
                MessageBox.Show("Resim ekleyiniz.");
            }
            else
            {


                if (txtK1.Text == "" && txtK2.Text == "" && txtK3.Text == "" && txtK4.Text == "")
                {
                    MessageBox.Show("Contrast değerlerini girin...");
                    txtK1.BackColor = Color.Red;
                    txtK1.Focus();
                    txtK2.BackColor = Color.Red;
                    txtK3.BackColor = Color.Red;
                    txtK4.BackColor = Color.Red;
                }
                else
                {
                    txtK1.BackColor = Color.White;
                    txtK2.BackColor = Color.White;
                    txtK3.BackColor = Color.White;
                    txtK4.BackColor = Color.White;
                    Color OkunanRenk, DonusenRenk;
                    int R = 0, G = 0, B = 0;
                    Bitmap GirisResmi, CikisResmi;
                    GirisResmi = new Bitmap(pictureBox1.Image);
                    int ResimGenisligi = GirisResmi.Width; //GirisResmi global tanımlandı. İçerisine görüntü yüklendi.
                    int ResimYuksekligi = GirisResmi.Height;
                    CikisResmi = new Bitmap(ResimGenisligi, ResimYuksekligi); //Cikis resmini oluşturuyor. Boyutları giriş resmi ile aynı olur. Tanımlaması globalde yapıldı.
                    int X1 = Convert.ToInt16(txtK1.Text);
                    int X2 = Convert.ToInt16(txtK2.Text);
                    int Y1 = Convert.ToInt16(txtK3.Text);
                    int Y2 = Convert.ToInt16(txtK4.Text);
                    int i = 0, j = 0; //Çıkış resminin x ve y si olacak.
                    for (int x = 0; x < ResimGenisligi; x++)
                    {
                        for (int y = 0; y < ResimYuksekligi; y++)
                        {
                            OkunanRenk = GirisResmi.GetPixel(x, y);
                            R = OkunanRenk.R;
                            G = OkunanRenk.G;
                            B = OkunanRenk.B;
                            int Gri = (R + G + B) / 3;
                            //*********** Kontras Formülü***************
                            int X = Gri;
                            int Y = ((((X - X1) * Y2 - Y1)) / (X2 - X1)) + Y1;
                            if (Y > 255) Y = 255;
                            if (Y < 0) Y = 0;
                            DonusenRenk = Color.FromArgb(Y, Y, Y);
                            CikisResmi.SetPixel(x, y, DonusenRenk);
                        }
                    }
                    pictureBox2.Refresh();
                    pictureBox2.Image = null;
                    pictureBox2.Image = CikisResmi;
                }


            }
           

        
    }

        private void tasımaToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (pictureBox1.Image == null)
            {
                MessageBox.Show("Resim ekleyiniz.");
            }
            else
            {
                Color OkunanRenk;
                Bitmap GirisResmi, CikisResmi;
                GirisResmi = new Bitmap(pictureBox1.Image);
                int ResimGenisligi = GirisResmi.Width;
                int ResimYuksekligi = GirisResmi.Height;
                CikisResmi = new Bitmap(ResimGenisligi, ResimYuksekligi);
                double x2 = 0, y2 = 0;
                //Taşıma mesafelerini atıyor. 
                int Tx = 100;
                int Ty = 50;
                for (int x1 = 0; x1 < (ResimGenisligi); x1++)
                {
                    for (int y1 = 0; y1 < (ResimYuksekligi); y1++)
                    {
                        OkunanRenk = GirisResmi.GetPixel(x1, y1);
                        x2 = x1 + Tx;
                        y2 = y1 + Ty;
                        if (x2 > 0 && x2 < ResimGenisligi && y2 > 0 && y2 < ResimYuksekligi)
                            CikisResmi.SetPixel((int)x2, (int)y2, OkunanRenk);
                    }
                }
                pictureBox2.Image = CikisResmi;
            }
            
        }

        private void aynalamaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image == null)
            {
                MessageBox.Show("Resim ekleyiniz.");
            }
            else
            {

                if (txtAci.Text == "")
                {
                    MessageBox.Show("Açı değeri giriniz!..");
                    txtAci.Focus();
                    txtAci.BackColor = Color.Red;
                }
                else
                {
                    txtAci.BackColor = Color.White;
                    Color OkunanRenk;
                    Bitmap GirisResmi, CikisResmi;
                    GirisResmi = new Bitmap(pictureBox1.Image);
                    int ResimGenisligi = GirisResmi.Width;
                    int ResimYuksekligi = GirisResmi.Height;
                    CikisResmi = new Bitmap(ResimGenisligi, ResimYuksekligi);
                    double Aci = Convert.ToDouble(txtAci.Text);
                    double RadyanAci = Aci * 2 * Math.PI / 360;
                    double x2 = 0, y2 = 0;
                    //Resim merkezini buluyor. Resim merkezi etrafında döndürecek. 
                    int x0 = ResimGenisligi / 2;
                    int y0 = ResimYuksekligi / 2;
                    for (int x1 = 0; x1 < (ResimGenisligi); x1++)
                    {
                        for (int y1 = 0; y1 < (ResimYuksekligi); y1++)
                        {
                            OkunanRenk = GirisResmi.GetPixel(x1, y1);
                            //----A-Orta dikey eksen etrafında aynalama ----------------
                            //x2 = Convert.ToInt16(-x1 + 2 * x0); 
                            //y2 = Convert.ToInt16(y1);
                            //----B-Orta yatay eksen etrafında aynalama ----------------
                            //x2 = Convert.ToInt16(x1);
                            //y2 = Convert.ToInt16(-y1 + 2 *y0);

                            //----C-Ortadan geçen 45 açılı çizgi etrafında aynalama----------
                            double Delta = (x1 - x0) * Math.Sin(RadyanAci) - (y1 - y0) * Math.Cos(RadyanAci);
                            x2 = Convert.ToInt16(x1 + 2 * Delta * (-Math.Sin(RadyanAci)));
                            y2 = Convert.ToInt16(y1 + 2 * Delta * (Math.Cos(RadyanAci)));
                            if (x2 > 0 && x2 < ResimGenisligi && y2 > 0 && y2 < ResimYuksekligi)
                                CikisResmi.SetPixel((int)x2, (int)y2, OkunanRenk);
                        }
                    }
                    pictureBox2.Image = CikisResmi;

                }

            }
           


        }

        private void egmeToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (pictureBox1.Image == null)
            {
                MessageBox.Show("Resim ekleyiniz.");
            }
            else
            {
                Color OkunanRenk;
                Bitmap GirisResmi, CikisResmi;
                GirisResmi = new Bitmap(pictureBox1.Image);
                int ResimGenisligi = GirisResmi.Width;
                int ResimYuksekligi = GirisResmi.Height;
                CikisResmi = new Bitmap(ResimGenisligi, ResimYuksekligi);
                //Taşıma mesafelerini atıyor. 
                double EgmeKatsayisi = 0.2;
                double x2 = 0, y2 = 0;
                for (int x1 = 0; x1 < (ResimGenisligi); x1++)
                {
                    for (int y1 = 0; y1 < (ResimYuksekligi); y1++)
                    {
                        OkunanRenk = GirisResmi.GetPixel(x1, y1);
                        // +X ekseni yönünde
                        //x2 = x1 + EgmeKatsayisi * y1;
                        //y2 = y1;
                        // -X ekseni yönünde
                        //x2 = x1 - EgmeKatsayisi * y1;
                        //y2 = y1;
                        // +Y ekseni yönünde
                        //x2 = x1;
                        //y2 = EgmeKatsayisi * x1 + y1;
                        // -Y ekseni yönünde
                        x2 = x1;
                        y2 = -EgmeKatsayisi * x1 + y1;

                        if (x2 > 0 && x2 < ResimGenisligi && y2 > 0 && y2 < ResimYuksekligi)
                            CikisResmi.SetPixel((int)x2, (int)y2, OkunanRenk);
                    }
                }
                pictureBox2.Image = CikisResmi;

            }

            
        }

        private void zoomInToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (pictureBox1.Image == null)
            {
                MessageBox.Show("Resim ekleyiniz.");
            }

            else
            {
                Color OkunanRenk, DonusenRenk;
                Bitmap GirisResmi, CikisResmi;
                GirisResmi = new Bitmap(pictureBox1.Image);
                int ResimGenisligi = GirisResmi.Width;
                int ResimYuksekligi = GirisResmi.Height;
                CikisResmi = new Bitmap(ResimGenisligi, ResimYuksekligi);
                int x2 = 0, y2 = 0; //Çıkış resminin x ve y si olacak.
                int KucultmeKatsayisi = 2;
                for (int x1 = 0; x1 < ResimGenisligi; x1 = x1 + KucultmeKatsayisi)
                {
                    y2 = 0;
                    for (int y1 = 0; y1 < ResimYuksekligi; y1 = y1 + KucultmeKatsayisi)
                    {
                        OkunanRenk = GirisResmi.GetPixel(x1, y1);
                        DonusenRenk = OkunanRenk;
                        CikisResmi.SetPixel(x2, y2, DonusenRenk);
                        y2++;
                    }
                    x2++;
                }
                pictureBox2.Image = CikisResmi;
            }
            

        }

        private void dondurmeToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (pictureBox1.Image == null)
            {
                MessageBox.Show("Resim ekleyiniz.");
            }
            else
            {

                if (txtDon.Text == "")
                {
                    MessageBox.Show("Döndürme açısı değerini giriniz!..");
                    txtDon.Focus();
                    txtDon.BackColor = Color.Red;
                }
                else
                {
                    txtDon.BackColor = Color.White;
                    Color OkunanRenk;
                    Bitmap GirisResmi, CikisResmi;
                    GirisResmi = new Bitmap(pictureBox1.Image);
                    int ResimGenisligi = GirisResmi.Width;
                    int ResimYuksekligi = GirisResmi.Height;
                    CikisResmi = new Bitmap(ResimGenisligi, ResimYuksekligi);
                    int Aci = Convert.ToInt16(txtDon.Text);
                    double RadyanAci = Aci * 2 * Math.PI / 360;
                    double x2 = 0, y2 = 0;
                    //Resim merkezini buluyor. Resim merkezi etrafında döndürecek. 
                    int x0 = ResimGenisligi / 2;
                    int y0 = ResimYuksekligi / 2;
                    for (int x1 = 0; x1 < (ResimGenisligi); x1++)
                    {
                        for (int y1 = 0; y1 < (ResimYuksekligi); y1++)
                        {
                            OkunanRenk = GirisResmi.GetPixel(x1, y1);
                            //Döndürme Formülleri
                            x2 = Math.Cos(RadyanAci) * (x1 - x0) - Math.Sin(RadyanAci) * (y1 - y0) + x0;
                            y2 = Math.Sin(RadyanAci) * (x1 - x0) + Math.Cos(RadyanAci) * (y1 - y0) + y0;
                            if (x2 > 0 && x2 < ResimGenisligi && y2 > 0 && y2 < ResimYuksekligi)
                                CikisResmi.SetPixel((int)x2, (int)y2, OkunanRenk);
                        }
                    }
                    pictureBox2.Image = CikisResmi;
                }


            }
            

        }

        private void perspektifToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (pictureBox1.Image == null)
            {
                MessageBox.Show("Resim ekleyiniz.");
            }
            else
            {
                double x1 = 1;
                double y1 = 2;
                double x2 = 3;
                double y2 = 4;
                double x3 = 5;
                double y3 = 6;
                double x4 = 7;
                double y4 = 8;
                double X1 = 9;
                double Y1 = 10;
                double X2 = 11;
                double Y2 = 12;
                double X3 = 13;
                double Y3 = 14;
                double X4 = 15;
                double Y4 = 16;
                double[,] GirisMatrisi = new double[8, 8];
                // { x1, y1, 1, 0, 0, 0, -x1 * X1, -y1 * X1 }



                GirisMatrisi[0, 0] = x1;
                GirisMatrisi[0, 1] = y1;
                GirisMatrisi[0, 2] = 1;
                GirisMatrisi[0, 3] = 0;
                GirisMatrisi[0, 4] = 0;
                GirisMatrisi[0, 5] = 0;
                GirisMatrisi[0, 6] = -x1 * X1;
                GirisMatrisi[0, 7] = -y1 * X1;
                //{ 0, 0, 0, x1, y1, 1, -x1 * Y1, -y1 * Y1 }
                GirisMatrisi[1, 0] = 0;
                GirisMatrisi[1, 1] = 0;
                GirisMatrisi[1, 2] = 0;
                GirisMatrisi[1, 3] = x1;
                GirisMatrisi[1, 4] = y1;
                GirisMatrisi[1, 5] = 1;
                GirisMatrisi[1, 6] = -x1 * Y1;
                GirisMatrisi[1, 7] = -y1 * Y1;
                //{ x2, y2, 1, 0, 0, 0, -x2 * X2, -y2 * X2 } 
                GirisMatrisi[2, 0] = x2;
                GirisMatrisi[2, 1] = y2;
                GirisMatrisi[2, 2] = 1;
                GirisMatrisi[2, 3] = 0;
                GirisMatrisi[2, 4] = 0;
                GirisMatrisi[2, 5] = 0;
                GirisMatrisi[2, 6] = -x2 * X2;
                GirisMatrisi[2, 7] = -y2 * X2;
                //{ 0, 0, 0, x2, y2, 1, -x2 * Y2, -y2 * Y2 }
                GirisMatrisi[3, 0] = 0;
                GirisMatrisi[3, 1] = 0;
                GirisMatrisi[3, 2] = 0;
                GirisMatrisi[3, 3] = x2;
                GirisMatrisi[3, 4] = y2;
                GirisMatrisi[3, 5] = 1;
                GirisMatrisi[3, 6] = -x2 * Y2;
                GirisMatrisi[3, 7] = -y2 * Y2;
                //{ x3, y3, 1, 0, 0, 0, -x3 * X3, -y3 * X3 }
                GirisMatrisi[4, 0] = x3;
                GirisMatrisi[4, 1] = y3;
                GirisMatrisi[4, 2] = 1;
                GirisMatrisi[4, 3] = 0;
                GirisMatrisi[4, 4] = 0;
                GirisMatrisi[4, 5] = 0;
                GirisMatrisi[4, 6] = -x3 * X3;
                GirisMatrisi[4, 7] = -y3 * X3;
                //{ 0, 0, 0, x3, y3, 1, -x3 * Y3, -y3 * Y3 }
                GirisMatrisi[5, 0] = 0;
                GirisMatrisi[5, 1] = 0;
                GirisMatrisi[5, 2] = 0;
                GirisMatrisi[5, 3] = x3;
                GirisMatrisi[5, 4] = y3;
                GirisMatrisi[5, 5] = 1;
                GirisMatrisi[5, 6] = -x3 * Y3;
                GirisMatrisi[5, 7] = -y3 * Y3;
                //{ x4, y4, 1, 0, 0, 0, -x4 * X4, -y4 * X4 }
                GirisMatrisi[6, 0] = x4;
                GirisMatrisi[6, 1] = y4;
                GirisMatrisi[6, 2] = 1;
                GirisMatrisi[6, 3] = 0;
                GirisMatrisi[6, 4] = 0;
                GirisMatrisi[6, 5] = 0;
                GirisMatrisi[6, 6] = -x4 * X4;
                GirisMatrisi[6, 7] = -y4 * X4;
                //{ 0, 0, 0, x4, y4, 1, -x4 * Y4, -y4 * Y4 } 
                GirisMatrisi[7, 0] = 0;
                GirisMatrisi[7, 1] = 0;
                GirisMatrisi[7, 2] = 0;
                GirisMatrisi[7, 3] = x4;
                GirisMatrisi[7, 4] = y4;
                GirisMatrisi[7, 5] = 1;
                GirisMatrisi[7, 6] = -x4 * Y4;
                GirisMatrisi[7, 7] = -y4 * Y4;
                //---------------------------------------------------------------------------
                double[,] matrisBTersi = MatrisTersiniAl(GirisMatrisi);
                //----------------------------------- A Dönüşüm Matrisi (3x3) -----------------


                double a00 = 0, a01 = 0, a02 = 0, a10 = 0, a11 = 0, a12 = 0, a20 = 0, a21 = 0, a22 = 0;
                a00 = matrisBTersi[0, 0] * X1 + matrisBTersi[0, 1] * Y1 + matrisBTersi[0, 2] * X2 + matrisBTersi[0, 3] * Y2 + matrisBTersi[0, 4] * X3 + matrisBTersi[0, 5] * Y3 + matrisBTersi[0, 6] * X4 + matrisBTersi[0, 7] * Y4;
                a01 = matrisBTersi[1, 0] * X1 + matrisBTersi[1, 1] * Y1 + matrisBTersi[1, 2] *
               X2 + matrisBTersi[1, 3] * Y2 + matrisBTersi[1, 4] * X3 + matrisBTersi[1, 5] * Y3 +
               matrisBTersi[1, 6] * X4 + matrisBTersi[1, 7] * Y4;
                a02 = matrisBTersi[2, 0] * X1 + matrisBTersi[2, 1] * Y1 + matrisBTersi[2, 2] *
               X2 + matrisBTersi[2, 3] * Y2 + matrisBTersi[2, 4] * X3 + matrisBTersi[2, 5] * Y3 +
               matrisBTersi[2, 6] * X4 + matrisBTersi[2, 7] * Y4;
                a10 = matrisBTersi[3, 0] * X1 + matrisBTersi[3, 1] * Y1 + matrisBTersi[3, 2] *
               X2 + matrisBTersi[3, 3] * Y2 + matrisBTersi[3, 4] * X3 + matrisBTersi[3, 5] * Y3 +
               matrisBTersi[3, 6] * X4 + matrisBTersi[3, 7] * Y4;
                a11 = matrisBTersi[4, 0] * X1 + matrisBTersi[4, 1] * Y1 + matrisBTersi[4, 2] *
               X2 + matrisBTersi[4, 3] * Y2 + matrisBTersi[4, 4] * X3 + matrisBTersi[4, 5] * Y3 +
               matrisBTersi[4, 6] * X4 + matrisBTersi[4, 7] * Y4;
                a12 = matrisBTersi[5, 0] * X1 + matrisBTersi[5, 1] * Y1 + matrisBTersi[5, 2] *
               X2 + matrisBTersi[5, 3] * Y2 + matrisBTersi[5, 4] * X3 + matrisBTersi[5, 5] * Y3 +
               matrisBTersi[5, 6] * X4 + matrisBTersi[5, 7] * Y4;
                a20 = matrisBTersi[6, 0] * X1 + matrisBTersi[6, 1] * Y1 + matrisBTersi[6, 2] *
               X2 + matrisBTersi[6, 3] * Y2 + matrisBTersi[6, 4] * X3 + matrisBTersi[6, 5] * Y3 +
               matrisBTersi[6, 6] * X4 + matrisBTersi[6, 7] * Y4;
                a21 = matrisBTersi[7, 0] * X1 + matrisBTersi[7, 1] * Y1 + matrisBTersi[7, 2] *
                X2 + matrisBTersi[7, 3] * Y2 + matrisBTersi[7, 4] * X3 + matrisBTersi[7, 5] * Y3 +
                matrisBTersi[7, 6] * X4 + matrisBTersi[7, 7] * Y4;
                a22 = 1;
                //------------------------- Perspektif düzeltme işlemi ------------------------

                PerspektifDuzelt(a00, a01, a02, a10, a11, a12, a20, a21, a22);
            }


          


        }

        public void meanFiltresi()
        {
            if (txtMean.Text=="")
            {
                MessageBox.Show("Şablon boyutu giriniz...");
                txtMean.Focus();
                txtMean.BackColor = Color.Red;
            }
            else
            {
                txtMean.BackColor = Color.White;
                Color OkunanRenk;
                Bitmap GirisResmi, CikisResmi;
                GirisResmi = new Bitmap(pictureBox1.Image);
                int ResimGenisligi = GirisResmi.Width;
                int ResimYuksekligi = GirisResmi.Height;
                CikisResmi = new Bitmap(ResimGenisligi, ResimYuksekligi);
                int SablonBoyutu = Convert.ToInt32(txtMean.Text); //şablon boyutu 3 den büyük tek rakam  olmalıdır(3, 5, 7 gibi).
                int x, y, i, j, toplamR, toplamG, toplamB, ortalamaR, ortalamaG, ortalamaB;
                for (x = (SablonBoyutu - 1) / 2; x < ResimGenisligi - (SablonBoyutu - 1) / 2; x++)
                {
                    for (y = (SablonBoyutu - 1) / 2; y < ResimYuksekligi - (SablonBoyutu - 1) / 2; y++)
                    {
                        toplamR = 0;
                        toplamG = 0;
                        toplamB = 0;
                        for (i = -((SablonBoyutu - 1) / 2); i <= (SablonBoyutu - 1) / 2; i++)
                        {
                            for (j = -((SablonBoyutu - 1) / 2); j <= (SablonBoyutu - 1) / 2; j++)
                            {
                                OkunanRenk = GirisResmi.GetPixel(x + i, y + j);
                                toplamR = toplamR + OkunanRenk.R;
                                toplamG = toplamG + OkunanRenk.G;
                                toplamB = toplamB + OkunanRenk.B;
                            }
                        }
                        ortalamaR = toplamR / (SablonBoyutu * SablonBoyutu);
                        ortalamaG = toplamG / (SablonBoyutu * SablonBoyutu);
                        ortalamaB = toplamB / (SablonBoyutu * SablonBoyutu);
                        CikisResmi.SetPixel(x, y, Color.FromArgb(ortalamaR, ortalamaG, ortalamaB));
                    }
                }
                pictureBox2.Image = CikisResmi;
            }
        }

        private void meanFilterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image == null)
            {
                MessageBox.Show("Resim ekleyiniz.");
            }
            else
            {
                meanFiltresi();
            }
            
        }


        public void medianFiltresi()
        {

            if (txtMean.Text == "")
            {
                MessageBox.Show("Şablon boyutu giriniz...");
                txtMean.Focus();
                txtMean.BackColor = Color.Red;
            }
            else
            {
                txtMean.BackColor = Color.White;
                Color OkunanRenk;
                Bitmap GirisResmi, CikisResmi;
                GirisResmi = new Bitmap(pictureBox1.Image);
                int ResimGenisligi = GirisResmi.Width;
                int ResimYuksekligi = GirisResmi.Height;
                CikisResmi = new Bitmap(ResimGenisligi, ResimYuksekligi);
                int SablonBoyutu = Convert.ToInt32(txtMean.Text); //şablon boyutu 3 den büyük tek rakam olmalıdır(3, 5, 7 gibi).
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
                pictureBox2.Image = CikisResmi;
            }

        }
           

        private void medianFilterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image == null)
            {
                MessageBox.Show("Resim ekleyiniz.");
            }
            else
            {
                medianFiltresi();
            }

        }

        private void gaussFilterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image == null)
            {
                MessageBox.Show("Resim ekleyiniz.");
            }
            else
            {
                Color OkunanRenk;
                Bitmap GirisResmi, CikisResmi;
                GirisResmi = new Bitmap(pictureBox1.Image);
                int ResimGenisligi = GirisResmi.Width;
                int ResimYuksekligi = GirisResmi.Height;
                CikisResmi = new Bitmap(ResimGenisligi, ResimYuksekligi);
                int SablonBoyutu = 5; //Çekirdek matrisin boyutu
                int ElemanSayisi = SablonBoyutu * SablonBoyutu;
                int x, y, i, j, toplamR, toplamG, toplamB, ortalamaR, ortalamaG, ortalamaB;
                int[] Matris = { 1, 4, 7, 4, 1, 4, 20, 33, 20, 4, 7, 33, 55, 33, 7, 4, 20, 33, 20, 4, 1, 4, 7, 4, 1 };
                int MatrisToplami = 1 + 4 + 7 + 4 + 1 + 4 + 20 + 33 + 20 + 4 + 7 + 33 + 55 + 33 + 7 + 4 + 20 +
               33 + 20 + 4 + 1 + 4 + 7 + 4 + 1;
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
                            ortalamaR = toplamR / MatrisToplami;
                            ortalamaG = toplamG / MatrisToplami;
                            ortalamaB = toplamB / MatrisToplami;
                            CikisResmi.SetPixel(x, y, Color.FromArgb(ortalamaR, ortalamaG, ortalamaB));
                        }
                    }
                }
                pictureBox2.Image = CikisResmi;
            }
            

        }

        public Bitmap OrjianalResimdenBulanikResmiCikarma(Bitmap OrjinalResim, Bitmap BulanikResim)
        {
            Color OkunanRenk1, OkunanRenk2, DonusenRenk;
            Bitmap CikisResmi;
            int ResimGenisligi = OrjinalResim.Width;
            int ResimYuksekligi = OrjinalResim.Height;
            CikisResmi = new Bitmap(ResimGenisligi, ResimYuksekligi);
            int R, G, B;
            double Olcekleme = 2; //Keskin kenaları daha iyi görmek için değerini artırıyoruz.
            for (int x = 0; x < ResimGenisligi; x++)
            {
                for (int y = 0; y < ResimYuksekligi; y++)
                {
                    OkunanRenk1 = OrjinalResim.GetPixel(x, y);
                    OkunanRenk2 = BulanikResim.GetPixel(x, y);
                    R = Convert.ToInt16(Olcekleme * Math.Abs(OkunanRenk1.R - OkunanRenk2.R));
                    G = Convert.ToInt16(Olcekleme * Math.Abs(OkunanRenk1.G - OkunanRenk2.G));
                    B = Convert.ToInt16(Olcekleme * Math.Abs(OkunanRenk1.B - OkunanRenk2.B));
                    //===============================================================
                    //Renkler sınırların dışına çıktıysa, sınır değer alınacak. (Dikkat: Normalizasyon yapılmamıştır. )
 if (R > 255) R = 255;
            if (G > 255) G = 255;
            if (B > 255) B = 255;
            if (R < 0) R = 0;
            if (G < 0) G = 0;
            if (B < 0) B = 0;
            //================================================================
            DonusenRenk = Color.FromArgb(R, G, B);
            CikisResmi.SetPixel(x, y, DonusenRenk);
        }
    }
 return CikisResmi;
 }

        public Bitmap KenarGoruntusuIleOrjinalResmiBirlestir(Bitmap OrjinalResim, Bitmap KenarGoruntusu)
        {
            Color OkunanRenk1, OkunanRenk2, DonusenRenk;
            Bitmap CikisResmi;
            int ResimGenisligi = OrjinalResim.Width;
            int ResimYuksekligi = OrjinalResim.Height;
            CikisResmi = new Bitmap(ResimGenisligi, ResimYuksekligi);
            int R, G, B;
            for (int x = 0; x < ResimGenisligi; x++)
            {
                for (int y = 0; y < ResimYuksekligi; y++)
                {
                    OkunanRenk1 = OrjinalResim.GetPixel(x, y);
                    OkunanRenk2 = KenarGoruntusu.GetPixel(x, y);
                    R = OkunanRenk1.R + OkunanRenk2.R;
                    G = OkunanRenk1.G + OkunanRenk2.G;
                    B = OkunanRenk1.B + OkunanRenk2.B;
                    //===============================================================
                    //Renkler sınırların dışına çıktıysa, sınır değer alınacak. //DİKKAT: Burada sınırı aşan değerler NORMALİZASYON yaparak programlanmalıdır.
                    if (R > 255) R = 255;
                    if (G > 255) G = 255;
                    if (B > 255) B = 255;
                    if (R < 0) R = 0;
                    if (G < 0) G = 0;
                    if (B < 0) B = 0;
                    //================================================================
                    DonusenRenk = Color.FromArgb(R, G, B);
                    CikisResmi.SetPixel(x, y, DonusenRenk);
                }
            }
            return CikisResmi;
        }



        private void netlestirmeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Bitmap OrjinalResim = new Bitmap(pictureBox1.Image);

            //Bitmap BulanikResim = meanFiltresi();
            ////Bitmap BulanikResim = GaussFiltresi();
            //Bitmap KenarGoruntusu = OrjianalResimdenBulanikResmiCikarma(OrjinalResim, BulanikResim);
            //Bitmap NetlesmisResim = KenarGoruntusuIleOrjinalResmiBirlestir(OrjinalResim, KenarGoruntusu);
            //pictureBox2.Image = NetlesmisResim;

        }

        private void konvolusyonToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (pictureBox1.Image == null)
            {
                MessageBox.Show("Resim ekleyiniz.");
            }
            else
            {
                Color OkunanRenk;
                Bitmap GirisResmi, CikisResmi;
                GirisResmi = new Bitmap(pictureBox1.Image);
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
                pictureBox2.Image = CikisResmi;
            }
            

        }

        private void sobelFilterToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (pictureBox1.Image == null)
            {
                MessageBox.Show("Resim ekleyiniz.");
            }
            else
            {
                Bitmap GirisResmi, CikisResmiXY, CikisResmiX, CikisResmiY;
                GirisResmi = new Bitmap(pictureBox1.Image);
                int ResimGenisligi = GirisResmi.Width;
                int ResimYuksekligi = GirisResmi.Height;
                CikisResmiX = new Bitmap(ResimGenisligi, ResimYuksekligi);
                CikisResmiY = new Bitmap(ResimGenisligi, ResimYuksekligi);
                CikisResmiXY = new Bitmap(ResimGenisligi, ResimYuksekligi);
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
                        //Hesaplamayı yapan Sobel Temsili matrisi ve formülü.
                        int Gx = Math.Abs(-P1 + P3 - 2 * P4 + 2 * P6 - P7 + P9); //Dikey çizgiler
                        int Gy = Math.Abs(P1 + 2 * P2 + P3 - P7 - 2 * P8 - P9); //Yatay Çizgiler
                                                                                //if (Gx > 100)
                                                                                // Gx = 255;
                                                                                //else
                                                                                // Gx = 0;
                                                                                //if (Gy > 100)
                                                                                // Gy = 255;
                                                                                //else
                                                                                // Gy = 0;
                        int Gxy = Gx + Gy;
                        //if (Gxy > Esikleme)
                        // Gxy = 255;
                        //else
                        // Gxy = 0;
                        //Renkler sınırların dışına çıktıysa, sınır değer alınacak. Negatif olamaz, formüllerde mutlak değer vardır.
                        if (Gx > 255) Gx = 255;
                        if (Gy > 255) Gy = 255;
                        if (Gxy > 255) Gxy = 255;
                        //int TetaRadyan = 0;
                        //if (Gy != 0)
                        // TetaRadyan = Convert.ToInt32(Math.Atan(Gx / Gy));
                        //else
                        // TetaRadyan = Convert.ToInt32(Math.Atan(Gx));
                        //int TetaDerece = Convert.ToInt32((TetaRadyan * 360) / (2 * Math.PI));
                        //if (TetaDerece >= 0 && TetaDerece < 45)
                        // CikisResmiXY.SetPixel(x, y, Color.FromArgb(0, 0, 0));
                        //if (TetaDerece >= 45 && TetaDerece < 90)
                        // CikisResmiXY.SetPixel(x, y, Color.FromArgb(0, 255, 0));
                        //if (TetaDerece >= 90 && TetaDerece < 135)
                        // CikisResmiXY.SetPixel(x, y, Color.FromArgb(0, 0, 255));
                        //if (TetaDerece >= 135 && TetaDerece < 180)
                        // CikisResmiXY.SetPixel(x, y, Color.FromArgb(255, 255, 0));
                        CikisResmiX.SetPixel(x, y, Color.FromArgb(Gx, Gx, Gx));
                        CikisResmiY.SetPixel(x, y, Color.FromArgb(Gy, Gy, Gy));
                        CikisResmiXY.SetPixel(x, y, Color.FromArgb(Gxy, Gxy, Gxy));
                    }
                }
                pictureBox2.Image = CikisResmiXY;
                // pictureBox3.Image = CikisResmiX;
                //pictureBox4.Image = CikisResmiY;

            }


        }

        private void prewittToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (pictureBox1.Image == null)
            {
                MessageBox.Show("Resim ekleyiniz.");
            }
            else
            {
                Bitmap GirisResmi, CikisResmi;
                GirisResmi = new Bitmap(pictureBox1.Image);
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
                pictureBox2.Image = CikisResmi;
            }


           
        }

        private void robertCrossToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image == null)
            {
                MessageBox.Show("Resim ekleyiniz.");
            }
            else
            {
                Bitmap GirisResmi, CikisResmi;
                GirisResmi = new Bitmap(pictureBox1.Image);
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
                pictureBox2.Image = CikisResmi;
            }


           

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
 }
