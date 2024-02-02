using PlugRMK.GenericUti;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;

namespace PlugRMK.UnityUti
{
    public static class TransformUtility
    {
        public static void SetPosX(this Transform transform, float x) => transform.position = new(x, transform.position.y, transform.position.z);
        public static void SetPosY(this Transform transform, float y) => transform.position = new(transform.position.x, y, transform.position.z);
        public static void SetPosZ(this Transform transform, float z) => transform.position = new(transform.position.x, transform.position.y, z);
        
        public static void IncrementPosX(this Transform transform, float increment) => transform.position = new(transform.position.x + increment, transform.position.y, transform.position.z);
        public static void IncrementPosY(this Transform transform, float increment) => transform.position = new(transform.position.x, transform.position.y + increment, transform.position.z);
        public static void IncrementPosZ(this Transform transform, float increment) => transform.position = new(transform.position.x, transform.position.y, transform.position.z + increment);
        
        public static void IncrementLocalPosX(this Transform transform, float increment) => transform.localPosition = new(transform.localPosition.x + increment, transform.localPosition.y, transform.localPosition.z);
        public static void IncrementLocalPosY(this Transform transform, float increment) => transform.localPosition = new(transform.localPosition.x, transform.localPosition.y + increment, transform.localPosition.z);
        public static void IncrementLocalPosZ(this Transform transform, float increment) => transform.localPosition = new(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z + increment);

        public static void SetLocalEulerX(this Transform transform, float x) => transform.localEulerAngles = new(x, transform.localEulerAngles.y, transform.localEulerAngles.z);
        public static void SetLocalEulerY(this Transform transform, float y) => transform.localEulerAngles = new(transform.localEulerAngles.x, y, transform.localEulerAngles.z);
        public static void SetLocalEulerZ(this Transform transform, float z) => transform.localEulerAngles = new(transform.localEulerAngles.x, transform.localEulerAngles.y, z);
        
        public static void IncrementLocalEulerX(this Transform transform, float increment) => transform.localEulerAngles = new(transform.localEulerAngles.x + increment, transform.localEulerAngles.y, transform.localEulerAngles.z);
        public static void IncrementLocalEulerY(this Transform transform, float increment) => transform.localEulerAngles = new(transform.localEulerAngles.x, transform.localEulerAngles.y + increment, transform.localEulerAngles.z);
        public static void IncrementLocalEulerZ(this Transform transform, float increment) => transform.localEulerAngles = new(transform.localEulerAngles.x, transform.localEulerAngles.y, transform.localEulerAngles.z + increment);

        public static void Look2D(this Transform transform, Vector2 target, float angleOffset = 0f) => transform.rotation = Quaternion.AngleAxis(MathUtility.GetAngle(transform.position, target) - angleOffset, Vector3.forward);
    }
}
