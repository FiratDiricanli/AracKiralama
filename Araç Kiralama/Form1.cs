using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Araç_Kiralama
{
    public class Arac
    {
        [DisplayName("Araç ID")]
        public int arac_id { get; set; }

        [DisplayName("Marka")]
        public string marka { get; set; }

        [DisplayName("Model")]
        public string model { get; set; }

        [DisplayName("Kilometre")]
        public int kilometre { get; set; }

        [DisplayName("Müsait Mi?")]
        public bool musait_mi { get; set; }

        public void arac_durumu_guncelle(bool yeniDurum)
        {
            this.musait_mi = yeniDurum;
        }

        public void kilometre_guncelle(int yeniKilometre)
        {
            if (yeniKilometre > this.kilometre)
            {
                this.kilometre = yeniKilometre;
            }
        }

        public override string ToString() => $"{marka} {model} ({kilometre} km)";
    }

    public class Musteri
    {
        [DisplayName("Müşteri ID")]
        public int musteri_id { get; set; }

        [DisplayName("Ad Soyad")]
        public string ad_soyad { get; set; }

        [DisplayName("Telefon")]
        public string telefon { get; set; }

        public override string ToString() => ad_soyad;
    }

    public class Kiralama
    {
        [Browsable(false)]
        public int kiralanan_arac_id { get; set; }

        [DisplayName("İşlem ID")]
        public int islem_id { get; set; }

        [DisplayName("Kiralanan Araç")]
        public string arac_bilgi { get; set; }

        [DisplayName("Müşteri")]
        public string musteri_bilgi { get; set; }

        [DisplayName("İşlem Tarihi")]
        public string tarih { get; set; }

        [DisplayName("İşlem Durumu")]
        public string durum { get; set; }
    }

    public partial class Form1 : Form
    {
        List<Arac> araclar = new List<Arac>();
        List<Musteri> musteriler = new List<Musteri>();
        List<Kiralama> kiralamaGecmisi = new List<Kiralama>();
        int islemSayac = 5001;
        int musteriSayac = 101;

        TabControl sekmeler;
        TabPage sekmeArac, sekmeMusteri, sekmeKiralama;
        DataGridView dgvAraclar, dgvMusteriler, dgvKiralamalar;
        ComboBox cmbAracSec, cmbMusteriSec;
        TextBox txtMusteriAd, txtMusteriTel, txtIadeKm;

        public Form1()
        {
            this.Text = "Araç Paylaşım Sistemi - 2300005412 Fırat Diricanlı";
            this.Size = new Size(1150, 750);
            this.StartPosition = FormStartPosition.CenterScreen;

            VerileriHazirla();
            ArayuzuInsaEt();
        }

        private void VerileriHazirla()
        {
            araclar.Add(new Arac { arac_id = 1, marka = "Renault", model = "Clio", kilometre = 45000, musait_mi = true });
            araclar.Add(new Arac { arac_id = 2, marka = "Fiat", model = "Egea", kilometre = 32000, musait_mi = true });
            araclar.Add(new Arac { arac_id = 3, marka = "Honda", model = "Civic", kilometre = 15000, musait_mi = true });
            araclar.Add(new Arac { arac_id = 4, marka = "Volkswagen", model = "Golf", kilometre = 60000, musait_mi = true });
            araclar.Add(new Arac { arac_id = 5, marka = "Ford", model = "Focus", kilometre = 55000, musait_mi = true });
            araclar.Add(new Arac { arac_id = 6, marka = "Toyota", model = "Corolla", kilometre = 25000, musait_mi = true });
            araclar.Add(new Arac { arac_id = 7, marka = "BMW", model = "320i", kilometre = 80000, musait_mi = true });
            araclar.Add(new Arac { arac_id = 8, marka = "Mercedes-Benz", model = "C200", kilometre = 90000, musait_mi = true });
            araclar.Add(new Arac { arac_id = 9, marka = "Audi", model = "A3", kilometre = 40000, musait_mi = true });
            araclar.Add(new Arac { arac_id = 10, marka = "Hyundai", model = "i20", kilometre = 12000, musait_mi = true });

            musteriler.Add(new Musteri { musteri_id = 100, ad_soyad = "Fırat Diricanlı", telefon = "0555 123 45 67" });
        }

        private void ArayuzuInsaEt()
        {
            sekmeler = new TabControl { Dock = DockStyle.Fill, Font = new Font("Segoe UI", 11, FontStyle.Bold) };

            sekmeArac = new TabPage("Araç Filosu");
            sekmeMusteri = new TabPage("Müşteri Yönetimi");
            sekmeKiralama = new TabPage("Kiralama ve İade İşlemleri");

            dgvAraclar = CreateGrid();
            sekmeArac.Controls.Add(dgvAraclar);

            Panel pnlMusteri = new Panel { Dock = DockStyle.Top, Height = 90, BackColor = Color.WhiteSmoke };
            txtMusteriAd = new TextBox { Location = new Point(20, 30), Width = 200, PlaceholderText = "Ad Soyad" };
            txtMusteriTel = new TextBox { Location = new Point(240, 30), Width = 150, PlaceholderText = "Telefon Numarası" };
            Button btnMusteriEkle = new Button { Text = "MÜŞTERİ KAYDET", Location = new Point(410, 28), Size = new Size(220, 32), BackColor = Color.SeaGreen, ForeColor = Color.White };
            btnMusteriEkle.Click += (s, e) => {
                if (!string.IsNullOrWhiteSpace(txtMusteriAd.Text))
                {
                    musteriler.Add(new Musteri { musteri_id = musteriSayac++, ad_soyad = txtMusteriAd.Text, telefon = txtMusteriTel.Text });
                    txtMusteriAd.Clear(); txtMusteriTel.Clear();
                    VerileriEkranaYansit();
                }
                else
                {
                    MessageBox.Show("Lütfen müşteri adı ve soyadı bilgisini giriniz.", "Eksik Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            };
            dgvMusteriler = CreateGrid();
            pnlMusteri.Controls.AddRange(new Control[] { txtMusteriAd, txtMusteriTel, btnMusteriEkle });
            sekmeMusteri.Controls.Add(dgvMusteriler);
            sekmeMusteri.Controls.Add(pnlMusteri);

            Panel pnlKiralama = new Panel { Dock = DockStyle.Top, Height = 140, BackColor = Color.WhiteSmoke };

            Label l1 = new Label { Text = "Müsait Araçlar:", Location = new Point(20, 20), AutoSize = true };
            cmbAracSec = new ComboBox { Location = new Point(160, 18), Width = 250, DropDownStyle = ComboBoxStyle.DropDownList };

            Label l2 = new Label { Text = "Müşteri Seçimi:", Location = new Point(20, 70), AutoSize = true };
            cmbMusteriSec = new ComboBox { Location = new Point(160, 68), Width = 250, DropDownStyle = ComboBoxStyle.DropDownList };

            Button btnKirala = new Button { Text = "ARACI KİRALA", Location = new Point(440, 18), Size = new Size(160, 80), BackColor = Color.Teal, ForeColor = Color.White };
            btnKirala.Click += (s, e) => AracKirala();

            Label l3 = new Label { Text = "İade Kilometresi:", Location = new Point(640, 20), AutoSize = true };
            txtIadeKm = new TextBox { Location = new Point(780, 18), Width = 100 };

            Button btnIadeAl = new Button { Text = "SEÇİLİ ARACI İADE AL", Location = new Point(640, 58), Size = new Size(240, 40), BackColor = Color.Indigo, ForeColor = Color.White };
            btnIadeAl.Click += (s, e) => AracIadeAl();

            dgvKiralamalar = CreateGrid();

            pnlKiralama.Controls.AddRange(new Control[] { l1, cmbAracSec, l2, cmbMusteriSec, btnKirala, l3, txtIadeKm, btnIadeAl });
            sekmeKiralama.Controls.Add(dgvKiralamalar);
            sekmeKiralama.Controls.Add(pnlKiralama);

            sekmeler.TabPages.AddRange(new TabPage[] { sekmeArac, sekmeMusteri, sekmeKiralama });
            this.Controls.Add(sekmeler);

            VerileriEkranaYansit();
        }

        private DataGridView CreateGrid()
        {
            return new DataGridView
            {
                Dock = DockStyle.Fill,
                BackgroundColor = Color.White,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                AllowUserToAddRows = false,
                ReadOnly = true,
                RowHeadersVisible = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect
            };
        }

        private void ComboKutulariniDoldur()
        {
            cmbAracSec.Items.Clear();
            cmbMusteriSec.Items.Clear();

            foreach (var arac in araclar.Where(a => a.musait_mi)) cmbAracSec.Items.Add(arac);
            foreach (var musteri in musteriler) cmbMusteriSec.Items.Add(musteri);
        }

        private void AracKirala()
        {
            if (cmbAracSec.SelectedItem is Arac seciliArac && cmbMusteriSec.SelectedItem is Musteri seciliMusteri)
            {
                seciliArac.arac_durumu_guncelle(false);

                kiralamaGecmisi.Add(new Kiralama
                {
                    islem_id = islemSayac++,
                    kiralanan_arac_id = seciliArac.arac_id,
                    arac_bilgi = $"{seciliArac.marka} {seciliArac.model}",
                    musteri_bilgi = seciliMusteri.ad_soyad,
                    tarih = DateTime.Now.ToString("dd.MM.yyyy HH:mm"),
                    durum = "Aktif"
                });

                VerileriEkranaYansit();
                MessageBox.Show("Kiralama işlemi başarıyla gerçekleştirilmiştir.", "İşlem Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Lütfen işleme devam etmek için bir araç ve müşteri seçiniz.", "Eksik Seçim", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void AracIadeAl()
        {
            if (dgvKiralamalar.SelectedRows.Count > 0)
            {
                var seciliIslem = dgvKiralamalar.SelectedRows[0].DataBoundItem as Kiralama;

                if (seciliIslem != null && seciliIslem.durum == "Aktif")
                {
                    if (int.TryParse(txtIadeKm.Text, out int yeniKm))
                    {
                        var iadeEdilenArac = araclar.FirstOrDefault(a => a.arac_id == seciliIslem.kiralanan_arac_id);
                        if (iadeEdilenArac != null)
                        {
                            iadeEdilenArac.kilometre_guncelle(yeniKm);
                            iadeEdilenArac.arac_durumu_guncelle(true);
                        }

                        seciliIslem.durum = "Tamamlandı";
                        txtIadeKm.Clear();

                        VerileriEkranaYansit();
                        MessageBox.Show("İade işlemi başarıyla tamamlanmış olup, araç filoya yeniden eklenmiştir.", "İşlem Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Lütfen geçerli bir kilometre değeri giriniz.", "Hatalı Veri Girişi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Seçili kiralama işlemi halihazırda tamamlanmış durumdadır.", "Geçersiz İşlem", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                MessageBox.Show("Lütfen iade işlemi için listeden aktif bir kayıt seçiniz.", "Seçim Yapılmadı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void VerileriEkranaYansit()
        {
            dgvAraclar.DataSource = null; dgvAraclar.DataSource = araclar.ToList();
            dgvMusteriler.DataSource = null; dgvMusteriler.DataSource = musteriler.ToList();
            dgvKiralamalar.DataSource = null; dgvKiralamalar.DataSource = kiralamaGecmisi.ToList();
            ComboKutulariniDoldur();
        }
    }
}