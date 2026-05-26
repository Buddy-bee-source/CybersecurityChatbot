// ============================================================
// MainWindow.xaml.cs - GUI Code
// Its only job is to react to UI events
// ============================================================

using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace CybersecurityChatBot
{
    public partial class MainWindow : Window
    {
        // The only field in this class
        private ChatBot _chatBot;

        //Constructor 
        public MainWindow()
        {
            InitializeComponent();

            // Initialise the ChatBot 
            _chatBot = new ChatBot();

            // Play voice greeting on startup
            PlayVoiceGreeting();

            // Load ASCII art into the header display
            LoadAsciiArt();

            // Show the bot's opening message in the chat
            string greeting = _chatBot.GetGreeting();
            AppendBotMessage(greeting);
        }


        // Plays the WAV greeting file using the AudioPlayer
        // The greeting.wav file must be in the bin/Debug/net8.0/ folder
        private void PlayVoiceGreeting()
        {
            try
            {
               // AudioPlayer audio = new AudioPlayer("greeting.wav");
                //audio.PlayGreeting();
            }
            catch
            {
                // If the file is missing, silently skip - don't crash the app
            }
        }

        // Sets the ASCII art text in the header TextBlock
        private void LoadAsciiArt()
        {
            AsciiArtDisplay.Text =
@"   ____      _               ____        _   
  / ___|   _| |__   ___ _ _| __ )  ___ | |_ 
 | |  | | | | '_ \ / _ \ '__|  _ \ / _ \| __|
 | |__| |_| | |_) |  __/ |  | |_) | (_) | |_ 
  \____\__, |_.__/ \___|_|  |____/ \___/ \__|
       |___/                                  ";
        }

        //Event Handlers

        // Fires when the Send button is clicked
        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            SendMessage();
        }

        // Fires on any key press in the input box
        // If the user presses Enter, it triggers SendMessage()
        private void UserInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                SendMessage();
        }

        //Core Send Logic

        // Reads the user's input, passes it to ChatBot, and displays result
        private void SendMessage()
        {
            string userText = UserInput.Text.Trim();

            // Ignore empty input
            if (string.IsNullOrWhiteSpace(userText))
                return;

            // Show the user's message in the chat display
            AppendUserMessage(userText);

            // Clear the input box ready for next message
            UserInput.Clear();

            // Get the bot's response via ProcessInput()
            string response = _chatBot.ProcessInput(userText);

            // Check if response is an exit signal
            if (response.StartsWith("EXIT:"))
            {
                string farewellMessage = response.Substring(5); // Strip "EXIT:" 
                AppendBotMessage(farewellMessage);
                SendButton.IsEnabled = false;
                UserInput.IsEnabled = false;
                return;
            }

            // Display the bot's response
            AppendBotMessage(response);
        }

        // Chat Display Helpers

        // Appends the user's message to the chat display in yellow
        private void AppendUserMessage(string message)
        {
            // Use a Run to colour the user's text differently
            var run = new System.Windows.Documents.Run($"\n🧑 You: {message}\n");

            // We use inline approach via text concatenation for TextBlock simplicity
            ChatDisplay.Inlines.Add(new System.Windows.Documents.Run($"\n🧑 You: {message}\n")
            {
                Foreground = new SolidColorBrush(Color.FromRgb(255, 215, 0)) // Gold
            });

            ScrollToBottom();
        }

        // Appends the bot's response to the chat display in cyan
        private void AppendBotMessage(string message)
        {
            ChatDisplay.Inlines.Add(new System.Windows.Documents.Run($"\n🤖 CyberBot: {message}\n")
            {
                Foreground = new SolidColorBrush(Color.FromRgb(0, 255, 255)) // Cyan
            });

            // Divider between messages
            ChatDisplay.Inlines.Add(new System.Windows.Documents.Run(
                "\n──────────────────────────────────────────────────────────\n")
            {
                Foreground = new SolidColorBrush(Color.FromRgb(0, 139, 139)) // Dark Cyan
            });

            ScrollToBottom();
        }

        // Scrolls the ScrollViewer to the bottom after every message
        private void ScrollToBottom()
        {
            ChatScrollViewer.ScrollToBottom();
        }
    }
}