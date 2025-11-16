using MorePartsMod.Buildings;
using SFS.Input;
using SFS.UI.ModGUI;
using MorePartsMod;
using UnityEngine;

namespace MorePartsMod.UI
{
    class BuildingColonyGUI : Screen_Base
    {
        public override bool PauseWhileOpen => false;

        private Window _holder;

        public override void OnClose()
        {
            GameObject.Destroy(this._holder.gameObject);
        }

        public override void OnOpen()
        {
            this._holder = Builder.CreateWindow(this.transform, 2, 500, 700, 0, 500, titleText: "Launch From Colony");
            this._holder.CreateLayoutGroup(Type.Vertical).spacing = 20f;
            this._holder.CreateLayoutGroup(Type.Vertical).DisableChildControl();
            this._holder.CreateLayoutGroup(Type.Vertical).childAlignment = TextAnchor.UpperCenter;
            this.generateUI();
        }

        public override void ProcessInput()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                ScreenManager.main.CloseCurrent();
            }
        }

        private void generateUI()
        {
            Builder.CreateButton(this._holder.ChildrenHolder, 480, 60, 50, 0, () => this.TryLaunchFromColony(null), "[Default] Space Center");

            foreach (ColonyData colony in MorePartsPack.Main.ColoniesInfo)
            {
                Builder.CreateButton(this._holder.ChildrenHolder, 480, 60, 0, 0, () => this.TryLaunchFromColony(colony), colony.name);
            }
        }
        private void TryLaunchFromColony(ColonyData colony)
        {
            if (colony == null ||
               (colony.IsBuildingActive(MorePartsTypes.LAUNCH_PAD_BUILDING) && colony.IsBuildingActive(MorePartsTypes.VAB_BUILDING)))
            {
                SetSpawnPoint(colony);
            }

            else
            {
                ShowMessage("No launchpad or VAB in this colony.");
            }
        }

        private void ShowMessage(string message)
        {
            MsgDrawer.main.Log(message); 
        }


        private void SetSpawnPoint(ColonyData spawnPoint)
        {
            MorePartsPack.Main.SpawnPoint = spawnPoint;
            ScreenManager.main.CloseCurrent();
        }
    }
}
