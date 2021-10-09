using System;
using System.Collections.Generic;
using System.Text;

namespace k3na_voice
{
    public class SpeechRecognitionModule
    {
        public delegate void OnSpeechDelegate(string speech);

        public event OnSpeechDelegate OnSpeech;

        private static Audist Recorder { get; set; }

        private const string FolderId = "b1glia0cgh1tqpjp48f4";
        private const string OauthToken = "AQAAAAAO_IRoAATuwQCcneB1GEPYlK7mFP_wcJg";
        private static string _iamToken = "";

        public SpeechRecognitionModule()
        {
            _iamToken = SpeechKit.GetToken(OauthToken).Result.IamToken;
            Console.WriteLine("Token: " + _iamToken);

            Recorder = new Audist();
            Recorder.OnAudioFile += Recorder_OnAudioFile;
            Recorder.Start();
        }
        private void Recorder_OnAudioFile(string path)
        {
            var recognitionResult = SpeechKit.RecognizeTask(_iamToken, FolderId, path).Result;
            if (recognitionResult.Contains("result"))
            {
                var i = recognitionResult.IndexOf("\"",10, StringComparison.Ordinal)+1;
                recognitionResult = recognitionResult.Substring(i, recognitionResult.Length - i - 2);
            }
            
            OnSpeech?.Invoke(recognitionResult);
        }

        ~SpeechRecognitionModule()
        {
            Recorder.Stop();
        }
    }
}
