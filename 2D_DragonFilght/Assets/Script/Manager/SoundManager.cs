using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : DontDestroy<SoundManager>
{
    public enum AudioType
    {
        BGM,
        SFX,
        Max
    }
    public enum BgmClip
    {
        Dragon_flight,
        Max
    }
    public enum SfxClip
    {
        Get_coin,
        Get_gem,
        Get_Invincible,
        Get_Item,
        Mon_die,
        Max
    }
    AudioSource[] m_audio;
    [SerializeField]
    AudioClip[] m_bgmClips;
    [SerializeField]
    AudioClip[] m_sfxClips;
    const int MaxVolumeLevel = 10;
    const int MaxSfxPlayCount = 3;
    Dictionary<SfxClip, int> m_sfxPlayList = new Dictionary<SfxClip, int>();
    //볼륨조절
    public void SetVolume(int level)
    {
        
        SetVolumeBgm(level);
        SetVolumeSfx(level);
    }
    public void SetVolumeBgm(int level)
    {
        if (level < 0) level = 0;
        if (level > MaxVolumeLevel) level = MaxVolumeLevel;
        m_audio[(int)AudioType.BGM].volume = (float)level / MaxVolumeLevel;
    }
    public void SetVolumeSfx(int level)
    {
        if (level < 0) level = 0;
        if (level > MaxVolumeLevel) level = MaxVolumeLevel;
        m_audio[(int)AudioType.SFX].volume = (float)level / MaxVolumeLevel;
    }
    public void PlayBgm(BgmClip bgm)
    {
        m_audio[(int)AudioType.BGM].clip = m_bgmClips[(int)bgm];
        m_audio[(int)AudioType.BGM].Play();
        m_audio[(int)AudioType.BGM].loop = true;
    }
    public void PlaySfx(SfxClip sfx)
    {
        if(m_sfxPlayList.ContainsKey(sfx))
        {
            if(m_sfxPlayList[sfx] >= MaxSfxPlayCount)
            {
                return;
            }
            else
            {
                m_sfxPlayList[sfx]++;
            }
        }
        else
        {
            m_sfxPlayList.Add(sfx, 1);
        }
        m_audio[(int)AudioType.SFX].PlayOneShot(m_sfxClips[(int)sfx]);
        StartCoroutine(RemoveSfxPlayList(sfx, m_sfxClips[(int)sfx].length));
    }
    IEnumerator RemoveSfxPlayList(SfxClip sfx, float length)
    {
        yield return new WaitForSeconds(length);
        if (m_sfxPlayList[sfx] > 1)
            m_sfxPlayList[sfx]--;
        else
            m_sfxPlayList.Remove(sfx);

    }


    void Initialize()
    {
        m_audio = new AudioSource[(int)AudioType.Max];
        m_audio[(int)AudioType.BGM] = gameObject.AddComponent<AudioSource>();
        m_audio[(int)AudioType.BGM].clip = m_bgmClips[0];
        m_audio[(int)AudioType.BGM].loop = true;
        m_audio[(int)AudioType.BGM].playOnAwake = false;
        m_audio[(int)AudioType.BGM].rolloffMode = AudioRolloffMode.Linear;

        m_audio[(int)AudioType.SFX] = gameObject.AddComponent<AudioSource>();
        m_audio[(int)AudioType.SFX].loop = false;
        m_audio[(int)AudioType.SFX].playOnAwake = false;
        m_audio[(int)AudioType.SFX].rolloffMode = AudioRolloffMode.Linear;
    }
    void LoadSoundAsset()
    {
        m_bgmClips = Resources.LoadAll<AudioClip>("Sound/BGM");
        m_sfxClips = Resources.LoadAll<AudioClip>("Sound/SFX");
    }
    protected override void OnStart()
    {
        LoadSoundAsset();
        Initialize();
    }
}
