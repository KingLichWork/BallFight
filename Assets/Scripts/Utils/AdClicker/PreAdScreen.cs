using MainAssets.Scripts.Utils;
using System;
using System.Collections;
using TMPro;
using UnityEngine;

namespace PreAdClicker.Scripts
{
    [RequireComponent(typeof(CanvasGroup))]
    public class PreAdScreen : UIPanel
    {
        [SerializeField] private TMP_Text _timer;
        [SerializeField] private int _adDelaySec;
        [SerializeField] private PreAdClicker _clicker;

        [SerializeField] private bool _shouldHideOthers;

        public static PreAdScreen Instance;

        private CanvasGroup _canvasGroup;

        private bool _isRu;

        public bool ShouldHideOthers => _shouldHideOthers;

        public GameObject GameObject => gameObject;

        private void Awake()
        {
            _isRu = Localizer.IsRu;
            if (Instance != null)
                Destroy(gameObject);

            Instance = this;

            _canvasGroup = GetComponent<CanvasGroup>();
        }

        public IEnumerator AdTimer(Action adCallback)
        {
            AnimatedShow();
            _clicker.StartField();

            for (var i = _adDelaySec; i > 0; i--)
            {
                _timer.text = Localizer.GetLocalizedString(TableNames.RuEn, "Ad in") + i;
                yield return new WaitForSecondsRealtime(1);
            }

            AnimatedHide();

            adCallback.Invoke();
        }

        private void AnimatedShow()
        {
            _canvasGroup.alpha = 1;
            _canvasGroup.interactable = true;
            _canvasGroup.blocksRaycasts = true;

            gameObject.SetActive(true);
            gameObject.transform.SetAsLastSibling();
        }

        private void AnimatedHide()
        {
            _canvasGroup.alpha = 0;
            _canvasGroup.interactable = false;
            _canvasGroup.blocksRaycasts = false;
        }

        public void StopField()
        {
            _clicker.StopField();
        }
    }
}
