using UnityEngine;

namespace Bag.Scripts.Generic
{
	public enum InputState
	{
		NONE,
		Down,
		Pressed,
		Up
	}

	public class InputHandler : MonoBehaviour
	{
		public static InputHandler Singleton;

		public static Vector2 MousePos { get; private set; }
		public static Vector2 MouseScrollDelta { get; private set; }
		public static InputState Pan { get; private set; }

		float pinchDelta = 0;
		Vector2[] touchPossOld;
		bool wasPinching;

		void Awake()
		{
			if(!Singleton)
			{
				Singleton = this;
				Init();
			}
		}

		void Init()
		{
#if UNITY_IOS
		MousePos = new Vector2(0.5f, 0.5f);
#endif
		}

		void Update()
		{
#if UNITY_IOS
		TouchInput();
#else
			MouseInput();
#endif
		}

		void MouseInput()
		{
			MousePos = new Vector2(Input.mousePosition.x / Screen.width, Input.mousePosition.y / Screen.height);
			MouseScrollDelta = Input.mouseScrollDelta;

			if(Input.GetMouseButtonDown(2))
				Pan = InputState.Down;
			else if(Input.GetMouseButtonUp(2))
				Pan = InputState.Up;
			else if(Input.GetMouseButton(2))
				Pan = InputState.Pressed;
			else
				Pan = InputState.NONE;
		}

		void TouchInput()
		{
			Pan = InputState.NONE;
			switch(Input.touchCount)
			{
				case 2: // Zooming
					Vector2[] touchPoss = new Vector2[] { Input.GetTouch(0).position, Input.GetTouch(1).position };
					if(!wasPinching)
					{
						touchPossOld = touchPoss;
						wasPinching = true;
					}
					else
					{
						// Zoom based on the distance between the new positions compared to the distance between the previous positions.
						float newDistance = Vector2.Distance(touchPoss[0], touchPoss[1]);
						float oldDistance = Vector2.Distance(touchPossOld[0], touchPossOld[1]);
						MouseScrollDelta = new Vector2(0, -(newDistance - oldDistance));
						touchPossOld = touchPoss;
					}
					break;
				case 3:
					for(int i = 0; i < Input.touchCount; i++)
					{
						switch(Input.touches[i].phase)
						{
							case TouchPhase.Ended:
								Pan = InputState.Up;
								break;
							case TouchPhase.Began:
								Pan = InputState.Pressed;
								break;
							case TouchPhase.Moved:
								Pan = InputState.Pressed;
								break;
							case TouchPhase.Stationary:
								Pan = InputState.Pressed;
								break;
							default:
								Pan = InputState.NONE;
								break;
						}
					}
					break;
				default:
					break;
			}
			if(Input.touchCount != 2)
			{
				wasPinching = false;
				MouseScrollDelta = Vector2.zero;
			}
		}
	}
}
