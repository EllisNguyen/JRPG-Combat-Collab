///Author: Phab Nguyen.
///Description: 
///Important note: Please DO NOT modify this script.

using UnityEngine;
using UnityEngine.Events;
using UnityEditor;
using TMPro;

[CustomEditor(typeof(P_Button), true)]
[CanEditMultipleObjects]
public class P_Button_Editor : Editor
{

    public override void OnInspectorGUI()
    {
        //Set P_Button class as target for editor change
        P_Button _data = (P_Button)target;

        EditorGUILayout.PrefixLabel("BUTTON EFFECT", EditorStyles.boldLabel);

        _data.buttonAnimation = (P_Button.ButtonAnimation)EditorGUILayout.EnumPopup("Button Animation", _data.buttonAnimation);
        ///
        ///FADE THE BUTTON COLOR.
        ///
        if (_data.buttonAnimation == P_Button.ButtonAnimation.Fade)
        {
            //Display for Fade animation only
            _data.NormalGraphicColor = EditorGUILayout.ColorField("Normal color", _data.NormalGraphicColor);
            _data.HoverGraphicColor = EditorGUILayout.ColorField("Normal color", _data.HoverGraphicColor);
        }
        ///
        ///MOVE THE BUTTON TO ASSIGNED POSITION.
        ///
        else if (_data.buttonAnimation == P_Button.ButtonAnimation.Move)
        {
            EditorGUILayout.HelpBox("Animation type not yet implemented", MessageType.Warning);
        }
        ///
        ///SCALE THE BUTTON BY INCREASING ITS RECT SIZE.
        ///
        else if (_data.buttonAnimation == P_Button.ButtonAnimation.Scale)
        {
            _data.ScaleIncreamental = EditorGUILayout.Vector2Field("Additional scale", _data.ScaleIncreamental);
        }
        ///
        ///FADE AND MOVE AT THE SAME TIME.
        ///
        else if (_data.buttonAnimation == P_Button.ButtonAnimation.FadeAndMove)
        {
            EditorGUILayout.HelpBox("Animation type not yet implemented", MessageType.Warning);
        }
        ///
        ///FADE AND SCALE AT THE SAME TIME.
        ///
        else if (_data.buttonAnimation == P_Button.ButtonAnimation.FadeAndScale)
        {
            //FADE STUFF
            _data.NormalGraphicColor = EditorGUILayout.ColorField("Normal color", _data.NormalGraphicColor);
            _data.HoverGraphicColor = EditorGUILayout.ColorField("Hover color", _data.HoverGraphicColor);

            //SCALE STUFF
            _data.ScaleIncreamental = EditorGUILayout.Vector2Field("Additional scale", _data.ScaleIncreamental);
        }
        ///
        ///MOVE AND SCALE AT THE SAME TIME.
        ///
        else if (_data.buttonAnimation == P_Button.ButtonAnimation.MoveAndScale)
        {
            EditorGUILayout.HelpBox("Animation type not yet implemented", MessageType.Warning);
        }
        _data.AnimationTime = EditorGUILayout.FloatField("Animation duration", _data.AnimationTime);


        _data.UseText = EditorGUILayout.Toggle("Use Text", _data.UseText);
        if (_data.UseText == true)
        {
            EditorGUILayout.PrefixLabel("TEXT EFFECT", EditorStyles.boldLabel);
            _data.textEffect = (P_Button.TextEffect)EditorGUILayout.EnumPopup("Text Effect", _data.textEffect);
            if (_data.textEffect == P_Button.TextEffect.Fade)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PrefixLabel("Text");//Label for the text data
                _data.Text = (TextMeshProUGUI)EditorGUILayout.ObjectField(_data.Text, typeof(TextMeshProUGUI), allowSceneObjects: true);
                EditorGUILayout.EndHorizontal();

                //Display for Fade animation only
                _data.NormalTextColor = EditorGUILayout.ColorField("Normal color", _data.NormalTextColor);
                _data.HoverTextColor = EditorGUILayout.ColorField("Hover color", _data.HoverTextColor);
            }
        }
        this.serializedObject.Update();
        EditorGUILayout.PropertyField(this.serializedObject.FindProperty("onHoverEvent"), true);
        this.serializedObject.ApplyModifiedProperties();

        this.serializedObject.Update();
        EditorGUILayout.PropertyField(this.serializedObject.FindProperty("onClickEvent"), true);
        this.serializedObject.ApplyModifiedProperties();
    }

}
