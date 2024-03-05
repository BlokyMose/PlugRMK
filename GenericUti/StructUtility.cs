using System;
using System.Collections;
using UnityEngine;

namespace PlugRMK.GenericUti
{
    public enum BoolStatus { False, True, AsIs }

    public static class BoolStatusExtensions
    {
        public static bool GetBool(this BoolStatus status, bool defaultValue = false)
        {
            switch (status)
            {
                case BoolStatus.False: return false;
                case BoolStatus.True: return true;
                case BoolStatus.AsIs: return defaultValue;
                default: return defaultValue;
            }
        }
    }

    public struct Padding
    {
        public float left;
        public float top;
        public float right;
        public float bottom;

        public Padding(float left, float top, float right, float bottom)
        {
            this.left = left;
            this.top = top;
            this.right = right;
            this.bottom = bottom;
        }

        public float Horizontal => left + right;
        public float Vertical => top + bottom;
    }

    public enum Direction4 { Right, Left, Up, Down }

    [Flags]
    public enum FollowMode
    {
        None = 0,
        Position = 1,
        Rotation = 2,
        Scale = 4,
    }

    public struct EditorPos
    {
        float width;
        public float Width => width;
        float height;
        public float Height => height;

        public EditorPos(float width, float height)
        {
            this.width = width;
            this.height = height;
        }        
        
        public EditorPos(Vector2 vector2)
        {
            this.width = vector2.x;
            this.height = vector2.y;
        }

        /// <summary>Add width</summary>
        public void AddColumn(float add) => width += add;

        /// <summary>Add height</summary>
        public void AddRow(float add) => height += add;

        public Vector2 Vector2 => new Vector2(width, height);

    }

    public class EditorColors
    {
        public Color Color { get; private set; }
        public Color Content { get; private set; }
        public Color Background { get; private set; }
        public EditorColors()
        {

        }

        public EditorColors(Color color, Color content, Color background)
        {
            this.Color = color;
            this.Content = content;
            this.Background = background;
        }
    }

    public enum UnityInitialMethod { Awake, Start, OnEnable }

}