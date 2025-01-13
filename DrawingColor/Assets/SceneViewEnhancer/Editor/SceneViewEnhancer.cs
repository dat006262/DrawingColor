using UnityEditor;
using UnityEngine;

[InitializeOnLoad] public class SceneViewEnhancer
{
	#region Inspector Variables

	#endregion

	#region Member Variables

	private static Vector3[] cardinalDirections =
	{
			Vector3.forward, // Front
			Vector3.back,    // Back
			Vector3.left,    // Left
			Vector3.right,   // Right
			Vector3.up,      // Top
			Vector3.down     // Bottom
	};
	
	private static Quaternion previousRotation;
	private static bool       previousOrthographicMode;
	private static bool       isEnterOrthographicMode;

	#endregion

	#region Properties

	#endregion

	#region Unity Methods

	static SceneViewEnhancer()
	{
		SceneView.duringSceneGui += OnSceneGUI;
	}

	private static void OnSceneGUI(SceneView sceneView)
	{
		var e = Event.current;
		if (e.type == EventType.KeyDown && e.keyCode == KeyCode.Alpha2)
		{
			ToggleViewMode();
			e.Use(); // Use the event so it's not processed further
		}
	}

	#endregion

	#region Public Methods

	#endregion

	#region Protected Methods

	#endregion

	#region Private Methods
	
	private static void ToggleViewMode()
	{
		if (!isEnterOrthographicMode)
			AlignToNearestCardinalDirection();
		else
			AlignToPreviousDirection();
	}

	private static void AlignToNearestCardinalDirection()
	{
		var sceneView = SceneView.lastActiveSceneView;

		// Store the current state before switching
		previousRotation         = sceneView.camera.transform.rotation;
		previousOrthographicMode = sceneView.orthographic;

		var closestCurrentDirection = GetClosestDirection(sceneView.camera.transform.forward);
		var closestRightDirection   = GetClosestDirection(sceneView.camera.transform.right);
		var newRotation             = Quaternion.LookRotation(closestCurrentDirection, Vector3.up);

		// Ensure the up direction is correctly oriented for top and bottom views
		if (closestCurrentDirection == Vector3.up || closestCurrentDirection == Vector3.down)
		{
			var right = Vector3.Cross(closestCurrentDirection, closestRightDirection);
			newRotation = Quaternion.LookRotation(closestCurrentDirection, right);
		}

		sceneView.LookAt(sceneView.pivot, newRotation, sceneView.size, true);
		isEnterOrthographicMode = true;
	}

	private static void AlignToPreviousDirection()
	{
		var sceneView = SceneView.lastActiveSceneView;

		// Restore the previous state
		sceneView.LookAt(sceneView.pivot, previousRotation, sceneView.size, previousOrthographicMode);
		isEnterOrthographicMode = false;
	}

	private static Vector3 GetClosestDirection(Vector3 currentDirection)
	{
		var maxDot           = -Mathf.Infinity;
		var closestDirection = Vector3.zero;

		foreach (var direction in cardinalDirections)
		{
			var dot = Vector3.Dot(currentDirection, direction);
			if (dot > maxDot)
			{
				maxDot           = dot;
				closestDirection = direction;
			}
		}

		return closestDirection;
	}

	#endregion
}