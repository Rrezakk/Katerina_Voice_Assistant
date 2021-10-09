using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using NAudio.Wave;
using Newtonsoft.Json;
using static k3na_voice.SpeechKit;

namespace k3na_voice
{
    
    public static class Usage
    {
        private static void Main(string[] args)
        {
            var module = new SpeechRecognitionModule();
            module.OnSpeech += Module_OnSpeech;
            Console.ReadLine();
        }

        private static void Module_OnSpeech(string speech)
        {
            Console.WriteLine($"Speech: {speech}");
        }
    }
}
