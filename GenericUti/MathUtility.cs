using System.Collections.Generic;
using UnityEngine;

namespace PlugRMK.GenericUti
{
    public static class MathUtility
    {
        public struct Line
        {
            public Vector2 p1;
            public Vector2 p2;

            public Line(Vector2 p1, Vector2 p2)
            {
                this.p1 = p1;
                this.p2 = p2;
            }
        }
        public static Vector2? GetIntersection(Line lineA, Line lineB)
        {
            var x =
                ((lineA.p1.x * lineA.p2.y - lineA.p1.y * lineA.p2.x) * (lineB.p1.x - lineB.p2.x) - (lineA.p1.x - lineA.p2.x) * (lineB.p1.x * lineB.p2.y - lineB.p1.y * lineB.p2.x))
                /
                ((lineA.p1.x - lineA.p2.x) * (lineB.p1.y - lineB.p2.y) - (lineA.p1.y - lineA.p2.y) * (lineB.p1.x - lineB.p2.x));
            
            if (float.IsNaN(x))
                return null;

            var y =
                ((lineA.p1.x * lineA.p2.y - lineA.p1.y * lineA.p2.x) * (lineB.p1.y - lineB.p2.y) - (lineA.p1.y - lineA.p2.y) * (lineB.p1.x * lineB.p2.y - lineB.p1.y * lineB.p2.x))
                /
                ((lineA.p1.x - lineA.p2.x) * (lineB.p1.y - lineB.p2.y) - (lineA.p1.y - lineA.p2.y) * (lineB.p1.x - lineB.p2.x));

            if (float.IsNaN(y))
                return null;

            return new Vector2(x, y);
        }

        public static Vector2 ToVector2(this float angle)
        {
            return new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
        }

        public static Vector2 NewVector2(this float value)
        {
            return new(value, value);
        }

        public static Vector3 NewVector3(this float value)
        {
            return new(value, value, value);
        }

        public static float ToAngle(this Vector2 vector)
        {
            return Mathf.Atan2(vector.y, vector.x) * Mathf.Rad2Deg;
        }

        public static List<int> GetRatio(List<int> list)
        {
            var lowestInt = GetTheLowestNumber(list);
            if (lowestInt < 0) 
                return list;

            var listResult = new List<int>(list);

            for (int i = 2; i < lowestInt; i++)
            {
                if (CanAllNumbersBeDividedBy(listResult, i))
                {
                    DivideAllNumbersBy(listResult, i);
                    i--;
                }
            }

            return listResult;

            int GetTheLowestNumber(List<int> allNumbers)
            {
                int lowestInt = allNumbers[0];
                for (int i = 1; i < allNumbers.Count; i++)
                    if (allNumbers[i] < lowestInt)
                        lowestInt = allNumbers[i];

                return lowestInt;
            }

            bool CanAllNumbersBeDividedBy(List<int> allNumbers, int divider)
            {
                foreach (var num in allNumbers)
                    if (num % divider != 0)
                        return false;
                return true;
            }

            void DivideAllNumbersBy(List<int> allNumbers, int divider)
            {
                for (int n = 0; n < allNumbers.Count; n++)
                    allNumbers[n] = allNumbers[n] / divider;
            }

        }

        public static string SecondsToTimeString(float seconds)
        {
            int minutes = (int)seconds / 60; // calculate the number of minutes
            int sec = (int)seconds % 60; // calculate the number of seconds
            return $"{minutes:D2}:{sec:D2}"; // return the time string in the format "mm:ss"
        }

        public static float GetAngle(Vector2 from, Vector2 to)
        {
            var direction = to - from;
            return Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        }

        public static void CrossOp(out float xCurrent, float xTotal, float yCurrent, float yTotal)
        {
            xCurrent = xTotal * yCurrent / yTotal;
        }

        public static void CrossOp(float xCurrent, out float xTotal, float yCurrent, float yTotal)
        {
            xTotal = xCurrent * yTotal / yCurrent;
        }

        public static Vector2 Clamp(this Vector2 vector2, float min, float max)
        {
            return vector2.Clamp(min.NewVector2(), max.NewVector2());
        }

        public static Vector2 Clamp(this Vector2 vector2, Vector2 min, Vector2 max)
        {
            return new(
                Mathf.Clamp(vector2.x, min.x, max.x),
                Mathf.Clamp(vector2.y, min.y, max.y)
                );
        }

        public static float Between(float min, float max, float ratio)
        {
            ratio = Mathf.Clamp01(ratio);
            var distance = max - min;
            return min + distance * ratio;
        }

        public static Vector3 Between(Vector2 min, Vector2 max, float ratio)
        {
            return new(Between(min.x, max.x, ratio), Between(min.y, max.y, ratio));
        }

        public static Vector3 Between(Vector3 min, Vector3 max, float ratio)
        {
            return new(Between(min.x, max.x, ratio), Between(min.y, max.y, ratio), Between(min.z, max.z, ratio));
        }

        public static bool IsBetween(this int value, int min, int max)
        {
            return value >= min && value <= max;
        }

        public static bool IsBetween(this float value, float min, float max)
        {
            return value > min && value < max;
        }

        public enum DistanceCalcMode { XY, XZ, YZ, XYZ }
        public static float Distance(Vector3 a, Vector3 b, DistanceCalcMode mode)
        {
            float num, num2, num3;
            switch (mode)
            {
                case DistanceCalcMode.XY:
                    num = a.x - b.x;
                    num2 = a.y - b.y;
                    return (float)Mathf.Sqrt(num * num + num2 * num2);
                case DistanceCalcMode.XZ:
                    num = a.x - b.x;
                    num2 = a.z - b.z;
                    return (float)Mathf.Sqrt(num * num + num2 * num2);
                case DistanceCalcMode.YZ:
                    num = a.y - b.y;
                    num2 = a.z - b.z;
                    return (float)Mathf.Sqrt(num * num + num2 * num2);
                case DistanceCalcMode.XYZ:
                    num = a.x - b.x;
                    num2 = a.y - b.y;
                    num3 = a.z - b.z;
                    return (float)Mathf.Sqrt(num * num + num2 * num2 + num3 * num3);
                default:
                    return 0;
            }
        }
    }
}
