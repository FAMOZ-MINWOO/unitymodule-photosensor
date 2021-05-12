using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace FAMOZ.PhotoSensor
{
    /// <summary>
    /// PhotoSensor에 대한 설정값으로, 포토센서에 들어오는 값은 변경이 쉽지 않습니다.
    /// 이에 따라서, 각 데이터의 중복여부는 SensorID를 기준으로만 판단하게 됩니다.
    /// </summary>
    [System.Serializable]
    [Newtonsoft.Json.JsonObject(MemberSerialization = Newtonsoft.Json.MemberSerialization.OptIn)]
    public struct PhotoSensorData : IEquatable<PhotoSensorData>
    {
        [Newtonsoft.Json.JsonProperty(nameof(SensorID))]
        [SerializeField] private string sensorID;
        public string SensorID => sensorID;


        [Newtonsoft.Json.JsonProperty(nameof(KeyForObjectIn))]
        [SerializeField] private KeyCode keyForObjectIn;
        public KeyCode KeyForObjectIn => keyForObjectIn;


        [Newtonsoft.Json.JsonProperty(nameof(KeyForObjectOut))]
        [SerializeField]
        private KeyCode keyForObjectOut;
        public KeyCode KeyForObjectOut => keyForObjectOut;

        public override bool Equals(object obj)
        {
            return Equals((PhotoSensorData)obj);
        }

        public bool Equals(PhotoSensorData other)
        {
            return SensorID.Equals(other.SensorID);
        }

        public override int GetHashCode()
        {
            return SensorID.GetHashCode();
        }
    }

    [Newtonsoft.Json.JsonObject(MemberSerialization = Newtonsoft.Json.MemberSerialization.OptIn)]
    public class PhotoSensorConfigure : SharedSettingsBase<PhotoSensorConfigure>
    {
        public const string DEFAULT_SETTING_FILE_NAME = "Photo Sensor Configure";

        public static readonly string DEFAULT_DIRECTORY_PATH = System.IO.Path.Combine(DEFAULT_ROOT_DIRECTORY, FAMOZ.Constants.FAMOZ_TAG);

        public static readonly string DEFAULT_FILE_FULL_PATH = System.IO.Path.Combine(DEFAULT_DIRECTORY_PATH, DEFAULT_SETTING_FILE_NAME);

        public static readonly string DEFAULT_JSON_FILE_PATH =
#if UNITY_EDITOR
            System.IO.Path.Combine(Application.streamingAssetsPath, Constants.FAMOZ_TAG, DEFAULT_SETTING_FILE_NAME);
#else
            System.IO.Path.Combine(Application.persistentDataPath, Constants.FAMOZ_TAG, DEFAULT_SETTING_FILE_NAME);
#endif

        public static PhotoSensorConfigure Instance
        {
            get
            {
                if(_instance == null)
                {
                    GetSettingInstanceData(DEFAULT_JSON_FILE_PATH, DEFAULT_FILE_FULL_PATH);
                }
                return _instance;
            }
        }

#if UNITY_EDITOR
        [UnityEditor.MenuItem(FAMOZ.Constants.FAMOZ_TAG + "/" + "Create " + DEFAULT_SETTING_FILE_NAME)]
        private static void CreateInstance()
        {
            CreateSharedSettingInstance(DEFAULT_FILE_FULL_PATH);
        } 
#endif


        public override bool OverwriteByJsonFile()
        {
            return OverwriteJson(this, DEFAULT_JSON_FILE_PATH);
        }

        public override void SaveToJsonFile(bool _overwrite = false)
        {
            SaveJsonDataToFile(this, DEFAULT_JSON_FILE_PATH, _overwrite);
        }



        [Newtonsoft.Json.JsonProperty(nameof(SensorDatas), ObjectCreationHandling = Newtonsoft.Json.ObjectCreationHandling.Replace)]
        [SerializeField] private List<PhotoSensorData> _sensorDatas;
        public List<PhotoSensorData> SensorDatas => _sensorDatas;

    }

}
