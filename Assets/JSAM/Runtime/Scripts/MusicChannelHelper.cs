using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JSAM
{
    [AddComponentMenu("")]
    [RequireComponent(typeof(AudioSource))]
    public class MusicChannelHelper : BaseAudioChannelHelper<MusicFileObject>
    {
        bool _isMuted = false;
        float _realVolume = 1;

        public bool IsMuted => _isMuted;

        public float RealVolume => AudioSource.volume / AudioFile.relativeVolume;

        protected override VolumeChannel DefaultChannel => VolumeChannel.Music;

        protected override void OnEnable()
        {
            base.OnEnable();
        }

        public void SetVolume(float newVolume)
        {
            _realVolume = newVolume;
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            if (audioFile)
            {
                if (audioFile.maxPlayingInstances > 0)
                {
                    AudioManager.InternalInstance.RemovePlayingMusic(audioFile, this);
                }
            }
        }

        protected override void OnUpdateVolume(float volume = 0)
        {
            _realVolume = volume;
            if (_isMuted) return;

            AudioSource.volume = Volume;
        }

        public void Mute()
        {
            _isMuted = true;
            AudioSource.volume = 0;
        }

        public void Unmute()
        {
            _isMuted = false;
            AudioSource.volume = _realVolume * AudioManager.MusicVolume;
        }

        public override AudioSource Play()
        {
            if (audioFile == null)
            {
                AudioManager.DebugWarning("Tried to play Music when no Music File was assigned!");
                return AudioSource;
            }

            AudioSource.pitch = 1;

            if (audioFile.loopMode == LoopMode.NoLooping)
            {
                AudioSource.loop = false;
            }
            else
            {
                AudioSource.loop = true;
            }

            return base.Play();
        }

        public override void Stop(bool stopInstantly = true)
        {
            base.Stop(stopInstantly);
            if (stopInstantly)
            {
                AudioSource.Stop();
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="fadeTime">Fade-out time in seconds</param>
        /// <returns></returns>
        protected override IEnumerator FadeOut(float fadeTime)
        {
            if (fadeTime != 0)
            {
                float startingVolume = AudioSource.volume;
                float timer = 0;
                while (timer < fadeTime)
                {
                    if (audioFile.ignoreTimeScale) timer += Time.unscaledDeltaTime;
                    else timer += Time.deltaTime;

                    AudioSource.volume = Mathf.Lerp(startingVolume, 0, timer / fadeTime);
                    yield return null;
                }
                // AudioSource.Stop();
            }
        }


#if UNITY_EDITOR
        public void PlayDebug(MusicFileObject file, bool dontReset)
        {
            if (!dontReset)
            {
                AudioSource.Stop();
            }
            audioFile = file;
            AudioSource.clip = file.Files[0];
            AudioSource.timeSamples = (int)Mathf.Clamp((float)AudioSource.timeSamples, 0, (float)AudioSource.clip.samples - 1);
            AudioSource.pitch = 1;
            AudioSource.volume = file.relativeVolume;

            base.PlayDebug(dontReset);
        }
#endif
    }
}