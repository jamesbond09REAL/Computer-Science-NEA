using System.IO;
using System.Net.Security;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using NAudio.Wave;
using NAudio.Wave.SampleProviders;

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
        
        
        
        public MainWindow()
        {
            InitializeComponent();
           
        }

        private void Pause_Button_Click(object sender, RoutedEventArgs e)//Pauses a potential Output_Device
        {
            Output_Device?.Pause();
        }

       

        

        private async void Play_Button_Click(object sender, RoutedEventArgs e)//Manages playback within the program async to prevent freezing the rest of the application
        {
            
            
            
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
            
        }

        private void Test(object sender, RoutedEventArgs e)
        {
            
            List<Song> SongsInDB = DB.LoadSongs();
            SongsInDB.ForEach(song =>
            {
                string Data = $"{song.id} {song.Name} {song.Primary_Artist}";
                Song_Combo_Box.Items.Add(Data);
            });
        }
    }
}