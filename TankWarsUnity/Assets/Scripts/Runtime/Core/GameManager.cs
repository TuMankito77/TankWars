namespace TankWars.Runtime.Core
{
    using TankWars.Runtime.Core.UI;
    using TankWars.Runtime.Gameplay;
    using TankWars.Runtime.Core.Tools;
    using TankWars.Runtime.Core.Events;
    using TankWars.Runtime.Core.Databases;
    using TankWars.Runtime.Core.UI.Buttons;
    using TankWars.Runtime.Gameplay.Levels; 
    using TankWars.Runtime.Gameplay.Player;
    using TankWars.Runtime.Core.Tools.Scenes;
    using TankWars.Runtime.Gameplay.Vehicles;
    using TankWars.Runtime.Core.ManagerSystem;
    using TankWars.Runtime.Core.StorageSystem;
    using TankWars.Runtime.Gameplay.Unlockables;
    using TankWars.Runtime.Gameplay.StorableClasses;
    using UnityEngine;
    using UnityEngine.SceneManagement;
    using System;

    public class GameManager : BaseManager, IEventListener
    {
        private TankInfoContainer[] tankInfoContainers = null;
        private LevelIdDatabase levelIdDatabase = null;
        private SceneLoader sceneLoader = null;
        private GameInformation gameInformation = null;
        private PlayerInformation playerInformation = null;
        private PlayerGameplayInformation playerGameplayInformation = null;
        private StorageAccessor storageAccessor = null;
        private LevelId currentLevelIdLoaded = LevelId.NONE;

        public GameInformation GameInformation => gameInformation;
        public PlayerInformation PlayerInformation => playerInformation;
        public LevelId CurrentLevelIdLoaded => currentLevelIdLoaded; 

        private DatabaseManager DatabaseManager => CoreManagers.Instance.GetManager<DatabaseManager>(); 

        #region Unity Methods

        private void Start()
        {
            EventManager.Instance.Register(this, typeof(ButtonId), typeof(GameplayEvent));
        }

        private void OnDestroy()
        {
            EventManager.Instance.Unregister(this, typeof(ButtonId), typeof(GameplayEvent));
        }

        #endregion

        public override void Init()
        {
            levelIdDatabase = DatabaseManager.GetDatabase<LevelIdDatabase>();
            tankInfoContainers = DatabaseManager.GetDatabase<TankInfoContainerDatabase>().TankInfoContainers;
            sceneLoader = new SceneLoader();
            storageAccessor = new StorageAccessor();

            if (storageAccessor.DoesInformationExist(PlayerInformation.STORABLE_KEY))
            {
                playerInformation = storageAccessor.Load<PlayerInformation>(PlayerInformation.STORABLE_KEY);
            }
            else
            {
                playerInformation = new PlayerInformation();
                storageAccessor.Save(playerInformation);
            }

            if (storageAccessor.DoesInformationExist(GameInformation.STORABLE_KEY))
            {
                gameInformation = storageAccessor.Load<GameInformation>(GameInformation.STORABLE_KEY);
            }
            else
            {
                gameInformation = new GameInformation(levelIdDatabase);
                storageAccessor.Save(gameInformation);
            }
        }

        public TankInfoContainer GetCurrentTankInfoContainer()
        {
            return tankInfoContainers[playerInformation.TankInfoContaierIndex];
        }

        public int GetCurrentTankInfoContainerIndex()
        {
            return playerInformation.TankInfoContaierIndex;
        }

        public void SaveSelectedTankInfoConatainer(int tankInfoContainerIndex)
        {
            PlayerInformation playerInformation = new PlayerInformation();
        }

        private void LoadLevel(LevelId levelId)
        {
            AsyncOperation loadLevelAsyncOperation = null;

            sceneLoader.LoadScene(levelIdDatabase.GetSceneName(levelId), out loadLevelAsyncOperation,
                OnSceneLoaded: (asyncOpertion) =>
                {
                    currentLevelIdLoaded = levelId;
                    SceneManager.SetActiveScene(levelIdDatabase.GetScene(levelId));
                    EventManager.Instance.Dispatch(GameplayEvent.OnLoadingOperationFished, levelId);
                },
                OnSceneLoadedFailure: () =>
                {
                    Debug.LogError($"{gameObject.name} - {GetType().Name}The scene {levelId} could not be loaded, please make sure that it has been added to the list of scenes in the build settings and that the name is correct.");
                });

            AsyncOperationsHandler asyncOperationsHandler = new AsyncOperationsHandler(loadLevelAsyncOperation);
            EventManager.Instance.Dispatch(GameplayEvent.OnLoadingOperationStarted, asyncOperationsHandler);
        }

        private void UnloadLevel(LevelId levelId)
        {
            AsyncOperation unloadLevelAsyncOperation = null;

            sceneLoader.UnloadScene(levelIdDatabase.GetSceneName(levelId), out unloadLevelAsyncOperation,
                OnSceneUnloaded: (AsyncOperation) =>
                {
                    currentLevelIdLoaded = LevelId.NONE;
                    SceneManager.SetActiveScene(levelIdDatabase.GetScene(LevelId.LAUNCHER));
                    EventManager.Instance.Dispatch(GameplayEvent.OnLoadingOperationFished, currentLevelIdLoaded);
                },
                OnSceneUnloadedFailre: () =>
                {
                    Debug.LogError($"{gameObject.name} - {GetType().Name}The scene {levelId} could not be unloaded, please make sure that it is loaded at the moment.");
                });

            AsyncOperationsHandler asyncOperationsHandler = new AsyncOperationsHandler(unloadLevelAsyncOperation);
            EventManager.Instance.Dispatch(GameplayEvent.OnLoadingOperationStarted, asyncOperationsHandler);
        }

        private void ReloadCurrentLevel()
        {
            AsyncOperation unloadLevelAsyncOperation = null;
            AsyncOperation loadLevelAsyncOperation = null;
            bool isEitherUnloadOrLoadDone = false;

            sceneLoader.UnloadScene(levelIdDatabase.GetSceneName(currentLevelIdLoaded), out unloadLevelAsyncOperation,
                OnSceneUnloaded: (asyncOperation) =>
                {
                    if (isEitherUnloadOrLoadDone)
                    {
                        EventManager.Instance.Dispatch(GameplayEvent.OnLoadingOperationFished, currentLevelIdLoaded);
                    }
                    else
                    {
                        isEitherUnloadOrLoadDone = true;
                    }
                });

            sceneLoader.LoadScene(levelIdDatabase.GetSceneName(currentLevelIdLoaded), out loadLevelAsyncOperation,
                OnSceneLoaded: (asyncOperation) =>
                {
                    SceneManager.SetActiveScene(levelIdDatabase.GetScene(currentLevelIdLoaded));

                    if (isEitherUnloadOrLoadDone)
                    {
                        EventManager.Instance.Dispatch(GameplayEvent.OnLoadingOperationFished, currentLevelIdLoaded);
                    }
                    else
                    {
                        isEitherUnloadOrLoadDone = true;
                    }
                });

            AsyncOperationsHandler asyncOperationsHandler = new AsyncOperationsHandler(unloadLevelAsyncOperation, loadLevelAsyncOperation);
            EventManager.Instance.Dispatch(GameplayEvent.OnLoadingOperationStarted, asyncOperationsHandler);
        }

        #region IEventListener

        public void OnEventReceived(IComparable eventType, object data)
        {
            switch (eventType)
            {
                case ButtonId buttonId:
                    {
                        HandleButtonIdEvents(buttonId, data);
                        break;
                    }
                case GameplayEvent gameplayEvent:
                    {
                        HandleGameplayEvents(gameplayEvent, data);
                        break;
                    }

                default:
                    {
                        Debug.LogError($"{eventType} is an unhandled case for the events received!");
                        break;
                    }
            }
        }

        private void HandleButtonIdEvents(ButtonId buttonId, object button)
        {
            switch (buttonId)
            {
                case ButtonId.LOAD_LEVEL:
                    {
                        LoadLevelButton loadLevelButton = (LoadLevelButton)button;

                        if (gameInformation.IsLevelUnlocked(loadLevelButton.LevelId))
                        {
                            LoadLevel(loadLevelButton.LevelId);
                        }

                        else
                        {
                            Debug.Log($"{gameObject.name} - {GetType().Name} The level cannot be loaded becuase it's locked.");
                        }

                        break;
                    }
                case ButtonId.MUSIC_SLIDER:
                    {
                        SliderButton sliderButton = (SliderButton)button;
                        gameInformation.UpdateMusicVolume(sliderButton.sliderValue);
                        storageAccessor.Save(gameInformation);
                        break;
                    }
                case ButtonId.SOUND_EFFECTS_SLIDER:
                    {
                        SliderButton sliderButton = (SliderButton)button;
                        gameInformation.UpdateSoundEffectsVolume(sliderButton.sliderValue);
                        storageAccessor.Save(gameInformation);
                        break;
                    }
                case ButtonId.GAME_DIFFICULTY:
                    {
                        DifficultyButton difficultyButton = (DifficultyButton)button;
                        gameInformation.ChangeGameDifficulty(difficultyButton.Difficulty);
                        storageAccessor.Save(gameInformation);
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
        }

        private void HandleGameplayEvents(GameplayEvent gameplayEvent, object data)
        {
            switch (gameplayEvent)
            {
                case GameplayEvent.NewTankSelected:
                    {
                        PlayerInformationParameters playerInformationParameters = (PlayerInformationParameters)data;
                        playerInformation = null;
                        playerInformation = new PlayerInformation(playerInformationParameters);
                        storageAccessor.Save(playerInformation);
                        break;
                    }

                case GameplayEvent.NewPlayerSpawned:
                    {
                        Tank tank = data as Tank;
                        playerGameplayInformation = new PlayerGameplayInformation(tank, GetCurrentTankInfoContainer());
                        break;
                    }

                case GameplayEvent.OnLevelRestarted:
                    {
                        Time.timeScale = 1;
                        playerGameplayInformation.Dispose();
                        playerGameplayInformation = null;
                        ReloadCurrentLevel();
                        break;
                    }

                case GameplayEvent.OnGamePaused:
                    {
                        Time.timeScale = 0;
                        break;
                    }

                case GameplayEvent.OnGameUnpaused:
                    { 
                        Time.timeScale = 1;
                        break;
                    }

                case GameplayEvent.OnGameQuit:
                    {
                        playerGameplayInformation.Dispose();
                        playerGameplayInformation = null;
                        Time.timeScale = 1;
                        UnloadLevel(currentLevelIdLoaded);
                        break;
                    }

                case GameplayEvent.OnGameOver:
                    {
                        PlayerGameplayInformation playerGameplayInformation = data as PlayerGameplayInformation; 
                        
                        if(playerGameplayInformation.Health != PlayerGameplayInformation.MIN_HEALTH_AMOUNT)
                        {
                            PlayerInformationParameters playerInformationParameters = new PlayerInformationParameters
                            {
                                tankColorSelected = playerInformation.TankColor,
                                tankIndexSelected = playerInformation.TankInfoContaierIndex,
                                //We are adding the momentanous points the player has won during the level to the ones that they have earned during the entire game 
                                points = playerInformation.Points + playerGameplayInformation.Points
                            };

                            playerInformation = null;
                            playerInformation = new PlayerInformation(playerInformationParameters);
                            storageAccessor.Save(playerInformation); 
                        }

                        playerGameplayInformation.Dispose();
                        playerGameplayInformation = null; 
                        UnloadLevel(currentLevelIdLoaded); 
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
