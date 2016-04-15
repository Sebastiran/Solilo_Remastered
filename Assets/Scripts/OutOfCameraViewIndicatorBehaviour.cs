using UnityEngine;
using System.Collections;
using MxUnity;
using System;

public class OutOfCameraViewIndicatorBehaviour : MonoBehaviour
{
	public GameObject trackedObject;
	public float viewBorderPadding;

	[Header("Debug Monitor (Read-Only)")]
	public Camera trackedCamera;

	void Awake()
	{
		trackedCamera = Camera.main;
		transform.SetParent(null);
	}

	// Is performed intentionally during the normal update, because these are called in sync with the current frame rate.
	void Update()
	{
		Transform targetTransform;
		Transform cameraTransform;
		Plane trackedObjectDepthPlane;
		Ray cameraForwardRay;
		float cameraDistanceToDepthPlane;

		targetTransform = trackedObject.transform;
		cameraTransform = trackedCamera.transform;
		trackedObjectDepthPlane = new Plane(cameraTransform.forward, targetTransform.position);
		cameraForwardRay = new Ray(cameraTransform.position, cameraTransform.forward);

		// Perform raycast. Return early if the tracked object appears to be behind the camera.
		if (!trackedObjectDepthPlane.Raycast(cameraForwardRay, out cameraDistanceToDepthPlane))
			return;

		Vector3 cameraPositionOnDepthPlane;
		Ray cameraPlanePositionToTargetRay;
		Plane[] cameraViewFrustumPlanes;
		float? distanceToRayHit = null; // Distance to the point where 'cameraPlanePositionToTargetRay' has hit a frustum plane.

		cameraPositionOnDepthPlane = cameraTransform.position + cameraTransform.forward * cameraDistanceToDepthPlane;
		cameraPlanePositionToTargetRay = new Ray(cameraPositionOnDepthPlane, targetTransform.position - cameraPositionOnDepthPlane);
		cameraViewFrustumPlanes = GeometryUtility.CalculateFrustumPlanes(trackedCamera);

		foreach (Plane frustumPlane in cameraViewFrustumPlanes)
		{
			// Ignore planes of which their normals point towards or away from the camera. */
			if (frustumPlane.normal == cameraTransform.forward)
				continue;
			else if (frustumPlane.normal == -cameraTransform.forward)
				continue;

			float raycastResult;

			if (!frustumPlane.Raycast(cameraPlanePositionToTargetRay, out raycastResult))
				continue;

			if (distanceToRayHit == null || raycastResult < distanceToRayHit.Value)
				distanceToRayHit = raycastResult;
		}

		if (distanceToRayHit != null && (targetTransform.position - cameraPositionOnDepthPlane).magnitude > distanceToRayHit.Value)
			goto HandleViewFrustumIntersection;

		/** At this point, none of the frustum planes were hit by a raycast. */

		foreach (Renderer childRenderer in GetComponentsInChildren<Renderer>())
			childRenderer.enabled = false;

		return;

	HandleViewFrustumIntersection:
		/** At this point, one of the frustum planes was hit by a raycast. */

		// (Re-)enable child renderers.
		foreach (Renderer childRenderer in GetComponentsInChildren<Renderer>())
			childRenderer.enabled = true;

		Vector3 rayHitPoint;
		Vector3 rayHitNormal;
		Vector3 finalIndicatorPosition;
		Quaternion finalIndicatorRotation;

		rayHitPoint = cameraPlanePositionToTargetRay.origin + cameraPlanePositionToTargetRay.direction * distanceToRayHit.Value;
		rayHitNormal = -cameraPlanePositionToTargetRay.direction;
		finalIndicatorPosition = rayHitPoint + rayHitNormal * viewBorderPadding;
		finalIndicatorRotation = Quaternion.LookRotation(-rayHitNormal, -cameraTransform.forward);

		// Change this transform's state.
		transform.position = finalIndicatorPosition;
		transform.rotation = finalIndicatorRotation;
	}
}
