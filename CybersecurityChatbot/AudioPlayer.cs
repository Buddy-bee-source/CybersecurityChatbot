using System;
using System.Media;

public class AudioPlayer
{
    public static void PlayGreeting()
    {
        try
        {
            SoundPlayer player = new SoundPlayer(@"bin/Debug/net8.0/greetings.wav");
            player.PlaySync();
        }
        catch
        {
            Console.WriteLine("Audio could not be played");
        }
    }
}


