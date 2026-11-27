using Microsoft.Data.Sqlite;
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


namespace Computer_Science_NEA
{
    public class DatabaseManager
    {

        static string Path_To_DB = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        static string True_DB_Path = Path.Combine(Path_To_DB, "Music Player");
        static string Full_DB_Path = Path.Combine(True_DB_Path, "library.db");
        
        public void InitDB()
        {
            Directory.CreateDirectory(True_DB_Path);
            
            var connection = new SqliteConnection($"Data Source={Full_DB_Path}");
            connection.Open();
            var createtable = connection.CreateCommand();
            createtable.CommandText = @"
            CREATE TABLE IF NOT EXISTS Artists
            (
                Artist string PRIMARY KEY
                
            )";
            createtable.ExecuteNonQuery();
            createtable.CommandText = @"
            CREATE TABLE IF NOT EXISTS Songs
            (
                SongID INTEGER PRIMARY KEY AUTOINCREMENT,
                Name string,
                Artist string,
                Path string,
                Era INTEGER,
                ThumbnailPath string,
                FOREIGN KEY (Artist)
                REFERENCES Artists(Artist)
            )";
            createtable.ExecuteNonQuery();
            createtable.CommandText = @"
            CREATE TABLE IF NOT EXISTS Albums
            (
                Album string PRIMARY KEY,
                Artist string,
                FOREIGN KEY (Artist)
                REFERENCES Artists(Artist)
            )";
            createtable.ExecuteNonQuery();
            createtable.CommandText = @"
            CREATE TABLE IF NOT EXISTS AlbumSongs
            (
                Album string,
                SongID INTEGER,
                Track INTEGER,
                PRIMARY KEY (Album, SongID),
                FOREIGN KEY (Album)
                REFERENCES Albums(Album),
                FOREIGN KEY (SongID)
                REFERENCES Songs(SongID)
                
            )";
            createtable.ExecuteNonQuery();
            connection.Close();
        }
        public void SaveNewSong(Song song,int tracknum) 
        {

            var connection = new SqliteConnection($"Data Source={Full_DB_Path}");
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = $@"
            INSERT OR IGNORE INTO Albums(Album)
            SELECT($album)
            
            ";
            command.Parameters.AddWithValue("$album", song.Album);

            command.ExecuteNonQuery();
            
            command.CommandText = $@"
            INSERT OR IGNORE INTO Artists(Artist)
            SELECT($artist)
            
            ";
            command.Parameters.AddWithValue("$artist", song.Primary_Artist);

            command.ExecuteNonQuery();
            
            command.CommandText = @"
                   INSERT INTO Songs (Name, Path, Artist)
                   VALUES ($name, $path, $artist)
                   RETURNING SongID
                                    ";
            command.Parameters.AddWithValue("$name", song.Name);
            command.Parameters.AddWithValue("$path", song.Path);
            
            int SongID = (int)(long)command.ExecuteScalar();
            command.CommandText = @"
                INSERT INTO AlbumSongs (Album, SongID, Track)
                VALUES ($album,$SongID,$track)";
            command.Parameters.AddWithValue("$SongID", SongID);
            command.Parameters.AddWithValue("$track", tracknum);
            command.ExecuteNonQuery();
            connection.Close();
            
        }
        public List<Song> LoadSongs()
        {
            var connection = new SqliteConnection($"Data Source={Full_DB_Path}");
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = @"
        SELECT SongID, Name, Artist, Path, Era, ThumbnailPath
        FROM Songs
        ORDER BY SongID";

            var reader = command.ExecuteReader();
            List<Song> DataList = new List<Song>();
            while(reader.Read())
            {
                Song song = new Song
                {
                    id = reader.GetInt32(0),
                    Name = reader.IsDBNull(1) ? "" : reader.GetString(1),
                    Primary_Artist = reader.IsDBNull(2) ? "" : reader.GetString(2),
                    Path = reader.IsDBNull(3) ? "" : reader.GetString(3),
                    Era = reader.IsDBNull(4) ? 0 : Convert.ToInt32(reader.GetValue(4)),
                    Thumbnail_Path = reader.IsDBNull(5) ? "" : reader.GetString(5)
                };
                DataList.Add(song);
            }
            return DataList;
        }
            
          
        
            
    }
    public class Create_And_Validate
    {
        
        
        public Song Create_Song(string Name, string[] Genres, string Album = "*EMPTY*", string Primary_Artist = "*EMPTY*", string Path = "*EMPTY*", string Thumbnail_Path = "*EMPTY*",int Era = -99)
        {
            
            Song Temp_Song = new Song();

            Temp_Song.Name = Name;

            try
            {
                Temp_Song.Album = Album;
            }
            catch
            {

            }
            
            try
            {
                Temp_Song.Genres[0] = Genres[0];
            }
            catch
            {
                Temp_Song.Genres[0] = "*EMPTY*";
            }
            try
            {
                Temp_Song.Genres[1] = Genres[1];
            }
            catch
            {
                Temp_Song.Genres[1] = "*EMPTY*";
            }
            try
            {
                Temp_Song.Genres[2] = Genres[2];
            }
            catch
            {
                Temp_Song.Genres[2] = "*EMPTY*";
            }
            
            try
            {
                Temp_Song.Primary_Artist = Primary_Artist;
            }
            catch
            {

            }
           
            
            try
            {
                Temp_Song.Path = Path;
            }
            catch
            {

            }
            try
            {
                Temp_Song.Thumbnail_Path = Thumbnail_Path;

            }
            catch
            {

            }
            try
            {
                Temp_Song.Era = Era;
            }
            catch
            {

            }
            return Temp_Song;
        }
        public string[] Validate_Song(Song Active_Song)
        {
            byte[] Path_As_Bytes = [];
            bool Valid_Path;
            try //If the path does not exist the code will throw an exception
            {
                Path_As_Bytes = File.ReadAllBytes(Active_Song.Path); //converts mp3 to bytes
                Valid_Path = true;
            }
            catch (Exception) //ensures that the further comparisons wont be made if readallbytes threw an exception
            {
                Path_As_Bytes = []; //frees up memory
                Valid_Path = false;
            }

            
            if ((Valid_Path == true) && ((Path_As_Bytes[0] == 0x49) && (Path_As_Bytes[1] == 0x44) && (Path_As_Bytes[2] == 0x33))) //enable/s buttons when valid data is present
            {
                Path_As_Bytes = []; //frees up memory
                
            }

            else //if invalid data is present disable buttons
            {
                return ["fail","Invalid File"];
            }
            return ["temp", "temp"];
        }
    }
}
