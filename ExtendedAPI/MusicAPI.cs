using Pinion;
using UnityEngine;

namespace Daybreak_Midnight.ExtendedAPI
{
    [APISource]
    public static class MusicAPI
    {
        private static AudioSource globalAudio = null;

        [APIMethod]
        public static void PlayCustomMusic(PinionContainer container, string filename)
        {
            if (!string.IsNullOrEmpty(filename) && NodeMapAPI.NodeMapActive() && Game.Controller.CustomSoundDatabase != null)
            {
                if (Game.Controller.CustomSoundDatabase.GetAudioClip(filename) == null)
                {
                    container.LogError("No audio file found named: '" + filename + "'. File must be in the Audio folder under the campaign's root directory and must be in WAV format.");
                }

                if (globalAudio == null) { globalAudio = GlobalAudio.Instance; }

                AudioClip audioClip = Game.Controller.CustomSoundDatabase.GetAudioClip(filename);
                BackgroundMusic bgm = GameObject.Find("BackgroundMusic").GetComponent<BackgroundMusic>();

                bgm.Stop();
                globalAudio.clip = audioClip;
                globalAudio.loop = true;
                globalAudio.volume = SystemData.Instance.musicVolume / 10f;
                globalAudio.Play();
            }
        }

        [APIMethod]
        public static void ResetBGM()
        {
            if(globalAudio == null) { return; }

            if(globalAudio.isPlaying) { globalAudio.Stop(false); }

            BackgroundMusic bgm = GameObject.Find("BackgroundMusic").GetComponent<BackgroundMusic>();
            bgm.SetMusic(BackgroundMusic.Music.TENSION);
        }
    }
}
