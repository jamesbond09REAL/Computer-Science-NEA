using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using System.IO;
using System.Net.Security;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Computer_Science_NEA
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //Define variables within scope
        WasapiPlayer Output_Device; 
        string Song_Path;
        AudioFileReader Audio_File;
        DatabaseManager DB = new DatabaseManager();
        Playback Main_Playback = new Playback();
        List<Song> Song_List = new List<Song>();
        Song Selected_Song = new Song();
        
        public MainWindow()
        {
            InitializeComponent();
           
        }

        private void Pause_Button_Click(object sender, RoutedEventArgs e)//Pauses a potential Output_Device
        {
            Main_Playback.Pause_Playback();
        }

       

        

        private async void Play_Button_Click(object sender, RoutedEventArgs e)//Manages playback within the program async to prevent freezing the rest of the application
        {
            List<string> DetailList = Main_Playback.Get_Playback_Details(Selected_Song);
            Main_Playback.Play_Song(Selected_Song); 
            Name_Box.Text = DetailList[0];
            Artist_Box.Text = DetailList[1];
            
            
        }



        

        private void Stop_Button_Click(object sender, RoutedEventArgs e)//Calls Stop_Playback
        {
            Main_Playback.Stop_Playback();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e) //prevents closing while audio is playing thus preventing audio being held in the buffer incorrectly
        {
            if (Output_Device != null)
            {
                Main_Playback.Stop_Playback();
            }
        }

        private void Add_Song_Button_Click(object sender, RoutedEventArgs e)
        {
            Add_Song New_Song_Window = new Add_Song();
            New_Song_Window.Show();
        }

        private void Song_Combo_Box_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Song_Combo_Box.SelectedItem is Song song)
            {
                Selected_Song = song;
                Play_Button.IsEnabled = true;
                Pause_Button.IsEnabled = true;
                Stop_Button.IsEnabled = true;
                
            }
            else
            {
                Play_Button.IsEnabled = false;
                Pause_Button.IsEnabled = false;
                Stop_Button.IsEnabled = false;
            }


        }

        private void Test(object sender, RoutedEventArgs e)
        {
            
            Song_List = DB.LoadSongs();
            Song_Combo_Box.ItemsSource = Song_List;
        }
    }
}