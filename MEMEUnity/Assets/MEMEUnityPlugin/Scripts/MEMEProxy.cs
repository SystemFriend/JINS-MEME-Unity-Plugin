using UnityEngine;
using System.Runtime.InteropServices;
using System.Collections.Generic;

public class MEMEProxy : MonoBehaviour {

	public class MEMERealtimeData
	{
		public uint fitError;
		public uint isWalking;

		public uint powerLeft;

		public uint eyeMoveUp;
		public uint eyeMoveDown;
		public uint eyeMoveLeft;
		public uint eyeMoveRight;

		public uint blinkSpeed;
		public uint blinkStrength;

		public float roll;
		public float pitch;
		public float yaw;

		public int accX;
		public int accY;
		public int accZ;

		public bool isValid;
		public string raw;

		public MEMERealtimeData()
		{
			this.isValid = false;
			this.raw = string.Empty;
		}
	}

	public enum MEMERealTimeDataIndex
	{
		Pitch,
		Yaw,
		Roll,
		EyeMoveUp,
		EyeMoveDown,
		EyeMoveLeft,
		EyeMoveRight,
		BlinkSpeed,
		BlinkStrength,
		IsWalking,
		AccX,
		AccY,
		AccZ,
		FitError,
		PowerLeft
	}

    public virtual void Start()
    {
    }


	[DllImport("__Internal")]
	private static extern void MEMEStartSession(string appClientId, string appClientSecret);
	public static void StartSession(string appClientId, string appClientSecret) 
    {
		if (Application.platform == RuntimePlatform.IPhonePlayer) 
        {
			MEMEStartSession(appClientId, appClientSecret);
		}
	}
	
	[DllImport("__Internal")]
	private static extern void MEMEEndSession();
	public static void EndSession() 
    {
		if (Application.platform == RuntimePlatform.IPhonePlayer) 
        {
			MEMEEndSession();
		}
	}

	[DllImport("__Internal")]
	private static extern string MEMEGetSensorValues();
	public static MEMERealtimeData GetSensorValues() 
    {
		MEMERealtimeData result = new MEMERealtimeData ();
		if (Application.platform == RuntimePlatform.IPhonePlayer) 
        {
			var values = MEMEGetSensorValues();
			if (string.IsNullOrEmpty(values))
			{
				return result;
			}

			var parts = values.Split(',');
			if (values.Length > 0) {
				result.fitError = uint.Parse (parts [(uint)MEMERealTimeDataIndex.FitError]);
				result.isWalking = uint.Parse (parts [(uint)MEMERealTimeDataIndex.IsWalking]);
				result.powerLeft = uint.Parse (parts [(uint)MEMERealTimeDataIndex.PowerLeft]);
				result.eyeMoveUp = uint.Parse(parts[(uint)MEMERealTimeDataIndex.EyeMoveUp]);
				result.eyeMoveDown = uint.Parse(parts[(uint)MEMERealTimeDataIndex.EyeMoveDown]);
				result.eyeMoveLeft = uint.Parse(parts[(uint)MEMERealTimeDataIndex.EyeMoveLeft]);
				result.eyeMoveRight = uint.Parse(parts[(uint)MEMERealTimeDataIndex.EyeMoveRight]);
				result.blinkStrength = uint.Parse(parts[(uint)MEMERealTimeDataIndex.BlinkSpeed]);
				result.blinkStrength = uint.Parse(parts[(uint)MEMERealTimeDataIndex.BlinkStrength]);
				result.pitch = float.Parse (parts [(uint)MEMERealTimeDataIndex.Pitch]); 
				result.yaw = float.Parse (parts [(uint)MEMERealTimeDataIndex.Yaw]); 
				result.roll = float.Parse(parts[(uint)MEMERealTimeDataIndex.Roll]);
				result.accX = int.Parse(parts[(uint)MEMERealTimeDataIndex.AccX]);
				result.accY = int.Parse(parts[(uint)MEMERealTimeDataIndex.AccY]);
				result.accZ = int.Parse(parts[(uint)MEMERealTimeDataIndex.AccZ]);

				result.raw = values;
				result.isValid = true;
			}
		}
		return result;
	}
}
