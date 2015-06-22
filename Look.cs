using UnityEngine;

[AddComponentMenu("Camera-Control/Mouse Orbit with zoom")]
public class Look : MonoBehaviour
{

    public Transform Target;
    public Vector3 Offset;
    public float Distance = 5.0f;
    public float SpeedX = 120.0f;
    public float SpeedY = 120.0f;

    public float MinLimitY = -20f;
    public float MaxLimitY = 80f;

    public float DistanceMin = .5f;
    public float DistanceMax = 15f;
	public float DistancePrefered = 6f;

    private Rigidbody _rigidbody;

    float x = 0.0f;
    float y = 0.0f;
	
    void Start()
    {
        Vector3 angles = transform.eulerAngles;
        x = angles.y;
        y = angles.x;

        _rigidbody = GetComponent<Rigidbody>();

        // Make the rigid body not change rotation
        if (_rigidbody != null)
        {
            _rigidbody.freezeRotation = true;
        }
    }

    void LateUpdate()
    {
        if (Target)
        {
            x += UnityEngine.Input.GetAxis("Mouse X") * SpeedX * Distance * 0.02f;
            y -= UnityEngine.Input.GetAxis("Mouse Y") * SpeedY * 0.02f;

            y = ClampAngle(y, MinLimitY, MaxLimitY);

            Quaternion rotation = Quaternion.Euler(y, x, 0);

			Vector3 direction = transform.position - Target.position;
			direction.Normalize();

			/// Clamp the prefered distance. Just a safetything to get rid of
			/// unwanted behaviour.
			DistancePrefered = Mathf.Clamp(DistancePrefered - UnityEngine.Input.GetAxis("Mouse ScrollWheel") * 5, DistanceMin, DistanceMax);

			RaycastHit hit;
			if(Physics.Linecast(Target.position, Target.position + direction * DistancePrefered, out hit))
			{
				Distance = DistancePrefered + (hit.distance - DistancePrefered);
			}
			else
			{
				Distance = DistancePrefered;
			}
			
			Vector3 negDistance = new Vector3(0.0f, 0.0f, -Distance);
            Vector3 position = rotation * negDistance + Target.position + Offset;

            transform.rotation = rotation;
            transform.position = position;
        }
    }

    public static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360F)
            angle += 360F;
        if (angle > 360F)
            angle -= 360F;
        return Mathf.Clamp(angle, min, max);
    }
}