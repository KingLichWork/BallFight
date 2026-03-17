using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Sounds", menuName = "Data/Sounds")]
public class Sounds : ScriptableObject
{
    [SerializeField] private List<Sound> sounds;

    public AudioClip GetSoundWithRandom(AudioType type)
    {
        var needTypeSounds = sounds.Where(s => s.Type == type).ToList();

        if (needTypeSounds.Count == 0)
            Debug.LogError($"��� �� ������ ����������� ����� ��� ��� {type}. �������� �� �������� � ������");

        return needTypeSounds[UnityEngine.Random.Range(0, needTypeSounds.Count)].Clip;
    }

    [Serializable]
    public struct Sound
    {
        public AudioType Type;
        public AudioClip Clip;

        public string Name => Clip.name;
    }
}
public enum AudioType
{
    MainMusic,
    WinMusic,
    LoseMusic,
    ClickButtonSound
}