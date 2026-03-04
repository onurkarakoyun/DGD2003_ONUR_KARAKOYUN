using UnityEngine;

public class GimbalLockDemonstrater : MonoBehaviour
{

    public Transform outerRing;
    public Transform middleRing;
    public Transform innerRing;
    public Transform eulerTransform;

    [Header("Settings")]
    [Range(-180, 180)] public float outerRotation;
    [Range(-180, 180)] public float middleRotation;
    [Range(-180, 180)] public float innerRotation;
    
    void Update()
    {
        eulerTransform.eulerAngles = new Vector3(middleRotation, innerRotation, outerRotation);
        outerRing.localRotation = Quaternion.Euler(0, 0, outerRotation);
        middleRing.localRotation = Quaternion.Euler(middleRotation, 0, 0);
        innerRing.localRotation = Quaternion.Euler(0, innerRotation, 0);
    }
}
