using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FAMOZ.PhotoSensor
{
    [AddComponentMenu(Constants.FAMOZ_TAG + "/" + nameof(PhotoSensor) + "/" + "Photo Sensor Manager")]
    public class PhotoSensorManager : SingletonMonoBehaviour<PhotoSensorManager>
    {
        protected delegate void OnKeyCodeDown();

        protected PhotoSensorManager() { }

        protected static PhotoSensorConfigure Configure => PhotoSensorConfigure.Instance;

        protected readonly Dictionary<string, PhotoSensorAction> SensorActions = new Dictionary<string, PhotoSensorAction>();

        protected readonly Dictionary<KeyCode, OnKeyCodeDown> KeyDownAction = new Dictionary<KeyCode, OnKeyCodeDown>();

        public bool IsDetectRunning { get; protected set; }

        [SerializeField]
        private bool OnAwakeDetectStart = false;



        public void Awake()
        {
            if (OnAwakeDetectStart) { DetectedStart(); }

            foreach (var config in Configure.SensorDatas)
            {
                AddPhotoSensorAction(config);
            }
        }

        public void Update()
        {
            if (false == IsDetectRunning)
            {
                return;
            }

            if (Input.anyKeyDown)
            {
                foreach (var keycode in KeyDownAction)
                {
                    if (Input.GetKeyDown(keycode.Key))
                    {
                        keycode.Value?.Invoke();
                    }
                }
            }
        }

        /// <summary>
        /// <paramref name="sensorID"/>�� ������� �Ͽ�, ���εǾ��ִ� <see cref="PhotoSensorAction"/>�� ��û�մϴ�.
        /// </summary>
        /// <param name="sensorID"></param>
        /// <returns></returns>
        public PhotoSensorAction GetPhotoSensorAction(string sensorID)
        {
            if(false == SensorActions.TryGetValue(sensorID, out var temp))
            {
                return null;
            }

            return temp;
        }

        public void DetectedStart()
        {
            IsDetectRunning = true;
        }

        public void DetectedStop()
        {
            IsDetectRunning = false;
        }

        private void LoggingKeyDown(KeyCode _key)
        {
            Debug.Log($"{_key}�� �ԷµǾ����ϴ�.");
        }

        protected void AddKeyDownEvent(System.Action<KeyCode> _action, KeyCode _key)
        {
            if (false == KeyDownAction.ContainsKey(_key))
            {
                KeyDownAction.Add(_key, null);

                if(Configure.UseLog)
                {
                    KeyDownAction[_key] += () => LoggingKeyDown(_key);
                }

            }
            KeyDownAction[_key] += () => _action.Invoke(_key);
        }

        protected void RemoveKeyDownEvent(System.Action<KeyCode> _action, KeyCode _key)
        {
            if (false == KeyDownAction.ContainsKey(_key))
            {
                return;
            }
            KeyDownAction[_key] -= () => _action.Invoke(_key);
        }

        protected void RemoveKeyDownAction(KeyCode _key)
        {
            KeyDownAction[_key] = null;
            KeyDownAction.Remove(_key);
        }

        public bool AddPhotoSensorAction(PhotoSensorData data)
        {
            string sensorID = data.SensorID;
            string coloredSensorID = FAMOZ.DEBUG.DebugUtility.ColorizeString($"{sensorID}", Color.yellow);

            if (SensorActions.TryGetValue(sensorID, out var temp))
            {
                if (null != temp)
                {
                    Debug.LogError($"SensorID [{coloredSensorID}]�� �̹� �����Ǿ��ֽ��ϴ�.");
                    return false;
                }
                SensorActions.Remove(sensorID);
            }

            var action = PhotoSensorAction.CreateSensorAction(data);

            SensorActions.Add(data.SensorID, action);

#if UNITY_EDITOR || FAMOZ_DEBUG_LOG
            Debug.Log($"���ο� Sensor Action�� {coloredSensorID} Key�� ��ϵǾ����ϴ�." +
                    $"{action}"); 
#endif


            AddKeyDownEvent(action.TriggeredSensor, action.SensorData.KeyForObjectIn);
            AddKeyDownEvent(action.TriggeredSensor, action.SensorData.KeyForObjectOut);
            return true;
        }

        public void RemovePhotoSensorAction(string sensorID)
        {
            if (false == SensorActions.TryGetValue(sensorID, out var action))
            {
                return;
            }


            if (null != action)
            {
                RemoveKeyDownEvent(action.TriggeredSensor, action.SensorData.KeyForObjectIn);
                RemoveKeyDownEvent(action.TriggeredSensor, action.SensorData.KeyForObjectOut);
            }

            SensorActions.Remove(sensorID);
        }



    }

}