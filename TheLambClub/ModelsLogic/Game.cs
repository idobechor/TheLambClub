
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.Messaging;
using Plugin.CloudFirestore;
using System;
using System.Collections.Generic;
using TheLambClub.Models;
using TheLambClub.Views;

namespace TheLambClub.ModelsLogic
{
    public class Game : GameModel
    {
        public override Player? CurrentPlayer 
        {   
            get 
            {
                Player p = null!;
                if (Players == null)
                    return p;                
                foreach (Player player in Players!)
                {
                    if (player != null && player.Id == new FbData().UserId)
                    {
                        p = player;
                    }
                }
                return p;
            }
        }
        public override string CurrentStatus
        {
            get
            {
                if (!IsFull)
                {
                    return Strings.WaitingStatus;
                }
                return Strings.PlayingStatus;
            }
        }

        public override bool IsMyTurn
        {
            get
            {
                if (Players == null)
                {
                    return false;
                }
                return Players[CurrentPlayerIndex].Id == fbd.UserId && IsFull;
            }
            set;
        }


        public override bool IsHost
        {
            get
            {
                return HostId == fbd.UserId;
            }
        }
        public Game() { RegisterTimer(); }
        public override void NextTurn()
        {
            CurrentPlayerIndex = (CurrentPlayerIndex + 1) % CurrentNumOfPlayers;
            UpdateFBTurnUpdate((task) => OnGameChanged?.Invoke(this, EventArgs.Empty));
        }
        public override void PickedFold()
        {
            CurrentPlayer?.IsFolded = true;
            CurrentPlayerIndex = (CurrentPlayerIndex + 1) % CurrentNumOfPlayers;
            Dictionary<string, object> dict = new()
            {
                { nameof(Players), Players! },
                { nameof(CurrentPlayerIndex), CurrentPlayerIndex }
            };
            fbd.UpdateFields(Keys.GamesCollection, Id, dict, OnComplete);
        }
        protected override void ChangeIsFoldedToFalse()
        {
            foreach (Player player in Players!)
            {
                if (player != null)
                {
                    player.IsFolded = false;
                }
            }
            Dictionary<string, object> dict = new()
            {
                { nameof(Players), Players! },
            };
            fbd.UpdateFields(Keys.GamesCollection, Id, dict, OnComplete);
        }
        public Game(NumberOfPlayers selectedNumberOfPlayers)
        {

            HostName = new User().UserName;
            Created = DateTime.Now;
            NumberOfPlayers = selectedNumberOfPlayers;
            CurrentNumOfPlayers = 1;
            MaxNumOfPlayers = selectedNumberOfPlayers.NumPlayers;
            CurrentPlayerIndex = 0;
            FillDummes();
            RegisterTimer();
        }
        private void RegisterTimer()
        {
            WeakReferenceMessenger.Default.Register<AppMessage<long>>(this, (r, m) =>
            {
                OnMessageReceived(m.Value);
            });
        }

        private void OnMessageReceived(long timeLeft)
        {
            TimeLeft = timeLeft == Keys.FinishedSignal ? Strings.TimeUp : double.Round(timeLeft / 1000, 1).ToString();
            TimeLeftChanged?.Invoke(this, EventArgs.Empty);
        }
        protected override void FillDummes()
        {
            for (int i = 0; i < MaxNumOfPlayers; i++)
            {
                //PlayersNames![i] = string.Empty;
                //PlayersIds![i] = string.Empty;
            }
        }

     

        public override void SetDocument(Action<Task> OnComplete)
        {
            Id = fbd.SetDocument(this, Keys.GamesCollection, Id, OnComplete);
        }

        protected override void FillArrayAndAddCards(Action<Task> OnComplete)
        {
            foreach (Player item in Players!)
            {
                Console.WriteLine("item");
                if(item!=null)
                {
                    item.FBCard1 = setOfCards.GetRandomCard();
                    item.FBCard2 = setOfCards.GetRandomCard();
                    Console.WriteLine("cards has haded");
                }
            }
            UpdatePlayersArray(_ => { });
        }
        protected override void UpdatePlayersArray(Action<Task> OnComplete)
        {
            Dictionary<string, object> dict = new()
            {
                { nameof(Players), Players! },
            };
            fbd.UpdateFields(Keys.GamesCollection, Id, dict, OnComplete);
        }

        public override void AddSnapShotListener()
        {
            ilr = fbd.AddSnapshotListener(Keys.GamesCollection, Id, OnChange);
        }
      
        public override void RemoveSnapShotListener()
        {
            ilr?.Remove();
            DeleteDocument(OnComplete);
        }

        protected override void OnComplete(Task task)
        {
            if (task.IsCompletedSuccessfully)
                OnGameDeleted?.Invoke(this, EventArgs.Empty);
        }

        public override void UpdateGuestUser(Action<Task> OnComplete)
        {
            Console.WriteLine("Updating guest user" + CurrentNumOfPlayers);
            foreach (Player player in Players!)
            {
                if (player!=null&& player!.Id == fbd.UserId)
                    return;
            }
            Console.WriteLine("players initalized");
            Player newPlayer = new(MyName, fbd.UserId);
            Console.WriteLine(MyName+",");
            Players[CurrentNumOfPlayers] = newPlayer;
            CurrentNumOfPlayers++;
            UpdateFireBaseJoinGame(OnComplete);
        }

        protected override void UpdateFireBaseJoinGame(Action<Task> OnComplete)
        {
            Dictionary<string, object> dict = new()
            {
                { nameof(Players), Players! },
                { nameof(CurrentNumOfPlayers), CurrentNumOfPlayers },
                { nameof(CurrentPlayerIndex), CurrentPlayerIndex },
                { nameof(RoundNumber), RoundNumber },
                { nameof(IsFull), IsFull },
            };
            fbd.UpdateFields(Keys.GamesCollection, Id, dict, OnComplete);
        }

        protected override void UpdateFBTurnUpdate(Action<Task> OnComplete)
        {
            Dictionary<string, object> dict = new()
            {
                { nameof(CurrentPlayerIndex), CurrentPlayerIndex },
            };
            fbd.UpdateFields(Keys.GamesCollection, Id, dict, OnComplete);
        }

        protected override void UpdateBoard(Action<Task> OnComplete)
        {
            Dictionary<string, object> dict = new()
            {
                { nameof(BoardCards), BoardCards },
                { nameof(RoundNumber), RoundNumber },
            };
            fbd.UpdateFields(Keys.GamesCollection, Id, dict, OnComplete);
        }

        public override bool IsFull
        {
            get { return CurrentNumOfPlayers == MaxNumOfPlayers; }

            set => _ = CurrentNumOfPlayers == MaxNumOfPlayers;
        }


        public override void DeleteDocument(Action<Task> OnComplete)
        {
            fbd.DeleteDocument(Keys.GamesCollection, Id, OnComplete);
        }
        protected override void FillBoard()
        {
            if(RoundNumber==0)
            {
                BoardCards[0] = null!;
                BoardCards[1] = null!;
                BoardCards[2] = null!;
                BoardCards[3] = null!;
                BoardCards[4] = null!;
            }
            if (RoundNumber == 1)
            {
                BoardCards[0] = setOfCards.GetRandomCard();
                BoardCards[1] = setOfCards.GetRandomCard();
                BoardCards[2] = setOfCards.GetRandomCard();
            }
            else if (RoundNumber == 2)
            {
                BoardCards[3] = setOfCards.GetRandomCard();
            }
            else if (RoundNumber == 3)
            {
                BoardCards[4] = setOfCards.GetRandomCard();
            }
        }
        //protected void AddCardsToPlayers()
        //{
        //    for (int i = 0; i < MaxNumOfPlayers; i++)
        //    {
        //        if(Players[i]!=null)
        //        {
        //            Players![i] = Players[i];
        //        }
        //    }
        //}
        protected override bool IsOneStaying()
        {
            int countNotFolded=0;
            bool result=false;
            foreach (Player player in Players!)
            {
                if(player != null && !player.IsFolded)
                {
                    countNotFolded++;
                }
            }
            result= countNotFolded == 1;
            return result;
        }
        protected  int FirstPlayerWhichIsNotFold()
        {
            int i = 0;
            foreach (Player player in Players!)
            {
                if(player != null && !player.IsFolded)
                {
                    return i;
                }
                i++;
            }
           return -1;
        }
        public void BetFunction(object obj)
        {

            CurrentPlayer?.CurrentMoney = CurrentPlayer.CurrentMoney - CurrentPlayer.CurrentBet;
            if (IsFull && Players?[BeforeCurrentPlayerIndex()].CurrentBet < CurrentPlayer?.CurrentBet && Players?[BeforeCurrentPlayerIndex()].CurrentBet != 0)
            {
                Console.WriteLine("both");
                CurrentPlayer.IsReRazed = true;
                CurrentPlayerIndex = FirstPlayerWhichIsNotFold();
            }
            Dictionary<string, object> dict = new()
            {
                { nameof(Players), Players! },
            };
            fbd.UpdateFields(Keys.GamesCollection, Id, dict, OnComplete);
            NextTurn();
        }
        private int BeforeCurrentPlayerIndex()
        {
            if (CurrentPlayerIndex==0)
            {
                for (int i = MaxNumOfPlayers-1; i >0; i--)
                {
                    if (Players[i] !=null&& !Players[i].IsFolded)
                    {
                        return i;
                    }
                }
            }
            return CurrentPlayerIndex-1 ;
        }
        public void CallFunction()
        {
            if (Players?[BeforeCurrentPlayerIndex()].CurrentBet != CurrentPlayer?.CurrentBet)
            {
             CurrentPlayer?.CurrentMoney = CurrentPlayer.CurrentMoney - CurrentPlayer.CurrentBet;
            }
            Dictionary<string, object> dict = new()
            {
                { nameof(Players), Players! },
            };
            fbd.UpdateFields(Keys.GamesCollection, Id, dict, OnComplete);
            NextTurn();
        }
        protected bool EveryOneIsNotRerazeing()
        {
           
                foreach (Player player in Players!)
                {
                    if (player != null && player.IsReRazed)
                    {
                        return false;
                    }
                }
                return true;
            
        }
        protected override void OnChange(IDocumentSnapshot? snapshot, Exception? error)
        {
            Console.WriteLine("Game OnChange called");
            Game? updatedGame = snapshot?.ToObject<Game>();
            if (updatedGame != null)
            {
                bool isEndOfRound = CurrentPlayerIndex > 0 && updatedGame.CurrentPlayerIndex == 0 && EveryOneIsNotRerazeing();
                bool changedToFull = CurrentNumOfPlayers < MaxNumOfPlayers && updatedGame.CurrentNumOfPlayers == MaxNumOfPlayers;  // = (RoundNumber < updatedGame.RoundNumber && updatedGame.RoundNumber == HandComplete);
                string WinnerName=string.Empty;
                Dictionary<Player, HandRank> ranks = []; 
                Player[] playersArray=null!; 
                Players = updatedGame.Players;
                CurrentNumOfPlayers = updatedGame.CurrentNumOfPlayers;               
                RoundNumber = updatedGame.RoundNumber;
                BoardCards = updatedGame.BoardCards;
                CurrentPlayerIndex = updatedGame.CurrentPlayerIndex;
                //if (IsFull && IsMyTurn && CurrentPlayer.IsReRazed)
                //{
                //    Console.WriteLine("Moving by rerazed");
                //    CurrentPlayer.IsReRazed = false;
                //    NextTurn();
                //    Dictionary<string, object> dict = new()
                //    {
                //      { nameof(Players), Players! },
                //    };
                //    fbd.UpdateFields(Keys.GamesCollection, Id, dict, OnComplete);
                //}
                if (IsFull&&Players?[BeforeCurrentPlayerIndex()].CurrentBet!=CurrentPlayer?.CurrentBet )
                {
                    CheckOrCall = "call "+ Players?[BeforeCurrentPlayerIndex()].CurrentBet+"$";
                    OnCheckOrCallChanged?.Invoke(this, EventArgs.Empty);                  
                }
                else
                {
                    CheckOrCall= "check";
                    OnCheckOrCallChanged?.Invoke(this, EventArgs.Empty);                 
                }
                if ((IsOneStaying() && IsFull || EndOfHand) && IsHost)
                {
                    if (IsOneStaying())
                    {
                        foreach (Player player in Players!)
                        {
                            if (player != null && !player.IsFolded)
                            {
                               WinnerName = player.Name;
                            }
                        }
                        Shell.Current.ShowPopupAsync(new OnlyOneStayedPopup(WinnerName));

                    }
                    else
                    {
                        foreach (Player player in Players!)
                        {
                            if (player != null && !player.IsFolded)
                            {
                                HandRank handRank = player.EvaluateBestHand(BoardCards);
                                ranks.Add(player, handRank);
                            }
                        }
                         playersArray = new Player[ranks.Count];
                        ranks.Keys.CopyTo(playersArray, 0);
                        Array.Sort(playersArray, (p1, p2) =>
                        {
                            return ranks[p2].Compare(ranks[p1]);
                        });
                        Shell.Current.ShowPopupAsync(new WinningPopupPage(playersArray, ranks));
                    }
                    EndOfHand = false;
                    ChangeIsFoldedToFalse();
                    RoundNumber = 0;
                    FillBoard();
                    UpdateBoard((t) => { });
                    FillArrayAndAddCards((t) => { });
                }
                if(CurrentPlayer != null && IsMyTurn && CurrentPlayer.IsFolded)                   
                {
                    WeakReferenceMessenger.Default.Send(new AppMessage<TimerSettings>(timerSettings));
                }
                else
                  WeakReferenceMessenger.Default.Send(new AppMessage<bool>(true));
                if (CurrentPlayer != null && IsMyTurn && CurrentPlayer.IsFolded)
                {
                    NextTurn();
                    IsMyTurn = false;
                }

                if (IsHost && isEndOfRound)
                {
                    RoundNumber++;
                    FillBoard();
                    UpdateBoard((t) => { });
                    EndOfHand = RoundNumber == HandComplete;
                }
                
                if (IsHost && changedToFull)
                {
                    FillArrayAndAddCards((t) => { });
                }              
                OnGameChanged?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                MainThread.InvokeOnMainThreadAsync(() =>
                {
                    Shell.Current.Navigation.PopAsync();
                    Toast.Make(Strings.GameDeleted, ToastDuration.Long).Show();
                });
                OnGameDeleted?.Invoke(this, EventArgs.Empty);
            }
        }

        //private void CalcWinner()
        //{
        //    Dictionary<Player, HandRank> ranks = new Dictionary<Player, HandRank>();
        //    foreach (Player player in Players!)
        //    {
        //        if (player != null && !player.IsFolded)
        //        {
        //            HandRank handRank = player.EvaluateBestHand(BoardCards);
        //            ranks.Add(player, handRank);
        //        }
        //    }
        //    Player[] playersArray = new Player[ranks.Count];
        //    ranks.Keys.CopyTo(playersArray, 0);
        //    Array.Sort(playersArray, (p1, p2) =>
        //    {
        //        return ranks[p2].Compare(ranks[p1]);
        //    });
        //    Shell.Current.ShowPopupAsync(new WinningPopupPage(playersArray, ranks));
        //}
    }
}
