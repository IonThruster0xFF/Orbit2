using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Manager
{
    public class AudioManager : MonoSingleton<AudioManager>, IPauseable
    {
        #region 音量
        private float effectIntensity;
        private float ambientIntensity;
        private float musicIntensity;
        public float EffectIntensity
        {
            get
            {
                return effectIntensity;
            }
            set
            {
                effectIntensity = Mathf.Clamp(value, 0f, 1f);
                foreach (AudioSource effect in effectSourceList)
                    effect.volume = effectIntensity;
            }
        }
        public float AmbientIntensity
        {
            get
            {
                return ambientIntensity;
            }
            set
            {
                ambientIntensity = Mathf.Clamp(value, 0f, 1f);
                ambientSource.volume = ambientIntensity;
            }
        }
        public float MusicIntensity
        {
            get
            {
                return musicIntensity;
            }
            set
            {
                musicIntensity = Mathf.Clamp(value, 0f, 1f);
                musicSource.volume = musicIntensity;
            }
        }
        #endregion
        #region 背景音乐, 环境音效, 效果音效
        AudioSource musicSource = null;
        AudioSource ambientSource = null;
        Queue<AudioSource> effectSourceList = null;

        private const int maxEffectCount = 20;//效果音数量,超过20之后自动增加减少
        private float switchDeltaVolumeSpeed;//切换音乐时音量变换速度
        private float stopDeltaVolumeSpeed;//暂停时音量变化速度
        #endregion
        void Start()// 请勿改成 Awake, 依赖EventManager
        {
            //初始化音频播放器
            musicSource =  gameObject.AddComponent<AudioSource>();
            musicSource.loop = true;
            ambientSource =  gameObject.AddComponent<AudioSource>();
            ambientSource.loop = true;
            effectSourceList = new Queue<AudioSource>();
            for (int i = 0; i < maxEffectCount; i++)
            {
                effectSourceList.Enqueue(gameObject.AddComponent<AudioSource>());
            }

            //初始化音量
            EffectIntensity = 1f;
            AmbientIntensity = 1f;
            MusicIntensity = 1f;

            switchDeltaVolumeSpeed = 0.3f * Time.fixedDeltaTime;
            stopDeltaVolumeSpeed = 2f * Time.fixedDeltaTime;
            //注册暂停事件
            OnEvent pause = OnPauseGame;
            OnEvent unpause = OnUnPauseGame;
            EventManager.instance.AddListener(GameEvent.GAME_PAUSE,pause);
            EventManager.instance.AddListener(GameEvent.GAME_UNPAUSE, unpause);
        }

        #region 基础音频功能
        //淡出效果
        private IEnumerator StopPlayAndDestroy(AudioSource oldAudio)
        {
            while (oldAudio.volume > Mathf.Epsilon)
            {
                oldAudio.volume -= switchDeltaVolumeSpeed;
                yield return new WaitForFixedUpdate();
            }
            oldAudio.Stop();
            Destroy(oldAudio);
            yield return null;
        }
        //淡入效果
        private IEnumerator StartPlay(AudioSource newAudio, float maxVolume)
        {
            newAudio.volume = 0f;
            newAudio.Play();
            while (newAudio.volume < maxVolume)
            {
                newAudio.volume += switchDeltaVolumeSpeed;
                yield return new WaitForFixedUpdate();
            }
            yield return null;
        }

        public void PlayBackgroundMusic(AudioClip bkMusic)
        {
            if (!musicSource.isPlaying)
            {
                //播放,淡入效果
                musicSource.clip = bkMusic;
                StartCoroutine(StartPlay(musicSource,MusicIntensity));
            }
            else
            {
                //平滑切换
                AudioSource oldMusicSource = musicSource;
                musicSource = gameObject.AddComponent<AudioSource>();
                musicSource.clip = bkMusic;
                musicSource.loop = true;

                StartCoroutine(StopPlayAndDestroy(oldMusicSource));
                StartCoroutine(StartPlay(musicSource, MusicIntensity));
            }
        }
        public void PlayAmbientSound(AudioClip ambientSound)
        {
            if (!ambientSource.isPlaying)
            {
                //播放,淡入效果
                ambientSource.clip = ambientSound;
                StartCoroutine(StartPlay(ambientSource,AmbientIntensity));
            }
            else
            {
                //平滑切换
                AudioSource oldAmbientSource = ambientSource;
                ambientSource = gameObject.AddComponent<AudioSource>();
                ambientSource.loop = true;
                ambientSource.clip = ambientSound;
                StartCoroutine(StopPlayAndDestroy(oldAmbientSource));
                StartCoroutine(StartPlay(ambientSource, AmbientIntensity));
            }
        }
        /// <summary>
        /// 播放效果音 (可多个)
        /// 比如怪物吼叫, 主角放屁
        /// </summary>
        /// <param name="sound">Sound.</param>
        public void PlayEffectSound(AudioClip sound)
        {
            for(int i=0;i<effectSourceList.Count;i++)
            {
                if (effectSourceList.Peek().isPlaying)
                {
                    effectSourceList.Enqueue(effectSourceList.Dequeue());
                    continue;
                }
                else 
                {
                    effectSourceList.Peek().clip = sound;
                    effectSourceList.Peek().Play();
                    effectSourceList.Enqueue(effectSourceList.Dequeue());
                    Debug.Log("播放音频:"+sound.name);

                    //这个时候试着删除多余音频播放器
                    //( i<max/4 意味着只找了1/4就找到了空的音频播放器)
                    //每次只删除一个,防止又出现大量声音
                    if ( i < maxEffectCount/4 && effectSourceList.Count > maxEffectCount)
                    {
                        for(int k=0;k< effectSourceList.Count;k++)
                        {
                            if (!effectSourceList.Peek().isPlaying)
                            {
                                Destroy(effectSourceList.Dequeue());
                                return;
                            }
                        }
                    }
                    return;
                }
            }
            AudioSource tempSource = gameObject.AddComponent<AudioSource>();
            tempSource.volume = EffectIntensity;
            tempSource.clip = sound;
            tempSource.Play();
            effectSourceList.Enqueue(tempSource);

        }
        #endregion

        #region 暂停事件
        //效果音减弱,环境音减弱暂停,背景音乐不变
        public void OnPauseGame(GameEvent type, Component comp, object p = null)
        {
            Debug.Log("AmbientSound Pause");
            StopCoroutine("UnPauseGame");
            StartCoroutine("PauseGame");
        }
        public void OnUnPauseGame(GameEvent type, Component comp, object p = null)
        {
            Debug.Log("AmbientSound UnPause");
            StopCoroutine("PauseGame");
            StartCoroutine("UnPauseGame");
        }
        private IEnumerator PauseGame()
        {
            float efVolume = effectSourceList.Peek().volume;
            float amVolume = ambientSource.volume;

            while (efVolume > Mathf.Epsilon || amVolume > Mathf.Epsilon)
            {
                if (amVolume > Mathf.Epsilon)
                {
                    amVolume -= stopDeltaVolumeSpeed;
                    ambientSource.volume -= stopDeltaVolumeSpeed;
                }
                if (efVolume > Mathf.Epsilon)
                {
                    efVolume -= stopDeltaVolumeSpeed;
                    foreach (AudioSource effect in effectSourceList)
                        effect.volume -= stopDeltaVolumeSpeed;
                }
                //Debug.Log(ambientSource.volume);
                yield return new WaitForFixedUpdate();
            }
            ambientSource.Pause();//先减少音量再暂停
            yield return null;
        }
        private IEnumerator UnPauseGame()
        {
            ambientSource.UnPause();//先播放再改音量
            float efVolume = effectSourceList.Peek().volume;
            float amVolume = ambientSource.volume;
            while (efVolume < EffectIntensity || amVolume < AmbientIntensity)
            {
                if (amVolume < AmbientIntensity)
                {
                    amVolume += stopDeltaVolumeSpeed;
                    ambientSource.volume += stopDeltaVolumeSpeed;
                }
                if (efVolume < EffectIntensity)
                {
                    efVolume += stopDeltaVolumeSpeed;
                    foreach (AudioSource effect in effectSourceList)
                        effect.volume += stopDeltaVolumeSpeed;
                }
                Debug.Log(ambientSource.volume);
                yield return new WaitForFixedUpdate();
            }
            ambientSource.volume = AmbientIntensity;
            foreach (AudioSource effect in effectSourceList)
                effect.volume = EffectIntensity;
            yield return null;
        }
        #endregion
    }
}

