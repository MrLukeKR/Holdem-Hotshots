# Pseudo Game Logic for holden
## Haskell notation assumed (for now)

```Haskell
type Game = ([Seat], Hand, Deck, Pot, Pool, DealerPos, PlayPos)
type Seat = (Hand, Chips, Pos, Bet, State)
type Hand = [Card]
type Deck = [Card]
type Card = [Rank, Suit]
type Pot  = Int
type Pool = Int
type DealerPos = Int
type PlayPos   = Int
type Chips = Int
type Pos = Int
type Bet = Int

data State = In | Out | Folded

data Rank = Deuce | Three | Four | Five | Six | Seven | Eight |
            Nine | Ten | Jack | Queen | King | Ace
  deriving (Eq, Ord)
-- Implement Show Rank for output

data Suit = Heart | Diamond | Spade | Club
  deriving (Eq, Ord)
-- Implement Show Suit for output

resetHands :: Game -> Game
-- remove Cards from Seat's Hand and communal Hand

deal :: Game -> Int -> Game
-- give each Seat's Hand a number of Cards from the Game's Deck
dealtoseat

bet :: Seat -> Int -> Seat

```
