// ============================================================
// Chatbot.cs - Main Chatbot Logic
// This class handles everything the chatbot does:
// - Displaying the ASCII art logo and console UI
// - The typing animation effect
// - Processing user input and returning responses
// - Running the main conversation loop
// ============================================================

using System;
using System.Threading;

namespace CyberBot
{
    public class Chatbot
    {
        // Stores the User object so we can personalise responses with their name
        private User _user;

        // Constructor - receives the User object from Program.cs
        public Chatbot(User user)
        {
            _user = user;
        }

        // ─── UI Helpers ────────────────────────────────────────────────

        // Prints a decorative border at the top of the console
        public static void PrintBorder()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("╔══════════════════════════════════════════════════════════╗");
            Console.WriteLine("║           CYBERSECURITY AWARENESS CHATBOT                ║");
            Console.WriteLine("╚══════════════════════════════════════════════════════════╝");
            Console.ResetColor();
        }

        // Prints the ASCII art logo using a verbatim string (@"...")
        // The @ symbol allows multi-line strings with backslashes
        public static void PrintAsciiArt()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(@"
   ____      _               ____        _   
  / ___|   _| |__   ___ _ _| __ )  ___ | |_ 
 | |  | | | | '_ \ / _ \ '__|  _ \ / _ \| __|
 | |__| |_| | |_) |  __/ |  | |_) | (_) | |_ 
  \____\__, |_.__/ \___|_|  |____/ \___/ \__|
       |___/                                  
");
            Console.ResetColor();
        }

        // Prints each character one at a time with a small delay
        // This creates a "typewriter" animation effect
        // delay = milliseconds between each character (default 30ms)
        public static void TypeEffect(string message, ConsoleColor color = ConsoleColor.White, int delay = 30)
        {
            Console.ForegroundColor = color;
            foreach (char c in message)
            {
                Console.Write(c);       // Print one character
                Thread.Sleep(delay);    // Wait before printing the next
            }
            Console.WriteLine(); // Move to next line when done
            Console.ResetColor();
        }

        // Prints a horizontal divider line to separate sections visually
        public static void PrintDivider()
        {
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("──────────────────────────────────────────────────────────");
            Console.ResetColor();
        }

        // ─── Response Logic ────────────────────────────────────────────

        // Takes the user's input and returns the appropriate response
        // Uses if/else if to match keywords in the input
        public string GetResponse(string input)
        {
            // Handle empty or whitespace-only input
            if (string.IsNullOrWhiteSpace(input))
                return "⚠️  Please enter something. I'm here to help!";

            // Convert to lowercase so matching is not case-sensitive
            // e.g. "PASSWORD", "Password", "password" all match
            string lower = input.ToLower().Trim();

            // ── General conversation responses ──

            if (lower == "how are you")
                return "I'm just code, but I'm running perfectly and ready to keep you safe! 😄";

            if (lower.Contains("your name") || lower.Contains("who are you"))
                return "I'm CyberBot – your personal Cybersecurity Awareness Assistant!";

            if (lower.Contains("purpose") || lower.Contains("what do you do"))
                return "My purpose is to educate you about cybersecurity threats and how to stay safe online.";

            if (lower == "hello" || lower == "hi" || lower == "hey")
                return $"Hey {_user.Name}! 👋 Ask me anything about cybersecurity!";

            // ── Cybersecurity topic responses ──
            // .Contains() checks if the keyword appears anywhere in the input

            if (lower.Contains("password"))
                return "🔐 Use strong passwords with uppercase, lowercase, numbers & symbols (e.g. P@ssw0rd!). " +
                       "Never reuse passwords and consider a password manager like Bitwarden.";

            if (lower.Contains("phishing"))
                return "🎣 Phishing is when attackers trick you via fake emails or websites. " +
                       "Never click suspicious links, always verify the sender's email address.";

            if (lower.Contains("malware") || lower.Contains("virus"))
                return "🦠 Malware is malicious software that can damage your device. " +
                       "Keep your antivirus updated, avoid downloading unknown files.";

            if (lower.Contains("vpn"))
                return "🛡️ A VPN (Virtual Private Network) encrypts your internet traffic. " +
                       "Use one on public Wi-Fi to protect your data from eavesdroppers.";

            if (lower.Contains("firewall"))
                return "🔥 A firewall monitors incoming and outgoing network traffic. " +
                       "Always keep your system firewall enabled to block unauthorized access.";

            // Matches "two factor", "2fa", or "mfa"
            if (lower.Contains("two factor") || lower.Contains("2fa") || lower.Contains("mfa"))
                return "📱 Two-Factor Authentication (2FA) adds an extra layer of security. " +
                       "Even if your password is stolen, attackers can't log in without the second factor.";

            if (lower.Contains("ransomware"))
                return "💰 Ransomware encrypts your files and demands payment. " +
                       "Back up your data regularly and never pay the ransom – it doesn't guarantee recovery.";

            if (lower.Contains("social engineering"))
                return "🧠 Social engineering manipulates people into revealing confidential info. " +
                       "Always verify identities before sharing any sensitive data.";

            if (lower.Contains("safe browsing") || lower.Contains("browse safely"))
                return "🌐 Safe browsing tips: look for HTTPS, avoid suspicious pop-ups, " +
                       "use an ad-blocker, and never enter personal info on unverified sites.";

            if (lower.Contains("update") || lower.Contains("patch"))
                return "🔄 Always keep your software and OS updated. " +
                       "Patches fix security vulnerabilities that attackers exploit.";

            // Show available topics if the user asks for help
            if (lower.Contains("help") || lower == "?")
                return "💡 You can ask me about: passwords, phishing, malware, VPN, firewall, " +
                       "2FA, ransomware, social engineering, safe browsing, or software updates.";

            // Return special EXIT signal to stop the chat loop
            if (lower == "exit" || lower == "quit" || lower == "bye")
                return "EXIT";

            // Default response for anything not recognised
            return $"🤔 I didn't quite understand that, {_user.Name}. " +
                   "Try asking about: passwords, phishing, malware, VPN, 2FA, ransomware, or type 'help'.";
        }

        // ─── Main Chat Loop ────────────────────────────────────────────

        // Starts the chatbot - displays the UI and loops until user exits
        public void Start()
        {
            // Display the header and ASCII logo
            PrintBorder();
            PrintAsciiArt();
            PrintDivider();

            // Show personalised welcome message with typing effect
            TypeEffect(_user.GetGreeting(), ConsoleColor.Green);
            TypeEffect("I'm here to help you stay safe online. Type 'help' for topics or 'exit' to quit.", ConsoleColor.White);
            PrintDivider();

            // Keep looping until the user types exit/quit/bye
            while (true)
            {
                // Show the input prompt with the user's name
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write($"\n[{_user.Name}] > ");
                Console.ResetColor();

                // Read what the user typed
                string input = Console.ReadLine();

                // Get the chatbot's response based on the input
                string response = GetResponse(input);

                // If the response is EXIT, break out of the loop and end the program
                if (response == "EXIT")
                {
                    PrintDivider();
                    TypeEffect($"Goodbye, {_user.Name}! Stay safe online! 🛡️", ConsoleColor.Cyan);
                    PrintDivider();
                    break;
                }

                // Otherwise, print the response with the typing effect
                PrintDivider();
                TypeEffect($"🤖 CyberBot: {response}", ConsoleColor.Cyan, 20);
                PrintDivider();
            }
        }
    }
}