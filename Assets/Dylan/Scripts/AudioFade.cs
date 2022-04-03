using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;





namespace JGDT.Audio.FadeInOut
{
    // Reusable asset
    // Last edited 10.02.2022

    /// <summary>
    /// This component will fade sound (in or out) whatever sound is currently played in the <see cref="AudioSource"/> over the specified amount of time.
    /// The component is designed to be used programmatically.
    /// </summary>
    
    // Originally made for UNITY 2019.4.13f1
    // Last test on UNITY 2020.3.3f1
    [RequireComponent(typeof(AudioSource))]
    public class AudioFade : MonoBehaviour
    {
        #region VARIABLES
        /// <summary>
        /// Fires when a FadeIn is started.
        /// </summary>
        public event EventHandler OnFadeInStart;
        /// <summary>
        /// Fires when a FadeIn has ended.
        /// </summary>
        public event EventHandler OnFadeInEnd;
        /// <summary>
        /// Fires when a FadeOut is started.
        /// </summary>
        public event EventHandler OnFadeOutStart;
        /// <summary>
        /// Fires when a FadeOut has ended.
        /// </summary>
        public event EventHandler OnFadeOutEnd;

        [Tooltip("The amount of time it takes to fade the sound in seconds.")]
        public float FadeTime = 1f;
        [Tooltip("The desired volume when fading in.")]
        public float VolumeFadeIn = 1f;
        [Tooltip("The desired volume when fading out.")]
        public float VolumeFadeOut = 0f;
        [Tooltip("The curve used to fade the sound.")]
        public AnimationCurve FadeCurve = new AnimationCurve(new Keyframe[] { new Keyframe(0, 0), new Keyframe(1f, 1f) });

        [SerializeField] private bool fadeInOnEnable = false;



        [Space(20), Header("Read Only")]
        [SerializeField] private bool _isPaused = false; // This bool will be used if the fading needs to be paused.
        [SerializeField] private bool _isFading = false; // This bool will be used to determine if the source is currently fading or not.
        /// <summary>
        /// The <see cref="AudioSource"/> that this component will use. Required and auto-added.
        /// </summary>
        public AudioSource audioSource;

        // Private Fields
        /// <summary>
        /// A queue in case Fade functions are called multiple times in succession.
        /// </summary>
        private Queue<FadeData> _queue;
        private struct FadeData
        {
            public float FadeTime;
            public bool IsFadeIn;
        }
        #endregion








        private void Start()
        {
            if (!audioSource && GetComponent<AudioSource>())
                audioSource = GetComponent<AudioSource>();
            if (!audioSource)
            {
                Debug.LogError($"{this} could not find an AudioSource. The Component will disable.");
                enabled = false;
            }
            else
                _queue = new Queue<FadeData>();
        }

        private void OnEnable()
        {
            if (fadeInOnEnable)
                FadeIn();
        }
        private void OnDisable() =>audioSource.volume = 0;


            #region Public Methods
        /// <summary>
        /// Will pause fading whether it's currently running or not. Will not pause audio.
        /// </summary>
        public void PauseFade() => _isPaused = true;                                                                                                               // PAUSE FADE
        
        /// <summary>
        /// Will unpause fading whehter it's currently running or not. Will not pause audio.
        /// </summary>
        public void UnPauseFade() => _isPaused = false;                                                                                                            // UNPAUSE FADE
        
        /// <summary>
        /// Will fade in the sound using the component <see cref="FadeTime"/>.
        /// </summary>
        public void FadeIn() => QueueFade(FadeTime, true);                                                                                                         // FADE IN
        
        /// <summary>
        /// An override version of <see cref="FadeIn"/> that uses the specified time rather than the components time.
        /// </summary>
        /// <param name="fadeTime">The time to use to fade the sound.</param>
        public void FadeIn(float fadeTime) => QueueFade(fadeTime, true);                                                                                            // FADE IN
        
        /// <summary>
        /// Will fade out the sound using the component <see cref="FadeTime"/>.
        /// </summary>
        public void FadeOut() => QueueFade(FadeTime, false);                                                                                                        // FADE OUT
        
        /// <summary>
        /// An override version of <see cref="FadeOut"/> that uses the specified time rather than the components time.
        /// </summary>
        /// <param name="fadeTime">The time to use to fade the sound.</param>
        public void FadeOut(float fadeTime) => QueueFade(fadeTime, false);                                                                                          // FADE OUT
        #endregion






        #region Private Methods
        /// <summary>
        /// Will either queue a fade if fading is already taking place or play it immediately.
        /// </summary>
        /// <param name="time">The time it should take to fade.</param>
        /// <param name="isFadeIn">Whether it's a fadeIn or not. Use false for a FadeOut.</param>
        private void QueueFade(float time, bool isFadeIn)
        {
            if (!_isFading)
            {
                _isFading = true;
                if (isFadeIn)
                    StartCoroutine(DoFadeIn(time));
                else
                    StartCoroutine(DoFadeOut(time));
            }
            else
                _queue.Enqueue(new FadeData(){FadeTime = time, IsFadeIn = isFadeIn});
        }

        /// <summary>
        /// Called by the Fade coroutines to see if more fades should be played after the previous one.
        /// </summary>
        private void TryPlayNextFade()
        {
            if (_queue.Count > 0)
            {
                FadeData data = _queue.Dequeue();
                if (data.IsFadeIn == true)
                    StartCoroutine(DoFadeIn(data.FadeTime));
                else
                    StartCoroutine(DoFadeOut(data.FadeTime));
            }
            else
                _isFading = false;
        }

        private IEnumerator DoFadeIn(float fadeTime)
        {
            OnFadeInStart?.Invoke(this, null);
            float time = 0f;
            float sourceVolumeStart = audioSource.volume;
            while (time < fadeTime)
            {
                if (_isPaused == false)
                {
                    time += Time.deltaTime;
                    float alpha = time / fadeTime;
                    float curveAlpha = FadeCurve.Evaluate(alpha);
                    audioSource.volume = Mathf.Lerp(sourceVolumeStart, VolumeFadeIn, curveAlpha);
                }
                yield return null;
            }
            audioSource.volume = VolumeFadeIn;
            OnFadeInEnd?.Invoke(this, null);
            TryPlayNextFade();
        }

        private IEnumerator DoFadeOut(float fadeTime)
        {
            OnFadeOutStart?.Invoke(this, null);
            float time = fadeTime;
            float sourceVolumeStart = audioSource.volume;
            while (time > 0f)
            {
                if (_isPaused == false)
                {
                    time -= Time.deltaTime;
                    float alpha = time / fadeTime;
                    float curveAlpha = FadeCurve.Evaluate(alpha);
                    audioSource.volume = Mathf.Lerp(VolumeFadeOut, sourceVolumeStart, curveAlpha);
                }
                yield return null;
            }
            audioSource.volume = VolumeFadeOut;
            OnFadeOutEnd?.Invoke(this, null);
            TryPlayNextFade();
        }
        #endregion






        #region EDITOR
        /// <summary>
        /// Automatically assign the audio source reference when the script is placed
        /// </summary>
        private void OnValidate()
        {
            if (!audioSource && GetComponent<AudioSource>())
            {
                AudioSource source = GetComponent<AudioSource>();
                if (source)
                    audioSource = source;
            }
        }
        #endregion
    }
}