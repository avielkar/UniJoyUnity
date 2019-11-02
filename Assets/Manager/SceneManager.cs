using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;
//avi:using Trajectories;

namespace Assets.Manager
{
    public class SceneManager : MonoBehaviour
    {
        private static SceneManager singleton;
        public static SceneManager Instance { get { return singleton; } }

        private enum ScenesState { MainWindow = 0, Space = 1, Forest = 2 }; // The Build order of each scene
        private bool toReturnToMain = false;
        private ScenesState currentLoadedScene;
        [SerializeField]
        private float delalBeforeReturningToMain = 4f;
        private float timeElapsed;

        void Awake()
        {
            if (singleton != null && singleton != this)
            {
                Destroy(gameObject);
                return;
            }
            else
            {
                singleton = this;
                DontDestroyOnLoad(gameObject);
                return;
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene((int)ScenesState.MainWindow);
            currentLoadedScene = ScenesState.MainWindow;
            print("scene manager - start");
        }

        // Update is called once per frame
        void Update()
        {
            if (toReturnToMain)
            {
                print("toReturnToMain is true");
                timeElapsed += Time.deltaTime;
                if (timeElapsed > delalBeforeReturningToMain)
                {
                    UnityEngine.SceneManagement.SceneManager.LoadScene((int)ScenesState.MainWindow);
                    currentLoadedScene = ScenesState.MainWindow;
                    print("scene manager - returned to main window");
                    toReturnToMain = false;
                }
            }
        }

        IEnumerator SceneSwitcher(int sceneIndex)
        {
            try
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene(sceneIndex, LoadSceneMode.Additive);
                currentLoadedScene = (ScenesState)sceneIndex;
                UnityEngine.SceneManagement.SceneManager.MoveGameObjectToScene(gameObject,
                    UnityEngine.SceneManagement.SceneManager.GetSceneByBuildIndex(sceneIndex));
                toReturnToMain = true;
            }
            catch (Exception e)
            {
                print("Could not load " + (ScenesState)sceneIndex + " scene.");
                print(e);
            }
            yield return null;
        }

        public void ReadStringMessageFromSocket(string msg)
        {
            msg = msg.ToLower();
            msg = new string(msg.Where(c => char.IsLetter(c) || char.IsDigit(c)).ToArray());
            switch (msg)
            {
                case "space":
                    print("Identified Space msg");
                    if (currentLoadedScene == ScenesState.MainWindow)
                        UnityMainThreadDispatcher.Instance().Enqueue(SceneSwitcher((int)ScenesState.Space));
                    break;
                case "forest":
                    print("Identified Forest msg");
                    if (currentLoadedScene == ScenesState.MainWindow)
                        UnityMainThreadDispatcher.Instance().Enqueue(SceneSwitcher((int)ScenesState.Forest));
                        //SceneSwitcher((int)ScenesState.Forest);
                    break;
                case "left":
                    break;
                case "right":
                    break;
                default:
                    print("Identified no msg");
                    break;
            }
        }

        public void ReadMovementMessageFromSocket(/*avi:Trajectory msg*/)
        {
            /*avi:var deserializedTrajectory = new Trajectory();
            var ms = new MemoryStream(Encoding.UTF8.GetBytes(json));
            var ser = new DataContractJsonSerializer(deserializedTrajectory.GetType());
            deserializedTrajectory = ser.ReadObject(ms) as Trajectory;
            ms.Close();
            return deserializedTrajectory;*/
        }

        //public static MessageToUnity Serialize(object serializable)
        //{
        //    using (var memoryStream = new MemoryStream())
        //    {
        //        (new BinaryFormatter()).Serialize(memoryStream, serializable);
        //        return new MessageToUnity { Data = memoryStream.ToArray() };
        //    }
        //}

        public static object Deserialize(MessageToUnity message)
        {
            using (var memoryStream = new MemoryStream(message.Data))
                return (new BinaryFormatter()).Deserialize(memoryStream);
        }

        public void OnMessageReceived(MessageToUnity message)
        {
            object obj = Deserialize(message);
            if (obj is string)
            {
                ReadStringMessageFromSocket(obj as string);
            }
            else
            {
                //avi:ReadMovementMessageFromSocket(obj as Trajectory);
            }
        }

        //// Deserialize a JSON stream to a Trajectory object.  
        //public static Trajectory ReadToTrajectory(string json)
        //{
        //    var deserializedTrajectory = new Trajectory();
        //    var ms = new MemoryStream(Encoding.UTF8.GetBytes(json));
        //    var ser = new DataContractJsonSerializer(deserializedUser.GetType());
        //    deserializedUser = ser.ReadObject(ms) as Trajectory;
        //    ms.Close();
        //    return deserializedUser;
        //}
    }
}
