using Assets.Scripts.GameComponents.Meshes;
using UnityEngine;

namespace Assets.Scripts.Configuration
{
    public class GameSettings
    {
        private static readonly object _locker = new object();
        private static GameSettings _instance;
        public static GameSettings Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_locker)
                    {
                        if (_instance == null)
                        {
                            return _instance = new GameSettings();
                        }
                    }
                }

                return _instance;
            }
        }

        private GameSettings()
        {
        }

        public SectorMesh.SectorMeshMode SectorMeshRenderMode { get; set; }
        public int GalaxySize { get; set; }
        public float HorizontalMouseSensitivity { get; set; }
        public float VerticalMouseSensitivity { get; set; }
        public Color SelectedSectorColor { get; set; }
    }
}
