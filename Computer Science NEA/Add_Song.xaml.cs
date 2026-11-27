using Microsoft.Win32;
using System.Windows;
namespace Computer_Science_NEA
{
    /// <summary>
    /// Interaction logic for Add_Song.xaml
    /// </summary>
    public partial class Add_Song : Window
    {
        public Add_Song()
        {
            InitializeComponent();
        }
        Create_And_Validate CV = new Create_And_Validate();
        DatabaseManager DB = new DatabaseManager();

        private void File_Load_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog Open_File = new OpenFileDialog();
            Open_File.Filter = "MP3 Files (*.MP3)|*.mp3;*.MP3";
            Open_File.Multiselect = false;
            Open_File.CheckFileExists = true;
            Open_File.InitialDirectory = (System.Environment.SpecialFolder.MyMusic).ToString();
            Open_File.Title = "Please select an mp3 file";
            if (Open_File.ShowDialog() == true)
            {
                Path_Box.Text = Open_File.FileName;
                
                var Metadata = TagLib.File.Create(Path_Box.Text);
                Name_Box.Text = Metadata.Tag.Title ?? "Not Found";
                Artist_Box.Text = Metadata.Tag.FirstPerformer ?? "Not Found";
                Album_Box.Text = Metadata.Tag.Album ?? "Not Found";
                Track_Box.Text = (Metadata.Tag.Track).ToString() ?? "0";
                string[] Genres = Metadata.Tag.Genres ?? ["Not Found", "Not Found", "Not Found"];
                try
                {
                    Genre_Box_1.Text = Genres[0];
                }
                catch
                {
                    Genre_Box_1.Text = "Not Found";
                }
                try
                {
                    Genre_Box_2.Text = Genres[1];
                }
                catch
                {
                    Genre_Box_2.Text = "Not Found";
                }
                try
                {
                    Genre_Box_3.Text = Genres[2];
                }
                catch
                {
                    Genre_Box_3.Text = "Not Found";
                }
                
                

            }
        }

        private void Add_Song_Button_Click(object sender, RoutedEventArgs e)
        {
            string[] Genres = [Genre_Box_1.Text, Genre_Box_2.Text,Genre_Box_3.Text];
            var New_Song = CV.Create_Song(Name_Box.Text,Genres,Album_Box.Text,Artist_Box.Text,Path_Box.Text);
            DB.InitDB();
            DB.SaveNewSong(New_Song,int.Parse(Track_Box.Text));
            
        }

        private void TextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {

        }
    }
}
