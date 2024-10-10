using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using SDKPlayerPrefs_1 = SDKPlayerPrefs;

public class PlayerPrefsManager
{
    #region Variables Definition

    private const string PREFS_CURRENT_COINS = "user_coins";
    private const string PREFS_CURRENT_MUSIC_VOLUME = "_current_music_on";
    private const string PREFS_CURRENT_SOUND_VOLUME = "_current_sound_on";
    private const string PREFS_VIBRATION_ON = "_vibration_on";
    private const string PREFS_BEST_SCORE = "_best_score";

    #endregion

    #region Setting In Game

    public static float MusicVolume
    {
        get => PlayerPrefs.GetFloat(PREFS_CURRENT_MUSIC_VOLUME, 0f);
        set { PlayerPrefs.SetFloat(PREFS_CURRENT_MUSIC_VOLUME, value); }
    }

    public static float SoundVolume
    {
        get => PlayerPrefs.GetFloat(PREFS_CURRENT_SOUND_VOLUME, 0f);
        set { PlayerPrefs.SetFloat(PREFS_CURRENT_SOUND_VOLUME, value); }
    }

    public static bool CurrentVibrationOn
    {
        get => PlayerPrefs.GetInt(PREFS_VIBRATION_ON, 1) == 1;
        set => PlayerPrefs.SetInt(PREFS_VIBRATION_ON, value ? 1 : 0);
    }

    #endregion

    #region Gameplay Data

    public static int BestScore
    {
        get => PlayerPrefs.GetInt(PREFS_BEST_SCORE, 0);
        set => PlayerPrefs.SetInt(PREFS_BEST_SCORE, value);
    }

    #endregion

    #region Coin/Cash

    public static int UserCoins
    {
        get => PlayerPrefs.GetInt(PREFS_CURRENT_COINS, 0);
        set
        {
            if (UserCoins < 0)
                value = 0;
            PlayerPrefs.SetInt(PREFS_CURRENT_COINS, value);
            EventDispatcher.Instance.PostEvent(EventID.OnCashChange);
        }
    }

    #endregion

    #region Level

    #endregion
}