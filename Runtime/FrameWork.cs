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

        private AGame mGame;
        private GameObject @object;

        private GameLoop gameLoop;
        private ERepository repository;
        private EStateMachine stateMachine;
        private Exception exception;
        private Logger logger;
        private ESerialize serialize;
        private EPool pool;
        private ETimer timer;
        private ECoroutine coroutine;
        private EResource resource;
        private World mWorld;
        private SystemManager mSystemManager;

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

            gameLoop = new GameLoop();
            repository = new ERepository();
            stateMachine = new EStateMachine();
            serialize = new ESerialize();
            pool = new EPool();
            timer = new ETimer();
            coroutine = new ECoroutine();
            resource = new EResource();
            mWorld = new World();
            entities = new Queue<Entity>(32);
            mSystemManager = new SystemManager();

            if (!Application.isEditor)
                quit += Application.Quit;
        }

        internal T CreateGame<T>(GameConfig gameConfig) where T:AGame,new()
        {
            @object = new GameObject();
            @object.name = "FrameWork";

            T game = new T();
            mGame = game;

            preload?.Invoke();
            OnGameLoad(gameConfig);
            NextFrame(delegate () { game.Init(gameConfig); load?.Invoke(); });

            return game;
        }

        public static void CreateFrameWork() 
        {
            gFrameWork = new FrameWork();
            gFrameWork.Init();
        }

        public void Quit()
        {
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
        }

        private void OnGameUnload()
        {
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

        #region Respository
        public void RegistRespository(IRepository repository)
        {
            this.repository.Regist(repository);
        }

        public void UnRegistRespository<T>() where T : class, IRepository
        {
            repository.UnRegist<T>();
        }

        public T GetRepository<T>() where T : class, IRepository
        {
            return repository.GetRepository<T>();
        }

        public void Command<T>() where T : struct, ICommand
        {
            repository.Command<T>();
        }

        public T Request<T>(IRequest<T> request)
        {
            return repository.Request<T>(request);
        }
        #endregion

        #region StateMachine
        public void AddStateMachine(StateMachine stateMachine)
        {
            this.stateMachine.AddStateMachine(stateMachine);
        }

        internal void AddStateUpdate(ITick tick)
        {
            stateMachine.AddStateUpdate(tick);
        }

        internal void RemoveStateUpdate(ITick tick)
        {
            stateMachine.RemoveStateUpdate(tick);
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

        #region Serialize
        public string Serialize(object serializeAble, SerializeType serializeType)
        {
            return serialize.Serialize(serializeAble, serializeType);
        }

        public T DeSerialize<T>(string data, SerializeType serializeType)
        {
            return serialize.DeSerialize<T>(data, serializeType);
        }

        public object DeSerialize(string data, Type type, SerializeType serializeType)
        {
            return serialize.DeSerialize(data, type, serializeType);
        }

        public void OverWrite(string data, object @object, SerializeType serializeType)
        {
            serialize.OverWrite(data, @object, serializeType);
        }
        #endregion

        #region Pool
        public void BuildPool<T>(Func<T> create, Action<T> destroy, int count) where T : IPoolAble
        {
            pool.BuildPool<T>(create, destroy, count);
        }
        public void DestroyPool<T>() where T : IPoolAble
        {
            pool.DestroyPool<T>();
        }
        public T GetPoolAble<T>() where T : IPoolAble
        {
            return pool.Get<T>();
        }
        public void ReleasePoolAble<T>(T @object) where T : IPoolAble
        {
            pool.Release<T>(@object);
        }
        #endregion

        #region Timer
        public Timer StartTimer(float time, Action finish, Action<float> update = null, bool isLoop = false)
        {
            return timer.StartTimer(time, finish, update, isLoop);
        }

        public void StopTimer(Timer timer)
        {
            this.timer.StopTimer(timer);
        }
        #endregion

        #region Coroutine
        public void StartCoroutine(CoroutineTask coroutineTask)
        {
            coroutine.StartCoroutine(coroutineTask);
        }

        public void StopCoroutine(CoroutineTask coroutineTask)
        {
            coroutine.StopCoroutine(coroutineTask);
        }
        #endregion

        #region Resource
        public EAsset LoadAssetAsync(string path)
        {
            return resource.LoadAssetAsync(path);
        }

        public void LoadSetting(ResourceSetting resourceSetting)
        {
            resource.LoadSetting(resourceSetting);
        }

        public void LoadInfo(string key, AssetInfoFile infoFile)
        {
            resource.LoadInfo(key, infoFile);
        }

        public void UnloadInfo(string key)
        {
            resource.UnloadInfo(key);
        }

        public void AddLoaderCreator(ResourceLoaderCreator resourceLoaderCreator)
        {
            resource.AddLoaderCreator(resourceLoaderCreator);
        }
        #endregion

    }
}
