using CommunityToolkit.Mvvm.Messaging;
using Plugin.CloudFirestore;
using System.Collections.ObjectModel;
using TheLambClub.Models;

namespace TheLambClub.ModelsLogic
{
    public class Game : GameModel
    {
        public override ObservableCollection<ViewCard>? BoardViewCards
        {
            get
            {
                return [.. BoardCards.Select(c =>
                {
                    if (c == null)
                        return new ViewCard();
                    return new ViewCard(c);
                })];

            }
        }
        public override ViewCard? ViewCard1
        { get
            {
                ViewCard vc;
                if (CurrentPlayer == null || CurrentPlayer.FBCard1 == null)
                    vc = new ViewCard();
                else
                    vc = new ViewCard(CurrentPlayer.FBCard1);
                return vc;
            }
        }
        public override ViewCard? ViewCard2
        { get
            {
                ViewCard vc;
                if (CurrentPlayer == null || CurrentPlayer.FBCard2 == null)
                    vc = new ViewCard();
                else
                    vc = new ViewCard(CurrentPlayer.FBCard2);
                return vc;
            }
        }

        public override Player? CurrentPlayer
        {
            get
            {
                Player p = null!;
                if (Players != null)
                {
                    foreach (Player player in Players!)
                    {
                        if (player != null && player.Id == new FbData().UserId)
                        {
                            p = player;
                        }
                    }
                }
                return p;
            }
        }
        public override string CurrentStatus => IsFull ? Strings.PlayingStatus : Strings.WaitingStatus;
        public override bool IsMyTurn
        {
            get
            {
                bool IsMyTurn = false;
                if (Players != null)               
                    IsMyTurn = Players[CurrentPlayerIndex].Id == fbd.UserId && IsFull;               
                return IsMyTurn;
            }
        }
        public override bool IsHost=> HostId == fbd.UserId;
        public Game(){ }

        public override void NextTurn()
        {
            PlayerBeforeId = Players![CurrentPlayerIndex].Id;
            CurrentPlayerIndex = (CurrentPlayerIndex + 1) % CurrentNumOfPlayers;
            UpdateFBTurnUpdate((task) => OnGameChanged?.Invoke(this, EventArgs.Empty));
            Dictionary<string, object> dict = new()
            {
                { nameof(PlayerBeforeId), PlayerBeforeId! },
            };
            fbd.UpdateFields(Keys.GamesCollection, Id, dict, (task) => { });
        }
        public override void PickedFold()
        {
            CurrentPlayer!.IsFolded = true;
            CurrentPlayerIndex = (CurrentPlayerIndex + 1) % CurrentNumOfPlayers;
            Dictionary<string, object> dict = new()
            {
                { nameof(Players), Players! },
                { nameof(CurrentPlayerIndex), CurrentPlayerIndex }
            };
            fbd.UpdateFields(Keys.GamesCollection, Id, dict, (task) => { });
        }
        protected override void ChangeIsFoldedToFalse()
        {
            foreach (Player player in Players!)
            {
                player.IsFolded = false;
            }
        }
        public Game(int selectedNumberOfPlayers)
        {
            HostName = new User().UserName;
            Created = DateTime.Now;
            NumberOfPlayers = selectedNumberOfPlayers;
            CurrentNumOfPlayers = 1;
            MaxNumOfPlayers = selectedNumberOfPlayers;
            CurrentPlayerIndex = 0;
        }
        protected override void RegisterTimer()
        {
            WeakReferenceMessenger.Default.Register<AppMessage<long>>(this, (r, m) =>
            {
                OnMessageReceived(m.Value);
            });
        }
        protected override void OnMessageReceived(long timeLeft)
        {
            TimeLeft = timeLeft == Keys.FinishedSignal ? Strings.TimeUp : double.Round(timeLeft / 1000, 1).ToString();
            if (timeLeft == Keys.FinishedSignal && CurrentPlayer?.IsFolded == false)
            {
                PickedFold();
            }
            TimeLeftChanged?.Invoke(this, EventArgs.Empty);
        }
        public override void SetDocument(Action<Task> OnComplete)
        {
            Id = fbd.SetDocument(this, Keys.GamesCollection, Id, OnComplete);
        }

        protected override void FillArrayAndAddCards(bool upDateFB, Action<Task> OnComplete)
        {
            foreach (Player item in Players!)
            {
                if (item != null)
                {
                    item.FBCard1 = SetOfCards.GetRandomCard();
                    item.FBCard2 = SetOfCards.GetRandomCard();
                }
            }
            if (upDateFB)
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
            foreach (Player player in Players!)
            {
                if (player != null && player!.Id == fbd.UserId)
                    return;
            }
            Player newPlayer = new(MyName, fbd.UserId);
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
        public override bool IsFull { get => CurrentNumOfPlayers == MaxNumOfPlayers; set => _ = CurrentNumOfPlayers == MaxNumOfPlayers; }
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
                BoardCards[0] = SetOfCards.GetRandomCard();
                BoardCards[1] = SetOfCards.GetRandomCard();
                BoardCards[2] = SetOfCards.GetRandomCard();
            }
            else if (RoundNumber == 2)
            {
                BoardCards[3] = SetOfCards.GetRandomCard();
            }
            else if (RoundNumber == 3)
            {
                BoardCards[4] = SetOfCards.GetRandomCard();
            }
        }
        protected override bool IsOneStaying()
        {
            int countNotFolded=0;
            foreach (Player player in Players!)           
                if(player != null && !player.IsFolded)               
                    countNotFolded++;               
            return countNotFolded == 1;
        }
        protected override int FirstPlayerWhichIsNotFold()
        {
            int i = 0;
            foreach (Player player in Players!)
            {
                if(player != null && !player.IsFolded)               
                    break;           
                i++;
            }
           return i;
        }

        protected override int BeforeCurrentPlayerIndex() {
            int index = CurrentPlayerIndex;
            if (Players != null && Players.Length == MaxNumOfPlayers)
            {
                for (int i = 0; i < Players.Length - 1; i++)
                {
                    index = i; 
                    if (Players[i] != null && Players[i].Id == PlayerBeforeId)
                    {
                        break;
                    } 
                } 
            }
            return index;
        }
        public override void BetFunction(object obj)
        {
            Player prevPlayer = Players![BeforeCurrentPlayerIndex()];
            Player current = CurrentPlayer!;
            current.CurrentMoney -= current.CurrentBet;
            Pot[RoundNumber] += current.CurrentBet;
            if (IsFull && prevPlayer.CurrentBet < current.CurrentBet && prevPlayer.CurrentBet != 0)
            {
                current.IsReRazed = true;
                CurrentPlayerIndex = FirstPlayerWhichIsNotFold();
                foreach (Player player in Players!)
                {
                    if (player.Id != current.Id)
                        player.IsReRazed = false;
                }
            }
            else
            {
                PlayerBeforeId = Players![CurrentPlayerIndex].Id;
                CurrentPlayerIndex = (CurrentPlayerIndex + 1) % CurrentNumOfPlayers;
            }
            Dictionary<string, object> update = new()
            {
                { nameof(CurrentPlayerIndex), CurrentPlayerIndex },
                { nameof(Players), Players },
                { nameof(Pot), Pot }
            };
            fbd.UpdateFields(Keys.GamesCollection, Id, update, _ => { });
            //OnMyMoneyChanged?.Invoke(this, EventArgs.Empty);
            Console.WriteLine("Betiing");
        }

        public override void CallFunction()
        {
            int prevIndex = BeforeCurrentPlayerIndex();
            Player prevPlayer = Players![prevIndex];
            double moneyToCall = prevPlayer.CurrentBet;
            if (moneyToCall > 0)
            {
                Players[CurrentPlayerIndex].CurrentBet = moneyToCall;
                Players[CurrentPlayerIndex].CurrentMoney -= moneyToCall;
                Pot[RoundNumber] += moneyToCall;
            }
            PlayerBeforeId = Players![CurrentPlayerIndex].Id;
            CurrentPlayerIndex = (CurrentPlayerIndex + 1) % CurrentNumOfPlayers;
            Dictionary<string, object> update = new()
            {
                { nameof(CurrentPlayerIndex), CurrentPlayerIndex },
                { nameof(Players), Players },
                { nameof(Pot), Pot }
            };
            fbd.UpdateFields(Keys.GamesCollection, Id, update, _ => { });
            //OnMyMoneyChanged?.Invoke(this, EventArgs.Empty);
            Console.WriteLine("calling");
        }

        protected override bool EveryOneIsNotRerazeing()
        {
            bool result = true;
            foreach (Player player in Players!)
                if (player != null && player.IsReRazed)
                    result= false;
            return result;  
        }
        protected override void EndHand()
        {
            EndOfHand = false;
            ChangeIsFoldedToFalse();
            RoundNumber = 0;
            CurrentPlayerIndex = 0;
            FillBoard();
            FillArrayAndAddCards(false,(t) => { });          
            Dictionary<string, object> dict = new()
            {
                { nameof(CurrentPlayerIndex), CurrentPlayerIndex! },
                { nameof(BoardCards), BoardCards },
                { nameof(RoundNumber), RoundNumber },
                { nameof(Players), Players! },
            };
            fbd.UpdateFields(Keys.GamesCollection, Id, dict, (task) => { });
        }
        protected override void OnChange(IDocumentSnapshot? snapshot, Exception? error)
        {
            Console.WriteLine("Game OnChange called");
            Game? updatedGame = snapshot?.ToObject<Game>();
            if (updatedGame != null)
            {
                //    if (Players !=null&& Players![BeforeCurrentPlayerIndex()] !=null&& updatedGame.Players![BeforeCurrentPlayerIndex()].CurrentMoney!= Players![BeforeCurrentPlayerIndex()].CurrentMoney)
                //    {
                //        OnMyMoneyChanged?.Invoke(this, EventArgs.Empty);
                //    }
                bool currentPlayerIndexChange = CurrentPlayerIndex != updatedGame.CurrentPlayerIndex;
                bool isEndOfRound = CurrentPlayerIndex > 0 && updatedGame.CurrentPlayerIndex == 0&&EveryOneIsNotRerazeing();
                bool changedToFull = CurrentNumOfPlayers < MaxNumOfPlayers && updatedGame.CurrentNumOfPlayers == MaxNumOfPlayers; 
                bool isGameStarted = CurrentNumOfPlayers != updatedGame.CurrentNumOfPlayers && updatedGame.CurrentNumOfPlayers == MaxNumOfPlayers;
                bool isCheckOrCallChanged = (updatedGame.CurrentPlayer?.CurrentBet != CurrentPlayer?.CurrentBet);
                bool isHandEnded = false;
                string WinnerName = string.Empty;
                Dictionary<Player, HandRank> ranks = [];
                Player[] playersArray = null!;
                Players = updatedGame.Players;
                CurrentNumOfPlayers = updatedGame.CurrentNumOfPlayers;
                RoundNumber = updatedGame.RoundNumber;
                BoardCards = updatedGame.BoardCards;
                CurrentPlayerIndex = updatedGame.CurrentPlayerIndex;
                PlayerBeforeId= updatedGame.PlayerBeforeId;
                Console.WriteLine("CurrentNumOfPlayers" + CurrentNumOfPlayers);
                Console.WriteLine("updatedGame.CurrentNumOfPlayers" + updatedGame.CurrentNumOfPlayers);
                if (IsFull && Players?[BeforeCurrentPlayerIndex()].CurrentBet > CurrentPlayer?.CurrentBet)
                {
                    CheckOrCall = Strings.Call + (Players[BeforeCurrentPlayerIndex()].CurrentBet - CurrentPlayer.CurrentBet) + "$";
                    int minimalBet = (int)(Players[BeforeCurrentPlayerIndex()].CurrentBet - CurrentPlayer.CurrentBet) * 2;
                    MinBet = CurrentPlayer.CurrentMoney > minimalBet ? minimalBet : (int)CurrentPlayer.CurrentMoney;
                    OnCheckOrCallChanged?.Invoke(this, EventArgs.Empty);
                }
                else
                {
                    CheckOrCall = Strings.Check;
                    MinBet = 0;
                    OnCheckOrCallChanged?.Invoke(this, EventArgs.Empty);
                }
                if ((IsOneStaying() && IsFull || RoundNumber == HandComplete) )
                {
                    if (IsOneStaying())
                    {
                        Player[] Winner = new Player[1];
                        foreach (Player player in Players!)
                            if (player != null && !player.IsFolded)                            
                                Winner[0] = player;                                                    
                        OnwinnerSelected?.Invoke(this, new WinningPopupEvent(Winner, null!));
                        IsPopupOpen= false;
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
                        Array.Sort(playersArray, (p1, p2) => ranks[p2].Compare(ranks[p1]));            
                        OnwinnerSelected?.Invoke(this, new WinningPopupEvent(playersArray, ranks));
                        IsPopupOpen = false;
                    }
                    if (IsHost)
                    {
                        EndHand();
                        isHandEnded= true;
                    }
                }
                if (CurrentPlayer != null && IsMyTurn && CurrentPlayer.IsFolded)              
                    NextTurn();
                if (CurrentPlayer != null && IsMyTurn && CurrentPlayer.IsReRazed)
                {
                    CurrentPlayer.IsReRazed = false;
                    NextTurn();
                    Dictionary<string, object> d = new()
                    {
                    { nameof(CurrentPlayerIndex), CurrentPlayerIndex! },
                    {nameof(Players), Players! },
                    };
                    fbd.UpdateFields(Keys.GamesCollection, Id, d, (task) => { });
                }
                if (IsFull&&IsHost &&EveryOneIsNotRerazeing()&&isEndOfRound && !isHandEnded)
                {
                    foreach (Player player in Players!)
                    {
                        Console.WriteLine($"Current index {CurrentPlayerIndex} Current Money {CurrentPlayer?.CurrentMoney} 1  ");
                        player!.CurrentBet = 0;
                        Console.WriteLine($"Current index {CurrentPlayerIndex} Current Money {CurrentPlayer?.CurrentMoney} 2  ");
                    }
                    RoundNumber++;
                    Console.WriteLine("Round Number Updated" + RoundNumber+ "  ,"+ DateTime.Now);
                    FillBoard();                  
                    EndOfHand = RoundNumber == HandComplete;
                    UpdateBoard((t) => { });
                    Dictionary<string, object> dict = new()
                    {
                       { nameof(Players), Players! },
                    };
                    fbd.UpdateFields(Keys.GamesCollection, Id, dict, (task) => { });
                }
                if (IsHost && changedToFull && !isHandEnded)
                {
                    FillArrayAndAddCards(true,(t) => { });
                }              
                if (!TimerCreated)
                {
                    RegisterTimer();
                    TimerCreated = true;
                }
                OnGameChanged?.Invoke(this, EventArgs.Empty);
            }
            else
            {            
                OnGameDeleted?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
