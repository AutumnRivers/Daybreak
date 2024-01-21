using System;
using System.Collections.Generic;
using System.Text;
using Daybreak_Midnight.Static;
using HarmonyLib;

using Pinion;

using UnityEngine;

namespace Daybreak_Midnight.Patches
{
    [HarmonyPatch]
    public class APISoundPatch
    {
        [HarmonyPrefix]
        [HarmonyPatch(typeof(APISound),nameof(APISound.PlaySFX))]
        public static bool PlaySFXPrefix(PinionContainer container, string fileName)
        {
            BackgroundMusic bgm = GameObject.Find("BackgroundMusic").GetComponent<BackgroundMusic>();

            if (fileName.ToLower() == "resetbgm")
            {
                bgm.SetMusic(BackgroundMusic.Music.TENSION);

                return false;
            }

            bool isMusic = false;

            if(fileName.StartsWith("mus_")) { isMusic = true; }

            if (!string.IsNullOrEmpty(fileName) && NodeMapAPI.NodeMapActive() && Game.Controller.CustomSoundDatabase != null)
            {
                if (Game.Controller.CustomSoundDatabase.GetAudioClip(fileName) == null)
                {
                    container.LogError("No audio file found named: '" + fileName + "'. File must be in the Audio folder under the campaign's root directory and must be in WAV format.");
                }

                AudioSource globalAudio = GlobalAudio.Instance;
                AudioClip audioClip = Game.Controller.CustomSoundDatabase.GetAudioClip(fileName);
                MusicManager.lastKnownTrack = bgm.fallbackMusic;

                if(isMusic)
                {
                    bgm.Stop();
                    globalAudio.clip = audioClip;
                    globalAudio.loop = true;
                    globalAudio.volume = SystemData.Instance.musicVolume / 10f;
                    globalAudio.Play(0);
                } else
                {
                    globalAudio.loop = false;
                    globalAudio.PlayOneShot(audioClip);
                }
            }

            return false;
        }
    }
}
