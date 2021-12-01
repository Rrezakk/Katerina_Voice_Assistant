using System;
using K3NA_Remastered_2.Yandex_API;

namespace K3NA_Remastered_2.ModulesSystem.Modules.Concrete.SRM
{
    public class SpeechRecognizer
    {
        public delegate void OnSpeechDelegate(string speech);
        public event OnSpeechDelegate OnSpeech;
        private static Audist Recorder { get; set; }
        public SpeechRecognizer(){}
        public void Start()
        {
            Recorder = new Audist();
            Recorder.OnAudioFile += Recorder_OnAudioFile;
            Recorder.Start();
        }
        private void Recorder_OnAudioFile(string path)
        {
            var recognitionResult = STT.RecognizeTask(SAuth.AccessToken.IamToken, SAuth.FolderId, path).Result;
            Console.WriteLine($"Recorded: {path} \nResult: {recognitionResult}");
            if (recognitionResult.Contains("result"))
            {
                var i = recognitionResult.IndexOf("\"",10, StringComparison.Ordinal)+1;
                recognitionResult = recognitionResult.Substring(i, recognitionResult.Length - i - 2);
            }
            OnSpeech?.Invoke(recognitionResult);
        }
        ~SpeechRecognizer()
        {
            Recorder.Stop();
        }
    }
}
