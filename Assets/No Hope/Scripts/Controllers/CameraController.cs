using Cinemachine;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NoHope.RunTime.Controllers
{
    public class CameraController : MonoBehaviour
    {
        #region Singleton
        private static CameraController instance = null;

        private void Awake()
        {
            if (instance == null)
                instance = this;
        }
        #endregion

        //-------------------------------------------------------------------

        #region Singleton Methods
        public static CinemachineVirtualCamera GetCamera(string CameraName)
        {
            if (instance == null)
                return null;

            return instance.getCamera(CameraName);
        }

        public static CinemachineVirtualCamera GetCamera(int Id)
        {
            if (instance == null)
                return null;

            return instance.getCamera(Id);
        }

        public static void ChangeToDefaultCamera()
        {
            if (instance == null)
                return;

            instance.changeToDefaultCamera();
        }

        public static void ChangeActiveCamera(CinemachineVirtualCamera NewCamera)
        {
            if (instance == null)
                return;

            instance.changeActiveCamera(NewCamera);
        }
        #endregion

        //-------------------------------------------------------------------

        #region Variables and Properties
        private CinemachineVirtualCamera _activeCamera = null;

        [BoxGroup("Cameras")]
        [SerializeField]
        private CinemachineVirtualCamera _defaultCamera = null;
        [BoxGroup("Cameras")]
        [SerializeField]
        private List<GameVirtualCameras> _gameCameras = new List<GameVirtualCameras>();

        [System.Serializable]
        public class GameVirtualCameras
        {
            public int Id;
            public string Name;
            public CinemachineVirtualCamera VirtualCamera;
        }
        #endregion

        //-------------------------------------------------------------------

        #region Unity Methods
        private void Start()
        {
            _activeCamera = _defaultCamera;
        }
        #endregion

        //-------------------------------------------------------------------

        #region Camera Methods
        public CinemachineVirtualCamera getCamera(string cameraName)
        {
            if (_gameCameras == null ||
                _gameCameras.Count <= 0)
                return null;

            for (int i = 0; i < _gameCameras.Count; i++)
            {
                if (_gameCameras[i].Name == cameraName)
                    return _gameCameras[i].VirtualCamera;
            }

            return null;
        }
        public CinemachineVirtualCamera getCamera(int id)
        {
            if (_gameCameras == null ||
                _gameCameras.Count <= 0)
                return null;

            for (int i = 0; i < _gameCameras.Count; i++)
            {
                if (_gameCameras[i].Id == id)
                    return _gameCameras[i].VirtualCamera;
            }

            return null;
        }

        public void changeToDefaultCamera()
        {
            if (_activeCamera != null)
                _activeCamera.gameObject.SetActive(false);

            _activeCamera = _defaultCamera;
            _activeCamera.gameObject.SetActive(true);
        }
        public void changeActiveCamera(CinemachineVirtualCamera newCamera)
        {
            if (_activeCamera != null)
                _activeCamera.gameObject.SetActive(false);

            _activeCamera = newCamera;
            _activeCamera.gameObject.SetActive(true);
        }
        #endregion
    }
}