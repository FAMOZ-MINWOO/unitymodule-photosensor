using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FAMOZ.PhotoSensor
{
    public class PhotoSensorAction
    {
        protected PhotoSensorAction(PhotoSensorData _data)
        {
            SensorData = _data;
        }

        public static PhotoSensorAction CreateSensorAction(PhotoSensorData _data)
        {
            return new PhotoSensorAction(_data);
        }

        public readonly PhotoSensorData SensorData;

        public string GetSensorID => SensorData.SensorID;


        private readonly NotifyVariableChange<bool> sensorDetectNotify = new NotifyVariableChange<bool>();
        public bool IsSensorDetected => sensorDetectNotify.Value;
        public event NotifyVariableChange<bool>.OnVariableChangeDelegate OnSensorDetected
        {
            add
            {
                sensorDetectNotify.AddEvent(value);
            }
            remove
            {
                sensorDetectNotify.RemoveEvent(value);
            }
        }

        public void TriggeredSensor(KeyCode _key)
        {
            if (_key == SensorData.KeyForObjectIn)
            {
                sensorDetectNotify.Value = true;
            }
            else if (_key == SensorData.KeyForObjectOut)
            {
                sensorDetectNotify.Value = false;
            }
        }
    }
}
