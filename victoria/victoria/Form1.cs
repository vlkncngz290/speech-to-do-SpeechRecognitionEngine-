using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Speech.Recognition;
using System.Speech.Synthesis;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace victoria
{
    public partial class Form1 : Form
    {

        SpeechSynthesizer s = new SpeechSynthesizer();
        Boolean wake = true;
        Choices list = new Choices();

        [DllImport("user32.dll")]
        public static extern int SetForegroundWindow(IntPtr hWnd);
        

        public Form1()
        {

            SpeechRecognitionEngine rec = new SpeechRecognitionEngine();
            
          
            list.Add(new String[] { "not al",
                "uyu",
                "dosya aç",
                "arama yap",
                "open facebook",
                "not tamamlandı",
                "saat kaç",
                "internet",
                "how are you",
                "kapan",
                "wake up",
                "merhaba",
                "nasılsın" });
            Grammar gr = new Grammar(new GrammarBuilder(list));


            try
            {
                rec.RequestRecognizerUpdate();
                rec.LoadGrammar(gr);
                rec.SpeechRecognized += rec_speechRecognized;
                rec.SetInputToDefaultAudioDevice();
                rec.RecognizeAsync(RecognizeMode.Multiple);
            }
            catch { return; }

            s.SelectVoiceByHints(VoiceGender.Female);
            s.Speak("Hello, my name is Vic!");

            InitializeComponent();
        }

        public void say(String h)
        {
            s.Speak(h);
        }



        private void rec_speechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            String r = e.Result.Text;
            
            if (wake)
            {

                int sayi = r.Split(' ').Length;
                say(sayi.ToString());
                
                if (r.Contains("dosya") && false)
                {
                    label1.Text = "Dosya açılıyor..";
                    FileStream fs = File.Create("c:\\\\victoriaDosyasi.txt");
                    byte[] text = new UTF8Encoding(true).GetBytes("Bu dosya Victoria tarafından oluşturulmuştur.\n Tüm yetkiler Volkan Cengiz\'indir.");
                    fs.Write(text, 0, text.Length);
                    fs.Close();
                    label1.Text = "Dosya oluşturuldu.";

                }
                if (r.Contains("saat"))
                {
                    say(DateTime.Now.ToString());
                }
                if (r.Contains("kapan"))
                {
                    this.Close();
                }
                if (r.Contains("facebook"))
                {
                    Process.Start("http://facebook.com");
                }
                if (r.Contains("not al"))
                {
                    Process.Start("https://speechnotes.co/");
                    System.Threading.Thread.Sleep(2000);

                    Process p = Process.GetProcessesByName("chrome")[0];
                    IntPtr pointer = p.Handle;
                    SetForegroundWindow(pointer);
                    SendKeys.Send("{F12}");
                    System.Threading.Thread.Sleep(1000);
                    SendKeys.Send("document.getElementById{(}\"start_button\"{)}.click{(}{)};");
                    SendKeys.Send("{ENTER}");
                    wake = false;
                   
                }

            }
            if (r.Contains("uyu")) { wake = false; say("goodbye!"); }
            if (r.Contains("wake up")) { wake = true; say("I am ready"); }
            if (r == "not tamamlandı") {
                SendKeys.Send("document.getElementById{(}\"copyButton\"{)}.click{(}{)};");
                SendKeys.Send("{ENTER}");
                wake = true;
                label1.Text = Clipboard.GetText();
            } 
            }
       
        

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
