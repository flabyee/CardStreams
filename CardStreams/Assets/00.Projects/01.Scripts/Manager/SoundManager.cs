using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public enum SFXType
{
    Moving, // �̵��Ҷ�
    BasicCardSet, // �⺻ī�� �ٴڿ���
    NextButton, // ������ư ������
    RandomMonster, // �������� ���� ���ö�
    CardReverse, // ����ī���������
    BuyCard, // �� �춧
    DrawCard, // �� ��ο��Ҷ�
}

public enum BGMType
{
    MainTheme,
    Boss
}

public enum SoundType // �����ͼҸ� ���Ҹ� ȿ���Ҹ�
{
    Master,
    BGM,
    SFX
}

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [Header("����Ŀ ����")]
    [SerializeField] AudioMixer _masterVolumeMixer;
    [SerializeField] AudioSource _bgmPlayer;
    [SerializeField] AudioSource _sfxPlayer;

    [Header("�����")]
    [SerializeField] List<AudioClip> sfxSounds; // ȿ���� ����, enum�� �Ȱ��� ������� ����־����
    [SerializeField] List<AudioClip> bgmSounds; // ȿ���� ����, enum�� �Ȱ��� ������� ����־����

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

    /// <summary> ȿ���� ��� </summary>
    /// <param name="type">ȿ���� Ÿ��</param>
    /// <param name="volume">�Ͻ����� ����(�⺻�� : 1)</param>
    public void PlaySFX(SFXType type, float volume = 1f)
    {
        if (sfxDictionary.TryGetValue(type, out AudioClip clip))
        {
            _sfxPlayer.PlayOneShot(clip, volume);
        }
    }

    /// <summary> BGM ��� </summary>
    /// <param name="type">BGM Ÿ��</param>
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

    /// <summary> ���� Ÿ�� | ����, ������ �ٲٴ� �Լ� </summary>
    /// <param name="type">���� Ÿ��</param>
    /// <param name="volume">����</param>
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

    /// <summary> ���� Ÿ�� / ���Ұ� true Ǯ���� false </summary>
    /// <param name="type">�� ����Ÿ����</param>
    /// <param name="isMute">���Ұ��� ���ΰ���? Y/N</param>
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

    /// <summary> ������ ������ ���Ұ��ϴ� �Լ� </summary>
    /// <param name="isMute"> ���Ұ� Y/N </param>
    private void MasterVolumeMute(bool isMute)
    {
        AudioListener.volume = isMute ? 0 : 1;
    }
}
