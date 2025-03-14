using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using Yun.Scripts.GamePlay.IdleGame.Managers;

namespace ExaGames.Common {

	public class LivesManagerShop : MonoBehaviour {
		#region Constants
	
		private const string MAX_LIVES_SAVEKEY_SHOP = "ExaGames.Common.MaxLivesShop";
	
		private const string LIVES_SAVEKEY_SHOP = "ExaGames.Common.LivesShop";
	
		private const string RECOVERY_TIME_SAVEKEY_SHOP = "ExaGames.Common.LivesRecoveryTimeShop";

		private const string INFINITE_LIVES_TIME_SAVEKEY_SHOP = "ExaGames.Common.InfiniteLivesStartTimeShop";

		private const string INFINITE_LIVES_MINUTES_SAVEKEY_SHOP = "ExaGames.Common.InfiniteLivesMinutesShop";
		#endregion

		#region Fields

		public bool ResetPlayerPrefsOnPlay = false;

		public int DefaultMaxLives = 1;

		public double MinutesToRecover = 30D;

		public string CustomFullLivesText = "Done";

		public bool SimpleHourFormat = false;


		public UnityEvent OnLivesChanged;

		public UnityEvent OnRecoveryTimeChanged;

		#region Normal lives count

		private int lives;

		private DateTime recoveryStartTime;
	
		private double secondsToNextLife;
		#endregion

		#region Infinite lives count

		private double remainingSecondsWithInfiniteLives;

		private DateTime infiniteLivesStartTime;

		private int infiniteLivesMinutes;
		#endregion

		private bool calculateSteps;

		private bool applicationWasPaused;
		#endregion

		#region Properties

		public int Lives { get { return lives; } }

		public Action CounterCompleted;

		public int MaxLives { get; private set; }


		public bool HasMaxLives { get { return (lives >= MaxLives); } }
		public bool HasInfiniteLives{ get { return remainingSecondsWithInfiniteLives > 0D; } }
		public string RemainingTimeString {
			get { 
				if(!HasInfiniteLives && HasMaxLives && !string.IsNullOrEmpty(CustomFullLivesText)) {
					return CustomFullLivesText;
				}
				TimeSpan timerToShow = TimeSpan.FromSeconds(HasInfiniteLives ? remainingSecondsWithInfiniteLives : secondsToNextLife);
				if(timerToShow.TotalHours > 1D) {
					if(SimpleHourFormat) {
						int hoursLeft = Mathf.RoundToInt((float)timerToShow.TotalHours);
						return string.Format(">{0} hr{1}", hoursLeft, hoursLeft > 1 ? string.Empty : "");
					}
					return timerToShow.ToString().Substring(0, 8);
				}
				return timerToShow.ToString().Substring(3, 5);
			}
		}
		
		public double SecondsToFullLives { get { return secondsToNextLife + ((MaxLives - lives - 1) * (MinutesToRecover * 60)); } }
		#endregion

		public LivesManagerShop() {
			calculateSteps = false;
		}

		#region Unity Behaviour Methods

		private void Awake() {

			#if !UNITY_EDITOR
			// This line ensures that preferences won't be reset by error with the game published.
			ResetPlayerPrefsOnPlay = false;
			#endif
			#if UNITY_IOS
			// Register for local notifications if set in the inspector.
			if(LocalNotificationSettings.AllowLocalNotifications) {
				UnityEngine.iOS.NotificationServices.RegisterForNotifications(UnityEngine.iOS.NotificationType.Alert | UnityEngine.iOS.NotificationType.Badge | UnityEngine.iOS.NotificationType.Sound);
			}
			#endif
			if(ResetPlayerPrefsOnPlay) {
				ResetPlayerPrefs();
			} else {
				retrievePlayerPrefs();
			}
		}

		private void Start() {
			initTimer();
		}

		private void Update() {
			if(calculateSteps) {
				stepRecoveryTime();
			}
		}


		private void OnApplicationPause(bool pauseStatus) {
			if(pauseStatus) {
				applicationWasPaused = true;
				calculateSteps = false;
			} else if(applicationWasPaused) {
				applicationWasPaused = false;
				initTimer();
			}
		}


		private void OnDestroy() {
			Debug.Log("Run In OnDestroy");
			savePlayerPrefs();
		}
		#endregion

		#region Public Methods

		public void GiveOneLife() {
			if(!HasMaxLives && !HasInfiniteLives) {
				lives++;
		
				CounterCompleted?.Invoke();
				recoveryStartTime = DateTime.Now;
				savePlayerPrefs();
				notifyAll();
			}
		}

		public void FillLives() {
			if(!HasInfiniteLives) {
				lives = MaxLives;
				setSecondsToNextLifeToZero();
				notifyAll();
			}
		}

		public void AddLifeSlots(int quantity, bool fillLives = false) {
			if(HasMaxLives && !HasInfiniteLives) {
				recoveryStartTime = DateTime.Now;
				resetSecondsToNextLife();
			}
			MaxLives += quantity;
			if(fillLives) {
				FillLives();
			} else {
				savePlayerPrefs();
			}
			initTimer();
		}

		#endregion
		/// <summary>
		/// Initializes the timer for next life.
		/// </summary>
		private void initTimer() {
			remainingSecondsWithInfiniteLives = calculateRemainingInfiniteLivesTime().TotalSeconds;
			if(!HasInfiniteLives) {
				secondsToNextLife = calculateLifeRecovery().TotalSeconds;
			}
			calculateSteps = true;
			notifyAll();
		}

		#region Data persistance

		private void retrievePlayerPrefs() {
			remainingSecondsWithInfiniteLives = 0D;
			MaxLives = PlayerPrefs.HasKey(MAX_LIVES_SAVEKEY_SHOP) ? PlayerPrefs.GetInt(MAX_LIVES_SAVEKEY_SHOP) : DefaultMaxLives;
			if(PlayerPrefs.HasKey(INFINITE_LIVES_TIME_SAVEKEY_SHOP) && PlayerPrefs.HasKey(INFINITE_LIVES_MINUTES_SAVEKEY_SHOP)) {
				infiniteLivesStartTime = new DateTime(long.Parse(PlayerPrefs.GetString(INFINITE_LIVES_TIME_SAVEKEY_SHOP)));
				infiniteLivesMinutes = PlayerPrefs.GetInt(INFINITE_LIVES_MINUTES_SAVEKEY_SHOP);
			} else {
				infiniteLivesStartTime = DateTime.MinValue;
				infiniteLivesMinutes = 0;
			}
			if(PlayerPrefs.HasKey(LIVES_SAVEKEY_SHOP) && PlayerPrefs.HasKey(RECOVERY_TIME_SAVEKEY_SHOP)) {
				lives = PlayerPrefs.GetInt(LIVES_SAVEKEY_SHOP);
				recoveryStartTime = new DateTime(long.Parse(PlayerPrefs.GetString(RECOVERY_TIME_SAVEKEY_SHOP)));
			} else {
				lives = MaxLives;
				recoveryStartTime = DateTime.Now;
			}
			if(lives > MaxLives) {
				FillLives();
			}
		}

		private void savePlayerPrefs() {
			PlayerPrefs.SetInt(MAX_LIVES_SAVEKEY_SHOP, MaxLives);
			PlayerPrefs.SetInt(LIVES_SAVEKEY_SHOP, lives);
			PlayerPrefs.SetString(RECOVERY_TIME_SAVEKEY_SHOP, recoveryStartTime.Ticks.ToString());
			PlayerPrefs.SetString(INFINITE_LIVES_TIME_SAVEKEY_SHOP, infiniteLivesStartTime.Ticks.ToString());
			PlayerPrefs.SetInt(INFINITE_LIVES_MINUTES_SAVEKEY_SHOP, infiniteLivesMinutes);
			try {
				PlayerPrefs.Save();
			} catch(Exception e) {
				Debug.LogError("Could not save LivesManager preferences: " + e.Message);
			}
		}

		/// <summary>
		/// Resets the all the preferences of the LivesManager. Use with care.
		/// </summary>
		public void ResetPlayerPrefs() {
			PlayerPrefs.DeleteKey(MAX_LIVES_SAVEKEY_SHOP);
			PlayerPrefs.DeleteKey(RECOVERY_TIME_SAVEKEY_SHOP);
			PlayerPrefs.DeleteKey(LIVES_SAVEKEY_SHOP);
			PlayerPrefs.DeleteKey(INFINITE_LIVES_TIME_SAVEKEY_SHOP);
			PlayerPrefs.DeleteKey(INFINITE_LIVES_MINUTES_SAVEKEY_SHOP);
			clearNotification();
			retrievePlayerPrefs();
		}
		#endregion

		#region TimeToNextLife control

		private TimeSpan calculateLifeRecovery() {
			DateTime now = DateTime.Now;
			TimeSpan elapsed = now - recoveryStartTime;
			double minutesElapsed = elapsed.TotalMinutes;
	
			while((!HasMaxLives) && (minutesElapsed >= MinutesToRecover)) {
				lives++;
		
				CounterCompleted?.Invoke();
				recoveryStartTime = DateTime.Now;
				minutesElapsed -= MinutesToRecover;
			}

			savePlayerPrefs();

			if(HasMaxLives) {
				return TimeSpan.Zero;
			} else {
				return TimeSpan.FromMinutes(MinutesToRecover - minutesElapsed);
			}
		}

		private TimeSpan calculateRemainingInfiniteLivesTime() {
			DateTime now = DateTime.Now;
			TimeSpan elapsed = now - infiniteLivesStartTime;
			double minutesElapsed = elapsed.TotalMinutes;

			if(minutesElapsed < (double)infiniteLivesMinutes) {
				return TimeSpan.FromMinutes(infiniteLivesMinutes - minutesElapsed);
			} else {
				return TimeSpan.Zero;
			}
		}

		private void stepRecoveryTime() {
			if(HasInfiniteLives) {
				remainingSecondsWithInfiniteLives -= Time.deltaTime;
				if(remainingSecondsWithInfiniteLives < 0D) {
					remainingSecondsWithInfiniteLives = 0D;
					infiniteLivesMinutes = 0;
					infiniteLivesStartTime = new DateTime(0);
			
					notifyLivesChanged();
				}
				notifyRecoveryTimeChanged();
			} else if(!HasMaxLives) {
				if(secondsToNextLife > 0D) {
					secondsToNextLife -= Time.deltaTime;
					notifyRecoveryTimeChanged();
				} else {
					GiveOneLife();
					notifyLivesChanged();
					if(HasMaxLives) {
						setSecondsToNextLifeToZero();
					} else {
						resetSecondsToNextLife();
					}
				}
			}
		}

		/// <summary>
		/// Sets the seconds to next life to zero.
		/// </summary>
		private void setSecondsToNextLifeToZero() {
			secondsToNextLife = 0;
			notifyRecoveryTimeChanged();
		}

		private void resetSecondsToNextLife() {
			secondsToNextLife = MinutesToRecover * 60;
			notifyRecoveryTimeChanged();
		}
		#endregion

		#region Notifications for observers

		private void notifyAll() {
			notifyRecoveryTimeChanged();
			notifyLivesChanged();
		}

		private void OnCouterCompleted()
		{
			CounterCompleted.Invoke();
		}

		private void notifyRecoveryTimeChanged() {
			OnRecoveryTimeChanged.Invoke();
		}

		private void notifyLivesChanged() {
			OnLivesChanged.Invoke();
		}
		#endregion

		#region Local iOS notifications

		private const string LOCAL_NOTIF_KEY = "ExaGames.TimeBasedLifeSystem";

		[Serializable]
		public class NotificationSettingsStruct {
			/// <summary>
			/// Indicates wether the lives manager should support Unity Nofitication Services.
			/// </summary>
			public bool AllowLocalNotifications = true;
			/// <summary>
			/// Custom alert action for the shceduled notifications.
			/// </summary>
			public string AlertAction;
			/// <summary>
			/// Custom text for scheduled notifications.
			/// </summary>
			public string AlertBody = "All lives have been recovered!";
		}

		/// <summary>
		/// Local notification settings set in the Inspector.
		/// </summary>
		public NotificationSettingsStruct LocalNotificationSettings;

		/// <summary>
		/// Schedules a notification to inform the player on lives replenished.
		/// </summary>
		private void scheduleNotification() {
			#if UNITY_IOS
			clearNotification();
			if(!HasMaxLives) {
				if(string.IsNullOrEmpty(LocalNotificationSettings.AlertBody)) {
					Debug.LogError("Could not schedule local notification because the AlertBody property has not been set.");
					return;
				}

				Debug.Log("Scheduling local notification...");
				var notification = new UnityEngine.iOS.LocalNotification();
				var secondsDelay = secondsToNextLife + ((MaxLives - lives - 1) * (MinutesToRecover * 60));
				notification.fireDate = DateTime.Now.AddSeconds(secondsDelay);
				if(!string.IsNullOrEmpty(LocalNotificationSettings.AlertAction.Trim()))
					notification.alertAction = LocalNotificationSettings.AlertAction.Trim();
				notification.alertBody = LocalNotificationSettings.AlertBody;
				notification.soundName = UnityEngine.iOS.LocalNotification.defaultSoundName;

				var options = new Dictionary<string,string>();
				options.Add(LOCAL_NOTIF_KEY, "");
				notification.userInfo = options;
				UnityEngine.iOS.NotificationServices.ScheduleLocalNotification(notification);
			}
			#endif
		}

		/// <summary>
		/// Clears the LivesManager local notification if previously set.
		/// </summary>
		private void clearNotification() {
			#if UNITY_IOS
			UnityEngine.iOS.LocalNotification notifToCancel = null;
			var localNotifications = UnityEngine.iOS.NotificationServices.scheduledLocalNotifications;

			try {
				for(int i=0; i<localNotifications.Length; i++) {
					if(localNotifications[i].userInfo != null && localNotifications[i].userInfo.Count > 0 && localNotifications[i].userInfo.Contains(LOCAL_NOTIF_KEY)) {
						notifToCancel = localNotifications[i];
						break;
					}
				}
			} finally {
				if(notifToCancel != null) {
					UnityEngine.iOS.NotificationServices.CancelLocalNotification(notifToCancel);
				}
			}
			#endif
		}
		#endregion
	}
}