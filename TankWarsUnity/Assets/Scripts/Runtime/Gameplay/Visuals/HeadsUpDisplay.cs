namespace TankWars.Runtime.Gameplay.Visuals
{
    using System;
    using TankWars.Runtime.Gameplay.Player;
    using TankWars.Runtime.Core.Events; 
    using UnityEngine;
    using UnityEngine.UI;
    using TankWars.Runtime.Core.UI.Menus;
    using TankWars.Runtime.Core.UI.Buttons;
    using System.Collections;

    public class HeadsUpDisplay : BaseMenu, IEventListener
    {
        private const int IMAGE_FILL_ORIGIN_LEFT = 0;
        private const float IMAGE_FILL_VALUE_FILLED = 1f; 
        private const float GAME_OVER_MASSAGE_ANIMATION_DURATION = 5; 

        [SerializeField]
        private Image healthBarImage = null;

        [SerializeField]
        private Text pointsText = null;

        [SerializeField]
        private CustomButton pauseButton = null;

        [SerializeField]
        private GameObject gameOverMessage = null; 

        #region Unity Methods

        private void Start()
        {
            EventManager.Instance.Register(this, typeof(PlayerGameplayEvents)); 
            pauseButton.onButtonPressed += OnPuaseButtonPressed; 
        }

        private void OnDestroy()
        {
            EventManager.Instance.Unregister(this, typeof(PlayerGameplayEvents)); 
            pauseButton.onButtonPressed -= OnPuaseButtonPressed; 
        }

        private void OnValidate()
        {
            Debug.Assert(healthBarImage != null, $"{GetType().Name}-{gameObject.name}: You must assign an image component to the Health Bar Image field.");

            healthBarImage.type = Image.Type.Filled;
            healthBarImage.fillMethod = Image.FillMethod.Horizontal;
            healthBarImage.fillOrigin = IMAGE_FILL_ORIGIN_LEFT; 
        }

        #endregion

        public override void InitializeMenu()
        {
            base.InitializeMenu();
            gameOverMessage.SetActive(false);
            healthBarImage.fillAmount = IMAGE_FILL_VALUE_FILLED;
        }

        private void OnPuaseButtonPressed()
        {
            EventManager.Instance.Dispatch(GameplayEvent.OnGamePaused, null); 
        }

        private IEnumerator ShowGameOverMessage(PlayerGameplayInformation playerGameplayInformation)
        {
            gameOverMessage.SetActive(true);
            yield return new WaitForSeconds(GAME_OVER_MASSAGE_ANIMATION_DURATION);
            EventManager.Instance.Dispatch(GameplayEvent.OnGameOver, playerGameplayInformation); 
        }

        #region IEventListener

        public void OnEventReceived(IComparable eventType, object data)
        {
            switch(eventType)
            {
                case PlayerGameplayEvents playerGameplayEvent:
                    {
                        HandlePlayerGameplayEvents(playerGameplayEvent, data);
                        break;
                    }

                default:
                    {
                        Debug.LogError($"{GetType().Name}-{gameObject.name}: The object has received an unhandled type of event.");
                        break; 
                    }
            }
        }

        private void HandlePlayerGameplayEvents(PlayerGameplayEvents playerGameplayEvent, object data)
        {
            PlayerGameplayInformation playerGameplayInformation = (PlayerGameplayInformation)data;

            switch(playerGameplayEvent)
            {
                case PlayerGameplayEvents.HealthChange:
                    {
                        healthBarImage.fillAmount = playerGameplayInformation.Health / 100f;
                        break;
                    }

                case PlayerGameplayEvents.PointsChange:
                    {
                        pointsText.text = playerGameplayInformation.Points.ToString(); 
                        break;
                    }

                case PlayerGameplayEvents.HasDied:
                    {
                        StartCoroutine(ShowGameOverMessage(playerGameplayInformation)); 
                        break; 
                    }

                default:
                    {
                        break; 
                    }
            }
        }

        #endregion
    }
}
