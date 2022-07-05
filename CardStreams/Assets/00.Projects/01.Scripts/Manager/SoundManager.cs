using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public enum SFXType
{
    Moving, // 이동할때
    BasicCardSet, // 기본카드 바닥에깔떄
    NextButton, // 다음버튼 누를때
    RandomMonster, // 랜덤으로 몬스터 나올때
    CardReverse, // 보상카드뒤집을때
    BuyCard, // 뭐 살때
    DrawCard, // 뭐 드로우할때
}

public enum BGMType
{
    MainTheme,
    Boss
}

public enum SoundType // 마스터소리 배경소리 효과소리
{
    Master,
    BGM,
    SFX
}

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [Header("스피커 설정")]
    [SerializeField] AudioMixer _masterVolumeMixer;
    [SerializeField] AudioSource _bgmPlayer;
    [SerializeField] AudioSource _sfxPlayer;

    [Header("사운드들")]
    [SerializeField] List<AudioClip> sfxSounds; // 효과음 모음, enum과 똑같은 순서대로 들어있어야함
    [SerializeField] List<AudioClip> bgmSounds; // 효과음 모음, enum과 똑같은 순서대로 들어있어야함

    private Dictionary<SFXType, AudioClip> sfxDictionary = new Dictionary<SFXType, AudioClip>();
    private Dictionary<BGMType, AudioClip> bgmDictionary = new Dictionary<BGMType, AudioClip>();

    private void Awake()
    {
        if (Instance != null) Destroy(gameObject);
        Instance = this;

        for (int i = 0; i < sfxSounds.Count; i++)
        {
            sfxDictionary.Add((SFXType)i, sfxSounds[i]);
        }

        for (int i = 0; i < bgmSounds.Count; i++)
        {
            bgmDictionary.Add((BGMType)i, bgmSounds[i]);
        }

        PlayBGM(BGMType.MainTheme);
    }

    /// <summary> 효과음 재생 </summary>
    /// <param name="type">효과음 타입</param>
    /// <param name="volume">일시적인 볼륨(기본값 : 1)</param>
    public void PlaySFX(SFXType type, float volume = 1f)
    {
        if (sfxDictionary.TryGetValue(type, out AudioClip clip))
        {
            _sfxPlayer.PlayOneShot(clip, volume);
        }
    }

    /// <summary> BGM 재생 </summary>
    /// <param name="type">BGM 타입</param>
    public void PlayBGM(BGMType type)
    {
        if (bgmDictionary.TryGetValue(type, out AudioClip clip))
        {
            _bgmPlayer.clip = clip;
        }

        if (_bgmPlayer.isPlaying == false)
            _bgmPlayer.Play();
    }

    public void StopBGM(bool isStop)
    {
        if (isStop)
        {
            _bgmPlayer.Pause();
        }
        else
        {
            _bgmPlayer.UnPause();
        }
    }

    /// <summary> 음악 타입 | 음량, 음량을 바꾸는 함수 </summary>
    /// <param name="type">음악 타입</param>
    /// <param name="volume">음량</param>
    public void ChangeVolume(SoundType type, float volume)
    {
        switch (type)
        {
            case SoundType.Master:
                if (volume <= 0)
                {
                    _masterVolumeMixer.SetFloat("Master", -80f);
                }
                else
                {
                    _masterVolumeMixer.SetFloat("Master", 0.2f * volume - 20);
                }
                break;

            case SoundType.BGM:
                _bgmPlayer.volume = volume / 100;
                break;

            case SoundType.SFX:
                _sfxPlayer.volume = volume / 100;
                break;

            default:
                break;
        }
    }

    /// <summary> 음악 타입 / 음소거 true 풀려면 false </summary>
    /// <param name="type">이 음악타입을</param>
    /// <param name="isMute">음소거할 것인가요? Y/N</param>
    public void MuteVolume(SoundType type, bool isMute)
    {
        switch (type)
        {
            case SoundType.Master:
                MasterVolumeMute(isMute);
                break;

            case SoundType.BGM:
                _bgmPlayer.mute = isMute;
                break;

            case SoundType.SFX:
                _sfxPlayer.mute = isMute;
                break;

            default:
                break;
        }
    }

    /// <summary> 마스터 볼륨을 음소거하는 함수 </summary>
    /// <param name="isMute"> 음소거 Y/N </param>
    private void MasterVolumeMute(bool isMute)
    {
        AudioListener.volume = isMute ? 0 : 1;
    }
}
