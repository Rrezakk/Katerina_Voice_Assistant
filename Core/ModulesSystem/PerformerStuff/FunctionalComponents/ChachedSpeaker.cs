using System;
using System.Collections.Concurrent;
using System.IO;
using System.Media;
using System.Threading;
using System.Threading.Tasks;
using K3NA_Remastered_2.Yandex_API;

namespace K3NA_Remastered_2.ModulesSystem.PerformerStuff.FunctionalComponents
{
    internal class ChachedSpeaker//to decrease tts api requests
    {
        private static Thread soundPlayThread;
        private static readonly BlockingCollection<string> speakQueue = new();
        private static readonly CancellationTokenSource cancelSoundPlay;
        static ChachedSpeaker()
        {
            cancelSoundPlay = new CancellationTokenSource();
        }
        public static void Speak(string text)
        {
            var chacheDirectory = Environment.CurrentDirectory + Tts.AudioFilesChachePath;
            if (!Directory.Exists(chacheDirectory))
            {
                Directory.CreateDirectory(chacheDirectory);
            }


            var hash = Hashing.ComputeSha256Hash(text);
            var fileName = hash + ".wav";
            var filePath = chacheDirectory + fileName;


            if (!File.Exists(filePath))
            {
                Tts.SynthPhraseAndSaveAudioFile(text, filePath);
            }

            queueAndPlay(filePath);
        }
        private static void queueAndPlay(string waveFilePath)
        {
            speakQueue.Add(waveFilePath);
            StartSoundPlay();
        }
        private static void StartSoundPlay()
        {
            //Sound Player Loop Thread
            if (soundPlayThread is {IsAlive: true}) return;
            soundPlayThread = new Thread(SoundPlayerLoop)
            {
                Name = "SoundPlayerLoop",
                IsBackground = true
            };
            soundPlayThread.Start();
        }
        private static void SoundPlayerLoop()//Method that the outside thread will use outside the thread of this class
        {
            var sound = new SoundPlayer();
            foreach (var soundToPlay in speakQueue.GetConsumingEnumerable(cancelSoundPlay.Token))
            {
                sound.SoundLocation = soundToPlay;
                //Here the outside thread waits for the following play to end before continuing.
                sound.PlaySync();
            }
        }
        public static void ClearChache()
        {
            var chacheDirectory = Environment.CurrentDirectory + Tts.AudioFilesChachePath;
            if (Directory.Exists(chacheDirectory))
                Directory.Delete(chacheDirectory,true);
        }
    }
}
