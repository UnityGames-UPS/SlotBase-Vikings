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
    private bool isForceMuted = false;

    private void Start()
    {
        if (bg_adudio) bg_adudio.Play();
        audioPlayer_button.clip = clips[clips.Length - 1];
        audioSpin_button.clip = clips[clips.Length - 2];
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

    //User-toggle-driven — the sound/music buttons in UIManager.
    internal void ToggleMute(bool toggle, string type = "all")
    {
        //An explicit user interaction proves the game currently has real interactive focus, so a
        //stale forced-mute must never block the button — clear it before applying the category state.
        isForceMuted = false;

        switch (type)
        {
            case "bg":
                bgUserMuted = toggle;
                break;
            case "button":
                buttonUserMuted = toggle;
                break;
            case "wl":
                wlUserMuted = toggle;
                break;
            case "all":
                bgUserMuted = toggle;
                buttonUserMuted = toggle;
                wlUserMuted = toggle;
                break;
        }

        ApplyUserMute();
    }

    private void ApplyUserMute()
    {
        if (bg_adudio) bg_adudio.mute = bgUserMuted;
        if (bg_audioBonus) bg_audioBonus.mute = bgUserMuted;
        if (audioPlayer_button) audioPlayer_button.mute = buttonUserMuted;
        if (audioSpin_button) audioSpin_button.mute = buttonUserMuted;
        if (audioPlayer_wl) audioPlayer_wl.mute = wlUserMuted;
        if (audioPlayer_Bonus) audioPlayer_Bonus.mute = wlUserMuted;
    }

    //Focus-driven — called from BOTH UIManager.OnFocusChanged (JS path) and OnApplicationFocus below.
    //Never writes the user flags, so regaining focus restores exactly the user's last chosen setting.
    internal void SetMuteAll(bool forceMute)
    {
        if (forceMute == isForceMuted) return;   //already in that state — don't re-force/re-restore
        isForceMuted = forceMute;

        if (forceMute)
        {
            if (bg_adudio) bg_adudio.mute = true;
            if (bg_audioBonus) bg_audioBonus.mute = true;
            if (audioPlayer_button) audioPlayer_button.mute = true;
            if (audioSpin_button) audioSpin_button.mute = true;
            if (audioPlayer_wl) audioPlayer_wl.mute = true;
            if (audioPlayer_Bonus) audioPlayer_Bonus.mute = true;
        }
        else
        {
            ApplyUserMute();
        }
    }

    //Native/editor focus path — calls the SAME method the WebGL OnFocusChanged path calls.
    private void OnApplicationFocus(bool focus)
    {
        SetMuteAll(!focus);
    }
}
