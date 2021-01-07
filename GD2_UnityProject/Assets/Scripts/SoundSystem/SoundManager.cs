using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

namespace SoundSystem
{
    //public class VolumeChangedEventArgs : EventArgs
    //{
    //    public float SfxVolume;
    //    public float BgmVolume;
    //
    //    public VolumeChangedEventArgs(float sfxVolume, float bgmVolume)
    //    {
    //        SfxVolume = sfxVolume;
    //        BgmVolume = bgmVolume;
    //    }
    //}

    public class SoundManager : SingletonMonoBehaviour<SoundManager>
    {
        /* 
         * Tutorial on this SoundManager;
         * Drop a mp3,wav, or other compatible type file in the Sounds folder
         * Add in the AudioSources a name for your soundeffect
         * Create a child in the SoundManager prefab with a SoundSource component (Turn of PlayOnAwake in editor)
         * Put the mp3 in a the AudioClip on that component
         * Assing that child to the AudioSource in the script of the prefab
         * Create a function like below
         * Call that function in any script using 
         * SoundManager.Instance.Play*InsertName*Sound();
         */

        [SerializeField]
        private AudioSource
            JumpSFX = null,
            SlowBlockLandSFX = null,
            FastBlockLandSFX = null,
            PushBlockSFX = null,
            TitleBGM = null,
            BattleBGM = null,
            BigOverworldBGM = null,
            EndGameBGM = null,
            FinalBGM = null,
            OverworldBGM = null,
            ValanoBGM = null;

        [SerializeField]
        [Range(0.0f, 1.0f)]
        private float _sfxVolume = 1.0f, _bgmVolume = 1.0f;

        private List<AudioSource>
            _sfxSounds = new List<AudioSource>(),
            _bgmSounds = new List<AudioSource>(),
            _playSceneSongs = new List<AudioSource>();

        private AudioSource _playingSong = null;
        private AudioSource _playingSongField
        {
            get
            {
                return _playingSong;
            }
            set
            {
                if (_playingSong != null)
                {
                    //StopAllCoroutines();
                    _playingSong.Stop();
                }

                _playingSong = value;

                if (_playingSong != null)
                {
                    _playingSong.Play();
                    //StartCoroutine(LoopSong(_playingSong));
                }
            }
        }
        private bool _isPlaying = true;
        private bool _inPlayScene = false;
        public bool InPlayScene
        {
            get
            {
                return _inPlayScene;
            }
            set
            {
                if (_inPlayScene != value && value == true)
                {
                    _inPlayScene = value;
                    _playingSongField = _playSceneSongs[UnityEngine.Random.Range(0, _playSceneSongs.Count)];
                }
                else
                    _inPlayScene = value;
            }
        }


        //public event EventHandler<VolumeChangedEventArgs> OnVolumeChanged;

        private void Awake()
        {
            _sfxSounds.Add(JumpSFX);
            _sfxSounds.Add(SlowBlockLandSFX);
            _sfxSounds.Add(FastBlockLandSFX);
            _sfxSounds.Add(PushBlockSFX);

            _bgmSounds.Add(TitleBGM);
            _bgmSounds.Add(BattleBGM);
            _bgmSounds.Add(BigOverworldBGM);
            _bgmSounds.Add(EndGameBGM);
            _bgmSounds.Add(FinalBGM);
            _bgmSounds.Add(OverworldBGM);
            _bgmSounds.Add(ValanoBGM);

            _playSceneSongs.Add(BattleBGM);
            _playSceneSongs.Add(BigOverworldBGM);
            _playSceneSongs.Add(ValanoBGM);

            ChangeVolumes(_sfxVolume, _bgmVolume);
        }

        private void Update()
        {
            var playing = _playingSongField.isPlaying;
            if(_isPlaying != playing && playing == false)
            {
                //Previous frame was playing, now not so loop
                if (InPlayScene)
                {
                    var nextsong = _playSceneSongs[UnityEngine.Random.Range(0, _playSceneSongs.Count)];
                    while (_playingSongField == nextsong && _playSceneSongs.Count != 1)
                    {
                        nextsong = _playSceneSongs[UnityEngine.Random.Range(0, _playSceneSongs.Count)];
                    }
                    _playingSongField = nextsong;
                }

                _playingSong.Play();
            }

            _isPlaying = playing;
        }

        public void ChangeVolumes(float SfxVolume, float BgmVolume)
        {
            foreach (var sfx in _sfxSounds)
            {
                sfx.volume = SfxVolume;
            }

            foreach (var bgm in _bgmSounds)
            {
                bgm.volume = SfxVolume;
            }
        }

        public void StopCurrentlyPlaying()
        {
            _playingSongField = null;
        }

        private IEnumerator LoopSong(AudioSource source)
        {
            var duration = source.clip.length;
            yield return new WaitForSecondsRealtime(duration);

            if (InPlayScene)
            {
                var nextsong = _playSceneSongs[UnityEngine.Random.Range(0, _playSceneSongs.Count)];
                while (_playingSongField == nextsong && _playSceneSongs.Count != 1)
                {
                    nextsong = _playSceneSongs[UnityEngine.Random.Range(0, _playSceneSongs.Count)];
                }
                _playingSongField = nextsong;
            }

            _playingSong.Play();
        }

        #region SFX
        public void PlayJump()
        {
            JumpSFX.Play();
        }

        public void PlaySlowBlockLanded()
        {
            SlowBlockLandSFX.Play();
        }

        public void PlayFastBlockLanded()
        {
            FastBlockLandSFX.Play();
        }

        public void PlayPushBlock()
        {
            PushBlockSFX.Play();
        }
        #endregion

        #region BGM
        public void PlayTitle()
        {
            _playingSongField = TitleBGM;
        }

        public void PlayBattle()
        {
            _playingSongField = BattleBGM;
        }

        public void PlayBigOverworld()
        {
            _playingSongField = BigOverworldBGM;
        }

        public void PlayEndGame()
        {
            _playingSongField = EndGameBGM;
        }

        public void PlayFinal()
        {
            _playingSongField = FinalBGM;
        }

        public void PlayOverworld()
        {
            _playingSongField = OverworldBGM;
        }

        public void PlayValcano()
        {
            _playingSongField = ValanoBGM;
        }
        #endregion
    }
}
