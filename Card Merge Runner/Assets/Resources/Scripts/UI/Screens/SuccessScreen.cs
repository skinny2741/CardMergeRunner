using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hyperlab.Core.UI;
using Hyperlab.Managers;
using DG.Tweening;
using UnityEngine.Events;
using TMPro;
using UnityEngine.UI;
using Hyperlab.Controllers;
using MoreMountains.Feedbacks;
using Sirenix.OdinInspector;

using Hyperlab.Core;

namespace Hyperlab.UI
{
    public class SuccessScreen : CoreScreen<SuccessScreen>
    {
        [FoldoutGroup("Bars", expanded: true)]
        public KeyBar m_KeyBar;
        [FoldoutGroup("Bars")]
        public RewardBar m_RewardBar;
        [FoldoutGroup("Bars")]
        public CoinBar m_CoinBar;


        [FoldoutGroup("Texts", expanded: true)]
        public TextMeshProUGUI m_RewardText;
        [FoldoutGroup("Buttons", expanded: true)]
        public CoreButton m_BtnNextLevel;
        [FoldoutGroup("Components")]
        public List<RectTransform> m_Coins;
        [FoldoutGroup("Components")]
        public RectTransform m_CoinTarget;
        [FoldoutGroup("Components")]
        public RectTransform m_RewardTextContainer;
        [FoldoutGroup("Components")]
        public RectTransform m_RewardBoxContainer;

        [FoldoutGroup("Feedbacks", expanded: true)]
        public MMFeedbacks m_CoinExplosionFeedback;
        [FoldoutGroup("Feedbacks")]
        public MMFeedbacks m_CoinAnimationCompleteFeedback;
        [FoldoutGroup("Feedbacks")]
        public MMFeedbacks m_RewardBoxShowFeedback;


        int m_CurrentCoin;
        int m_RewardedCoin;
        protected override void Awake()
        {
            base.Awake();
        }
        protected override void OnEnable()
        {
            base.OnEnable();
        }
        protected override void Start()
        {
            base.Start();
            GameManager.Instance.onAppStart += Hide;
            GameManager.Instance.onLevelComplete += OnLevelComplete;
            GameManager.Instance.onLevelFinish += OnLevelFinish;
            GameManager.Instance.onNextLevel += Hide;
        }
        void OnLevelFinish()
        {
            //m_CurrentCoin = (int)WalletManager.GetBalance(GameManager.Instance.m_CoinCurrency);
            m_RewardedCoin = GameManager.Instance.m_InGameCoin - m_CurrentCoin;
        }
        void OnLevelComplete()
        {
            DOVirtual.DelayedCall(1, () => ShowScreen());
        }
        public void ShowScreen()
        {
            base.Show();
            HideCoins();
            m_BtnNextLevel.gameObject.SetActive(false);

            if (ProjectSettings.Instance.m_Shop)
            {
                //SetCurrent Coin Amount
                m_CoinBar.UpdateCoin(m_CurrentCoin, m_CurrentCoin, false);
                m_RewardText.text = "+" + m_RewardedCoin.ToString();
                DOVirtual.DelayedCall(0.1f, () =>
                {
                    m_RewardBoxShowFeedback.PlayFeedbacks();
                });
                DOVirtual.DelayedCall(1, () =>
                {
                    m_CoinExplosionFeedback.PlayFeedbacks();
                    AnimateCoins(() =>
                    {
                        if (ProjectSettings.Instance.m_Prizes)
                        {
                            //if (GameManager.Instance.m_InGameKey >= (int)GameFoundation.catalogs.currencyCatalog.FindItem("ckey").maximumBalance)
                            //    PrizeScreen.Instance.Show();
                        }
                        m_BtnNextLevel.gameObject.SetActive(true);
                    });
                });
                DOVirtual.DelayedCall(2f, () =>
                {
                    m_CoinBar.UpdateCoin(m_CurrentCoin, GameManager.Instance.m_InGameCoin, true);
                });


                m_KeyBar.UpdateKeys(GameManager.Instance.m_InGameKey);
            }
            else
            {
                m_RewardTextContainer.gameObject.SetActive(false);
                m_RewardBoxContainer.gameObject.SetActive(false);
                m_BtnNextLevel.gameObject.SetActive(true);
            }
            
            //m_RewardBar.UpdateKeys();
        }
        void AnimateCoins(UnityAction _callback)
        {
            int _index = 0;
            //List<Sequence> _coinSequences = new List<Sequence>();
            Sequence _coinSequence = DOTween.Sequence().OnComplete(() =>
            {

                if (_callback != null) _callback();
            }).Pause();
            foreach (var _coin in m_Coins)
            {
                AnimateCoin(_coinSequence, _coin, _index, null);
                _index++;
            }
            _coinSequence.Play();
        }
        void AnimateCoin(Sequence _coinSequence, RectTransform _coin, int _index, UnityAction _callback)
        {
            _coin.localPosition = Vector3.zero;
            _coin.localScale = Vector3.zero;

            // Show Animation
            Tween _showTween = _coin.DOScale(1, 0.2f).SetDelay(_index * 0.05f).Pause();
            Tween _explodeTween = _coin.DOLocalMove(new Vector3(Random.Range(-100, 100), Random.Range(-100, 100), 0), 0.2f).Pause();
            Tween _hideTween = _coin.DOScale(0, 0.1f).OnComplete(() =>
            {
                m_CoinAnimationCompleteFeedback.PlayFeedbacks();
            }).Pause();
            Tween _moveTargetTween = _coin.DOMove(m_CoinTarget.position, 0.2f).OnComplete(() =>
            {
                _hideTween.Play();
            }).SetDelay(_index * 0.05f).Pause();
            _coinSequence.Insert(0, _showTween);
            // Explode Coins
            _coinSequence.Insert(0, _explodeTween);
            _coinSequence.Insert(1, _moveTargetTween);
        }
        void HideCoins()
        {
            foreach (var _coin in m_Coins)
            {
                _coin.localPosition = Vector3.zero;
                _coin.localScale = Vector3.zero;
            }
        }
    }
}