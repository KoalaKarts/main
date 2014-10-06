using UnityEngine;
using System.Collections;

public class KartWheelGeomController : MonoBehaviour 
{
    [System.Serializable]
    public class WheelGeometry
    {
        public Transform FL, FR, RL, RR;
    }
    public WheelGeometry Geometry;
    
    public void SwitchToKartMode()
    {
        Vector3 origAngle = new Vector3(90, 270, 0);

        Geometry.FL.localEulerAngles = origAngle;
        Geometry.FR.localEulerAngles = origAngle;
        Geometry.RL.localEulerAngles = origAngle;
        Geometry.RR.localEulerAngles = origAngle;
    }

    public void SwitchToHovercraftMode()
    {
        Vector3 newLAngle = new Vector3(180, 270, 0);
        Vector3 newRAngle = new Vector3(0, 270, 0);

        Geometry.FL.localEulerAngles = newLAngle;
        Geometry.FR.localEulerAngles = newRAngle;
        Geometry.RL.localEulerAngles = newLAngle;
        Geometry.RR.localEulerAngles = newRAngle;
    }

    public void SpinWheels(float amount)
    {
        Geometry.FL.Rotate(0, amount, 0);
        Geometry.FR.Rotate(0, amount, 0);
        Geometry.RL.Rotate(0, -amount, 0);
        Geometry.RR.Rotate(0, -amount, 0);
    }

    public void TurnFrontWheels(float flSteerAngle, float frSteerAngle)
    {
        Vector3 wheelFLTransAngle = Geometry.FL.localEulerAngles;
        Vector3 wheelFRTransAngle = Geometry.FR.localEulerAngles;
        wheelFLTransAngle.y = flSteerAngle + 90 - Geometry.FL.localEulerAngles.z;
        wheelFRTransAngle.y = frSteerAngle + 90 - Geometry.FR.localEulerAngles.z;
        Geometry.FL.localEulerAngles = wheelFLTransAngle;
        Geometry.FR.localEulerAngles = wheelFRTransAngle;
    }
}
