using System.Collections.Generic;
using Common.Scripts.GoogleSheets;
using Cysharp.Threading.Tasks;
using SO.Scripts;
using UnityEngine;
using Zenject;

namespace Game.Scripts.Entetyes
{
    public class DialogsManager : ConfiguratedManager<Dialog, DialogsDTO>
    {
        protected override string SheetUrl => "https://docs.google.com/spreadsheets/d/e/2PACX-1vRgOkzycG2abPB1w6LScMWBTGLVAx62vSXtimAjeuA2t2Qp5KyRUScbnuoGO8m9H3hl0J2YRJEn3BTR/pub?gid=822432066&single=true&output=csv";
        private AvatarImageService _avatarImageService;

        [Inject]
        private void Construct(GoogleSheetsService googleSheetsService, AvatarImageService avatarImageService)
        {
            _avatarImageService = avatarImageService;
            base.Construct(googleSheetsService);
            Load().Forget();
        }
        
        protected override void InitData(List<DialogsDTO> data)
        {
            _items.Clear();
            foreach (var dto in data)
            {
               Create(dto);
            }
            Debug.Log($"{typeof(Dialog).Name} loaded: {_items.Count}");
            LoadCompleat?.Invoke(true);
        }

        protected override Dialog Create(DialogsDTO dto)
        {
            if (!_items.TryGetValue(dto.ID, out Dialog dialog))
            {
                dialog = new Dialog
                {
                    ID = dto.ID,
                    DialoguePersonageID = dto.DialoguePersonage_ID
                };
                if (dto.Icon_ID > -1)
                {
                    dialog.IconID = _avatarImageService.GeAvatar(dto.Icon_ID).Image;
                }

                dialog.DialogString = new Dictionary<int, DialogString>();
                _items.Add(dto.ID, dialog);
            }

            var st = CreateDialogString(dto);
            dialog.DialogString[dto.Sub_ID] = st;
            return dialog;
        }

        private DialogString CreateDialogString(DialogsDTO dto)
        {
            var answers = new List<Answer>(3);

            AddAnswer(answers, dto.Ansver_0, dto.Charisma_Ansver_0, dto.Result_0);
            AddAnswer(answers, dto.Ansver_1, dto.Charisma_Ansver_1, dto.Result_1);
            AddAnswer(answers, dto.Ansver_2, dto.Charisma_Ansver_2, dto.Result_2);

            var participants = new List<Participants>(3);

            AddParticipant(participants, dto.Personage_ID_0, dto.Count_0);
            AddParticipant(participants, dto.Personage_ID_1, dto.Count_1);
            AddParticipant(participants, dto.Personage_ID_2, dto.Count_2);

            return new DialogString
            {
                ID = dto.Sub_ID,
                SceneText = dto.Scene_Text,
                Result = (DialogResult)dto.Result,
                Text = dto.Text,
                Answers = answers,
                Participants = participants,
                LootID = dto.Loot,
                QuestID = dto.Quest
            };
        }

        private static void AddAnswer(List<Answer> answers, string text, int charisma, int result)
        {
            if (string.IsNullOrWhiteSpace(text)) return;
            answers.Add(new Answer
            {
                AnswerText = text,
                OpportunityCharisma = charisma,
                Result = (DialogResult)result
            });
        }

        private static void AddParticipant(List<Participants> participants, int personageId, int count)
        {
            if (personageId == -1) return;
            participants.Add(new Participants
            {
                ID = personageId,
                Counts = count
            });
        }
        
    }
}