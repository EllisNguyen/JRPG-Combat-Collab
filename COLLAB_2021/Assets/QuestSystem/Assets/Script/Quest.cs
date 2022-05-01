using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;
using UnityEditor;
using UnityEngine.UI;


    
    
public class Quest : ScriptableObject
{
    //check if the button is clicked or not so the player can not click dropdown again so it not bug again
    public bool HaveClicked = false;
    
    [System.Serializable]
    
    public struct Info
    {
        //Name of the quest
        public string Name;
        //The drop down icon
        public Sprite Icon;
        //The Description of the quest
        public string Description;
        
    }
    [Header("Info")] public Info Infomation;

    [System.Serializable]
    
    public struct Stat
    {
        //How many XP they can get 
        public int XP;
       
    }
    [Header("Reward")] public Stat Reward = new Stat { XP = 10 };

    //check if the mission is completed  or not
    public bool Completed { get; protected set; }
    //quest completed event 
    public QuestCompletedEvent QuestCompleted;
   
    public abstract class QuestGoal:ScriptableObject
    {
        //get the description
        protected string Description;
        //get the current amount in the QuestGoal
        public int CurrentAmount { get; protected set; }
        //set the amount of the quest in hierarchy 
        public int RequiredAmount = 1;
        //get it complete  or not 
        public bool Completed { get; protected set; }
        [HideInInspector] public UnityEvent GoalCompleted;
        public virtual string GetDescription()
        {
            return Description;
        }
        public virtual void Initilize()
        {
            for (var i = 0; i<6; i++) ; //Error messages on this line
            {
                
            }
            //when initialize set complete to false 
            Completed = false;
            
            GoalCompleted = new UnityEvent();
        }
        protected void Evaluate()
        {
            if(CurrentAmount>= RequiredAmount)
            {
                //if the current amount more than required amount then is complete
                Complete(); 
            }
        }
        private void Complete()
        {
            //set complete to true
            Completed = true;
            GoalCompleted.Invoke();
            //remove all the listener
            GoalCompleted.RemoveAllListeners();
        }
        public void Skip()
        {
            //if we click the skip button it will complete
            Complete();
        }
        

    }
    public List<QuestGoal> Goals;
    public void Initialize()
    {
        Completed = false;
        QuestCompleted = new QuestCompletedEvent();
        foreach(var goal in Goals)
        {
            goal.Initilize();
            goal.GoalCompleted.AddListener(delegate { CheckGoals(); });
        }
    }
    private void CheckGoals()
    {
        Completed = Goals.All(g => g.Completed);
        if(Completed)
        {
            //reward
            QuestCompleted.Invoke(this);
            QuestCompleted.RemoveAllListeners();
        }
    }
}
public class QuestCompletedEvent : UnityEvent<Quest> { }
#if UNITY_EDITOR
[CustomEditor(typeof(Quest))]
public class QuestEditor : Editor
{

    SerializedProperty m_QuestInfoProperty;
    SerializedProperty m_QuestStatProperty;
    List<string> m_QuestGoalType;
    SerializedProperty m_QuestGoalListProperty;
    [MenuItem("Assets/Quest", priority = 0)]
    public static void CreateQuest()
    {
        var newQuest = CreateInstance<Quest>();
        ProjectWindowUtil.CreateAsset(newQuest, "quest.asset");
    }
    void OnEnable()
    {
        m_QuestInfoProperty = serializedObject.FindProperty(nameof(Quest.Infomation));
        m_QuestStatProperty = serializedObject.FindProperty(nameof(Quest.Reward));
        m_QuestGoalListProperty = serializedObject.FindProperty(nameof(Quest.Goals));
        var lookup = typeof(Quest.QuestGoal);
        m_QuestGoalType = System.AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(assembly => assembly.GetTypes())
            .Where(x => x.IsClass && !x.IsAbstract && x.IsSubclassOf(lookup))
            .Select(type => type.Name)
            .ToList();
    }
    public override void OnInspectorGUI()
    {
        var child = m_QuestInfoProperty.Copy();
        var depth = child.depth;
        child.NextVisible(true);
        EditorGUILayout.LabelField("QuestInfo", EditorStyles.boldLabel);
        while (child.depth > depth)
        {
            EditorGUILayout.PropertyField(child, true);
            child.NextVisible(false);
        }
        child = m_QuestStatProperty.Copy();
        depth = child.depth;
        child.NextVisible(true);
        EditorGUILayout.LabelField("Quest reward", EditorStyles.boldLabel);
        while (child.depth > depth)
        {
            EditorGUILayout.PropertyField(child, true);
            child.NextVisible(false);
        }
        int choice = EditorGUILayout.Popup("Add new Quest Goal", -1, m_QuestGoalType.ToArray());
        if(choice!=-1)
        {
            var newInstance = ScriptableObject.CreateInstance(m_QuestGoalType[choice]);
            AssetDatabase.AddObjectToAsset(newInstance, target);
            m_QuestGoalListProperty.InsertArrayElementAtIndex(m_QuestGoalListProperty.arraySize);
            m_QuestGoalListProperty.GetArrayElementAtIndex(m_QuestGoalListProperty.arraySize - 1)
                .objectReferenceValue = newInstance;

        }
        Editor ed = null;
        int toDelete = -1;
        for(int i=0;i<m_QuestGoalListProperty.arraySize;++i)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.BeginVertical();
            var item = m_QuestGoalListProperty.GetArrayElementAtIndex(i);
            SerializedObject obj = new SerializedObject(item.objectReferenceValue);
            Editor.CreateCachedEditor(item.objectReferenceValue, null, ref ed);
            ed.OnInspectorGUI();
            EditorGUILayout.EndVertical();
            if(GUILayout.Button("-",GUILayout.Width(32)))
            {
                toDelete = i;
            }
            EditorGUILayout.EndHorizontal();
            

        }
        if(toDelete!=-1)
        {
            var item = m_QuestGoalListProperty.GetArrayElementAtIndex(toDelete).objectReferenceValue;
            DestroyImmediate(item, true);
            m_QuestGoalListProperty.DeleteArrayElementAtIndex(toDelete);
            m_QuestGoalListProperty.DeleteArrayElementAtIndex(toDelete);
        }
        serializedObject.ApplyModifiedProperties();
    }

}
#endif


