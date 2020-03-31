using UnityEngine;
using FrostweepGames.Plugins.Native;
using UnityEngine.UI;

namespace FrostweepGames.Plugins.WebGL.Examples
{
	/// <summary>
	/// not fully works and doesnt supported officially
	/// </summary>
	public class LeapSync_Example : MonoBehaviour
	{
		public AudioSource _audioSource;

		public Button startRecord,
					  stopRecord;

		private int _sampleRate = 44100;

		private int _recordingTime = 1;

#if UNITY_WEBGL && !UNITY_EDITOR

		private bool _isReadyToCreateClip;

		private bool _createdClip;

		private float _delay;

		private bool _playing;
#endif

		private void Start()
		{
			CustomMicrophone.RequestMicrophonePermission();

			startRecord.onClick.AddListener(StartRecordHandler);
			stopRecord.onClick.AddListener(StopRecordHandler);
		}

#if UNITY_WEBGL && !UNITY_EDITOR
		private void Update()
		{
			if (CustomMicrophone.IsRecording(string.Empty))
			{
				float[] array = new float[0];
				CustomMicrophone.GetRawData(ref array);

				if (array.Length > 0 && !_isReadyToCreateClip)
				{
					_isReadyToCreateClip = true;
				}

				if (_isReadyToCreateClip && !_createdClip)
				{
					_audioSource.clip = AudioClip.Create("BufferedClip", _sampleRate * _recordingTime, 1, _sampleRate, false);
					_audioSource.Play();
					_createdClip = true;

					_delay = _recordingTime;
					_playing = true;
				}

				if (_createdClip)
				{
					if (_playing)
					{
						_delay -= Time.deltaTime;

						if (_delay <= 0)
						{
							Debug.Log("stop play");
							_playing = false;
						}
					}

					if (!_playing)
					{
						_audioSource.clip.SetData(array, 0);
						_audioSource.Play();

						Debug.Log("PALY AGAIN");

						_delay = _recordingTime;
						_playing = true;
					}
				}
			}
		}

#endif
		private void StartRecordHandler()
		{
			if (!CustomMicrophone.HasConnectedMicrophoneDevices())
				return;

#if UNITY_EDITOR
			_audioSource.clip =
#endif
			CustomMicrophone.Start(CustomMicrophone.devices[0], true, _recordingTime, _sampleRate);
#if UNITY_EDITOR
			_audioSource.loop = true;
			_audioSource.Play();
#endif
		}

		private void StopRecordHandler()
		{
			if (!CustomMicrophone.HasConnectedMicrophoneDevices())
				return;

			CustomMicrophone.End(CustomMicrophone.devices[0]);
			_audioSource.Stop();
		}
	}
}