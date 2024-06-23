using NoHope.RunTime.AI;
using NoHope.RunTime.EnemyScripts;

namespace NoHope.RunTime.EnemyScripts
{
    public class TractorBoss : EnemyBase
    {
        #region Variables and Proeprties
        private StateMachine _myStateMachine = null;
        #endregion

        //-------------------------------------------------------------------

        protected override void Start()
        {
            base.Start();

            _myStateMachine = GetComponent<StateMachine>();
        }

        //-------------------------------------------------------------------

        public void StartTractorBoss()
        {
            _myStateMachine.enabled = true;
        }
    }
}