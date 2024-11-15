﻿/*
* ┌──────────────────────────────────┐
* │  描    述: 主控制器               
* │  类    名: VNManager.cs       
* │  创 建 人: 4463fger                 
* └──────────────────────────────────┘
*/

using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace VN
{
    public class VNManager : UnityEngine.MonoBehaviour
    {
        public TextMeshProUGUI speakerName;
        public TextMeshProUGUI speakingContent;
        public TypeWriterEffect typeWriterEffect;
        public Image avatarImage;
        public AudioSource vocalAudio;
        public Image backgroundImage;
        public AudioSource backgroundMusic;
        public Image CharacterImage1;
        public Image CharacterImage2;

        private string storyPath = Constants.STROY_PATH;
        private string defaultStoryFileName = Constants.DEFAULT_STORY_FILE_NAME;
        private List<ExcelReader.ExcelData> storyData;
        private int currentLine = Constants.DEFAULT_START_LINE;

        private void Start()
        {
            LoadStoryFromFile(storyPath + defaultStoryFileName);
            DisplayNextLine();
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                DisplayNextLine();
            }
        }

        /// <summary>
        /// 从Excel文件中读取文本
        /// </summary>
        /// <param name="path">文件存储路径</param>
        void LoadStoryFromFile(string path)
        {
            storyData = ExcelReader.ReadExcel(path);
            if (storyData == null || storyData.Count == 0)
            {
                Debug.LogError(Constants.NO_DATA_FOUND);
            }
        }

        /// <summary>
        /// 一行一行显示文本
        /// </summary>
        void DisplayNextLine()
        {
            if (currentLine >= storyData.Count)
            {
                Debug.Log(Constants.END_OF_STORY);
                return;
            }

            // 如果正在打字，则直接打字完成
            if (typeWriterEffect.IsTyping())
            {
                typeWriterEffect.CompleteLine();
            }
            else
            {
                DisplayThisLine();
            }
        }
        
        void DisplayThisLine()
        {
            var data = storyData[currentLine];
            speakerName.text = data.speakerName;
            speakingContent.text = data.speakingContent;
            typeWriterEffect.StartTyping(speakingContent.text);
            //头像名不为空,更新
            if (NotNullNorEmpty(data.avatarImageFileName))
            {
                UpdateAvatorImage(data.avatarImageFileName);
            }
            else
            {
                avatarImage.gameObject.SetActive(false);
            }
            if (NotNullNorEmpty(data.vocalAudioFileName))
            {
                PlayerVocalAudio(data.vocalAudioFileName);
            }
            if (NotNullNorEmpty(data.backgroundImageFileName))
            {
                UpdateBackgroundImage(data.backgroundImageFileName);
            }
            if (NotNullNorEmpty(data.backgroundMusicFileName))
            {
                PlayerBackgroundMusic(data.backgroundMusicFileName);
            }

            if (NotNullNorEmpty(data.character1Action))
            {
                UpdateCharacterImage(data.character1Action, data.character1ImageFileName,CharacterImage1);
            }

            if (NotNullNorEmpty(data.character2Action))
            {
                UpdateCharacterImage(data.character2Action, data.character2ImageFileName,CharacterImage2);
            }
            currentLine++;
        }

        /// <summary>
        /// 判断是否为空
        /// </summary>
        /// <param name="str">需要判断的字符名</param>
        /// <returns></returns>
        bool NotNullNorEmpty(string str)
        {
            return !string.IsNullOrEmpty(str);
        }
        
        /// <summary>
        /// 更新头像
        /// </summary>
        /// <param name="imageFileName"></param>
        void UpdateAvatorImage(string imageFileName)
        {
            string imagePath = Constants.AVATAR_PATH + imageFileName;
            UpdateImage(imagePath, avatarImage);
        }
        
        /// <summary>
        /// 播放音效
        /// </summary>
        /// <param name="audioFileName"></param>
        void PlayerVocalAudio(string audioFileName)
        {
            string audioPath = Constants.VOCAL_PATH + audioFileName;
            PlayAudio(audioPath,vocalAudio,false);
        }


        /// <summary>
        /// 更新背景图片
        /// </summary>
        /// <param name="imageFileName"></param>
        private void UpdateBackgroundImage(string imageFileName)
        {
            string imagePath = Constants.BACKGROUND_PATH + imageFileName;
            UpdateImage(imagePath, backgroundImage);
        }

        /// <summary>
        /// 播放背景音乐
        /// </summary>
        /// <param name="musicFileName"></param>
        void PlayerBackgroundMusic(string musicFileName)
        {
            string musicPath = Constants.MUSIC_PATH + musicFileName;
            PlayAudio(musicPath,backgroundMusic,true);
        }
        
        void UpdateCharacterImage(string action, string imageFileName,Image characterImage)
        {
            if (action.StartsWith(Constants.characterActionAppearAt)) // 解析appear(x,y)总做并在(x,y)显示角色立绘
            {
                string imagePath = Constants.CHARACTER_PATH + imageFileName;
                UpdateImage(imagePath,characterImage);
            }
            else if (action == Constants.characterActionDisappear) // 隐藏角色立绘
            {
                characterImage.gameObject.SetActive(false);
            }
            else if (action.StartsWith(Constants.characterActionMoveTo)) // 解析moveTo(x,y)动作并移动角色立绘到(x,y)位置
            {
                
            }
        }
        
        /// <summary>
        /// 更新图片
        /// </summary>
        /// <param name="imagePath">图片路径</param>
        /// <param name="image">图片</param>
        private void UpdateImage(string imagePath, Image image)
        {
            Sprite sprite = Resources.Load<Sprite>(imagePath);
            if (sprite is not null)
            {
                image.sprite = sprite;
                image.gameObject.SetActive(true);
            }
            else
            {
                Debug.LogError(Constants.IMAGE_LOAD_FAILED + imagePath);
            }
        }
        
        /// <summary>
        /// 播放音频
        /// </summary>
        /// <param name="audioPath">音频路径</param>
        /// <param name="audioSource">音频源</param>
        /// <param name="isLoop">是否循环</param>
        private void PlayAudio(string audioPath, AudioSource audioSource, bool isLoop)
        {
            AudioClip audioClip = Resources.Load<AudioClip>(audioPath);
            if (audioClip is not null)
            {
                audioSource.clip = audioClip;
                audioSource.Play();
                audioSource.loop = isLoop;
            }
            else
            {
                if (audioSource == vocalAudio)
                {
                    Debug.LogError(Constants.AUDIO_LOAD_FAILED + audioPath);
                }
                else if(audioSource == backgroundMusic)
                {
                    Debug.LogError(Constants.MUSIC_LOAD_FAILED + audioPath);
                }
            }
        }
    }
}