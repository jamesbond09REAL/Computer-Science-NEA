using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Net.Security;

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
    public class Song
    {
        public int id { get; set; } 
       
        public string Name { get; set; } = "";
        public string Album { get; set; } = "";
        public int Era {  get; set; }
        public string[] Genres { get; private set; } = new string[3];
        public string Primary_Artist { get; set; } = "";
        public string Path { get; set; } = "";
        public string Thumbnail_Path { get; set; } = "";
        
        

    }
    public class Playback
    {
        WasapiPlayer Output_Device;
        AudioFileReader Audio_File;
        public void Stop_Playback() //Method to stop playback and clear variables for the Output_Device and Audio_File
        {


            Output_Device?.Stop();
            System.Threading.Thread.Sleep(150); //ensure playback fully stops
            Output_Device?.Dispose();
            Output_Device = null;
            Audio_File?.Dispose();
            Audio_File = null;
            

        }
        async public void Play_Song(Song Active_Song)
        {
            
            if (Output_Device == null) //asign Output_Device to a new WasapiPlayer if it doesnt already have one
            {
                Output_Device = new WasapiPlayerBuilder().Build();

            }
            if (Audio_File == null) //asign Audio_File to a new AudioFileReader if it doesnt already have one
            {
                Audio_File = new AudioFileReader(Active_Song.Path);
                Output_Device.Init(Audio_File); //asign the Audio_File to the Output_Device
            }
            if (Output_Device.PlaybackState == PlaybackState.Paused)//Prevent new loops from being created if music is being unpaused
            {
                Output_Device.Play();
                return;
            }
            else if (Output_Device.PlaybackState == PlaybackState.Playing)//Prevent new loops from being created if the program is already playing music
            {
                return;
            }
            Output_Device.Play();


            while ((Output_Device.PlaybackState == PlaybackState.Playing) || (Output_Device.PlaybackState == PlaybackState.Paused)) //loop while song is playing without breaking the loop during a pause
            {
                await Task.Delay(500); //await prevents program from freezing.
                if (Output_Device == null) //prevents exceptions with comparisons if Stop_Playback is called elsewhere
                {


                    return;
                }
            }
            Stop_Playback(); //call Stop_Playback after song is finished
        }
    }
}

