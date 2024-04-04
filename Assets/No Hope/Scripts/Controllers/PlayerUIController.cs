using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NoHope.RunTime.Controllers
{
    public class PlayerUIController : MonoBehaviour
    {
        #region Variables and Properties
        [BoxGroup("Components")]
        [SerializeField]
        private Image _healthBar = null;
        #endregion

        //-------------------------------------------------------------------

        public void UpdateHealthBar(float HealthPercentage)
        {
            _healthBar.fillAmount = HealthPercentage;
        }

        //-------------------------------------------------------------------
    }
}