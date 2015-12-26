using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AppMEMEProxy : MEMEProxy
{
	// Please set your app's id and secret.
	private const string appClientId = "";
	private const string appClientSecret = "";

	private const float PositionMoveScale = 0.1f;

	public GameObject model;
	public Text message;

	void OnEnable()
	{
		MEMEProxy.StartSession(AppMEMEProxy.appClientId, AppMEMEProxy.appClientSecret);
	}
	
	void OnDisable()
	{
		MEMEProxy.EndSession();
	}

	private Vector3 orgRotation;
	private Vector3 orgPosition;
	public override void Start()
	{
		base.Start();
		this.orgRotation = this.model.transform.localRotation.eulerAngles;
		this.orgPosition = this.model.transform.position;
	}

	// Update is called once per frame
	void Update() 
	{
		var data = MEMEProxy.GetSensorValues ();
		if (!data.isValid)
		{
			return;
		}
		
		model.transform.localRotation = Quaternion.Euler(
			new Vector3(
				this.orgRotation.x + data.pitch, 
				this.orgRotation.y + data.yaw, 
				this.orgRotation.z + data.roll)
				);
		/* 眼球の動きなかなかうまく取れない(笑)

		var pos = model.transform.position;
		var up = data.eyeMoveUp;
		var down = data.eyeMoveDown;
		var left = data.eyeMoveLeft;
		var right = data.eyeMoveRight;
		if ((0 < up) && (up <= 2))
		{
			pos.y += PositionMoveScale;
		}
		if ((0 < down) && (down <= 2))
		{
			pos.y -= PositionMoveScale;
		}
		if ((0 < left) && (left <= 2))
		{
			pos.x  -= PositionMoveScale;
		}
		if ((0 < right) && (right <= 2))
		{
			pos.x += PositionMoveScale;
		}
		model.transform.position = pos;
		
		*/
		this.message.text = data.raw;
	}

	public void RestoreCenter()
	{
		this.model.transform.position = this.orgPosition;
	}
}