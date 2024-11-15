﻿/*
* ┌──────────────────────────────────┐
* │  描    述: Excel文件的存储路径              
* │  类    名: Constants.cs       
* │  创 建 人: 4463fger                     
* └──────────────────────────────────┘
*/

using UnityEngine;

namespace VN
{
    public class Constants
    {
        public static string STROY_PATH = "Assets/Resources/story/";
        public static string DEFAULT_STORY_FILE_NAME = "1.xlsx";
        public static int DEFAULT_START_LINE = 1;
        
        public static string AVATAR_PATH = "image/avatar/"; 
        public static string BACKGROUND_PATH = "image/background/";
        public static string CHARACTER_PATH = "image/character/";
        public static string IMAGE_LOAD_FAILED = "Failed to load image: ";
        
        public static string VOCAL_PATH = "audio/vocal/";
        public static string AUDIO_LOAD_FAILED = "Failed to load audio: ";

        public static string MUSIC_PATH = "audio/music/";
        public static string MUSIC_LOAD_FAILED = "Failed to load music: ";
        
        public static string NO_DATA_FOUND = "No data found";
        public static string END_OF_STORY = "End of story";
        public static float DEFAULT_WAITING_SECONDS = 0.05f;    //默认打字机速度

        public static string characterActionAppearAt = "appearAt";
        public static string characterActionDisappear = "disappear";
        public static string characterActionMoveTo = "moveTo";
    }
}