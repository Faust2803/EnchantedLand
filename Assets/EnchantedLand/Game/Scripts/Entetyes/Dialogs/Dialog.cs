using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Scripts.Entetyes
{
    public class Dialog : Configurated
    {
        public int ID { get; set; }
        public Sprite IconID;
        public int DialoguePersonageID;
        
        public Dictionary<int, DialogString> DialogString;
    }

    public class DialogString
    {
        public int ID;
        public string SceneText;
        public DialogResult Result;
        public List<Participants> Participants;
        public string Text;
        public List<Answer> Answers;
        public int QuestID;
        public int LootID;
    }

    public class Answer
    {
        public string AnswerText;
        public int OpportunityCharisma;
        public DialogResult Result;
    }

    public class Participants
    {
        public int ID;
        public int Counts;
    }
    

    public enum DialogResult
    {
        Start     = 0,
        Finished  = -1,
        Battle    = -2,
        Continue  = -3,
        NextStep  = 1,
        NextStep1 = 2,
        NextStep2 = 3,
        NextStep3 = 4,
        NextStep4 = 5,
        NextStep5 = 6,
        NextStep6 = 7,
        NextStep7 = 8,
        NextStep8 = 9,
        NextStep9 = 10,
        NextStep10 = 11,
        NextStep11 = 12,
        NextStep12 = 13,
        NextStep13 = 14,
        NextStep15 = 15,
    }
    
    [Serializable]
    public class DialogsDTO 
    {
        public int ID;
        public int Sub_ID;

        public int Icon_ID;
        public string Scene_Text;
        public int Result;
        
        public int Personage_ID_0;
        public int Count_0;
        public int Personage_ID_1;
        public int Count_1;
        public int Personage_ID_2;
        public int Count_2;
        
        public int DialoguePersonage_ID;
        public string Text;
        
        public string Ansver_0;
        public int Charisma_Ansver_0;
        public int Result_0;
        public string Ansver_1;
        public int Charisma_Ansver_1;
        public int Result_1;
        public string Ansver_2;
        public int Charisma_Ansver_2;
        public int Result_2;
        
        public int Quest;
        public int Loot;
    }
}