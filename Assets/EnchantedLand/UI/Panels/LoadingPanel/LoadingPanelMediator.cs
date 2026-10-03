using System;
using Common.Scripts.GoogleSheets;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Util;
using Zenject;

namespace UI.Panels
{
    public class LoadingPanelMediator :BasePanelMediator <LoadingPanelView, UIData>
    {
        private SceneLoader _sceneLoader;
        private GoogleSheetsService _googleSheetsService;
        
        [Inject]
        private void Construct(SceneLoader sceneLoader, GoogleSheetsService googleSheetsService)
        {
            _sceneLoader = sceneLoader; 
            _googleSheetsService = googleSheetsService;
        }
		
        protected override void ShowStart()
        {
            base.ShowStart();
            Target.StartButton.onClick.AddListener(OnStartButtonClicked);
            Target.StartButton.gameObject.SetActive(false);
            Target.ProgressSlider.value = 0;
            //_googleSheetsService.LoadSheet(GoogleSheetsURL.Tutorial);

            StartProgress().Forget();
        }

        private async UniTask StartProgress()
        {
            while (Target.ProgressSlider.value < 1)
            {
                Target.ProgressSlider.value += 0.05f;
                await UniTask.Delay(TimeSpan.FromSeconds(0.05));
                
            }
            Target.StartButton.gameObject.SetActive(true);
        }

        private async void OnStartButtonClicked()
        {
            var result = await _sceneLoader.LoadScene(SceneName.Lobby.ToString(),
                new Progress<float>(value =>
                {
                    Debug.Log($"Loading: {value:P0}");
                })
            );

            if (result)
            {
                CloseSelf();
            }
        }
    }
}