using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;
using Utils;
using VContainer.Unity;

namespace FindTheDifference.Audio
{
    public class AudioManager : IInitializable
    {
        private static Sounds _soundsData;

        private static MusicChannel _music;
        private static SfxChannel _sfx;

        public MusicChannel MusicChannel { get { return _music; } }
        public SfxChannel SfxChannel { get { return _sfx; } }

        public AudioManager(MusicChannel music, SfxChannel sfx, Sounds soundsData)
        {
            _music = music;
            _sfx = sfx;
            _soundsData = soundsData;
        }

        public void Initialize()
        {
            _music.SetSoundsData(_soundsData);

            _sfx.SetVolume(SaveManager.PlayerData.Sounds);
            _music.SetVolume(SaveManager.PlayerData.Music);

            PlayMusic(AudioType.MainMusic);
        }

        public static void PlayMusic(AudioType sound) => _music.Play(_soundsData.GetSoundWithRandom(sound), false);
        public static void PlaySfx(AudioType sound) => _sfx.Play(_soundsData.GetSoundWithRandom(sound));
    }

    public sealed class MusicChannel
    {
        private readonly AudioSource _source;
        private readonly AudioMixer _mixer;

        private float _musicVolume = 0.1f;

        private bool _isWaitingForTrackEnd;

        private static Sounds _soundsData;

        private AudioType _currentMusicType;

        public MusicChannel(AudioSource source, AudioMixer mixer)
        {
            _source = source;
            _mixer = mixer;
        }

        public void SetSoundsData(Sounds sounds)
        {
            _soundsData = sounds;
        }

        public async void Play(AudioClip clip, bool loop)
        {
            _source.clip = clip;
            _source.loop = loop;
            _source.Play();

            if (!_isWaitingForTrackEnd)
                await WaitTrackEnd();
        }

        public void SetVolume(bool isOn)
        {
            _source.volume = isOn ? _musicVolume : 0;
            float dB = Mathf.Lerp(-80f, 0f, isOn ? _musicVolume : 0);
            _mixer.SetFloat("Volume", dB);

            SaveManager.PlayerData.Music = isOn;
        }

        private async UniTask WaitTrackEnd()
        {
            if (_source.clip == null)
                return;

            _isWaitingForTrackEnd = true;

            await UniTask.WaitForSeconds(_source.clip.length);

            _isWaitingForTrackEnd = false;

            AudioClip next = _soundsData.GetSoundWithRandom(AudioType.MainMusic);

            Play(next, loop: false);
        }
    }

    public sealed class SfxChannel
    {
        private readonly AudioSource _source;
        private readonly AudioMixer _mixer;

        private float _soundsVolume = 0.2f;

        public SfxChannel(AudioSource source, AudioMixer mixer)
        {
            _source = source;
            _mixer = mixer;
        }

        public void Play(AudioClip clip, float volume = 1f) => _source.PlayOneShot(clip, volume);


        public void SetVolume(bool isOn)
        {
            _source.volume = isOn ? _soundsVolume : 0;
            float dB = Mathf.Lerp(-80f, 0f, isOn ? _soundsVolume : 0);
            _mixer.SetFloat("Volume", dB);

            SaveManager.PlayerData.Sounds = isOn;
        }
    }

}
