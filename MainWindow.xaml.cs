using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Biblioteke
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private ObservableCollection<Biblioteka> biblioteke = new ObservableCollection<Biblioteka>();
        private int indeks;
        public MainWindow()
        {


            InitializeComponent();

            try
            {
                ucitaj("../Podaci.txt");
           
            }
            catch (Exception e)
            {
                MessageBox.Show("Greska pri ucitavanju fajla!!!");
                MessageBox.Show(e.Message);
            }

            lb_biblioteke.ItemsSource = biblioteke;     //biblioteke

            Biblioteka b1 = new Biblioteka();       //pregled svih knjiga
            foreach (Biblioteka b in biblioteke) { 
                foreach (Knjiga k in b.Knjige) {
                    b1.Knjige.Add(k);
                }
             }
            lb_knjige.ItemsSource = b1.Knjige;
       



           Biblioteka b2 = new Biblioteka();       //pregled svih korisnika
            foreach (Biblioteka b in biblioteke)
            {
                foreach (Korisnik k in b.Korisnici_knjiga)
                {
                    b2.Korisnici_knjiga.Add(k);     
                }
            }
            lb_korisnici.ItemsSource = b2.Korisnici_knjiga;
            



            lb_pretraga_korisnik.ItemsSource = biblioteke;
            lb_pretraga_knjiga.ItemsSource = biblioteke;
            Selekcija1.ItemsSource = biblioteke;
            Selekcija2.ItemsSource = biblioteke;


        }





    

  

        public void ucitaj(string file)
        {
            biblioteke.Clear();

            string readText = File.ReadAllText(file);

            string[] biblioteka_1_2 = readText.Split("#"); // do # je prva biblioteka kompletna 

            for (int i = 0; i < biblioteka_1_2.Length; i++)
            {

                string[] linije;
                string biblioteka; 
                string knjige;
                string clanovi;

                string[] deloviBiblioteke;
               
                string[] knjige_jpj; // knjige jedna po jedna 
                string[] delovi_knjige; // pomocni niz za svaki deo knjige 



                string[] delovi_clana; // svaki clan splitovan po , 
                string[] clanovi_jpj; // clanovi splitovani po | 

                linije = biblioteka_1_2[i].Split("\n"); // -> do prvog \n (info biblioteka) ,drugi \n(info knjige),treci \n(info clanovi)

                biblioteka = linije[0];  // bibl je ceo red info o bibl 

                knjige = linije[1]; // info o knjigama 

                clanovi = linije[2]; // info o clanovima 

                deloviBiblioteke = biblioteka.Split(","); // niz od 4 elementa 


           
                Biblioteka b = new Biblioteka(deloviBiblioteke[0], deloviBiblioteke[1], Int32.Parse(deloviBiblioteke[2]), deloviBiblioteke[3]);

                knjige_jpj= knjige.Split("|"); // svaka knjiga je odvojena sa | --> imam niz knjiga 

                for (int j = 0; j < knjige_jpj.Length; j++)
                {
                    delovi_knjige = knjige_jpj[j].Split(",");

                    Knjiga knjiga = new Knjiga(delovi_knjige[0], delovi_knjige[1], Int32.Parse(delovi_knjige[2]));

                    b.Knjige.Add(knjiga);             
                 }

                clanovi_jpj = clanovi.Split("|");

                for (int k = 0; k < clanovi_jpj.Length; k++)
                {
                    delovi_clana = clanovi_jpj[k].Split(",");
                    Korisnik korisnik = new Korisnik(delovi_clana[0], delovi_clana[1], delovi_clana[2], delovi_clana[3],delovi_clana[4]);
                    b.Korisnici_knjiga.Add(korisnik);
                }

                biblioteke.Add(b);
                
            }
        }



        private void lb_biblioteke_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
          
            slika_biblioteka.Source = new BitmapImage(new Uri(biblioteke[lb_biblioteke.SelectedIndex].Logo, UriKind.Relative));
            avatar_korisnik.Visibility = Visibility.Hidden;


            this.DataContext = (lb_biblioteke.SelectedItem as Biblioteka);   

            indeks  = lb_biblioteke.SelectedIndex; 
            lb_knjige.ItemsSource = biblioteke[indeks].Knjige;
            lb_korisnici.ItemsSource = biblioteke[indeks].Korisnici_knjiga;
           

        }

        private void lb_knjige_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.DataContext = (lb_knjige.SelectedItem as Knjiga);
            
                if (lb_biblioteke.SelectedItem==null)
                {
                    // MessageBox.Show("Biblioteka nije odabrana");

                }
                else
                {
                    
                    tb_naziv_biblioteke.Text=biblioteke[lb_biblioteke.SelectedIndex].Naziv;
                    tb_adresa_biblioteke.Text=biblioteke[lb_biblioteke.SelectedIndex].Adresa;
                    tb_godina_osnivanja_biblioteke.Text=biblioteke[lb_biblioteke.SelectedIndex].Godina_osnivanja.ToString();


                }
            


        }

        private void lb_korisnici_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
            this.DataContext = (lb_korisnici.SelectedItem as Korisnik);

            if (lb_korisnici.SelectedItem!=null)
            {
                if (lb_biblioteke.SelectedItem==null)
                {
                   // MessageBox.Show("Biblioteka nije odabrana");

                }
                else 
                {
                    avatar_korisnik.Visibility = Visibility.Visible;
                    avatar_korisnik.Source = new BitmapImage(new Uri(biblioteke[lb_biblioteke.SelectedIndex].Korisnici_knjiga[lb_korisnici.SelectedIndex].Slika_korisnika, UriKind.Relative));
                    tb_naziv_biblioteke.Text=biblioteke[lb_biblioteke.SelectedIndex].Naziv;
                    tb_adresa_biblioteke.Text=biblioteke[lb_biblioteke.SelectedIndex].Adresa;
                    tb_godina_osnivanja_biblioteke.Text=biblioteke[lb_biblioteke.SelectedIndex].Godina_osnivanja.ToString();
                    


                }
            }
        }


        private void lb_pretraga_korisnik_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.DataContext = (lb_biblioteke.SelectedItem as Biblioteka);


        }

        private void lb_pretraga_knjiga_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.DataContext = (lb_biblioteke.SelectedItem as Biblioteka);

        }

        private void dugme_pretraga_korisnik_Click(object sender, RoutedEventArgs e)
        {
            string selektovaniJMBG = tb_jmbg_pretagra.Text;
            if (selektovaniJMBG != "" && lb_pretraga_korisnik.SelectedItem!=null)
            {
             
                foreach (Korisnik k in biblioteke[lb_pretraga_korisnik.SelectedIndex].Korisnici_knjiga)
                {
                    if (k.Jmbg == selektovaniJMBG)
                    {
                        MessageBox.Show("Pronadjen je korisnik!");
                        tb_nadjeno_ime.Text = k.Ime;
                        tb_nadjeno_prezime.Text = k.Prezime;
                        tb_nadjeni_jmbg.Text = k.Jmbg;
                        tb_datum_uclanjenja_nadjeni.Text = k.Datum_uclanjenja;
                        slika_nadjeni_korisnik.Visibility = Visibility.Visible;
                        slika_nadjeni_korisnik.Source = new BitmapImage(new Uri(k.Slika_korisnika, UriKind.Relative));
                        
                        break;
                    }
                    else {
                        MessageBox.Show("Biblioteka nema takvog korisnika.");
                        tb_nadjeno_ime.Text = "";
                        tb_nadjeno_prezime.Text = "";
                        tb_nadjeni_jmbg.Text = "";
                        tb_datum_uclanjenja_nadjeni.Text = "";
                        slika_nadjeni_korisnik.Visibility = Visibility.Collapsed;
                        break;
                    }
                }
            }
            else
            {
                MessageBox.Show("Greska pri unosu!");
                tb_nadjeno_ime.Text = "/";
                tb_nadjeno_prezime.Text = "/";
                tb_nadjeni_jmbg.Text = "/";
                tb_datum_uclanjenja_nadjeni.Text = "/";
                slika_nadjeni_korisnik.Visibility = Visibility.Collapsed;       //ovo
            }

        }



        private void dugme_pretraga_knjiga_Click(object sender, RoutedEventArgs e)
        {
            string selektovanaKnj = tb_ime_knj.Text;
            if (selektovanaKnj != "" && lb_pretraga_knjiga.SelectedItem!=null)
            {
                
                foreach (Knjiga k in biblioteke[lb_pretraga_knjiga.SelectedIndex].Knjige)
                {
                    if (k.Naziv_kn == selektovanaKnj)
                    {
                        MessageBox.Show("Pronadjena je knjiga!");
                        tb_nk.Text = k.Naziv_kn;
                        tb_a.Text = k.Autor;
                        tb_gi.Text = k.Godina_izdanja.ToString();

                        break;
                    }
                    else
                    {
                        MessageBox.Show("Nema takve knjige.");
                        tb_nk.Text = "";
                        tb_a.Text = "";
                        tb_gi.Text = "";
                        break;
                    }
                }
            }
            else
            {
                MessageBox.Show("Greska pri unosu!");
                tb_nk.Text = "/";
                tb_a.Text = "/";
                tb_gi.Text = "/";
            }
        }

        private void Dodaj_Click(object sender, RoutedEventArgs e)
        {
            bool tf = true;
            string putanja = "../../../avatar_korisnici/avatar6.jpg";
            if (Selekcija1.SelectedItem != null)
            {
                Korisnik k = provjera1(txtIme1.Text, txtPrezime1.Text, txtJmbg1.Text, txtDatum1.Text, putanja);
                if (DodajKorisnika(k))
                {
                    tf = true;
                }
                else
                {
                    tf = false;
                }

                if (tf)
                {
                    MessageBox.Show("Uspjesno dodavanje korisnika!");
                }
                else
                {
                    MessageBox.Show("Neuspjesno dodavanje kosrisnika!", "Greska!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Niste izabrali biblioteku!", "Greska!", MessageBoxButton.OK, MessageBoxImage.Error);
                tf = false;
            }

            txtIme1.Text = "";
            txtPrezime1.Text = "";
            txtJmbg1.Text = "";
            txtDatum1.Text = "";
        }

        private bool DodajKorisnika(Korisnik k)
        {
            int id = Selekcija1.SelectedIndex;
            if (k == null)
            {
                return false;
            }
            else
            {
          
                biblioteke[id].Korisnici_knjiga.Add(k);
                return true;
            }
        }

        private void Izmijeni1_Click(object sender, RoutedEventArgs e)
        {

            Korisnik k = izmjene1.SelectedItem as Korisnik;
            int id = Selekcija1.SelectedIndex;
            if (k != null)
            {
                Korisnik noviKorisnik = provjera1(txtIme1.Text, txtPrezime1.Text, txtJmbg1.Text, txtDatum1.Text, txtSlika1.Text);
               
                if (noviKorisnik == null)
                {
                    MessageBox.Show("Neispravan unos korisnika!", "Greska!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    int index = izmjene1.SelectedIndex;
                    biblioteke[id].Korisnici_knjiga.RemoveAt(index);
                    biblioteke[id].Korisnici_knjiga.Insert(index, noviKorisnik);
                    MessageBox.Show("Izmijenili ste korisnika!", "Uspjesna operacija!", MessageBoxButton.OK, MessageBoxImage.Information);
                    Export("../../../Podaci_izmenjeni.txt", String.Join("#", biblioteke));
                }
            }
            else
            {
                MessageBox.Show("Selektujte korisnika!", "Greska!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private Korisnik provjera1(string ime, string prezime, string jmbg, string datum, string slika)
        {

            if (ime == "" || prezime == "" || jmbg == "" || datum == "")
            {
                return null;
            }
            if (jmbg.Length < 13)
            {
                return null;
            }
            else
            {
                for (int i = 0; i < jmbg.Length; i++)
                {
                    if (jmbg[i] < '0' || jmbg[i] > '9')
                    {
                        return null;
                    }
                }
            }

            if (datum.Length == 11)     //dd.mm.gggg.
            {
                if (datum[2] == '.' && datum[5] == '.' && datum[10] == '.')
                {
                    string[] niz = datum.Split('.');
                    if (!int.TryParse(niz[0], out int dan) || !int.TryParse(niz[1], out int mjesec) || !int.TryParse(niz[2], out int godina))
                    {
                        return null;
                    }
                    else
                    {
                        if ((dan < 1 || dan > 31) || (mjesec < 1 || mjesec > 12) || (godina < 1900 || godina>2022))
                        {
                            return null;
                        }
                    }
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }

            return new Korisnik(ime, prezime, jmbg, datum, slika);
        }






        private void Obrisi1_Click(object sender, RoutedEventArgs e)
        {
            Korisnik k = izmjene1.SelectedItem as Korisnik;
            int id = Selekcija1.SelectedIndex;
            if (k != null)
            {
                if (System.Windows.MessageBox.Show("Da li zelite da obrisete korisnika?", "Potvrda o brisanju korisnika", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                 
                      biblioteke[id].Korisnici_knjiga.RemoveAt(izmjene1.SelectedIndex);
                      MessageBox.Show("Uspjesno brisanje!", "Uspjesna validacija!", MessageBoxButton.OK, MessageBoxImage.Information);
                      Export("../../../Podaci_izmenjeni.txt", String.Join("#", biblioteke));

                }
            }
            else
            {
                MessageBox.Show("Korisnik nije selektovan!", "Greska!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void izmjene1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.DataContext = (izmjene1.SelectedItem as Korisnik);
            Izmijeni1.IsEnabled = true;
            Obrisi1.IsEnabled = true;
        }



        private void Izmijeni2_Click(object sender, RoutedEventArgs e)
        {
            Knjiga r = izmjene2.SelectedItem as Knjiga;
            int id = Selekcija2.SelectedIndex;
            if (r != null && txtIzdato2.Text!="")
            {
                Knjiga novaKnjiga = provjera2(txtNaziv2.Text, txtAutor2.Text, Int32.Parse(txtIzdato2.Text));

                if (novaKnjiga == null)
                {
                    MessageBox.Show("Neispravan unos korisnika!", "Greska!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    int index = Selekcija2.SelectedIndex;
                    biblioteke[id].Knjige.RemoveAt(index);
                    biblioteke[id].Knjige.Insert(index, novaKnjiga);
                    MessageBox.Show("Izmijenili ste knjigu!", "Uspjesna operacija!", MessageBoxButton.OK, MessageBoxImage.Information);
                    Export("../../../Podaci_izmenjeni.txt", String.Join("#", biblioteke));
                }
            }
            else
            {
                MessageBox.Show("Selektujte knjigu!", "Greska!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private Knjiga provjera2(string naziv, string autor, int izdato)
        {
            if (naziv == "" || autor == "" || izdato < 1000 || izdato > 2022)
            {
                return null;
            }

            return new Knjiga(naziv, autor, izdato);
        }



        private void Obrisi2_Click(object sender, RoutedEventArgs e)
        {
            Knjiga k = izmjene2.SelectedItem as Knjiga;
            int id = Selekcija2.SelectedIndex;
            if (k != null)
            {
                if (System.Windows.MessageBox.Show("Da li zelite da obrisete knjigu?", "Potvrda o brisanju knjige", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {

                    
                      biblioteke[id].Knjige.RemoveAt(izmjene2.SelectedIndex);
                      MessageBox.Show("Uspjesno brisanje!", "Uspjesna validacija!", MessageBoxButton.OK, MessageBoxImage.Information);
                      Export("../../../Podaci_izmenjeni.txt", String.Join("#", biblioteke));

                }
            }
            else
            {
                MessageBox.Show("Knjiga nije selektovana!", "Greska!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Dodaj2_Click(object sender, RoutedEventArgs e)
        {
                bool tf = true;     //radi ispisa greske
                if (Selekcija2.SelectedItem != null)
                {
                    Knjiga k = provjera2(txtNaziv2.Text, txtAutor2.Text, Int32.Parse(txtIzdato2.Text));
                    if (DodajKnjigu(k))
                    {
                        tf = true;
                    }
                    else
                    {
                        tf = false;
                    }

                    if (tf)
                    {
                        MessageBox.Show("Uspijesno dodavanje knjige!");
                        Export("../../../Podaci_izmenjeni.txt", String.Join("#", biblioteke));
                    }
                    else
                    {
                        MessageBox.Show("Neuspijesno dodavanje knjige!", "Greska!", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Niste izabrali biblioteku!", "Greska!", MessageBoxButton.OK, MessageBoxImage.Error);
                    tf = false;
                }

                txtNaziv2.Text = "";
                txtAutor2.Text = "";
                txtIzdato2.Text = "";
        }

        private bool DodajKnjigu(Knjiga k)
        {
            int id = Selekcija2.SelectedIndex;
            if (k == null)
            {
                return false;
            }
            else
            {
               
                biblioteke[id].Knjige.Add(k);
                return true;
            }
        }

        private void izmjene2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.DataContext = (izmjene2.SelectedItem as Knjiga);
            Izmijeni2.IsEnabled = true;
            Obrisi2.IsEnabled = true;


        }

        private void Selekcija1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
          

            int id = Selekcija1.SelectedIndex;
            if (id != -1)
            {
                izmjene1.ItemsSource = biblioteke[id].Korisnici_knjiga;
            }
           
        }

        private void Selekcija2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int id = Selekcija2.SelectedIndex;
            if (id != -1)
            {
                izmjene2.ItemsSource = biblioteke[id].Knjige;
            }

        }

       
        public void Export(string file, string txt)
        {
            StreamWriter sw = null;
            try
            {
                sw = new StreamWriter(file);
                sw.Write(txt);

            }
            catch (Exception e)
            {
                MessageBox.Show(e.StackTrace);
                MessageBox.Show("ERROR");
            }
            finally
            {
                try { sw.Close(); } catch (Exception e) { }

            }
        }


    

        
        
    }
}
