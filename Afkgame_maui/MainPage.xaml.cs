﻿using Afkgame;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;


namespace Afkgame_maui;

public partial class MainPage : ContentPage
{
	
	private GameManager gameManager;
	int cost = 300;


	public MainPage()
	{
		InitializeComponent();
		gameManager = new GameManager();
		gameManager.MoneyGenerated += GameManager_MoneyGenerated;
        gameManager.WorkersPaidSuccessfully += OnWorkersPaidSuccessfully;
        gameManager.RentPaidSuccessfully += OnRentPaidSuccessfully;

		UpdateUI();
		AnimateProgressBar();
	}

private void GameManager_MoneyGenerated(object sender, EventArgs e)
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                UpdateUI();
            });
        }


        private void IncreaseWorkersLimit(object sender, EventArgs e)
        {
            increasWorkersLimitBtn.IsEnabled = false;
            if (gameManager.Money >= cost)
            {
                gameManager.MaxWorkers += 10;
                gameManager.Money -= 300;
                gameManager.Rent *= 3;
                cost = cost * 3;
                UpdateUI();
            }
            else
            {
                increasWorkersLimitBtn.IsEnabled = false;
                UpdateUI();
            }
        }

        private void BuyJuniorDev(object sender, EventArgs e)
        {
            Worker newJuniorDev = new Worker("Junior Dev", 5, 3, 3);
            if (gameManager.BuyWorker(newJuniorDev))
            {
               string message; 
                 message = $"Junior Dev bought";
                 var toast = Toast.Make(message, CommunityToolkit.Maui.Core.ToastDuration.Long, 30).Show();
                UpdateUI();
            }
            else
            {
                buyJuniorDevBtn.IsEnabled = false;
            }
        }

        private void BuySeniorDev(object sender, EventArgs e)
        {
            Worker newSeniorDev = new Worker("Senior Dev", 25, 9, 6);
            if (gameManager.BuyWorker(newSeniorDev))
            {
                string message; 
                 message = $"Senior Dev bought";
                 var toast = Toast.Make(message, CommunityToolkit.Maui.Core.ToastDuration.Long, 30).Show();
                UpdateUI();
            }
            else
            {
                buySeniorDevBtn.IsEnabled = false;
            }
        }

        private void BuyDesigner(object sender, EventArgs e)
        {
            Worker newDesigner = new Worker("Designer", 32, 13, 7);
            if (gameManager.BuyWorker(newDesigner))
            {
                string message; 
                 message = $"Designer bought";
                 var toast = Toast.Make(message, CommunityToolkit.Maui.Core.ToastDuration.Long, 30).Show();
                UpdateUI();
            }
            else
            {
                buyDesignerBtn.IsEnabled = false;
            }
        }

        private void OnRentPaidSuccessfully(object sender, EventArgs e)
        {
                   MainThread.BeginInvokeOnMainThread(() =>
    {
         string message; 
            message = $"You paid your rent";
            var toast = Toast.Make(message, CommunityToolkit.Maui.Core.ToastDuration.Long, 30);
            toast.Show();
    }); 
        }

        private void OnWorkersPaidSuccessfully(object sender, EventArgs e)
{
        MainThread.BeginInvokeOnMainThread(() =>
    {
        ToastPayWorker();
    });
}
         private void ToastPayWorker()
        {
            string message; 
            message = $"You paid your workers";
            var toast = Toast.Make(message, CommunityToolkit.Maui.Core.ToastDuration.Long, 30);
            toast.Show();
        }

        private void UpdateUI()
        {
            cycleLabel.Text = gameManager.Cycles.ToString();
            moneyLabel.Text = $"Money: {gameManager.Money}$";
            totalDevJuniorsLabel.Text = $"Dev Juniors: {gameManager.TotalDevJuniors}";
            totalDevSeniorsLabel.Text = $"Dev Seniors: {gameManager.TotalDevSeniors}";
            totalDesignerLabel.Text = $"Designers: {gameManager.TotalDesigners}";
            workerLimitLabel.Text = $"{gameManager.Workers.Count} / {gameManager.MaxWorkers} Workers";
            rentLabel.Text = $"Rent : {gameManager.Rent} ";

            increasWorkersLimitBtn.Text = $"Add 10 slots ({cost}$)";

            bool canBuyMoreWorkers = (gameManager.TotalDevJuniors + gameManager.TotalDevSeniors + gameManager.TotalDesigners) < gameManager.MaxWorkers;


            buyJuniorDevBtn.IsEnabled = canBuyMoreWorkers && gameManager.Money >= 5; 
            buySeniorDevBtn.IsEnabled = canBuyMoreWorkers && gameManager.Money >= 25;
            buyDesignerBtn.IsEnabled = canBuyMoreWorkers && gameManager.Money >= 32;

            if (cost >= gameManager.Money)
            {
                increasWorkersLimitBtn.IsEnabled = false;
            }
            else
            {
                increasWorkersLimitBtn.IsEnabled = true;
            }
        }

          private void AnimateProgressBar()
        {
            var progressBarAnimation = new Animation(v => moneyProgressBar.Progress = v, 0, 1);
            progressBarAnimation.Commit(this, "ProgressBarAnimation", 16, 10000, Easing.Linear, (v, c) =>
            {
                moneyProgressBar.Progress = 0;
                AnimateProgressBar();
            });
        }
}