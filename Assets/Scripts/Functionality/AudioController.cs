using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;
using Best.SocketIO;

public class AudioController : MonoBehaviour
{
    [SerializeField] private AudioSource bg_adudio;
    [SerializeField] internal AudioSource audioPlayer_wl;
    [SerializeField] internal AudioSource audioPlayer_button;
    [SerializeField] internal AudioSource audioSpin_button;
    [SerializeField] private AudioClip[] clips;
    [SerializeField] private AudioClip[] Bonusclips;
    [SerializeField] private AudioSource bg_audioBonus;
    [SerializeField] private AudioSource audioPlayer_Bonus;

    private bool bgUserMuted = false;
    private bool buttonUserMuted = false;
    private bool wlUserMuted = false;

    private void Start()
    {
        if (bg_adudio) bg_adudio.Play();
        audioPlayer_button.clip = clips[clips.Length - 1];
        audioSpin_button.clip = clips[clips.Length - 2];
    }

  internal void CheckFocusFunction(bool focus, bool IsSpinning)
    {
        if (!focus)
        {
            bg_adudio.Pause();
            audioPlayer_wl.Pause();
            audioPlayer_button.Pause();
        }
        else
        {
            if (!bg_adudio.mute) bg_adudio.UnPause();
            if (IsSpinning)
            {
                if (!audioPlayer_wl.mute) audioPlayer_wl.UnPause();
            }
            else
            {
                StopWLAaudio();
            }
            if (!audioPlayer_button.mute) audioPlayer_button.UnPause();

        }
    }

    internal void SwitchBGSound(bool isbonus)
    {
        if (isbonus)
        {
            if (bg_audioBonus) bg_audioBonus.enabled = true;
            if (bg_adudio) bg_adudio.enabled = false;
        }
        else
        {
            if (bg_audioBonus) bg_audioBonus.enabled = false;
            if (bg_adudio) bg_adudio.enabled = true;
        }
    }

    internal void PlayWLAudio(string type)
    {
        audioPlayer_wl.loop = false;
        int index = 0;
        switch (type)
        {
            case "spin":
                index = 0;
                audioPlayer_wl.loop = true;
                break;
            case "win":
                index = 1;
                break;
            case "lose":
                index = 2;
                break;
            case "spinStop":
                index = 3;
                break;
            case "megaWin":
                index = 4;
                break;
        }
        StopWLAaudio();
        audioPlayer_wl.clip = clips[index];
        audioPlayer_wl.Play();

    }

    internal void PlayBonusAudio(string type)
    {
        audioPlayer_wl.loop = false;
        int index = 0;
        switch (type)
        {
            case "win":
                index = 0;
                break;
            case "lose":
                index = 1;
                break;
            case "cycleSpin":
                index = 2;
                break;
        }
        StopBonusAaudio();
        audioPlayer_Bonus.clip = Bonusclips[index];
        audioPlayer_Bonus.Play();

    }

    internal void PlayButtonAudio()
    {
        audioPlayer_button.Play();
    }

    internal void PlaySpinButtonAudio()
    {
        audioSpin_button.Play();
    }

    internal void StopWLAaudio()
    {

        audioPlayer_wl.Stop();
        audioPlayer_wl.loop = false;

    }

    internal void StopBonusAaudio()
    {
        audioPlayer_Bonus.Stop();
        audioPlayer_Bonus.loop = false;
    }

    internal void StopBgAudio()
    {
        bg_adudio.Stop();
    }

    internal void ToggleMute(bool toggle, string type = "all")
    {
        switch (type)
        {
            case "bg":
                bgUserMuted = toggle;
                bg_adudio.mute = toggle;
                bg_audioBonus.mute = toggle;
                if (!toggle) { bg_adudio.UnPause(); bg_audioBonus.UnPause(); }
                break;
            case "button":
                buttonUserMuted = toggle;
                audioPlayer_button.mute = toggle;
                audioSpin_button.mute = toggle;
                if (!toggle) { audioPlayer_button.UnPause(); audioSpin_button.UnPause(); }
                break;
            case "wl":
                wlUserMuted = toggle;
                audioPlayer_wl.mute = toggle;
                audioPlayer_Bonus.mute = toggle;
                if (!toggle) { audioPlayer_wl.UnPause(); audioPlayer_Bonus.UnPause(); }
                break;
            case "all":
                bgUserMuted = toggle;
                buttonUserMuted = toggle;
                wlUserMuted = toggle;
                audioPlayer_wl.mute = toggle;
                bg_adudio.mute = toggle;
                audioPlayer_button.mute = toggle;
                audioSpin_button.mute = toggle;
                if (!toggle)
                {
                    bg_adudio.UnPause();
                    audioPlayer_button.UnPause();
                    audioSpin_button.UnPause();
                    audioPlayer_wl.UnPause();
                }
                break;
        }
    }

    private void OnApplicationFocus(bool focus)
    {
        bg_adudio.mute = focus ? bgUserMuted : true;
        bg_audioBonus.mute = focus ? bgUserMuted : true;
        audioPlayer_button.mute = focus ? buttonUserMuted : true;
        audioSpin_button.mute = focus ? buttonUserMuted : true;
        audioPlayer_wl.mute = focus ? wlUserMuted : true;
        audioPlayer_Bonus.mute = focus ? wlUserMuted : true;
    }
}
