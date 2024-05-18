using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DialogueSyntax 
{
    [CreateAssetMenu(menuName = "SO/Dialogue/Dialogue Syntax Settings", fileName = "DialogueSyntaxSettings")]
    public class DSyntaxSettings : ScriptableObject
    {
        #region Classes

        [System.Serializable]
        public class StylingFromTo
        {
            public string oldOpenToken;
            public string oldCloseToken;
            public string newOpenToken;
            public string newCloseToken;

            public StylingFromTo(string oldOpenToken, string oldCloseToken, string newOpenToken, string newCloseToken)
            {
                this.oldOpenToken = oldOpenToken;
                this.oldCloseToken = oldCloseToken;
                this.newOpenToken = newOpenToken;
                this.newCloseToken = newCloseToken;
            }
        }


        #endregion

        public string TOKEN_BRANCH_OPENING = "<";
        public string TOKEN_BRANCH_CLOSING = ">";

        public string TOKEN_COMMAND_OPENING = "[";
        public string TOKEN_COMMAND_CLOSING = "]";

        public string TOKEN_COMMENT_OPENING = "/*";
        public string TOKEN_COMMENT_CLOSING = "*/";

        public string TOKEN_PARAMETER_OPENING = "{";
        public string TOKEN_PARAMETER_CLOSING = "}";
        public string TOKEN_PARAMETER_NAME = ":";

        [SerializeField]
        List<string> invalidTokensInBranchName = new List<string>() { "}", "=", "/" };

        [Header("Logic Tokens")]
        public string TOKEN_IS_EQUAL = "==";
        public string TOKEN_IS_NOT_EQUAL = "!=";
        public string TOKEN_IS_GREATER_THAN = ">";
        public string TOKEN_IS_LESS_THAN = "<";
        public string TOKEN_IS_GREATER_OR_EQUAL = ">=";
        public string TOKEN_IS_LESS_OR_EQUAL = "<=";

        [Header("Operation Tokens")]
        public string TOKEN_EQUAL = "=";
        public string TOKEN_INCREMENT = "+=";
        public string TOKEN_DECREMENT = "-=";
        public string TOKEN_MULTIPLICATION = "*=";
        public string TOKEN_DIVISION = "/=";

        // When adding new command names, add it to GetCommandNames() as well
        public string COMMAND_BRANCH = "BRANCH";
        public string COMMAND_CHOICES = "CHOICES";
        public string COMMAND_URGENT = "URGENT";
        public string COMMAND_GOTO = "GOTO";
        public string COMMAND_IF = "IF";
        public string COMMAND_SET = "SET";
        public string COMMAND_ONCE = "ONCE";
        public string START = "START";
        public string PASS = "PASS";

        public StylingFromTo italic = new StylingFromTo("*", "*", "<i>", "</i>");
        public StylingFromTo bold = new StylingFromTo("**", "**", "<b>", "</b>");
        public StylingFromTo underline = new StylingFromTo("__", "__", "<u>", "</u>");
        public StylingFromTo strikethrough = new StylingFromTo("--", "--", "<s>", "</s>");

        public string EXPRESSION_LISTENING = "F0-9F-A4-94"; // thinking face
        public string EXPRESSION_SAD = "F0-9F-99-81"; // slightly frowning face
        public string EXPRESSION_HAPPY_BIT = "F0-9F-99-82"; // slightly smiling face
        public string EXPRESSION_HAPPY = "F0-9F-98-81"; // beaming face with smiling eyes

        public string EXPRESSION_CONFUSED = "F0-9F-98-95"; // confused face
        public string EXPRESSION_SURPRISED = "F0-9F-98-AE"; // face with open mouth
        public string EXPRESSION_ANGRY = "F0-9F-98-A0"; // angry face
        public string EXPRESSION_UNTRUST = "F0-9F-A4-A8"; // face with raised eyebrow

        public string GESTURE_SPEAKING = "speaking";
        public string GESTURE_NOD = "nod";
        public string GESTURE_THINKING = "thinking";
        public string GESTURE_PONDERING = "pondering";

        public string GESTURE_THIS = "this";
        public string GESTURE_NOIDEA = "noIdea";
        public string GESTURE_LEANBACK = "leanBack";


        public List<string> GetCommandNames()
        {
            return new List<string>()
            {
                COMMAND_BRANCH,
                COMMAND_CHOICES,
                COMMAND_URGENT,
                COMMAND_GOTO,
                COMMAND_IF,
                COMMAND_SET,
                COMMAND_ONCE,
                START,
                PASS,
            };
        }
    }
}
