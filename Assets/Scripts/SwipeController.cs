using UnityEngine;
namespace TechJuego.InputControl
{
	public class SwipeController : Singleton<SwipeController>
	{
		public delegate void OnAction();
		//Event To Subscribe
		public event OnAction OnSwipeRight;
		public event OnAction OnSwipeLeft;
		public event OnAction OnSwipeUp;
		public event OnAction OnSwipeDown;

		//Set Swipe Sensitivity
		float tSensitivity = 15;
		private float swipe_Initial_X, swipe_Final_X;
		private float swipe_Initial_Y, swipe_Final_Y;
		private int touchCount;
		private float present_Input_X, present_Input_Y;
		private float angle;
		private float swipe_Distance;
		/// <summary>
		/// Set Start Values
		/// </summary>
		void Start()
		{
			swipe_Initial_X = 0.0f;
			swipe_Initial_Y = 0.0f;
			swipe_Final_X = 0.0f;
			swipe_Final_Y = 0.0f;
			present_Input_X = 0.0f;
			present_Input_Y = 0.0f;
		}
		/// <summary>
		/// Get User Input
		/// </summary>
		void Update()
		{
			if (Input.GetKeyDown(KeyCode.Mouse0) && touchCount == 0)
			{
				swipe_Initial_X = Input.mousePosition.x;
				swipe_Initial_Y = Input.mousePosition.y;
				touchCount = 1;
			}
			if (touchCount == 1)
			{
				swipe_Final_X = Input.mousePosition.x;
				swipe_Final_Y = Input.mousePosition.y;
			}
			swipeDirection();

			if (Input.GetKeyUp(KeyCode.Mouse0))
			{
				touchCount = 0;
			}
		}
		/// <summary>
		/// Check Direction of swipe
		/// </summary>
		void swipeDirection()
		{

			if (touchCount != 1)
				return;
			present_Input_X = swipe_Final_X - swipe_Initial_X;
			present_Input_Y = swipe_Final_Y - swipe_Initial_Y;
			angle = present_Input_Y / present_Input_X;
			swipe_Distance = Mathf.Sqrt(Mathf.Pow((swipe_Final_Y - swipe_Initial_Y), 2) + Mathf.Pow((swipe_Final_X - swipe_Initial_X), 2));

			if (swipe_Distance <= (Screen.width / tSensitivity))
				return;

			if ((present_Input_X >= 0 || present_Input_X <= 0) && present_Input_Y > 0 && (angle > 1 || angle < -1))
			{
				//Swipe Up
				OnSwipeUp?.Invoke();
				touchCount = -1;
			}
			else
			if (present_Input_X > 0 && (present_Input_Y >= 0 || present_Input_Y <= 0) && (angle < 1 && angle >= 0 || angle > -1 && angle <= 0))
			{
				//Swipe Right
				OnSwipeRight?.Invoke();
				touchCount = -1;
			}
			else
			if (present_Input_X < 0 && (present_Input_Y >= 0 || present_Input_Y <= 0) && (angle > -1 && angle <= 0 || angle >= 0 && angle < 1))
			{
				//Swipe Left
				OnSwipeLeft?.Invoke();
				touchCount = -1;
			}
			else
			if ((present_Input_X >= 0 || present_Input_X <= 0) && present_Input_Y < 0 && (angle < -1 || angle > 1))
			{
				//Swipe Down
				OnSwipeDown?.Invoke();
				touchCount = -1;
			}
			else
			{
				touchCount = 0;
			}
		}
	}
}