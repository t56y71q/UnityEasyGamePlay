using System;
using System.Collections.Generic;
using UnityEngine;

namespace EasyGamePlay
{
    public class FrameWork: ITick
    {
        public AGame game { get=> mGame; }
        public GameObject gameObject { get => @object; }

        public World world { get => mWorld; }
        public SystemManager systemManager { get => mSystemManager; }
        public ERepository repository { get => mRepository; }
        public EStateMachine stateMachine { get => mStateMachine; }
        public ESerialize serialize { get => mSerialize; }
        public EPool pool { get => mPool; }
        public ETimer timer { get => mTimer; }
        public ECoroutine coroutine { get => mCoroutine; }
        public EResource resource { get => mResource; }
        public PanelSystem panelSystem { get => mPanelSystem; }

        private AGame mGame;
        private GameObject @object;

        private ERepository mRepository;
        private EStateMachine mStateMachine;
        private ESerialize mSerialize;
        private EPool mPool;
        private ETimer mTimer;
        private ECoroutine mCoroutine;
        private EResource mResource;
        private World mWorld;
        private SystemManager mSystemManager;
        private PanelSystem mPanelSystem;

        private GameLoop gameLoop;
        private Exception exception;
        private Logger logger;

        private Queue<Entity> entities;

        public event PreLoad  preload;
        public event Load load;
        public event Unload unLoad;
        public event AppQuit quit;

        public delegate void PreLoad();
        public delegate void Load();
        public delegate void Unload();
        public delegate void AppQuit();

        public static FrameWork frameWork { get => gFrameWork; }
        private static FrameWork gFrameWork;

        #region FrameWork
        public void Init()
        {
            exception = new Exception();

            logger = new Logger(Debug.unityLogger.logHandler, exception);
            Debug.unityLogger.logHandler = logger;

            mRepository = new ERepository();
            mSerialize = new ESerialize();
            mPool = new EPool();
            mSystemManager = new SystemManager();

            if (!Application.isEditor)
                quit += Application.Quit;
        }

        internal void CreateGame(string gameType,GameConfig gameConfig) 
        {
            @object = new GameObject();
            @object.name = "FrameWork";

            Type type = Type.GetType(gameType);
            mGame = Activator.CreateInstance(type) as AGame;

            gameLoop = new GameLoop();
            mTimer = new ETimer(mPool);
            mCoroutine = new ECoroutine();
            mResource = new EResource(mSerialize);
            mStateMachine = new EStateMachine();
            mPanelSystem = new PanelSystem();
            mWorld = new World();

            entities = new Queue<Entity>(32);
            Entity.stateUpdate = entities.Enqueue;

            preload?.Invoke();
            OnGameLoad(gameConfig);
            NextFrame(delegate () { game.Init(gameConfig); load?.Invoke(); });
        }

        public static void CreateFrameWork() 
        {
            gFrameWork = new FrameWork();
            gFrameWork.Init();
        }

        public void Quit()
        {
            Debug.Log("Quit");
            game.gameState.isPlaying = false;
        }

        internal void DestroyGame()
        {
            mGame.Destroy();
            unLoad?.Invoke();
            OnGameUnload();
            mGame = null; 
        }

        private void OnGameLoad(GameConfig gameConfig)
        {
            exception.Init();

            gameLoop.Init(gameObject);

            stateMachine.Init();

            timer.Init();

            coroutine.Init(gameObject);

            resource.LoadSetting(gameConfig.resourceSetting);

            gameLoop.AddTick(this);

            mWorld.Init(mResource, mSerialize, mCoroutine, gameObject);
        }

        private void OnGameUnload()
        {
            mWorld.Destroy();

            gameLoop.RemoveTick(this);

            coroutine.Destroy();

            timer.Destroy();

            stateMachine.Destroy();

            gameLoop.Destroy();

            Debug.unityLogger.logHandler = logger.GetDefualtLogHandler();

            exception.Destroy();

            GameObject.Destroy(@object);

            quit();
        }

        public void Tick()
        {
            while (entities.Count > 0)
            {
                entities.Dequeue().Update();
            }
        }

        #endregion

        #region GameLoop
        public void AddTick(ITick tick)
        {
            gameLoop.AddTick(tick);
        }

        public void RemoveTick(ITick tick)
        {
            gameLoop.RemoveTick(tick);
        }

        public void NextFrame(Action action)
        {
            gameLoop.NextFrame(action);
        }

        public void AddFixedUpdate(IFixedUpdate fixedUpdate)
        {
            gameLoop.AddFixedUpdate(fixedUpdate);
        }

        public void RemoveFixedUpdate(IFixedUpdate fixedUpdate)
        {
            gameLoop.RemoveFixedUpdate(fixedUpdate);
        }
        #endregion

        #region Exception
        public void AddException<T>(Action<System.Exception> action) where T : System.Exception
        {
            exception.AddException<T>(action);
        }

        public void AddGlobalException(Action<System.Exception> action)
        {
            exception.AddGlobalException(action);
        }

        internal void SendException(System.Exception exception)
        {
            this.exception.SendException(exception);
        }
        #endregion

    }
}
