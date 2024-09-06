using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "QuestMap", menuName = "Quests", order = 0)]
public class ScrObjQuestMap : ScriptableObject
{
    [Header("Установить первый гланый квест")]
    [SerializeField] private Assignment[] _firstMainAssignment;

    public class AssignmentData
    {
        public string NameAssign;
        public string NameParent;
    }
    public void LoadAssignment(Assignment[] map)
    {
        _firstMainAssignment = ( Assignment[] ) map.Clone();
    }
}
